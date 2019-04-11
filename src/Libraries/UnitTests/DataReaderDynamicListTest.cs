using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SenDev.Xaf.Dashboards.Scripting;
using Xunit;

namespace UnitTests
{
	public class DataReaderDynamicListTests
	{
		[Fact]
		public void BasicReaderTest()
		{
			DataTable table = new DataTable();
			table.Columns.Add("IntColumn", typeof(int));
			table.Columns.Add("StringColumn", typeof(string));
			table.Rows.Add(1, "Row 1");
			table.Rows.Add(2, "Row 2");
			table.Rows.Add(3, "Row 3");
			var reader = table.CreateDataReader();

			DataReaderDynamicList list = new DataReaderDynamicList(reader);
			var properties = list.GetItemProperties(null);
			Assert.Equal(2, properties.Count);
			Assert.Equal("IntColumn", properties[0].Name);
			Assert.Equal(typeof(int), properties[0].PropertyType);

			Assert.Equal(1, properties[0].GetValue(list[0]));
			Assert.Equal(2, properties[0].GetValue(list[1]));
			Assert.Equal(3, properties[0].GetValue(list[2]));

		}

		[Fact]
		public void ForEachTest()
		{
			DataTable table = new DataTable();
			table.Columns.Add("IntColumn", typeof(int));
			table.Columns.Add("StringColumn", typeof(string));
			table.Rows.Add(1, "Row 1");
			table.Rows.Add(2, "Row 2");
			table.Rows.Add(3, "Row 3");
			var reader = table.CreateDataReader();

			DataReaderDynamicList dynamicList = new DataReaderDynamicList(reader);
			var properties = dynamicList.GetItemProperties(null);
			List<object> list = new List<object>();
			foreach (var item in dynamicList)
			{
				list.Add(item);
			}
			Assert.Equal(3, list.Count);

		}
	}
}
