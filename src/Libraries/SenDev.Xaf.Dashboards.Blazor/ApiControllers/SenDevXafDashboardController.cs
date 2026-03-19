using DevExpress.DashboardWeb;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Blazor;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using SenDev.Xaf.Dashboards.Utils;

namespace SenDev.Xaf.Dashboards.Blazor.ApiControllers
{

	public class SenDevXafDashboardController  : XafBlazorDashboardController
	{
		private readonly INonSecuredObjectSpaceFactory objectSpaceFactory;
		private readonly SenDevDashboardsOptions options;

		public SenDevXafDashboardController(DashboardConfigurator configurator, IDataProtectionProvider dataProtectionProvider,
			INonSecuredObjectSpaceFactory objectSpaceFactory,
			IOptions<SenDevDashboardsOptions> options)  : base(configurator, dataProtectionProvider)
		{
			configurator.ConfigureDataConnection += Configurator_ConfigureDataConnection;
			this.objectSpaceFactory = objectSpaceFactory;
			this.options = options.Value;
		}

		private void Configurator_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e)
		{
			var extractType = options.ExtractType;
			using var objectSpace = objectSpaceFactory.CreateNonSecuredObjectSpace(extractType);
			DashboardConnectionHelper connectionHelper = new DashboardConnectionHelper(objectSpace, extractType);
			var extract = connectionHelper.ConfigureDataConnection(e.ConnectionParameters);
			if (extract != null)
				extract.PreserveTempFile = true;
		}

	}
}
