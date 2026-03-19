using System;
using DevExpress.DashboardCommon;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.ExpressApp;
using SenDev.Xaf.Dashboards.BusinessObjects;

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
			if (dataConnectionParameters is ExtractDataSourceConnectionParameters extractParameters
				&& Guid.TryParse(extractParameters.FileName, out var id))
			{
				IDashboardDataExtract extract = GetDataExtract(id);
				if (extract != null)
					extract.ConfigureConnectionParameters(Application, extractParameters);

				return extract;
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
