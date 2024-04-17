using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
			using var application = XpoInMemoryXafApplication.CreateInstance();
			using var objectSpace = application.CreateObjectSpace();
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
        return context.Query<TestClassWithNameAndNumber>();
    }
}";


			using var application = XpoInMemoryXafApplication.CreateInstance();
			using var objectSpace = application.CreateObjectSpace();
			var extract = objectSpace.CreateObject<DashboardDataExtract>();
			var testObject1 = objectSpace.CreateObject<TestClassWithNameAndNumber>();
			testObject1.Name = "Name 1";
			testObject1.SequentialNumber = 1;
			testObject1.NullableNumber = 33;
			var testObject2 = objectSpace.CreateObject<TestClassWithNameAndNumber>();
			testObject1.Name = "Name 2";
			testObject2.SequentialNumber = 2;
			extract.Script = script;
			objectSpace.CommitChanges();

			var dataManager = new DataExtractDataManager(application);
			dataManager.UpdateDataExtractByKey(extract.Oid);
			extract.Reload();
			Assert.NotEmpty(extract.ExtractData);
			Assert.Equal(2, extract.RowCount);

		}


		[Fact]
		public void AsQueryableTest()
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
        return context.Query<TestClassWithNameAndNumber>().ToList().AsQueryable();
    }
}";


			using var application = XpoInMemoryXafApplication.CreateInstance();
			using var objectSpace = application.CreateObjectSpace();
			var extract = objectSpace.CreateObject<DashboardDataExtract>();
			var testObject1 = objectSpace.CreateObject<TestClassWithNameAndNumber>();
			testObject1.Name = "Name 1";
			testObject1.SequentialNumber = 1;
			testObject1.NullableNumber = 33;
			var testObject2 = objectSpace.CreateObject<TestClassWithNameAndNumber>();
			testObject1.Name = "Name 2";
			testObject2.SequentialNumber = 2;
			extract.Script = script;
			objectSpace.CommitChanges();

			var dataManager = new DataExtractDataManager(application);
			dataManager.UpdateDataExtractByKey(extract.Oid);
			extract.Reload();
			Assert.NotEmpty(extract.ExtractData);
			Assert.Equal(2, extract.RowCount);

		}


		[Fact]
		public void CancellationTest()
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
        for(int i = 0; true;i++)
		{
			if (i > 1000) context.CancellationToken.ThrowIfCancellationRequested();
		}
    }
}";


			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			using var application = XpoInMemoryXafApplication.CreateInstance();
			using var objectSpace = application.CreateObjectSpace();
			var extract = objectSpace.CreateObject<DashboardDataExtract>();
			var testObject1 = objectSpace.CreateObject<TestClassWithNameAndNumber>();
			testObject1.Name = "Name 1";
			testObject1.SequentialNumber = 1;

			var testObject2 = objectSpace.CreateObject<TestClassWithNameAndNumber>();
			testObject1.Name = "Name 2";
			testObject2.SequentialNumber = 2;
			extract.Script = script;
			objectSpace.CommitChanges();

			Assert.NotNull(application.TypesInfo);
			var task = Task.Run(() =>
			{
				Assert.NotNull(application.TypesInfo);
				var dataManager = new DataExtractDataManager(application);
				dataManager.UpdateDataExtractByKey(extract.Oid, cancellationTokenSource.Token);
			}, cancellationTokenSource.Token);

			cancellationTokenSource.Cancel();
			try
			{
				task.Wait();

			}
			catch (AggregateException aex)
			{
				Assert.IsType<TaskCanceledException>(aex.InnerExceptions.Single());
			}

		}

		[Fact]
		public void NullReturnTest()
		{
			using var application = XpoInMemoryXafApplication.CreateInstance();
			using var objectSpace = application.CreateObjectSpace();
			var extract = objectSpace.CreateObject<DashboardDataExtract>();
			extract.ExtractData = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var testObject = objectSpace.CreateObject<TestClassWithNameAndNumber>();
			testObject.Name = "Name 1";
			testObject.SequentialNumber = 1;
			extract.Script = @"
					using System;
					using System.Linq;

					public class Script
					{
						public object GetData(SenDev.Xaf.Dashboards.Scripting.ScriptContext context) => null;
					}";

			objectSpace.CommitChanges();
			var dataManager = new DataExtractDataManager(application);
			dataManager.UpdateDataExtractByKey(extract.Oid);
			extract.Reload();
			Assert.Null(extract.ExtractData);
		}

		[Fact]
		public void EmptyCollectionTest()
		{
			using var application = XpoInMemoryXafApplication.CreateInstance();
			using var objectSpace = application.CreateObjectSpace();
			var extract = objectSpace.CreateObject<DashboardDataExtract>();
			extract.ExtractData = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var testObject = objectSpace.CreateObject<TestClassWithNameAndNumber>();
			testObject.Name = "Name 1";
			testObject.SequentialNumber = 1;
			extract.Script = @"
					using System;
					using System.Linq;

					public class Script
					{
						public object GetData(SenDev.Xaf.Dashboards.Scripting.ScriptContext context) => new object[0];
					}";

			objectSpace.CommitChanges();
			var dataManager = new DataExtractDataManager(application);
			dataManager.UpdateDataExtractByKey(extract.Oid);
			extract.Reload();
			Assert.Null(extract.ExtractData);
		}


		[Fact]
		public void ScriptCompilationErrorTest()
		{
			using var application = XpoInMemoryXafApplication.CreateInstance();
			var compiler = application.CreateCompiler();
			Assert.Throws<InvalidOperationException>(() => compiler.CreateObject("aaa"));
		}
	}
}
