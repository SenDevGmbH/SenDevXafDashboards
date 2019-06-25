using System;
using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using DevExpress.Persistent.Base;
using DevExpress.Web;

namespace SenDev.DashboardsDemo.Web
{
	public class Global : System.Web.HttpApplication
	{
		public Global()
		{
			InitializeComponent();
		}
		protected void Application_Start(Object sender, EventArgs e)
		{
			ASPxWebControl.CallbackError += new EventHandler(Application_Error);
#if EASYTEST
            DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
#endif
		}
		protected void Session_Start(Object sender, EventArgs e)
		{
			Tracing.Initialize();
			WebApplication.SetInstance(Session, new DashboardsDemoAspNetApplication());
			DevExpress.ExpressApp.Web.Templates.DefaultVerticalTemplateContentNew.ClearSizeLimit();
			//WebApplication.Instance.SwitchToNewStyle();
			WebApplication.Instance.ConnectionString = ConnectionString;
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached && WebApplication.Instance.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)
			{
				WebApplication.Instance.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
			}
#endif
			WebApplication.Instance.Setup();
			WebApplication.Instance.Start();
		}

		internal static string ConnectionString => ConfigurationManager.ConnectionStrings["ConnectionString"]?.ConnectionString;

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
		}
		protected void Application_EndRequest(Object sender, EventArgs e)
		{
		}
		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
		}
		protected void Application_Error(Object sender, EventArgs e)
		{
			ErrorHandling.Instance.ProcessApplicationError();
		}
		protected void Session_End(Object sender, EventArgs e)
		{
			WebApplication.LogOff(Session);
			WebApplication.DisposeInstance(Session);
		}
		protected void Application_End(Object sender, EventArgs e)
		{
		}
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion
	}
}
