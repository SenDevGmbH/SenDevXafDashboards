using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DevExpress.DashboardCommon;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace SenDev.Xaf.Dashboards.BusinessObjects
{

	[ImageName("BO_Unknown")]
	[VisibleInReports(false)]
	[CreatableItem(false)]
	[NavigationItem("Reports")]
	[ModelDefault(nameof(IModelClass.Caption), "Dashboard Data Extract")]
	public class DashboardDataExtract : BaseObject, IDashboardDataExtract
	{
		private const string RootFolderName = "XafDashboards";
		private const string ExtractDataFileName = "ExtractData.dat";

		// The hash is a lowercase SHA-256 hex string. The temp folder named after it is only
		// ever deleted when it exactly matches this format, to avoid accidentally removing
		// unrelated directories.
		private static readonly Regex hashFolderRegex = new Regex("^[0-9a-f]{64}$", RegexOptions.Compiled);

		private string tempFileName;

		public DashboardDataExtract(Session session) : base(session)
		{
		}

		public override void AfterConstruction()
		{
			base.AfterConstruction();
			CronExpression = "0 1 * * *";
		}



		private string name;
		public string Name
		{
			get => name;
			set => SetPropertyValue(nameof(Name), ref name, value);
		}


		private string cronExpression;

		public string CronExpression
		{
			get => cronExpression;
			set => SetPropertyValue(nameof(CronExpression), ref cronExpression, value);
		}

		private string script;

		[Size(SizeAttribute.Unlimited)]
		[ImmediatePostData]
		[EditorAlias(EditorAliases.CSCodePropertyEditor)]
		public string Script
		{
			get => script;
			set => SetPropertyValue(nameof(Script), ref script, value);
		}



		[VisibleInDetailView(false)]
		[VisibleInListView(false)]
		[VisibleInLookupListView(false)]
		[ModelDefault(nameof(IModelMember.Caption), "Data")]
		[Delayed]
		public byte[] ExtractData
		{
			get => GetDelayedPropertyValue<byte[]>(nameof(ExtractData));
			set => SetDelayedPropertyValue(nameof(ExtractData), value);
		}


		private DateTime startTime;

		[VisibleInDetailView(true)]
		[VisibleInListView(true)]
		[VisibleInLookupListView(false)]
		[ModelDefault(nameof(IModelMember.DisplayFormat), "{0:g}")]
		[ModelDefault(nameof(IModelMember.EditMask), "g")]
		public DateTime StartTime
		{
			get
			{
				return startTime;
			}
			set
			{
				SetPropertyValue(nameof(StartTime), ref startTime, value);
			}
		}


		private DateTime finishTime;

		[VisibleInDetailView(false)]
		[VisibleInListView(true)]
		[VisibleInLookupListView(false)]
		[ModelDefault(nameof(IModelMember.Caption), "Endzeit")]
		[ModelDefault(nameof(IModelMember.DisplayFormat), "{0:g}")]
		[ModelDefault(nameof(IModelMember.EditMask), "g")]
		public DateTime FinishTime
		{
			get
			{
				return finishTime;
			}
			set
			{
				SetPropertyValue(nameof(FinishTime), ref finishTime, value);
			}
		}



		[VisibleInDetailView(true)]
		[VisibleInListView(true)]
		[VisibleInLookupListView(false)]
		[NonPersistent]
		public TimeSpan Duration => FinishTime - StartTime;



		private long extractDataSize;

		[VisibleInDetailView(false)]
		[VisibleInListView(true)]
		[VisibleInLookupListView(false)]
		[ModelDefault(nameof(IModelMember.Caption), "Extract Data Size")]
		public long ExtractDataSize
		{
			get => extractDataSize;
			set => SetPropertyValue(nameof(ExtractDataSize), ref extractDataSize, value);
		}


		private int rowCount;
        [VisibleInDetailView(false)]
		[VisibleInListView(true)]
		[VisibleInLookupListView(false)]
		[ModelDefault(nameof(IModelMember.Caption), "Row Count")]
		public int RowCount
		{
			get => rowCount;
			set => SetPropertyValue(nameof(RowCount), ref rowCount, value);
		}





		
		[NonPersistent]
		public bool PreserveTempFile
		{
			get;
			set;
		}

		private string lastError;
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
		[Size(SizeAttribute.Unlimited)]
		[ModelDefault(nameof(IModelMember.Caption), "Last Error")]
		[ModelDefault(nameof(IModelMember.AllowEdit), "False")]
        public string LastError
        {
            get => lastError;
            set => SetPropertyValue(nameof(LastError), ref lastError, value);
        }

		private DateTime lastExtractDataUpdateDate;
		[VisibleInDetailView(true)]
        [VisibleInListView(true)]
        [VisibleInLookupListView(false)]
        [ModelDefault(nameof(IModelMember.Caption), "Last ExtractData Update Date")]
        [ModelDefault(nameof(IModelMember.DisplayFormat), "{0:g}")]
        [ModelDefault(nameof(IModelMember.EditMask), "g")]
        public DateTime LastExtractDataUpdateDate
        {
            get => lastExtractDataUpdateDate;
            set => SetPropertyValue(nameof(LastExtractDataUpdateDate), ref lastExtractDataUpdateDate, value);
        }

		private string hash;
		[VisibleInDetailView(false)]
		[VisibleInListView(false)]
		[VisibleInLookupListView(false)]
		[Size(64)]
		[ModelDefault(nameof(IModelMember.Caption), "Hash")]
		[ModelDefault(nameof(IModelMember.AllowEdit), "False")]
		public string Hash
		{
			get => hash;
			set => SetPropertyValue(nameof(Hash), ref hash, value);
		}


		
		public void ConfigureConnectionParameters(XafApplication application, ExtractDataSourceConnectionParameters parameters)
		{
			parameters.FileName = EnsureTempFileCreated(application);
		}


		protected virtual byte[] GetExtractData(XafApplication application) => ExtractData;


		public string EnsureTempFileCreated(XafApplication application)
		{
			byte[] data = GetExtractData(application);
			if (data == null)
				return null;

			string hashValue = Hash;
			if (string.IsNullOrEmpty(hashValue))
				hashValue = ComputeHash(data);

			string extractFolder = Path.Combine(Path.GetTempPath(), RootFolderName, Oid.ToString());
			string hashFolder = Path.Combine(extractFolder, hashValue);
			string filePath = Path.Combine(hashFolder, ExtractDataFileName);

			if (!File.Exists(filePath))
			{
				DeleteOldExtractFiles(extractFolder, hashValue);
				Directory.CreateDirectory(hashFolder);
				File.WriteAllBytes(filePath, data);
			}

			tempFileName = filePath;
			return tempFileName;
		}

		private static void DeleteOldExtractFiles(string extractFolder, string currentHash)
		{
			if (!Directory.Exists(extractFolder))
				return;

			foreach (string directory in Directory.GetDirectories(extractFolder))
			{
				string folderName = Path.GetFileName(directory);
				if (string.Equals(folderName, currentHash, StringComparison.OrdinalIgnoreCase))
					continue;

				// Only delete folders whose name exactly matches the hash format. This guards
				// against accidentally deleting directories that were not created by us.
				if (!hashFolderRegex.IsMatch(folderName))
					continue;

				try
				{
					Directory.Delete(directory, true);
				}
				catch (IOException ex)
				{
					Tracing.Tracer.LogError(ex);
				}
			}
		}

		public static string ComputeHash(byte[] data)
		{
			if (data == null)
				return null;

			using (var sha = SHA256.Create())
			{
				byte[] hashBytes = sha.ComputeHash(data);
				var builder = new StringBuilder(hashBytes.Length * 2);
				foreach (byte b in hashBytes)
					builder.Append(b.ToString("x2"));
				return builder.ToString();
			}
		}

		public string GetKeyAsString() => Oid.ToString();
	}
}
