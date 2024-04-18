using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using DevExpress.Data.Filtering;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public class ScriptCompiler : IScriptCompiler
	{

		private static readonly ConcurrentDictionary<string, Assembly> scriptAssemblyCache = new ConcurrentDictionary<string, Assembly>();

		private static readonly IList<string> namespaces = new List<string>() { "System" };

		private static readonly CSharpCompilationOptions compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
			.WithUsings(namespaces);

		private readonly IList<MetadataReference> references = new List<MetadataReference>()
		{
			MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(GCSettings).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(CriteriaOperator).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(IDataReader).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(BitArray).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(Queryable).Assembly.Location)
		};

		public void AddReferences(IEnumerable<string> referencedAssembliesPaths)
		{
			if (referencedAssembliesPaths == null)
				return;

			foreach (var assembly in referencedAssembliesPaths)
			{
				references.Add(MetadataReference.CreateFromFile(assembly));
			}
		}
		public EmitResult Compile(string script, string outputPath)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(script);
			var compilation = CSharpCompilation.Create(Path.GetFileName(outputPath), new SyntaxTree[] { syntaxTree }, references, compilationOptions);
			return compilation.Emit(outputPath);
		}


		private Assembly GetScriptAssembly(string script)
		{
			if (scriptAssemblyCache.TryGetValue(script, out var cachedAssembly))
				return cachedAssembly;

			var assemblyFilePath = Path.GetTempFileName();

			var compileResult = Compile(script, assemblyFilePath);

			if (compileResult.Success)
			{
				var assembly = Assembly.Load(File.ReadAllBytes(assemblyFilePath));
				File.Delete(assemblyFilePath);
				scriptAssemblyCache[script] = assembly;
				return assembly;
			}

			var errors = compileResult.Diagnostics
				.Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
				.Select(diagnostic => diagnostic.ToString());

			throw new InvalidOperationException("Compilation failed:\n" + string.Join("\n", errors));
		}

		public dynamic CreateObject(string script)
		{
			var assembly = GetScriptAssembly(script);
			return assembly.CreateInstance(assembly.GetExportedTypes().Single().FullName);
		}
	}

}

