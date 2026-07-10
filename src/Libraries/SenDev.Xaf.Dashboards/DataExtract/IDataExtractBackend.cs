using System;
using System.Threading;
using DevExpress.DashboardCommon;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.ExpressApp;
using SenDev.Xaf.Dashboards.BusinessObjects;

namespace SenDev.Xaf.Dashboards.DataExtract
{
	public interface IDataExtractBackend
	{
		string BackendType { get; }

		/// <summary>
		/// Tries to extract the data extract ID from connection parameters.
		/// Returns null if this backend does not recognize the parameters.
		/// </summary>
		Guid? TryGetExtractId(DataConnectionParametersBase connectionParameters);

		/// <summary>
		/// Creates the binary extract payload from raw data returned by the script.
		/// Data may be ICollection, IDataReader, IQueryable, or a pre-serialized byte[].
		/// </summary>
		byte[] CreateExtract(object data, CancellationToken cancellationToken);

		/// <summary>
		/// Returns the row count from raw data for metadata; null if unknown.
		/// </summary>
		int? GetRowCount(object data);

		/// <summary>
		/// Configures the connection parameters so the dashboard viewer/designer
		/// can access the extract data. Called when the dashboard fires ConfigureDataConnection.
		/// </summary>
		void ConfigureDataConnection(
			DataConnectionParametersBase connectionParameters,
			IDashboardDataExtract extract,
			XafApplication application);

		/// <summary>
		/// Called after the dashboard is fully loaded in view or design mode.
		/// Allows patching data source references, types, or connection strings in the dashboard.
		/// </summary>
		void PatchDashboard(Dashboard dashboard, IDashboardDataExtract extract, bool designMode);
	}
}
