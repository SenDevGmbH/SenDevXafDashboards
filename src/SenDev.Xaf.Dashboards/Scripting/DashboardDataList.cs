using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.ExpressApp.DC;

namespace SenDev.Xaf.Dashboards.Scripting
{

	public class DashboardDataList : TypedListBase, IList
	{

		private object[] buffer;
		private int bufferStartIndex = -1;


		public DashboardDataList(IQueryable queryable, ITypesInfo typesInfo, int elementsPerSourceRow) : base(queryable, typesInfo, true)
		{
			Queryable = queryable;
			ElementsPerSourceRow = elementsPerSourceRow;
		}



		public int SourceRowsBufferSize { get; set; } = 10000;

		public int ElementsPerSourceRow
		{
			get;
		}
		public int BufferSize => SourceRowsBufferSize * ElementsPerSourceRow;

		private int CalculateBufferStartIndex(int index) => index / BufferSize * BufferSize;

		internal object GetElement(int index)
		{
			var neededBufferStartIndex = CalculateBufferStartIndex(index);
			if (neededBufferStartIndex != bufferStartIndex)
			{
				buffer = Queryable.Skip(neededBufferStartIndex / ElementsPerSourceRow).Take(SourceRowsBufferSize).Cast<object>().ToArray();
				System.Diagnostics.Debug.WriteLine("Loaded buffer for index={0}", neededBufferStartIndex);
				bufferStartIndex = neededBufferStartIndex;
			}

			return buffer[index - bufferStartIndex];
		}


		private IQueryable Queryable
		{
			get;
		}


		#region IList implementation

		private int? count;
		public int Count => (count ?? (count = Queryable.Count() * ElementsPerSourceRow)) ?? 0;

		public bool IsReadOnly => true;

		public bool IsFixedSize => throw new NotImplementedException();

		public object SyncRoot => throw new NotImplementedException();

		public bool IsSynchronized => throw new NotImplementedException();


		object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


		public void Clear() => throw new NotSupportedException();

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}



		IEnumerator IEnumerable.GetEnumerator() => new DashboardDataListEnumerator(this);

		public int Add(object value)
		{
			throw new NotImplementedException();
		}

		public bool Contains(object value)
		{
			throw new NotImplementedException();
		}

		public int IndexOf(object value)
		{
			throw new NotImplementedException();
		}

		public void Insert(int index, object value)
		{
			throw new NotImplementedException();
		}

		public void Remove(object value)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
