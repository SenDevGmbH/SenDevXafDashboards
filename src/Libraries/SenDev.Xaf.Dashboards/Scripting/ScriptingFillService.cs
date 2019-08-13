using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DevExpress.DashboardCommon;

namespace SenDev.Xaf.Dashboards.Scripting
{

	public class ScriptingFillService : IObjectDataSourceCustomFillService
	{

		public ScriptingFillService(ScriptingDashboardDataProvider dataProvider, IObjectDataSourceCustomFillService defaultFillService)
		{
			DataProvider = dataProvider;
			DefaultFillService = defaultFillService;
		}

		private ScriptingDashboardDataProvider DataProvider
		{
			get;
		}
		private IObjectDataSourceCustomFillService DefaultFillService
		{
			get;
		}

		public object GetData(DashboardObjectDataSource dataSource, ObjectDataSourceFillParameters fillParameters)
		{

			if (fillParameters.CtorParameters != null && fillParameters.CtorParameters.Count > 0)
			{
				ScriptDataSource scriptDataSource = new ScriptDataSource((string)fillParameters.CtorParameters.Single().Value)
				{
					Application = DataProvider.ContextApplication
				};
				return scriptDataSource.GetData(fillParameters.Parameters.ToDictionary(p => p.Name, p => p.Value));
			}
			else
				return DefaultFillService.GetData(dataSource, fillParameters);
		}


		
	}
}
