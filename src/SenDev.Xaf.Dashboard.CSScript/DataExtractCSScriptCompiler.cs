using CSScriptLib;
using SenDev.Xaf.Dashboards.Scripting;

namespace SenDev.Xaf.Dashboard.CSScriptCompiler
{
	public class DataExtractCSScriptCompiler : IScriptCompiler
	{
		/// <summary>
		/// Adding references is not supported yet by this class
		/// </summary>
		/// <param name="referencedAssembliesPaths"></param>
		public void AddReferences(IEnumerable<string> referencedAssembliesPaths)
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
