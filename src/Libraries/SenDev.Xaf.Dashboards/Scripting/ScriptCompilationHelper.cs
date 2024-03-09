using CSScriptLib;
using DevExpress.CodeParser;
using DevExpress.Data.Filtering;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public class ScriptCompilationHelper
	{


		public ScriptCompilationHelper()
		{

		}
		public ScriptCompilationHelper(string[] referenceAssemblies)
		{

		}



		public dynamic CreateObject(string script)
		{

			try
			{
				return CSScript.Evaluator.LoadCode(script);
			}
			catch (Exception ex)
			{

				throw new InvalidOperationException("Compilation failed:\n" + ex.Message, ex);
			}
		}





	}

}

