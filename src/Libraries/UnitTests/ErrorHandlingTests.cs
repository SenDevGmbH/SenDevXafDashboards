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
	public class ErrorHandlingTests
	{
		[Fact]
		public void LastErrorTest()
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
							throw new InvalidOperationException(""Test exception"");
						}
					}";

				extract.ExtractData = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
				objectSpace.CommitChanges();
				var dataManager = new DataExtractDataManager(application);
				Assert.Throws<InvalidOperationException>(() => dataManager.UpdateDataExtractByKey(extract.Oid));
				extract.Reload();
				Assert.Contains("Test exception", extract.LastError);
				Assert.Null(extract.ExtractData);	
			}
		}
		
		[Fact]
		public void LastErrorClearAfterSuccessTest()
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

				extract.LastError = "Test exception"; 
				objectSpace.CommitChanges();
				var dataManager = new DataExtractDataManager(application);
				dataManager.UpdateDataExtractByKey(extract.Oid);

				extract.Reload();
				Assert.Null(extract.LastError);
				Assert.Equal(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, extract.ExtractData);

			}
		}
	}
}
