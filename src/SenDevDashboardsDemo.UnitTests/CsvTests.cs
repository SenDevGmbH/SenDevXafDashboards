using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SenDev.DashboardsDemo.Module.Covid;
using Xunit;

namespace SenDevDashboardsDemo.UnitTests
{
    public class CsvReaderTests
    {
		[Fact]
		public void CsvMissingFieldsTest()
		{
			var stream1 = GetCsvStream("01-22-2020.csv");
			Assert.NotNull(stream1);
			var records1 = CovidDataManager.GetRecords(new DateTime(2020, 1, 22), stream1);

			var stream2 = GetCsvStream("03-20-2020.csv");
			Assert.NotNull(stream2);
			var records2 = CovidDataManager.GetRecords(new DateTime(2020, 3, 20), stream1);

		}

		[Fact]
		public void CsvNewFormatTest()
		{
			var stream1 = GetCsvStream("03-22-2020.csv");
			Assert.NotNull(stream1);
			var records1 = CovidDataManager.GetRecords(new DateTime(2020, 3, 22), stream1);


		}

		private Stream GetCsvStream(string name)
		{
			var assembly = GetType().Assembly;
			string resourceName = assembly.GetName().Name + ".TestData." + name;
			return assembly.GetManifestResourceStream(resourceName);
		}
    }
}
