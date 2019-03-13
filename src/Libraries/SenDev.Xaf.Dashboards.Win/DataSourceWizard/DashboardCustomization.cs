using DevExpress.DashboardCommon;
using DevExpress.DashboardWin.ServiceModel;
using DevExpress.DataAccess.UI.Wizard;
using DevExpress.DataAccess.Wizard.Model;
using DevExpress.DataAccess.Wizard.Views;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Win;
using SenDev.Xaf.Dashboards.Win.BusinessObjects;
using SenDev.Xaf.Dashboards.Win.DataSourceWizard.DataExtractPages;
using SenDev.Xaf.Dashboards.Win.DataSourceWizard.ScriptingPages;
using SenDev.Xaf.Dashboards.Win.DataSourceWizard.Xaf;

namespace SenDev.Xaf.Dashboards.Win.DataSourceWizard
{

	public class DashboardCustomization : XafDashboardDataSourceWizardCustomization, IDashboardDataSourceWizardCustomization
	{
		public DashboardCustomization(ScriptDashboardWizardParameters wizardParameters, IObjectSpace objectSpace, XafApplication application) : base(wizardParameters, objectSpace, application)
		{
			WizardParameters = wizardParameters;
			ObjectSpace = objectSpace;
			Application = application;
		}

		public XafApplication Application
		{
			get;
		}
		private IObjectSpace ObjectSpace
		{
			get;
		}
		private ScriptDashboardWizardParameters WizardParameters
		{
			get;
		}


		public new void CustomizeDataSourceWizard(IWizardCustomization<DashboardDataSourceModel> customization)
		{

			AddDefaultCustomizations(customization);
			if (customization.Model.ObjectType != null)
			{
				customization.StartPage = typeof(EnterScriptPage<DashboardDataSourceModel>);
			}
			var connectionModel = customization.Model as IDataComponentModelWithConnection;
			bool hasDataConnection = connectionModel?.DataConnection != null;
			if (DataExtractHelper.IsXafDataExtract(customization.Model))
			{
				customization.StartPage = typeof(ChoiceDataExtractPage<DashboardDataSourceModel>);
			}
			else if (customization.Model.DataSchema == null && !hasDataConnection)
			{
				customization.StartPage = typeof(XafDashboardChooseDataSourceTypePage<DashboardDataSourceModel>);
			}
			customization.RegisterPage<XafDashboardChooseDataSourceTypePage<DashboardDataSourceModel>, XafDashboardChooseDataSourceTypePage<DashboardDataSourceModel>>();
			customization.RegisterPageView<IChooseDataSourceTypePageView, XafDashboardChooseDataSourceTypePageView>();
			customization.RegisterPage<EnterScriptPage<DashboardDataSourceModel>, EnterScriptPage<DashboardDataSourceModel>>();
			customization.RegisterPage<ChoiceDataExtractPage<DashboardDataSourceModel>, ChoiceDataExtractPage<DashboardDataSourceModel>>();
			customization.RegisterPageView<IChoiceDataExctractPageView, ChoiceDataExtractPageView>();
			customization.RegisterPageView<IEnterScriptPageView, EnterScriptPageView>();
			customization.RegisterInstance(WizardParameters);
			customization.RegisterInstance(ObjectSpace);
			customization.RegisterInstance(Application);
		}

		private void AddDefaultCustomizations(IWizardCustomization<DashboardDataSourceModel> customization)
		{
			customization.RegisterPage<XafDashboardChooseDataSourceTypePage<DashboardDataSourceModel>, XafDashboardChooseDataSourceTypePage<DashboardDataSourceModel>>();
			customization.RegisterPageView<IChooseDataSourceTypePageView, XafDashboardChooseDataSourceTypePageView>();
			customization.RegisterPage<ChooseXafTypePage<DashboardDataSourceModel>, ChooseXafTypePage<DashboardDataSourceModel>>();
			customization.RegisterPageView<IChooseXafTypePageView, DefaultXafChooseTypePageView>();
		}
	}
}
