using DevExpress.Data.Helpers;
using DevExpress.DataProcessing.ExtractStorage;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;

namespace SenDev.Xaf.Dashboards.Scripting
{

	internal class DisplayTextPropertyDescriptor : PropertyDescriptor
	{
		public DisplayTextPropertyDescriptor(PropertyDescriptor originalDescriptor) : base(originalDescriptor.Name, null)
		{
			OriginalDescriptor = originalDescriptor;
		}

		public override Type ComponentType => typeof(object);

		public override bool IsReadOnly => true;

		public override Type PropertyType => typeof(string);

		private PropertyDescriptor OriginalDescriptor
		{
			get;
		}

		public override bool CanResetValue(object component) => false;

		public override object GetValue(object component)
		{
			var value = OriginalDescriptor.GetValue(component);
			return CaptionHelper.GetDisplayText(value);
		}

		public override void ResetValue(object component)
		{
			throw new NotImplementedException();
		}

		public override void SetValue(object component, object value)
		{
			throw new NotImplementedException();
		}

		public override bool ShouldSerializeValue(object component)
		{
			throw new NotImplementedException();
		}
	}
}
