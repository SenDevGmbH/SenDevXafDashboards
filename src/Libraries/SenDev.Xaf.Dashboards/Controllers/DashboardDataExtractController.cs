using System.IO;
using System.Linq;
using DevExpress.ExpressApp;
using Microsoft.CodeAnalysis;
using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.Scripting;

namespace SenDev.Xaf.Dashboards.Controllers
{
	public class DashboardDataExtractController : ObjectViewController<ObjectView, IDashboardDataExtract>
	{

		protected override void OnActivated()
		{
			base.OnActivated();
			ObjectSpace.ObjectSaving += ObjectSaving;
		}

		protected override void OnDeactivated()
		{
			base.OnDeactivated();
			ObjectSpace.ObjectSaving -= ObjectSaving;
		}


		private void ObjectSaving(object sender, ObjectManipulatingEventArgs e)
		{
			var compilationHelper = new ScriptCompilationHelper(AssembliesHelper.GetReferencedAssembliesPaths(Application));
			var script = (e.Object as IDashboardDataExtract).Script;
			var tempFile = Path.GetTempFileName();
			var compileResult = compilationHelper.Compile(script, tempFile);
			File.Delete(tempFile);

			if (!compileResult.Success)
			{
				var errors = compileResult.Diagnostics
					.Where(o => o.Severity == DiagnosticSeverity.Error)
					.Select(o => o.ToString());

				throw new UserFriendlyException(string.Join("\n", errors));
			}
		}
	}
}
