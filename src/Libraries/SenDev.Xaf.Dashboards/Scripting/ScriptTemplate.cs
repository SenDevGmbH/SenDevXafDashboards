using System;
using System.Linq;
using DevExpress.Xpo;
using {0}
using SenDev.Xaf.Dashboards.Scripting;


public class Script
{
    public object GetData(ScriptContext context)
    {
        return context.Query<{1}>().Take(1000);
    }
}

