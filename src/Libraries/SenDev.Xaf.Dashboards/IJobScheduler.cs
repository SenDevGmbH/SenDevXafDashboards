using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SenDev.Xaf.Dashboards.BusinessObjects;

namespace SenDev.Xaf.Dashboards
{
	public interface IJobScheduler
	{
		void ScheduleDataExtractCreationJob(DashboardDataExtract dataExtract);
		void RemoveDataExtractCreationJob(DashboardDataExtract dataExtract);
		void StartDataExtractUpdate(DashboardDataExtract dataExtract);
	}
}
