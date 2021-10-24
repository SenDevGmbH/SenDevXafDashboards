using DevExpress.Data.Linq.Helpers;
using DevExpress.DataProcessing.ExtractStorage;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;

namespace SenDev.Xaf.Dashboards.Scripting
{

    public class DashboardDataList : TypedListBase, IList
    {

        private object[] buffer;
        private int bufferStartIndex = -1;
        private IObjectSpace objectSpace;
        private int queriesCount;

        public DashboardDataList(Func<(IQueryable queryable, IObjectSpace objectSpace)> queryableFunc, int elementsPerSourceRow) 
        {
            ElementsPerSourceRow = elementsPerSourceRow;
            QueryableFunc = queryableFunc;
        }

        protected override object ScriptResult => Queryable;

        protected override ITypesInfo TypesInfo => ObjectSpace.TypesInfo;
        public int SourceRowsBufferSize { get; set; } = 100000;

        public int MaxQueriesPerObjectSpace { get; set; } = 5;

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
                if (queriesCount >= MaxQueriesPerObjectSpace)
                {
                    objectSpace?.Dispose();
                    objectSpace = null;
                    queryable = null;
                }

                buffer = Queryable.Skip(neededBufferStartIndex / ElementsPerSourceRow).Take(SourceRowsBufferSize).Cast<object>().ToArray();
                queriesCount++;
                bufferStartIndex = neededBufferStartIndex;
            }

            return buffer[index - bufferStartIndex];
        }


        private void EnsureQueryable()
        {
            if (queryable == null)
            {
                var result = QueryableFunc();
                objectSpace = result.objectSpace;
                queryable = result.queryable;
            }
        }

        private IObjectSpace ObjectSpace
        {
            get
            {
                EnsureQueryable();
                return objectSpace;
            }
        }
        private IQueryable queryable;
        private IQueryable Queryable
        {
            get
            {
                EnsureQueryable();
                return queryable;
            }
        }


		protected override PropertyDescriptor GetSupportedPropertyDescriptor(PropertyDescriptor descriptor)
		{
			return ValuesSerializer.IsSupportedType(descriptor.PropertyType) ? descriptor : new DisplayTextPropertyDescriptor(descriptor);
		}
		#region IList implementation

		private int? count;
        public int Count => (count ?? (count = Queryable.Count() * ElementsPerSourceRow)) ?? 0;

        public bool IsReadOnly => true;

        public bool IsFixedSize => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        public Func<(IQueryable queryable, IObjectSpace objectSpace)> QueryableFunc { get; }

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
