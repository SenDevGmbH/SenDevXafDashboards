﻿using System;
using DevExpress.Xpo;
using SenDev.DashboardsDemo.Module;
using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.Scripting;

namespace SenDev.DashboardsDemo.Web.Services
{
	public class JobSchedulerService : MarshalByRefObject, IJobSchedulerService
	{
		public void RemoveUpdateDataExtractJob(string dataExtractId)
		{
			Hangfire.RecurringJob.RemoveIfExists(dataExtractId);
		}

		public void ScheduleUpdateDataExtractJob(string dataExtractId)
		{
			using (var dataLayer = CreateDataLayer())
			using (var unitOfWork = new UnitOfWork(dataLayer))
			{
				var dataExtract = unitOfWork.GetObjectByKey<DashboardDataExtract>(new Guid(dataExtractId));
				if (!string.IsNullOrWhiteSpace(dataExtract.CronExpression))
					Hangfire.RecurringJob.AddOrUpdate(dataExtractId.ToString(), () => UpdateDataExtract(dataExtractId.ToString()), dataExtract.CronExpression);
			}
		}

        public void StartDataExtractUpdate(string dataExtractId)
        {
            Hangfire.BackgroundJob.Enqueue(() => UpdateDataExtract(dataExtractId));
        }

        public void UpdateDataExtract(string dataExtractId)
		{

			var appDomain = AppDomain.CreateDomain("ServerSideInstanceDomain",null, AppDomain.CurrentDomain.SetupInformation);
			try
			{
				var service = (JobSchedulerService)appDomain.CreateInstanceAndUnwrap(GetType().Assembly.FullName, GetType().FullName);
				service.UpdateDataExtractCore(dataExtractId);
			}
			finally
			{
				AppDomain.Unload(appDomain);
			}
		}


		public void UpdateDataExtractCore(string dataExtractId)
		{
			var dataManager = new DataExtractDataManager(ServerSideApplication.Instance);
			dataManager.UpdateDataExtractByKey(Guid.Parse(dataExtractId));
		}
		private IDataLayer CreateDataLayer()
		{
			return new SimpleDataLayer(XpoDefault.GetConnectionProvider(Global.ConnectionString, DevExpress.Xpo.DB.AutoCreateOption.None));
		}
	}
}
