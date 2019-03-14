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

	public class InMemoryDataStoreProvider : IXpoDataStoreProvider
	{

		private readonly IDataStore dataStore;

		public InMemoryDataStoreProvider(IDataStore dataStore)
		{
			this.dataStore = dataStore;
		}
		public string ConnectionString => null;

		public IDataStore CreateSchemaCheckingStore(out IDisposable[] disposableObjects)
		{
			disposableObjects = new IDisposable[0];
			return dataStore;
		}

		public IDataStore CreateUpdatingStore(bool allowUpdateSchema, out IDisposable[] disposableObjects)
		{
			disposableObjects = new IDisposable[0];
			return dataStore;
		}

		public IDataStore CreateWorkingStore(out IDisposable[] disposableObjects)
		{
			disposableObjects = new IDisposable[0];
			return dataStore;
		}
	}
}
