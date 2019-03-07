using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.Data.Helpers;
using DevExpress.DataProcessing.ExtractStorage;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;

namespace SenDev.Xaf.Dashboards.Scripting
{

	public class ScriptResultList : TypedListBase, IList
	{

		private readonly IList resultList;
        private readonly object scriptResult;
        private readonly ITypesInfo typesInfo;
		public ScriptResultList(object scriptResult, ITypesInfo typesInfo) 
		{
            this.scriptResult = scriptResult;
            this.typesInfo = typesInfo;
			if (scriptResult is IEnumerable enumerable)
			{
				resultList = enumerable.Cast<object>().ToList();
			}

		}

        protected override ITypesInfo TypesInfo => typesInfo;
        protected override object ScriptResult => scriptResult;

        public bool IsReadOnly => resultList.IsReadOnly;

		public bool IsFixedSize => resultList.IsFixedSize;

		public int Count => resultList.Count;

		public object SyncRoot => resultList.SyncRoot;

		public bool IsSynchronized => resultList.IsSynchronized;



		public object this[int index] { get => resultList[index]; set => resultList[index] = value; }



		public int Add(object value)
		{
			return resultList.Add(value);
		}

		public bool Contains(object value)
		{
			return resultList.Contains(value);
		}

		public void Clear()
		{
			resultList.Clear();
		}

		public int IndexOf(object value)
		{
			return resultList.IndexOf(value);
		}

		public void Insert(int index, object value)
		{
			resultList.Insert(index, value);
		}

		public void Remove(object value)
		{
			resultList.Remove(value);
		}

		public void RemoveAt(int index)
		{
			resultList.RemoveAt(index);
		}

		public void CopyTo(Array array, int index)
		{
			resultList.CopyTo(array, index);
		}

		public IEnumerator GetEnumerator()
		{
			return resultList.GetEnumerator();
		}


	}
}
