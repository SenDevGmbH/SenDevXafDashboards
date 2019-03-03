using CSScriptLibrary;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public class ScriptDataSource
	{
		public ScriptDataSource(string script)
		{
			Script = script;
		}

		public XafApplication Application
		{
			get; set;
		}

		public bool OnlySerializableTypes
		{
			get; set;
		}

		public string Script
		{
			get;
		}

		public object GetData()
		{
			var objectSpace = (XPObjectSpace)Application.CreateObjectSpace();
			var context = new ScriptContext(objectSpace);
			dynamic scriptObject = CSScript.LoadCode(Script).CreateObject("*");
			return new ScriptResultList(scriptObject.GetData(context), objectSpace.TypesInfo, OnlySerializableTypes);

		}
	}
}
