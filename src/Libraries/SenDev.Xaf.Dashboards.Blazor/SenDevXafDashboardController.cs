using System;
using DevExpress.DashboardWeb;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.ExpressApp.Dashboards.Blazor;
using Microsoft.AspNetCore.DataProtection;
using SenDev.Xaf.Dashboards.Utils;
using static System.Net.Mime.MediaTypeNames;

namespace SenDev.Xaf.Dashboards.Blazor
{
	public class SenDevXafDashboardController : XafBlazorDashboardController
	{
		public SenDevXafDashboardController(DashboardConfigurator configurator, IDataProtectionProvider dataProtectionProvider, 
			IXafApplicationProvider xafApplicationProvider) : base(configurator, dataProtectionProvider)
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
