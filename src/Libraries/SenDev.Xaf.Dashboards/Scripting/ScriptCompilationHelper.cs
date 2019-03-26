using DevExpress.Data.Filtering;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public class ScriptCompilationHelper
	{

		private static readonly ConcurrentDictionary<string, Assembly> scriptAssemblyCache = new ConcurrentDictionary<string, Assembly>();
		public ScriptCompilationHelper(string[] referencedAssemblies)
		{
			ReferencedAssemblies = referencedAssemblies;
		}

		private string[] ReferencedAssemblies
		{
			get;
		}

		public dynamic CreateObject(string script)
		{
			var assembly = GetScriptAssembly(script);
			return assembly.CreateInstance(assembly.GetExportedTypes().Single().FullName);
		}


		private Assembly GetScriptAssembly(string script)
		{
			if (scriptAssemblyCache.TryGetValue(script, out var cachedAssembly))
				return cachedAssembly;
			var codeProvider = new CSharpCodeProvider();
			var options = new CompilerParameters();
			var assemblyFilePath = Path.GetTempFileName();
			options.OutputAssembly = assemblyFilePath;
			options.ReferencedAssemblies.Add(typeof(System.Linq.Enumerable).Assembly.Location);
			options.ReferencedAssemblies.Add(typeof(CriteriaOperator).Assembly.Location);
			options.ReferencedAssemblies.Add(typeof(IDataReader).Assembly.Location);
			options.ReferencedAssemblies.AddRange(ReferencedAssemblies);
			var compileResult = codeProvider.CompileAssemblyFromSource(options, script);

			if (!compileResult.Errors.HasErrors)
			{
				var assembly = Assembly.Load(File.ReadAllBytes(compileResult.PathToAssembly));
				File.Delete(compileResult.PathToAssembly);
				scriptAssemblyCache[script] = assembly;
				return assembly;
			}

			throw new InvalidOperationException("Compilation failed:\r\n" + string.Join(Environment.NewLine, compileResult.Errors.OfType<CompilerError>().Select(e => e.ToString())));
		}
	}

}

