using DevExpress.DashboardWeb;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.ExpressApp.Dashboards.Blazor;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SenDev.Xaf.Dashboards.Utils;

namespace SenDev.Xaf.Dashboards.Blazor.ApiControllers
{

	public class SenDevXafDashboardController  : XafBlazorDashboardController
	{
		public SenDevXafDashboardController(DashboardConfigurator configurator, IDataProtectionProvider dataProtectionProvider, 
			IXafApplicationProvider xafApplicationProvider)  : base(configurator, dataProtectionProvider)
		{
			configurator.ConfigureDataConnection += Configurator_ConfigureDataConnection;
			XafApplicationProvider = xafApplicationProvider;
		}

		private IXafApplicationProvider XafApplicationProvider
		{
			get;
		}

		private void Configurator_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e)
		{
			var application = XafApplicationProvider.GetApplication();
			using var objectSpace = application.CreateObjectSpace();
			DashboardConnectionHelper connectionHelper = new DashboardConnectionHelper(application, objectSpace);
			var extract = connectionHelper.ConfigureDataConnection(e.ConnectionParameters);
			extract.PreserveTempFile = true;
		}



	}
}
