using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using SenDev.Xaf.Dashboards.Scripting;
using Xunit;

namespace UnitTests
{


	public class UnitTestsModule : ModuleBase
	{
		public UnitTestsModule()
		{
			RequiredModuleTypes.Add(typeof(SenDev.Xaf.Dashboards.SenDevDashboardsModule));
		}
	}
}
