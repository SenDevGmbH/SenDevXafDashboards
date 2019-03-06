namespace SenDev.DashboardsDemo.Win {
    partial class DashboardsDemoWindowsFormsApplication {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule();
            this.module3 = new SenDev.DashboardsDemo.Module.DashboardsDemoModule();
            this.module4 = new SenDev.DashboardsDemo.Module.Win.DashboardsDemoWindowsFormsModule();
            this.objectsModule = new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule();
            this.dashboardsModule = new DevExpress.ExpressApp.Dashboards.DashboardsModule();
            this.dashboardsWindowsFormsModule = new DevExpress.ExpressApp.Dashboards.Win.DashboardsWindowsFormsModule();
            this.senDevDashboardsModule = new SenDev.Xaf.Dashboards.SenDevDashboardsModule();
            this.senDevDashboardsWinModule1 = new SenDev.Xaf.Dashboards.Win.SenDevDashboardsWinModule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // dashboardsModule
            // 
            this.dashboardsModule.DashboardDataType = typeof(DevExpress.Persistent.BaseImpl.DashboardData);
            // 
            // dashboardsWindowsFormsModule
            // 
            this.dashboardsWindowsFormsModule.DesignerFormStyle = DevExpress.XtraBars.Ribbon.RibbonFormStyle.Ribbon;
            // 
            // senDevDashboardsModule1
            // 
            this.senDevDashboardsModule.JobScheduler = null;
            // 
            // DashboardsDemoWindowsFormsApplication
            // 
            this.ApplicationName = "SenDev.DashboardsDemo";
            this.CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.objectsModule);
            this.Modules.Add(this.dashboardsModule);
            this.Modules.Add(this.senDevDashboardsModule);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.dashboardsWindowsFormsModule);
            this.Modules.Add(this.senDevDashboardsWinModule1);
            this.Modules.Add(this.module4);
            this.UseOldTemplates = false;
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.DashboardsDemoWindowsFormsApplication_DatabaseVersionMismatch);
            this.CustomizeLanguagesList += new System.EventHandler<DevExpress.ExpressApp.CustomizeLanguagesListEventArgs>(this.DashboardsDemoWindowsFormsApplication_CustomizeLanguagesList);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule module2;
        private SenDev.DashboardsDemo.Module.DashboardsDemoModule module3;
        private SenDev.DashboardsDemo.Module.Win.DashboardsDemoWindowsFormsModule module4;
        private DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule objectsModule;
        private DevExpress.ExpressApp.Dashboards.DashboardsModule dashboardsModule;
        private DevExpress.ExpressApp.Dashboards.Win.DashboardsWindowsFormsModule dashboardsWindowsFormsModule;
		private Xaf.Dashboards.SenDevDashboardsModule senDevDashboardsModule;
		private Xaf.Dashboards.Win.SenDevDashboardsWinModule senDevDashboardsWinModule1;
	}
}
