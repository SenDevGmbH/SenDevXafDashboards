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

	public class ScriptResultList : IList, ITypedList
	{

		private IList resultList;
		private readonly PropertyDescriptorCollection properties;

		public ScriptResultList(object scriptResult, ITypesInfo typesInfo, bool onlySerializableProperties)
		{
			TypesInfo = typesInfo ?? throw new ArgumentNullException(nameof(typesInfo));
			OnlySerializableProperties = onlySerializableProperties;
			if (scriptResult is IEnumerable enumerable)
			{
				var type = GenericTypeHelper.GetGenericIListTypeArgument(scriptResult.GetType());

				properties = GetProperties(type);
				resultList = enumerable.Cast<object>().ToList();
			}

		}

		private PropertyDescriptorCollection GetProperties(Type type)
		{
			var typeInfo = TypesInfo.FindTypeInfo(type);
			if (typeInfo != null)
			{

				XafPropertyDescriptorCollection collection = new XafPropertyDescriptorCollection(typeInfo);
				foreach (var memberInfo in typeInfo.Members.Where(m => !OnlySerializableProperties || ValuesSerializer.IsSupportedType(m.MemberType)))
					collection.CreatePropertyDescriptor(memberInfo, memberInfo.Name);

				return collection;
			}
			else
				return TypeDescriptor.GetProperties(type);
		}
		public bool IsReadOnly => resultList.IsReadOnly;

		public bool IsFixedSize => resultList.IsFixedSize;

		public int Count => resultList.Count;

		public object SyncRoot => resultList.SyncRoot;

		public bool IsSynchronized => resultList.IsSynchronized;

		private ITypesInfo TypesInfo
		{
			get;
		}
		public bool OnlySerializableProperties
		{
			get;
		}

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

		public string GetListName(PropertyDescriptor[] listAccessors)
		{
			return string.Empty;
		}

		public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			if (listAccessors == null || listAccessors.Length == 0)
				return properties;
			else
				return GetProperties(listAccessors.Last().PropertyType);
		}
	}
}
