using System.ComponentModel;
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


		public ScriptDashboardWizardParameters()
		{
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


		private DashboardDataExtract dataExtract;
		[ImmediatePostData]
		public DashboardDataExtract DataExtract
		{
			get => dataExtract;
			set => SetPropertyValue(nameof(DataExtract), ref dataExtract, value);
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
