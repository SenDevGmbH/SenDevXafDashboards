using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using DevExpress.ExpressApp;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public static class Extendions
	{
		public static IScriptCompiler CreateCompiler(this XafApplication application, string[] referencedAssembliesPaths = null)
		{
			var module = application.Modules.FindModule<SenDevDashboardsModule>() ?? throw new InvalidOperationException($"{nameof(SenDevDashboardsModule)} is not installed.");
			var scriptCompilerType = module.ScriptCompilerType ?? throw new ArgumentException($"{nameof(SenDevDashboardsModule.ScriptCompilerType)} is null.");
			var compiler = (IScriptCompiler)Activator.CreateInstance(scriptCompilerType);
			compiler.AddReferences(referencedAssembliesPaths);

			return compiler;
		}
	}
}
