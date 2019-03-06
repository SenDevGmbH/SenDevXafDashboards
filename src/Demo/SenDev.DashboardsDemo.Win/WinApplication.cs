using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Xpo;
using SenDev.DashboardsDemo.Win.Properties;

namespace SenDev.DashboardsDemo.Win
{
	// For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/DevExpressExpressAppWinWinApplicationMembersTopicAll.aspx
	public partial class DashboardsDemoWindowsFormsApplication : WinApplication
	{
		#region Default XAF configuration options (https://www.devexpress.com/kb=T501418)
		static DashboardsDemoWindowsFormsApplication()
		{
			DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
			DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
			DevExpress.ExpressApp.Utils.ImageLoader.Instance.UseSvgImages = true;
		}
		private void InitializeDefaults()
		{
			LinkNewObjectToParentImmediately = false;
			OptimizedControllersCreation = true;
			UseLightStyle = true;
		}
		#endregion
		public DashboardsDemoWindowsFormsApplication()
		{
			InitializeComponent();
			InitializeDefaults();
			senDevDashboardsModule.JobScheduler = new WcfJobScheduler(new Uri(Settings.Default.JobSchedulerServiceUrl));
		}
		protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
		{
			args.ObjectSpaceProviders.Add(new XPObjectSpaceProvider(XPObjectSpaceProvider.GetDataStoreProvider(args.ConnectionString, args.Connection, true), false));
			args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
		}
		private void DashboardsDemoWindowsFormsApplication_CustomizeLanguagesList(object sender, CustomizeLanguagesListEventArgs e)
		{
			string userLanguageName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
			if (userLanguageName != "en-US" && e.Languages.IndexOf(userLanguageName) == -1)
			{
				e.Languages.Add(userLanguageName);
			}
		}
		private void DashboardsDemoWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e)
		{
#if EASYTEST
            e.Updater.Update();
            e.Handled = true;
#else
			if (System.Diagnostics.Debugger.IsAttached)
			{
				e.Updater.Update();
				e.Handled = true;
			}
			else
			{
				string message = "The application cannot connect to the specified database, " +
					"because the database doesn't exist, its version is older " +
					"than that of the application or its schema does not match " +
					"the ORM data model structure. To avoid this error, use one " +
					"of the solutions from the https://www.devexpress.com/kb=T367835 KB Article.";

				if (e.CompatibilityError != null && e.CompatibilityError.Exception != null)
				{
					message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
				}
				throw new InvalidOperationException(message);
			}
#endif
		}
	}
}
