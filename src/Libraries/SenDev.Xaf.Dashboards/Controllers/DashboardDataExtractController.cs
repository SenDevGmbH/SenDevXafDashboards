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
		public ScriptCompilationHelper Compiler { get; private set; }

		protected override void OnActivated()
		{
			base.OnActivated();
			Compiler = new ScriptCompilationHelper(AssembliesHelper.GetReferencedAssembliesPaths(Application));
			ObjectSpace.ObjectSaving += ObjectSaving;
		}

		protected override void OnDeactivated()
		{
			base.OnDeactivated();
			ObjectSpace.ObjectSaving -= ObjectSaving;
		}


		private void ObjectSaving(object sender, ObjectManipulatingEventArgs e)
		{
			var script = (e.Object as IDashboardDataExtract).Script;
			var tempFile = Path.GetTempFileName();
			var compileResult = Compiler.Compile(script, tempFile);
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
