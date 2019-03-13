using DevExpress.DashboardCommon;
using DevExpress.DashboardCommon.DataSourceWizard;

namespace SenDev.Xaf.Dashboards.Win.DataSourceWizard.DataExtractPages
{

	public class ChoiceDataExtractPage<TModel> : DataSourceWizardPageBase<TModel> where TModel : DashboardDataSourceModel
	{

		public ChoiceDataExtractPage(IChoiceDataExctractPageView view) : base(view)
		{
		}
		public override void Begin()
		{
			base.Begin();
			View.WizardParameters.DataExtract = DataExtractHelper.GetDataExtract(View.ParamsObjectSpace, Model);
		}


		public override void Commit()
		{
			var dataExtract = View.WizardParameters.DataExtract;
			IExtractDataSourceModel model = Model;
			model.FileName = dataExtract?.Oid.ToString();
		}
		public override bool FinishEnabled => View.WizardParameters.DataExtract != null;
		public override bool MoveNextEnabled => false;
	}
}
