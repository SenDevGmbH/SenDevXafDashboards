using System.Linq;
using DevExpress.DashboardWin.DataSourceWizard;
using DevExpress.DashboardWin.ServiceModel;
using DevExpress.DataAccess.Wizard.Services;
using DevExpress.ExpressApp.Dashboards.Win;
using DevExpress.ExpressApp.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using SenDev.Xaf.Dashboards.Win.BusinessObjects;

namespace SenDev.Xaf.Dashboards.Win.DataSourceWizard
{

	public class XafDashboardChooseDataSourceTypePageView : DashboardChooseDataSourceTypePageView
	{
		public XafDashboardChooseDataSourceTypePageView(ISupportedDataSourceTypesService dataSourceTypesService, DataSourceTypes dataSourceTypes)
		  : base(dataSourceTypesService, dataSourceTypes)
		{
			//TODO: Localize
			if (Controls.Find("dataSourceTypesListBox", true).FirstOrDefault() is ListBoxControl listbox)
			{
				listbox.Items.Add(new CheckedListBoxItem
				{
					Value = XAFDataSourceType.XAF,
					Description = CaptionHelper.GetLocalizedText("Captions", "XafObjectDataSource"),
					Tag = CaptionHelper.GetLocalizedText("Captions", "ObjectDataSourceWithXafPersistentTypes")
				});

				listbox.Items.Add(new CheckedListBoxItem
				{
					Value = SenDevDataSourceType.Script,
					Description = "C# Script data source",  //CaptionHelper.GetLocalizedText("Captions", "XafObjectDataSource"),
					Tag = "C#" //CaptionHelper.GetLocalizedText("Captions", "ObjectDataSourceWithXafPersistentTypes")
				});


				listbox.Items.Add(new CheckedListBoxItem
				{
					Value = SenDevDataSourceType.XafDataExtract,
					Description = "XAF Data Extract",  //CaptionHelper.GetLocalizedText("Captions", "XafObjectDataSource"),
					Tag = "Preprocessed data extract" //CaptionHelper.GetLocalizedText("Captions", "ObjectDataSourceWithXafPersistentTypes")
				});

			}
		}
	}
}
