using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.Scripting;
using Xunit;

namespace UnitTests.NetCore
{
    public class CompilationTests
    {
		[Fact]
		public void CSharp8SyntaxTest()
		{
			using (var application = XpoInMemoryXafApplication.CreateInstance())
			using (var objectSpace = application.CreateObjectSpace())
			{
				var extract = objectSpace.CreateObject<DashboardDataExtract>();
				var testObject = objectSpace.CreateObject<TestClassWithNameAndNumber>();
				testObject.Name = "Name 1";
				testObject.SequentialNumber = 1;
				extract.Script = @"
					using System;
					using System.Linq;

					public class Script
					{
						private byte[] bytes = null;
	
						private byte[] LoadBytes => bytes ??= new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

						public object GetData(SenDev.Xaf.Dashboards.Scripting.ScriptContext context) => LoadBytes;
					}";

				objectSpace.CommitChanges();
				var dataManager = new DataExtractDataManager(application);
				dataManager.UpdateDataExtractByKey(extract.Oid);
				extract.Reload();
				Assert.Equal(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, extract.ExtractData);
			}
		}

		
		[Fact]
		public void QueryObjectsScriptTest()
		{
			var script = @"
using System;
using System.Linq;
using DevExpress.Xpo;	
using SenDev.Xaf.Dashboards.Scripting;
using UnitTests;		


public class Script
{
    public object GetData(ScriptContext context)
    {
        return context.Query<TestClassWithNameAndNumber>().Take(1000);
    }
}";


			using var application = XpoInMemoryXafApplication.CreateInstance();
			using var objectSpace = application.CreateObjectSpace();
			var extract = objectSpace.CreateObject<DashboardDataExtract>();
			var testObject = objectSpace.CreateObject<TestClassWithNameAndNumber>();
			testObject.Name = "Name 1";
			testObject.SequentialNumber = 1;
			extract.Script = script;
			objectSpace.CommitChanges();
		
			var dataManager = new DataExtractDataManager(application);
			dataManager.UpdateDataExtractByKey(extract.Oid);
			extract.Reload();
			Assert.NotEmpty(extract.ExtractData);

		}
	}
}
