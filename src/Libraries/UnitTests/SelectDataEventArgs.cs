using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;

namespace UnitTests
{

	public class SelectDataEventArgs : EventArgs
	{
		public SelectDataEventArgs(params SelectStatement[] selects) => Selects = selects;
		public SelectStatement[] Selects
		{
			get; private set;
		}
	}
}
