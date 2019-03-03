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

	public abstract class TypedListBase : ITypedList
	{
		private readonly PropertyDescriptorCollection properties;

		public TypedListBase(object scriptResult, ITypesInfo typesInfo, bool onlySerializableProperties)
		{
			TypesInfo = typesInfo;
			OnlySerializableProperties = onlySerializableProperties;
			if (scriptResult is IEnumerable enumerable)
			{
				var type = GenericTypeHelper.GetGenericIListTypeArgument(scriptResult.GetType());

				properties = GetProperties(type);
			}
		}

		private ITypesInfo TypesInfo
		{
			get;
		}

		public bool OnlySerializableProperties
		{
			get;
		}
		public string GetListName(PropertyDescriptor[] listAccessors) => string.Empty;

		public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			if (listAccessors == null || listAccessors.Length == 0)
				return properties;
			else
				return GetProperties(listAccessors.Last().PropertyType);
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
	}
}
