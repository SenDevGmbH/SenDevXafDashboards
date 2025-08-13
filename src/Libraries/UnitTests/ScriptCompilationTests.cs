using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.Scripting;
using Xunit;

namespace UnitTests
{
	public class ScriptCompilationTests : XpoTestBase
	{

		[Fact]
		public void DataExtractCompilationTest()
		{
			using (var application = XpoInMemoryXafApplication.CreateInstance())
			using (var objectSpace = application.CreateObjectSpace())
			{
				var extract = objectSpace.CreateObject<DashboardDataExtract>();
				var testObject = objectSpace.CreateObject<TestClassWithNameAndNumber>();
				testObject.Name = "Name 1";
				testObject.SequentialNumber = 1;
				extract.Script = TemplateHelper.GetScriptTemplate(testObject.GetType());
				objectSpace.CommitChanges();
				var dataManager = new DataExtractDataManager(application);
				dataManager.UpdateDataExtractByKey(extract.Oid);
			}
		}

		[Fact]
		public void DataExtractCollectionDataTest()
		{
			using (var application = XpoInMemoryXafApplication.CreateInstance())
			using (var objectSpace = application.CreateObjectSpace())
			{
				var extract = objectSpace.CreateObject<DashboardDataExtract>();
				var testObject = objectSpace.CreateObject<TestClassWithNameAndNumber>();
				testObject.Name = "Name 1";
				testObject.SequentialNumber = 1;
				extract.Script = TemplateHelper.GetScriptTemplate(testObject.GetType(), GetType().Assembly.GetManifestResourceStream("UnitTests.ScriptTemplate.template"));
				objectSpace.CommitChanges();
				var dataManager = new DataExtractDataManager(application);
				dataManager.UpdateDataExtractByKey(extract.Oid);
			}
		}

		[Fact]
		public void DataExtractResultTest()
		{
			using (var application = XpoInMemoryXafApplication.CreateInstance())
			using (var objectSpace = application.CreateObjectSpace())
			{
				var extract = objectSpace.CreateObject<DashboardDataExtract>();
				var testObject = objectSpace.CreateObject<TestClassWithNameAndNumber>();
				testObject.Name = "Name 1";
				testObject.SequentialNumber = 1;
				extract.Script = @"using System;
					public class Script
					{
						public object GetData(SenDev.Xaf.Dashboards.Scripting.ScriptContext context)
						{
							return new byte[] {0,1,2,3,4,5,6,7,8,9};
						}
					}";

				objectSpace.CommitChanges();
				var dataManager = new DataExtractDataManager(application);
				dataManager.UpdateDataExtractByKey(extract.Oid);
				extract.Reload();
				Assert.Equal(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, extract.ExtractData);
			}
		}

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
				Assert.Equal(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9}, extract.ExtractData);
			}
		}

		[Fact]
		public void CompileExceptionTest()
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
	
						private byte[] LoadBytes => bytes ??= new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

						public object GetData(SenDev.Xaf.Dashboards.Scripting.ScriptContext context) => LoadBytes;
					}";

				objectSpace.CommitChanges();
				var dataManager = new DataExtractDataManager(application);

				var exception = Assert.Throws<InvalidOperationException>(() => dataManager.UpdateDataExtractByKey(extract.Oid));
				Assert.StartsWith("Compilation failed:\n", exception.Message);
			}
		}




	}
}
