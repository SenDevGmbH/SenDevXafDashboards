using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using Microsoft.AspNetCore.Mvc;

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
            // Manage various aspects of the application UI and behavior at the module level.
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
    }
}
