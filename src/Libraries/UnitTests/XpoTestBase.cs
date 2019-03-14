using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;

namespace UnitTests
{
    public abstract class XpoTestBase
    {

        private SimpleDataLayer dataLayer;

        protected SimpleDataLayer DataLayer
        {
            get
            {
                if (dataLayer == null)
                    dataLayer = CreateDataLayer();
                return dataLayer;
            }
        }
        private SimpleDataLayer CreateDataLayer()
        {
            return new TestDataLayer(new InMemoryDataStore());
        }

        protected UnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(DataLayer) { TrackPropertiesModifications = true };
        }

        
        protected IObjectSpace CreateObjectSpace()
        {
            return new XPObjectSpaceProvider(new MemoryDataStoreProvider()).CreateObjectSpace();
        }

        protected virtual void InitializeTestCore()
        {

        }

        public XpoTestBase()
        {
            dataLayer = null;
            InitializeTestCore();

        }

        

        
    }
}
