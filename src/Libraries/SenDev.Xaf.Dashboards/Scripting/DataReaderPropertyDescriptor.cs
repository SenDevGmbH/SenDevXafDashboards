using System;
using System.ComponentModel;
using System.Data;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public class DataReaderPropertyDescriptor : PropertyDescriptor
	{
		public DataReaderPropertyDescriptor(IDataReader reader, int fieldIndex) : base(reader.GetName(fieldIndex), null)
		{
			PropertyType = reader.GetFieldType(fieldIndex);
			FieldIndex = fieldIndex;
		}

		public override Type ComponentType => typeof(object[]);

		public override bool IsReadOnly => true;

		public override Type PropertyType
		{
			get;
		}
		private int FieldIndex
		{
			get;
		}

		public override bool CanResetValue(object component) => false;
		public override object GetValue(object component) => ((object[])component)[FieldIndex];

		public override void ResetValue(object component) => throw new NotSupportedException();

		public override void SetValue(object component, object value) => throw new NotSupportedException();

		public override bool ShouldSerializeValue(object component) => false;
	}
}
