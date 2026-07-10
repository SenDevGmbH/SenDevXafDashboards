using System;
using System.Collections;
using System.Threading;
using DevExpress.DashboardCommon;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.ExpressApp;
using SenDev.Xaf.Dashboards.BusinessObjects;

namespace SenDev.Xaf.Dashboards.DataExtract
{
	public abstract class DataExtractBackendBase : IDataExtractBackend
	{
		public abstract string BackendType { get; }

		public abstract Guid? TryGetExtractId(DataConnectionParametersBase connectionParameters);

		public abstract byte[] CreateExtract(object data, CancellationToken cancellationToken);

		public abstract void ConfigureDataConnection(
			DataConnectionParametersBase connectionParameters,
			IDashboardDataExtract extract,
			XafApplication application);

		public virtual int? GetRowCount(object data) => (data as ICollection)?.Count;

		public virtual void PatchDashboard(Dashboard dashboard, IDashboardDataExtract extract, bool designMode) { }
	}
}
