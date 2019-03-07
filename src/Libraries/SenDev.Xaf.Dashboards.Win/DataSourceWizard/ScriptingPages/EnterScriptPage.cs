using System;
using DevExpress.DashboardCommon;
using DevExpress.Entity.ProjectModel;
using SenDev.Xaf.Dashboards.Scripting;

namespace SenDev.Xaf.Dashboards.Win.DataSourceWizard.ScriptingPages
{

	public class EnterScriptPage<TModel> : DataSourceWizardPageBase<TModel> where TModel : DashboardDataSourceModel
	{
		public EnterScriptPage(IEnterScriptPageView view) : base(view)
		{
		}
		public override void Begin()
		{
			View.WizardParameters.Script = GetConstructorParameterValueByName<string>("script");
		}
		public override void Commit()
		{
			Type type = typeof(ScriptDataSource);
			Model.Assembly = new DXAssemblyInfo(type.Assembly, false, true, null);
			Model.ObjectType = new DXTypeInfo(type);
			Model.ObjectConstructor = type.GetConstructor(new[] { typeof(string) });
			Model.CtorParameters = new[] { new DevExpress.DataAccess.ObjectBinding.Parameter("script", typeof(string), View.WizardParameters.Script) };
		}
		public override bool FinishEnabled => !string.IsNullOrWhiteSpace(View.WizardParameters.Script);
		public override bool MoveNextEnabled => false;
	}
}
