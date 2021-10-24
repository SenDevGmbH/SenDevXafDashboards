using System;
using DevExpress.DashboardCommon;

namespace SenDev.Xaf.Dashboards.BusinessObjects
{
	public interface IDashboardDataExtract
	{
		string CronExpression
		{
			get;
			set;
		}
		TimeSpan Duration
		{
			get;
		}
		byte[] ExtractData
		{
			get;
			set;
		}

		long ExtractDataSize
		{
			get;
			set;
		}
		DateTime FinishTime
		{
			get;
			set;
		}
		string Name
		{
			get;
			set;
		}
		string Script
		{
			get;
			set;
		}
		DateTime StartTime
		{
			get;
			set;
		}

		void ConfigureConnectionParameters(ExtractDataSourceConnectionParameters parameters);
		string EnsureTempFileCreated();
		string GetKeyAsString();
	}
}
