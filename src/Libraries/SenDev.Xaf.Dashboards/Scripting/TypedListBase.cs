using DevExpress.Data.Helpers;
using DevExpress.DataProcessing.ExtractStorage;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SenDev.Xaf.Dashboards.Scripting
{

    public abstract class TypedListBase : ITypedList
    {
        private PropertyDescriptorCollection properties;

        public TypedListBase()
        {
        }

        protected abstract object ScriptResult { get; }

        protected abstract ITypesInfo TypesInfo { get; }


        private PropertyDescriptorCollection Properties
        {
            get
            {
                if (properties == null)
                {
                    if (ScriptResult is IEnumerable enumerable)
                    {
                        var type = GenericTypeHelper.GetGenericIListTypeArgument(ScriptResult.GetType());

                        properties = GetProperties(type);
                    }
                    else
                        properties = PropertyDescriptorCollection.Empty;
                }

                return properties;
            }
        }

        public string GetListName(PropertyDescriptor[] listAccessors) => string.Empty;

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            if (listAccessors == null || listAccessors.Length == 0)
                return Properties;
            else
                return GetProperties(listAccessors.Last().PropertyType);
        }

        private PropertyDescriptorCollection GetProperties(Type type)
        {
            var typeInfo = TypesInfo.FindTypeInfo(type);
            if (typeInfo != null)
            {

                var collection = new XafPropertyDescriptorCollection(typeInfo);
				var descriptors = new List<PropertyDescriptor>();
				foreach (var memberInfo in typeInfo.Members.Where(m => m.IsPersistent))
				{
					
					var descriptor = collection.CreatePropertyDescriptor(memberInfo, memberInfo.Name);
					if (ValuesSerializer.IsSupportedType(memberInfo.MemberType))
					{
						descriptors.Add(descriptor);
					}
					else
					{
						descriptors.Add(new DisplayTextPropertyDescriptor(descriptor));
					}
				}

                return new PropertyDescriptorCollection(descriptors.ToArray());
            }
            else
                return TypeDescriptor.GetProperties(type);
        }
    }
}
