using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public sealed class DataReaderList : IList, ITypedList
	{
		private readonly object syncRoot = new object();
		private List<object[]> rows;
		public DataReaderList(IDataReader dataReader)
		{
			DataReader = dataReader;
		}

		private void EnsureRows()
		{
			if (rows == null)
			{
				rows = new List<object[]>();
				while (DataReader.Read())
				{
					object[] values = new object[DataReader.FieldCount];
					DataReader.GetValues(values);
					rows.Add(values);
				}
			}
		}

		private IList<object[]> Rows
		{
			get
			{
				EnsureRows();
				return rows;
			}
		}
		public object this[int index]
		{
			get => Rows[index];

			set => throw new NotSupportedException();
		}


		private IDataReader DataReader
		{
			get;
		}

		bool IList.IsReadOnly => true;

		bool IList.IsFixedSize => false;

		public int Count
		{
			get;
			private set;
		}

		object ICollection.SyncRoot => syncRoot;

		bool ICollection.IsSynchronized => false;

		public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			var descriptors = new List<DataReaderPropertyDescriptor>();

			for (int i = 0; i < DataReader.FieldCount; i++)
				descriptors.Add(new DataReaderPropertyDescriptor(DataReader, i));

			return new PropertyDescriptorCollection(descriptors.ToArray());
		}

		public string GetListName(PropertyDescriptor[] listAccessors) => null;

		int IList.Add(object value) => throw new NotSupportedException();

		void IList.Clear() => throw new NotSupportedException();

		bool IList.Contains(object value) => throw new NotSupportedException();

		void ICollection.CopyTo(Array array, int index) => throw new NotSupportedException();

		IEnumerator IEnumerable.GetEnumerator() => Rows.GetEnumerator();

		int IList.IndexOf(object value)
		{
			throw new NotImplementedException();
		}

		void IList.Insert(int index, object value)
		{
			throw new NotImplementedException();
		}

		void IList.Remove(object value)
		{
			throw new NotImplementedException();
		}

		void IList.RemoveAt(int index) => throw new NotSupportedException();


		
	}
}
