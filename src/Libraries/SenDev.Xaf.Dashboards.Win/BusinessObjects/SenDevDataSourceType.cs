using DevExpress.DataAccess.Wizard.Model;
using DevExpress.ExpressApp.Dashboards.Win;

namespace SenDev.Xaf.Dashboards.Win.BusinessObjects
{
	public class SenDevDataSourceType : XAFDataSourceType
	{
		public static readonly DataSourceType Script = new SenDevDataSourceType();
		public static readonly DataSourceType XafDataExtract = new SenDevDataSourceType();
	}
}
