﻿using System;
using System.Collections.Generic;
using DevExpress.Data.Internal;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using SenDev.DashboardsDemo.Module.BusinessObjects;
using SenDev.Xaf.Dashboards;
using SenDev.Xaf.Dashboards.Scripting;

namespace SenDev.DashboardsDemo.Module
{
	// For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
	public sealed partial class DashboardsDemoModule : ModuleBase {
        public DashboardsDemoModule() {
            InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
			SafeSerializationBinder.RegisterKnownType(typeof(ScriptDataSource));
			Application.SetupComplete += Application_SetupComplete;
			// Manage various aspects of the application UI and behavior at the module level.
		}

		private void Application_SetupComplete(object sender, EventArgs e)
		{
			var module = Application.Modules.FindModule<SenDevDashboardsModule>();
			module.DefaultBusinessObjectType = typeof(OnlineSales);
		}

		public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
    }
}
