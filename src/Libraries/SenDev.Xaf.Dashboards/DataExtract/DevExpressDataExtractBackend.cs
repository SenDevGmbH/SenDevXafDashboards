using System;
using System.IO;
using System.Threading;
using DevExpress.DashboardCommon;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.ExpressApp;
using SenDev.Xaf.Dashboards.BusinessObjects;

namespace SenDev.Xaf.Dashboards.DataExtract
{
	public class DevExpressDataExtractBackend : DataExtractBackendBase
	{
		public const string BackendTypeName = "DevExpress";
		public override string BackendType => BackendTypeName;

		public override Guid? TryGetExtractId(DataConnectionParametersBase connectionParameters)
		{
			if (connectionParameters is ExtractDataSourceConnectionParameters ep &&
				Guid.TryParse(ep.FileName, out var id))
				return id;
			return null;
		}

		public override byte[] CreateExtract(object data, CancellationToken cancellationToken)
		{
			if (data is byte[] buffer)
				return buffer;

			using var ods = new DashboardObjectDataSource { DataSource = data };
			string fileName = Path.GetTempFileName();
			try
			{
				using var extractDataSource = new DashboardExtractDataSource();
				extractDataSource.ExtractSourceOptions.DataSource = ods;
				extractDataSource.FileName = fileName;
				extractDataSource.UpdateExtractFile(cancellationToken);
				return File.ReadAllBytes(fileName);
			}
			finally
			{
				if (File.Exists(fileName))
					File.Delete(fileName);
			}
		}

		public override void ConfigureDataConnection(
			DataConnectionParametersBase connectionParameters,
			IDashboardDataExtract extract,
			XafApplication application)
		{
			if (connectionParameters is ExtractDataSourceConnectionParameters extractParameters)
				extractParameters.FileName = extract.EnsureTempFileCreated(application);
		}
	}
}
