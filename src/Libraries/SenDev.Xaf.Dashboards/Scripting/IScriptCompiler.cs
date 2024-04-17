using System.Collections.Generic;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public interface IScriptCompiler
	{
		dynamic CreateObject(string script);
		void AddReferences(IEnumerable<string> referencedAssembliesPaths);	
	}

}

