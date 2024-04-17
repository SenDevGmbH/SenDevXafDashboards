using System;
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
			IScriptCompiler compilationHelper = Application.CreateCompiler(AssembliesHelper.GetReferencedAssembliesPaths(Application));
			var script = (e.Object as IDashboardDataExtract).Script;

			try
			{
				var compileResult = compilationHelper.CreateObject(script);
			}
			catch (InvalidOperationException ex)
			{
				throw new UserFriendlyException(ex.Message, ex);
			}
		}
	}
}
