using DevExpress.Xpo;
using SenDev.Xaf.Dashboards.DataExtract;

namespace SenDev.Xaf.Dashboards.BusinessObjects
{
	/// <summary>
	/// Backend type that uses DevExpress <c>DashboardExtractDataSource</c> to create
	/// and serve data extracts. This is the default backend seeded during database update.
	/// </summary>
	public class DevExpressBackendType : BackendTypeBase
	{
		public DevExpressBackendType(Session session) : base(session) { }

		public override IDataExtractBackend CreateBackend() => new DevExpressDataExtractBackend();
	}
}
