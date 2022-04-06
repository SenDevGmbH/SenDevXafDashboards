using System;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo.DB;

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
