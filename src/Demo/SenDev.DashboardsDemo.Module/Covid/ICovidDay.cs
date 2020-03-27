using System;

namespace SenDev.DashboardsDemo.Module.Covid
{
	public interface ICovidDay
	{
		int ConfirmedCount
		{
			get;
			set;
		}
		string Country
		{
			get;
			set;
		}
		DateTime Date
		{
			get;
			set;
		}
		int DaysSinceFirst100InfectionsByCountry
		{
			get;
			set;
		}
		int DaysSinceFirst100InfectionsByProvince
		{
			get;
			set;
		}
		int DeathCount
		{
			get;
		}
		CovidDayHolder FirstDayWith100InfectionsPerCountry
		{
			get;
			set;
		}
		CovidDayHolder FirstDayWith100InfectionsPerProvince
		{
			get;
			set;
		}
		double Latitude
		{
			get;
			set;
		}
		double Longitude
		{
			get;
			set;
		}
		string Province
		{
			get;
			set;
		}
		int RecoveredCount
		{
			get;
			set;
		}
	}
}