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
	public sealed class DataReaderDynamicList : IList, ITypedList
	{
		private readonly object syncRoot = new object();
		private object[] currentRow;
		private object[] nextRow;
		public DataReaderDynamicList(IDataReader dataReader)
		{
			DataReader = dataReader;
		}

		private bool MoveToNextRow()
		{
			currentRow = nextRow;
			if (DataReader.Read())
			{
				nextRow = new object[DataReader.FieldCount];
				DataReader.GetValues(nextRow);
				Count++;
				return true;
			}

			return false;
		}
		public object this[int index]
		{
			get
			{
				if (currentRow == null)
					MoveToNextRow();

				if (index < 0)
					throw new IndexOutOfRangeException("Index cannot be negative");

				if (index == Count - 1)
				{
					MoveToNextRow();
					return currentRow;
				}

				if (index == Count - 2)
					return currentRow;

				throw new IndexOutOfRangeException("Unsupported index");
			}

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

		IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

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


		private class Enumerator : IEnumerator
		{
			public Enumerator(DataReaderDynamicList list)
			{
				List = list;
			}
			public object Current => List.currentRow;

			public DataReaderDynamicList List
			{
				get;
			}

			public bool MoveNext()
			{
				return List.MoveToNextRow();
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}
	}
}
