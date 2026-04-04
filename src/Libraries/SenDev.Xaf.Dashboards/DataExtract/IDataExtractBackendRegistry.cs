using System.Collections.Generic;

namespace SenDev.Xaf.Dashboards.DataExtract
{
	public interface IDataExtractBackendRegistry
	{
		void Register(IDataExtractBackend backend, bool isDefault = false);
		IDataExtractBackend GetBackend(string backendType);
		IDataExtractBackend GetDefaultBackend();
		IEnumerable<IDataExtractBackend> GetAllBackends();
	}
}
