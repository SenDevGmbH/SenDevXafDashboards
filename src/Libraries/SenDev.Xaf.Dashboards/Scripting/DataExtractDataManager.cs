using System;
using System.IO;
using DevExpress.DashboardCommon;
using DevExpress.ExpressApp;
using SenDev.Xaf.Dashboards.BusinessObjects;

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
				foreach (var extract in objectSpace.GetObjectsQuery<DashboardDataExtract>())
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
				var extract = objectSpace.GetObjectByKey<DashboardDataExtract>(key);
				if (extract == null)
				{
					throw new ArgumentException($"No DashboardExtract found for the key '{key}'", nameof(key));
				}

				UpdateDataExtract(extract);
				objectSpace.CommitChanges();
			}
		}
		private void UpdateDataExtract(DashboardDataExtract extract)
		{
			if (extract == null)
				throw new ArgumentNullException(nameof(extract));
			if (string.IsNullOrWhiteSpace(extract.Script))
				return;
			string fileName = Path.GetTempFileName();
			using (DashboardObjectDataSource ods = new DashboardObjectDataSource())
			{
				ScriptDataSource dataSource = new ScriptDataSource(extract.Script) { Application = Application};
				ods.DataSource = dataSource.GetDataForDataExtract();
				using (DashboardExtractDataSource extractDataSource = new DashboardExtractDataSource())
				{
					extractDataSource.ExtractSourceOptions.DataSource = ods;
					extractDataSource.FileName = fileName;
					extractDataSource.UpdateExtractFile();
					extract.ExtractData = File.ReadAllBytes(fileName);
					extract.ExtractDataSize = extract.ExtractData.LongLength;
				}
			}
			File.Delete(fileName);
		}


	}
}
