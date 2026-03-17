using System;
using System.ServiceModel;

namespace SenDev.DashboardsDemo.Module
{
	public interface IJobSchedulerService
	{
		void ScheduleUpdateDataExtractJob(string dataExtractId);

		void RemoveUpdateDataExtractJob(string dataExtractId);

        void StartDataExtractUpdate(string dataExtractId);
    }
}
