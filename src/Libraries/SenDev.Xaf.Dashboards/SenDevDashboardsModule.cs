﻿using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.XtraReports.Diagnostics;
using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.Scripting;

namespace SenDev.Xaf.Dashboards
{
	// For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
	public sealed partial class SenDevDashboardsModule : ModuleBase
	{
		public SenDevDashboardsModule()
		{
			InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
		}

		public IJobScheduler JobScheduler
		{
			get; set;
		}

		/// <summary>
		/// List of simple names of reference assemblies for script compilation
		/// </summary>
		public string[] ScriptReferenceAssemblies
		{
			get; set;
		}

		/// <summary>
		/// Type of the default business object for script templates
		/// </summary>
		public Type DefaultBusinessObjectType
		{
			get; set;
		}

		/// <summary>
		/// Sets or returns type 
		/// </summary>
		public Type DashboardExtractType { get; set; } = typeof(DashboardDataExtract);
		public Type ScriptCompilerType { get; set; } = typeof(ScriptCompiler);

		public static Type GetDashboardDataExtractType(XafApplication application)
		{
			if (application is null)
				throw new ArgumentNullException(nameof(application));

			return application.Modules.FindModule<SenDevDashboardsModule>()?.DashboardExtractType ?? typeof(DashboardDataExtract);
		}

		public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
		{
			ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
			return new ModuleUpdater[] { updater };
		}
		public override void Setup(XafApplication application)
		{
			base.Setup(application);
			DashboardsModule.DataProvider = new ScriptingDashboardDataProvider();
		}

		public override void CustomizeTypesInfo(ITypesInfo typesInfo)
		{
			base.CustomizeTypesInfo(typesInfo);
			CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
		}
	}
}
