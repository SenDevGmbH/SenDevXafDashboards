using System;
using DevExpress.DashboardCommon;
using DevExpress.DashboardCommon.DataSourceWizard;
using DevExpress.DataAccess.Wizard;
using DevExpress.DataAccess.Wizard.Model;
using DevExpress.DataAccess.Wizard.Services;
using DevExpress.DataAccess.Wizard.Views;
using DevExpress.Entity.ProjectModel;
using DevExpress.ExpressApp.Dashboards.Win;
using SenDev.Xaf.Dashboards.Win.BusinessObjects;
using SenDev.Xaf.Dashboards.Win.DataSourceWizard.DataExtractPages;
using SenDev.Xaf.Dashboards.Win.DataSourceWizard.ScriptingPages;

namespace SenDev.Xaf.Dashboards.Win.DataSourceWizard
{

	public class XafDashboardChooseDataSourceTypePage<TModel> : DashboardChooseDataSourceTypePage<TModel> where TModel : class, IDashboardDataSourceModel
	{
		public XafDashboardChooseDataSourceTypePage(IChooseDataSourceTypePageView view, IWizardRunnerContext context, IConnectionStorageService connectionStorageService, IJsonConnectionStorageService jsonConnectionStorageService, ISolutionTypesProvider solutionTypesProvider, SqlWizardOptions options) 
			: base(view, context, connectionStorageService, jsonConnectionStorageService, solutionTypesProvider, options)
		{
		}

		public override void Commit()
		{
			base.Commit();
			if (IsXafDataSource())
			{
				Model.DataSourceType = DataSourceType.Object;
			}

			if (View.DataSourceType == SenDevDataSourceType.XafDataExtract)
				Model.DataSourceType = DashboardDataSourceType.Extract;
		}
		public override Type GetNextPageType()
		{
			if (View.DataSourceType == XAFDataSourceType.XAF)
				return typeof(ChooseXafTypePage<DashboardDataSourceModel>);
			if (View.DataSourceType == SenDevDataSourceType.Script)
				return typeof(EnterScriptPage<DashboardDataSourceModel>);
			if (View.DataSourceType == SenDevDataSourceType.XafDataExtract)
				return typeof(ChoiceDataExtractPage<DashboardDataSourceModel>);
			return base.GetNextPageType();
		}

		private bool IsXafDataSource()
		{
			return View.DataSourceType == XAFDataSourceType.XAF || View.DataSourceType == SenDevDataSourceType.Script;
		}
	}
}
