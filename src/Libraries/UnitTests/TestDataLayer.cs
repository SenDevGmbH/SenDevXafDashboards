using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;

namespace UnitTests
{

	public class TestDataLayer : SimpleDataLayer
	{
		public TestDataLayer(IDataStore provider) : base(provider)
		{
		}

		public TestDataLayer(XPDictionary dictionary, IDataStore provider) : base(dictionary, provider)
		{
		}

		public bool DisableSelectedData
		{
			get; set;
		}

		public override SelectedData SelectData(params SelectStatement[] selects)
		{
			if (DisableSelectedData)
				throw new InvalidOperationException("SelectData is disabled");

			OnSelectDataExecuting(new SelectDataEventArgs(selects));
			return base.SelectData(selects);
		}

		public event EventHandler<SelectDataEventArgs> SelectDataExecuting;
		protected virtual void OnSelectDataExecuting(SelectDataEventArgs e) => SelectDataExecuting?.Invoke(this, e);
	}
}
