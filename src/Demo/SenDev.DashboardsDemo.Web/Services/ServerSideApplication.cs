using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using SenDev.DashboardsDemo.Module;
using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.Scripting;

namespace SenDev.DashboardsDemo.Web.Services
{

	public class ServerSideApplication : XafApplication
	{
		private static ServerSideApplication instance;
		private static readonly object instanceCreationLock = new object();

		public ServerSideApplication()
		{
			Modules.Add(new DashboardsDemoModule());
			ApplicationName = "SenDev.DashboardsDemo";
		}

		public static ServerSideApplication Instance
		{
			get
			{
				if (instance == null)
				{
					lock (instanceCreationLock)
					{
						if (instance == null)
						{
							var application = new ServerSideApplication() { ConnectionString = Global.ConnectionString };
							application.Setup();
							instance = application;
						}
					}
				}

				return instance;
			}
		}

		protected override void OnCustomCheckCompatibility(CustomCheckCompatibilityEventArgs args)
		{
			args.Handled = true;
		}

		protected override LayoutManager CreateLayoutManagerCore(bool simple) => throw new NotImplementedException();


		protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
		{
			args.ObjectSpaceProvider = new XPObjectSpaceProvider(args.ConnectionString, args.Connection, false);
		}



	}
}
