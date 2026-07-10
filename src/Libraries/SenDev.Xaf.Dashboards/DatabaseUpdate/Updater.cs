using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using SenDev.Xaf.Dashboards.BusinessObjects;

namespace SenDev.Xaf.Dashboards.DatabaseUpdate
{
	// For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
	public class Updater : ModuleUpdater
	{
		public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
			base(objectSpace, currentDBVersion)
		{
		}

		public override void UpdateDatabaseAfterUpdateSchema()
		{
			base.UpdateDatabaseAfterUpdateSchema();
			EnsureDevExpressBackendType();
		}

		private void EnsureDevExpressBackendType()
		{
			var existing = ObjectSpace.FindObject<DevExpressBackendType>(null);
			if (existing == null)
			{
				var backendType = ObjectSpace.CreateObject<DevExpressBackendType>();
				backendType.Name = "DevExpress Extract";
				ObjectSpace.CommitChanges();
			}
		}
	}
}
