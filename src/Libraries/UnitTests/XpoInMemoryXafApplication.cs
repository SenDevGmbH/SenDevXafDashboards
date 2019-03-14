using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using SenDev.Xaf.Dashboards.Scripting;
using Xunit;

namespace UnitTests
{
	public class XpoInMemoryXafApplication : XafApplication
	{
		public static XpoInMemoryXafApplication CreateInstance()
		{

			var application = new XpoInMemoryXafApplication();
			application.Modules.Add(new UnitTestsModule());

			application.DatabaseUpdateMode = DatabaseUpdateMode.Never;
			application.DatabaseVersionMismatch += (s, e) =>
			{
				e.Updater.Update();
				e.Handled = true;
			};

			application.Setup();
			return application;
		}

		protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
		{
			args.ObjectSpaceProviders.Add(new XPObjectSpaceProvider(new InMemoryDataStoreProvider(new InMemoryDataStore()), true));
		}

		protected override LayoutManager CreateLayoutManagerCore(bool simple)
		{
			throw new NotImplementedException();
		}

		protected override void CustomizeTypesInfo()
		{
			base.CustomizeTypesInfo();
			TypesInfo xafTypesInfo = (TypesInfo)TypesInfo;
			var xpotis = xafTypesInfo.EntityStores.OfType<XpoTypeInfoSource>().FirstOrDefault();
			XPDictionary dict = XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary;
			xafTypesInfo.RegisterEntities(xpotis.XPDictionary.Classes
				.OfType<XPClassInfo>()
				.Where(ci => ci.IsPersistent)
				.Select(ci => ci.ClassType)
				.ToArray());

		}
	}
}
