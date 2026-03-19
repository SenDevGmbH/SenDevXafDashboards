using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SenDev.Xaf.Dashboards.Blazor
{
	// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
	public sealed partial class SenDavXafDashboardsBlazorModule : ModuleBase {
        public SenDavXafDashboardsBlazorModule() {
            InitializeComponent();
			RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Dashboards.Blazor.DashboardsBlazorModule));
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            var options = application.ServiceProvider?.GetService<IOptions<SenDevDashboardsOptions>>()?.Value;
            if (options != null)
            {
                var dashboardsModule = application.Modules.FindModule<SenDevDashboardsModule>();
                if (dashboardsModule != null)
                    dashboardsModule.DashboardExtractType = options.ExtractType;
            }
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
    }
}
