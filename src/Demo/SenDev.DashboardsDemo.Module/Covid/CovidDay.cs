using CsvHelper.Configuration.Attributes;
using System;
using System.Net;
using System.Text;

namespace SenDev.DashboardsDemo.Module.Covid
{
	public class CovidDay
	{

		public DateTime Date
		{
			get; set;
		}
		public int DaysSinceFirst100InfectionsByCountry
		{
			get; set;
		}
		public int DaysSinceFirst100InfectionsByProvince
		{
			get; set;
		}

		[Index(1)]
		public string Country
		{
			get; set;
		}

		[Index(0)]
		public string Province
		{
			get; set;
		}

		[Index(3)]
		public int ConfirmedCount
		{
			get; set;
		}

		[Index(4)]
		public int DeathCount
		{
			get;
		}

		[Index(5)]
		public int RecoveredCount
		{
			get; set;
		}

		[Index(6)]
		public double Latitude
		{
			get; set;
		}

		[Index(7)]
		public double Longitude
		{
			get; set;
		}

		public CovidDayHolder FirstDayWith100InfectionsPerCountry
		{
			get; set;
		}
		public CovidDayHolder FirstDayWith100InfectionsPerProvince
		{
			get; set;
		}
	}
}
