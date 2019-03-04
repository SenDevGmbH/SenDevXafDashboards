using CSScriptLibrary;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
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
            var data = GetDataCore(out var objectSpace);
            return new ScriptResultList(data, objectSpace.TypesInfo, OnlySerializableTypes);

        }

        private object GetDataCore(out XPObjectSpace objectSpace)
        {
            objectSpace = (XPObjectSpace)Application.CreateObjectSpace();
            var context = new ScriptContext(objectSpace);
            dynamic scriptObject = CSScript.LoadCode(Script).CreateObject("*");
            return scriptObject.GetData(context);
        }

        public object GetDataForDataExtract()
        {
            return new DashboardDataList(() =>
                {
                    var queryable = (IQueryable)GetDataCore(out var objectSpace);
                    return (queryable, objectSpace);
                }, 1);
        }

    }


}

