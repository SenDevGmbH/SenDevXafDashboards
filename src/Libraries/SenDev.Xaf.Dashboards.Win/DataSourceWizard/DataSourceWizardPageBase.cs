using System.Linq;
using DevExpress.DashboardCommon;
using DevExpress.Data.WizardFramework;
using SenDev.Xaf.Dashboards.Win.DataSourceWizard.ScriptingPages;

namespace SenDev.Xaf.Dashboards.Win.DataSourceWizard
{
	public abstract class DataSourceWizardPageBase<TModel> : WizardPageBase<IEnterScriptPageView, TModel> where TModel : DashboardDataSourceModel
	{
		protected DataSourceWizardPageBase(IEnterScriptPageView view) : base(view)
		{
		}

		protected T GetConstructorParameterValueByName<T>(string parameterName) => (T)Model?.CtorParameters?.Single(p => p.Name == parameterName)?.Value;

		public override void Begin()
		{
			View.WizardParameters.PropertyChanged += (s, e) => RaiseChanged();
		}
	}
}
