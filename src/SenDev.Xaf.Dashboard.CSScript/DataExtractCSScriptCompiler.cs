using CSScriptLib;
using SenDev.Xaf.Dashboards.Scripting;

namespace SenDev.Xaf.Dashboard.CSScriptCompiler
{
	public class DataExtractCSScriptCompiler : IScriptCompiler
	{
		public void AddReferences(IEnumerable<string> referencedAssembliesPaths) => throw new NotImplementedException();
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
