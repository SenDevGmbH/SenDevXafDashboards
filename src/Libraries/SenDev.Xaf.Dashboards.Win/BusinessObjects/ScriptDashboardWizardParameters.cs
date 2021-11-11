using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Win;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using SenDev.Xaf.Dashboards.BusinessObjects;

namespace SenDev.Xaf.Dashboards.Win.BusinessObjects
{

	[DomainComponent]
	public class ScriptDashboardWizardParameters : XafWizardParameters, INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;


		public ScriptDashboardWizardParameters(IObjectSpace objectSpace, Type extractType)
		{
			ObjectSpace = objectSpace;
			ExtractType = extractType;
		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private string script;

		[FieldSize(FieldSizeAttribute.Unlimited)]
		[ImmediatePostData]
		[EditorAlias(EditorAliases.CSCodePropertyEditor)]
		public string Script
		{
			get => script;
			set => SetPropertyValue(nameof(Script), ref script, value);
		}



		public IList<IDashboardDataExtract> DataExtractDataSource => ObjectSpace.GetObjects(ExtractType).OfType<IDashboardDataExtract>().ToList();

		private IDashboardDataExtract dataExtract;
		[ImmediatePostData]
		[DataSourceProperty(nameof(DataExtractDataSource))]
		[EditorAlias(DevExpress.ExpressApp.Editors.EditorAliases.LookupPropertyEditor)]
		public IDashboardDataExtract DataExtract
		{
			get => dataExtract;
			set => SetPropertyValue(nameof(DataExtract), ref dataExtract, value);
		}
		
		private IObjectSpace ObjectSpace
		{
			get;
		}
		private Type ExtractType
		{
			get;
		}

		protected bool SetPropertyValue<T>(string propertyName, ref T propertyHolder, T value)
		{
			if (!Equals(value, propertyHolder))
			{
				var oldValue = propertyHolder;
				propertyHolder = value;
				OnChanged(propertyName, oldValue, value);
				return true;
			}

			return false;
		}

		protected virtual void OnChanged(string propertyName, object oldValue, object newValue)
		{
			OnPropertyChanged(propertyName);
		}

	}
}
