using System;
using System.IO;
using DevExpress.DashboardCommon;
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
	public class DashboardDataExtract : BaseObject
	{
		private string tempFileName;

		public DashboardDataExtract(Session session) : base(session)
		{
			Session.Disposed += Session_Disposed;
		}

		private void Session_Disposed(object sender, EventArgs e)
		{
			DeleteTempFileSafe();
		}



		private void DeleteTempFileSafe()
		{
			if (!string.IsNullOrWhiteSpace(tempFileName) && File.Exists(tempFileName))
			{
				try
				{
					File.Delete(tempFileName);
					tempFileName = null;
				}
				catch (IOException) { }
			}
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
		[ModelDefault(nameof(IModelMember.Caption), "Startzeit")]
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
		[VisibleInListView(false)]
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
		[VisibleInListView(false)]
		[VisibleInLookupListView(false)]
		[ModelDefault(nameof(IModelMember.Caption), "Dauer")]
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


		public void ConfigureConnectionParameters(ExtractDataSourceConnectionParameters parameters)
		{

			parameters.FileName = EnsureTempFileCreated();
		}

		public string EnsureTempFileCreated()
		{
			if (string.IsNullOrWhiteSpace(tempFileName))
			{
				tempFileName = Path.GetTempFileName();
				File.WriteAllBytes(tempFileName, ExtractData);
			}

			return tempFileName;
		}


	}
}
