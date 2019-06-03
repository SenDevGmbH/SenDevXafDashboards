using System.IO;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using SenDev.Xaf.Dashboards.BusinessObjects;

namespace SenDev.Xaf.Dashboards.Win.Controllers
{
	public class ExportDataExtractController : ObjectViewController<ListView, DashboardDataExtract>
	{
		public ExportDataExtractController()
		{
			ExportDataExtractAction = new SimpleAction(this, "ExportDataExtract", PredefinedCategory.Export);
			ExportDataExtractAction.Caption = "Export data extract";
			ExportDataExtractAction.ImageName = "Action_LocalizationExport";
			ExportDataExtractAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
			ExportDataExtractAction.Execute += ExportDataExtractAction_Execute;
			ExportDataExtractAction.TargetObjectsCriteria = $"Not IsNull({nameof(DashboardDataExtract.ExtractData)})";
		}


		private string GetFileName()
		{
			using (var dialog = new System.Windows.Forms.SaveFileDialog())
			{
				dialog.OverwritePrompt = true;
				if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					return dialog.FileName;
				}
			}

			return null;
		}
		private void ExportDataExtractAction_Execute(object sender, SimpleActionExecuteEventArgs e)
		{
			var fileName = GetFileName();
			if (!string.IsNullOrWhiteSpace(fileName))
			{
				File.WriteAllBytes(fileName, ViewCurrentObject.ExtractData);
			}
		}

		public SimpleAction ExportDataExtractAction
		{
			get;
		}

	}
}
