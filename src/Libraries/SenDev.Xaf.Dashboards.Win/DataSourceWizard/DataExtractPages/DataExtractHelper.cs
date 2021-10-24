using System;
using DevExpress.DashboardCommon.DataSourceWizard;
using DevExpress.ExpressApp;
using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.Utils;

namespace SenDev.Xaf.Dashboards.Win.DataSourceWizard.DataExtractPages
{


	static class DataExtractHelper
	{
		internal static IDashboardDataExtract GetDataExtract(XafApplication application, IObjectSpace objectSpace, object model)
		{
			if (model is IExtractDataSourceModel extractModel)
			{
				if (Guid.TryParse(extractModel.FileName, out var id))
					return DashboardHelper.GetDataExtract(application, objectSpace, id);
			}

			return null;
		}

		internal static bool IsXafDataExtract(object model) => model is IExtractDataSourceModel extractModel && Guid.TryParse(extractModel.FileName, out var guid);

	}
}
