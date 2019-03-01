using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SenDev.Xaf.Dashboards.Scripting
{

	public class DashboardDataList<TQuery, TElement> : IList<TElement>, IList
	{

		private TElement[] buffer;
		private int bufferStartIndex = -1;




		protected DashboardDataList(IQueryable<TQuery> queryable, int elementsPerSourceRow)
		{
			Queryable = queryable;
			ElementsPerSourceRow = elementsPerSourceRow;
		}
		public DashboardDataList(IQueryable<TQuery> queryable, Func<TQuery, TElement> conversionFunc) : this(queryable, 1)
		{
			ConvertionFunc = conversionFunc;
		}


		private Func<TQuery, TElement> ConvertionFunc
		{
			get;
		}


		public int SourceRowsBufferSize { get; set; } = 10000;

		public int ElementsPerSourceRow
		{
			get;
		}
		public int BufferSize => SourceRowsBufferSize * ElementsPerSourceRow;

		private int CalculateBufferStartIndex(int index) => index / BufferSize * BufferSize;

		internal TElement GetElement(int index)
		{
			var neededBufferStartIndex = CalculateBufferStartIndex(index);
			if (neededBufferStartIndex != bufferStartIndex)
			{
				buffer = Queryable.Skip(neededBufferStartIndex / ElementsPerSourceRow).Take(SourceRowsBufferSize).SelectMany(ConvertElements).ToArray();
				System.Diagnostics.Debug.WriteLine("Loaded buffer for index={0}", neededBufferStartIndex);
				bufferStartIndex = neededBufferStartIndex;
			}

			return buffer[index - bufferStartIndex];
		}

		protected virtual IEnumerable<TElement> ConvertElements(TQuery source) => new[] { ConvertionFunc(source) };

		private IQueryable<TQuery> Queryable
		{
			get;
		}


		#region IList<T> implementation
		public TElement this[int index] { get => GetElement(index); set => throw new NotSupportedException(); }

		private int? count;
		public int Count => (count ?? (count = Queryable.Count() * ElementsPerSourceRow)) ?? 0;

		public bool IsReadOnly => true;

		public bool IsFixedSize => throw new NotImplementedException();

		public object SyncRoot => throw new NotImplementedException();

		public bool IsSynchronized => throw new NotImplementedException();


		object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public void Add(TElement item) => throw new NotSupportedException();

		public void Clear() => throw new NotSupportedException();

		public bool Contains(TElement item) => throw new NotImplementedException();

		public void CopyTo(TElement[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public IEnumerator<TElement> GetEnumerator() => new DashboardDataListEnumerator<TQuery, TElement>(this);

		public int IndexOf(TElement item)
		{
			throw new NotImplementedException();
		}

		public void Insert(int index, TElement item)
		{
			throw new NotImplementedException();
		}

		public bool Remove(TElement item)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}



		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		#endregion

		#region IList implementation
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
