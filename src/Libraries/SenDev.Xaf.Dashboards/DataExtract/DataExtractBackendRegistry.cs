using System;
using System.Collections.Generic;

namespace SenDev.Xaf.Dashboards.DataExtract
{
	public class DataExtractBackendRegistry : IDataExtractBackendRegistry
	{
		private readonly Dictionary<string, IDataExtractBackend> backends = new Dictionary<string, IDataExtractBackend>();
		private string defaultBackendType;

		public void Register(IDataExtractBackend backend, bool isDefault = false)
		{
			if (backend == null) throw new ArgumentNullException(nameof(backend));
			backends[backend.BackendType] = backend;
			if (isDefault || defaultBackendType == null)
				defaultBackendType = backend.BackendType;
		}

		public IDataExtractBackend GetBackend(string backendType)
		{
			if (string.IsNullOrEmpty(backendType))
				return GetDefaultBackend();
			return backends.TryGetValue(backendType, out var backend) ? backend : GetDefaultBackend();
		}

		public IDataExtractBackend GetDefaultBackend() =>
			defaultBackendType != null && backends.TryGetValue(defaultBackendType, out var b) ? b : null;

		public IEnumerable<IDataExtractBackend> GetAllBackends() => backends.Values;
	}
}
