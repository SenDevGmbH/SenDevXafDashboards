using CSScriptLibrary;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

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

        public string Script
        {
            get;
        }

        public object GetData(IDictionary<string, object> parameters)
        {
            var data = GetDataCore(parameters, out var objectSpace);
            return new ScriptResultList(data, objectSpace.TypesInfo);

        }

        private object GetDataCore(IDictionary<string, object> parameters, out XPObjectSpace objectSpace)
        {
            objectSpace = (XPObjectSpace)Application.CreateObjectSpace();
            var context = new ScriptContext(objectSpace, parameters);
            dynamic scriptObject = CSScript.LoadCode(Script).CreateObject("*");
            return scriptObject.GetData(context);
        }

        public object GetDataForDataExtract()
        {
            return new DashboardDataList(() =>
                {
                    var queryable = (IQueryable)GetDataCore(new Dictionary<string, object>(), out var objectSpace);
                    return (queryable, objectSpace);
                }, 1);
        }

    }


}

