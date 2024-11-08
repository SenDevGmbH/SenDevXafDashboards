using SenDev.Xaf.Dashboards;
using SenDev.Xaf.Dashboards.BusinessObjects;

namespace SenDev.DashboardsDemo.Web.Services
{
	public class HangfireJobScheduler : IJobScheduler
	{
		private readonly JobSchedulerService jobSchedulerService = new JobSchedulerService();	
		public void RemoveDataExtractCreationJob(IDashboardDataExtract dataExtract) => jobSchedulerService.RemoveUpdateDataExtractJob(dataExtract.GetKeyAsString());
		public void ScheduleDataExtractCreationJob(IDashboardDataExtract dataExtract) => jobSchedulerService.ScheduleUpdateDataExtractJob(dataExtract.GetKeyAsString());
		public void StartDataExtractUpdate(IDashboardDataExtract dataExtract) => jobSchedulerService.StartDataExtractUpdate(dataExtract.GetKeyAsString());	
	}
}
