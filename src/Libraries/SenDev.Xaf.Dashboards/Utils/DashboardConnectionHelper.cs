using System;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.ExpressApp;
using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.DataExtract;

namespace SenDev.Xaf.Dashboards.Utils
{
	public class DashboardConnectionHelper
	{
		private readonly Type extractType;

		public DashboardConnectionHelper(XafApplication application, IObjectSpace objectSpace)
		{
			Application = application;
			ObjectSpace = objectSpace;
		}

		public DashboardConnectionHelper(IObjectSpace objectSpace, Type extractType)
		{
			ObjectSpace = objectSpace;
			this.extractType = extractType;
		}

		public XafApplication Application
		{
			get;
		}
		public IObjectSpace ObjectSpace
		{
			get;
		}

		public IDashboardDataExtract ConfigureDataConnection(DataConnectionParametersBase dataConnectionParameters)
		{
			foreach (var backend in SenDevDashboardsModule.BackendRegistry.GetAllBackends())
			{
				var id = backend.TryGetExtractId(dataConnectionParameters);
				if (id.HasValue)
				{
					IDashboardDataExtract extract = GetDataExtract(id.Value);
					if (extract != null)
						backend.ConfigureDataConnection(dataConnectionParameters, extract, Application);
					return extract;
				}
			}
			return null;
		}

		protected virtual IDashboardDataExtract GetDataExtract(Guid id)
		{
			var type = Application != null
				? SenDevDashboardsModule.GetDashboardDataExtractType(Application)
				: extractType;
			return (IDashboardDataExtract)ObjectSpace.GetObjectByKey(type, id);
		}
	}
}
