using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SenDev.DashboardsDemo.Module
{
	[ServiceContract]
	public interface IJobSchedulerService
	{
		[OperationContract]
		void ScheduleUpdateDataExtractJob(Guid dataExtractId);

		[OperationContract]
		void DeleteUpdateDataExtractJob(Guid dataExtractId);
	}
}
