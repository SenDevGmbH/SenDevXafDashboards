using System.IO;
using DevExpress.ExpressApp;
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
			var isCompiled = Compiler.Compile(script, tempFile).Success;
			File.Delete(tempFile);

			if (!isCompiled)
			{
				ObjectSpace.RemoveFromModifiedObjects(e.Object);
			}
		}
	}
}
