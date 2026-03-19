using System;
using DevExpress.DashboardWeb;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Blazor;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.Utils;

namespace SenDev.Xaf.Dashboards.Blazor.ApiControllers
{

	public class SenDevXafDashboardController  : XafBlazorDashboardController
	{
		private readonly INonSecuredObjectSpaceFactory objectSpaceFactory;

		protected virtual Type ExtractType => typeof(DashboardDataExtract);

		public SenDevXafDashboardController(DashboardConfigurator configurator, IDataProtectionProvider dataProtectionProvider,
			INonSecuredObjectSpaceFactory objectSpaceFactory)  : base(configurator, dataProtectionProvider)
		{
			configurator.ConfigureDataConnection += Configurator_ConfigureDataConnection;
			this.objectSpaceFactory = objectSpaceFactory;
		}

		private void Configurator_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e)
		{
			using var objectSpace = objectSpaceFactory.CreateNonSecuredObjectSpace(ExtractType);
			DashboardConnectionHelper connectionHelper = new DashboardConnectionHelper(objectSpace, ExtractType);
			var extract = connectionHelper.ConfigureDataConnection(e.ConnectionParameters);
			if (extract != null)
				extract.PreserveTempFile = true;
		}



	}
}
