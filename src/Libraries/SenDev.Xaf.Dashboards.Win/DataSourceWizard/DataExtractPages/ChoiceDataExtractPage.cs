using System;
using DevExpress.DashboardCommon;
using DevExpress.DashboardCommon.DataSourceWizard;
using SenDev.Xaf.Dashboards.BusinessObjects;

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
			if (Model.FileName != null)
				View.WizardParameters.DataExtract = View.ObjectSpace.GetObjectByKey<DashboardDataExtract>(Guid.Parse(Model.FileName));
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
