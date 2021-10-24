using System;
using System.IO;
using System.Linq;
using DevExpress.DashboardCommon;
using DevExpress.ExpressApp;
using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.Utils;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public class DataExtractDataManager
	{

		public DataExtractDataManager(XafApplication application)
		{
			Application = application;
		}

		private XafApplication Application
		{
			get;
		}

		public void UpdateAllExtracts()
		{
			using (var objectSpace = Application.CreateObjectSpace())
			{
				var extracts = objectSpace
					.GetObjects(SenDevDashboardsModule.GetDashboardDataExtractType(Application))
					.Cast<IDashboardDataExtract>();
				
				foreach (var extract in extracts)
				{
					UpdateDataExtract(extract);
					objectSpace.CommitChanges();
				}
			}
		}

		public void UpdateDataExtractByKey(object key)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));

			using (var objectSpace = Application.CreateObjectSpace())
			{
				var extract = DashboardHelper.GetDataExtract(Application, objectSpace, key);
				if (extract == null)
				{
					throw new ArgumentException($"No DashboardExtract found for the key '{key}'", nameof(key));
				}

				UpdateDataExtract(extract);
				objectSpace.CommitChanges();
			}
		}
		private void UpdateDataExtract(IDashboardDataExtract extract)
		{
			if (extract == null)
				throw new ArgumentNullException(nameof(extract));
			if (string.IsNullOrWhiteSpace(extract.Script))
				return;

			using (DashboardObjectDataSource ods = new DashboardObjectDataSource())
			{
				ScriptDataSource dataSource = CreateScriptDataSource(extract, Application);
				object data = dataSource.GetDataForDataExtract();
				if (data is byte[] buffer)
				{
					SetDataExtractContent(extract, buffer);
					return;
				}
				ods.DataSource = data;
				string fileName = Path.GetTempFileName();
				try
				{
					using (DashboardExtractDataSource extractDataSource = new DashboardExtractDataSource())
					{
						extractDataSource.ExtractSourceOptions.DataSource = ods;
						extractDataSource.FileName = fileName;
						extractDataSource.UpdateExtractFile();
						SetDataExtractContent(extract, File.ReadAllBytes(fileName));
					}
				}
				finally
				{
					if (File.Exists(fileName))
						File.Delete(fileName);
				}

			}
		}

		protected virtual ScriptDataSource CreateScriptDataSource(IDashboardDataExtract extract, XafApplication application) =>
			new ScriptDataSource(extract.Script) { Application = application };

		private static void SetDataExtractContent(IDashboardDataExtract extract, byte[] fileData)
		{
			extract.ExtractData = fileData;
			extract.ExtractDataSize = extract.ExtractData.LongLength;
		}
	}
}
