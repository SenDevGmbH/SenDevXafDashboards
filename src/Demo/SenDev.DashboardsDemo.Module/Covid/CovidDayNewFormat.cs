using System;
using CsvHelper.Configuration.Attributes;

namespace SenDev.DashboardsDemo.Module.Covid
{
	public class CovidDayNewFormat : ICovidDay
	{


		[Name("Province_State")]
		public string Province
		{
			get; set;
		}

		[Name("Country_Region")]
		public string Country
		{
			get; set;
		}

		[Name("Lat")]
		public double Latitude
		{
			get; set;
		}

		[Name("Long_")]
		public double Longitude
		{
			get; set;
		}

		[Name("Confirmed")]
		public int ConfirmedCount
		{
			get; set;
		}

		[Name("Deaths")]
		public int DeathCount
		{
			get; set;
		}

		[Name("Recovered")]
		public int RecoveredCount
		{
			get; set;
		}

		[Ignore]
		public DateTime Date
		{
			get; set;
		}
		[Ignore]
		public int DaysSinceFirst100InfectionsByCountry
		{
			get; set;
		}
		[Ignore]
		public int DaysSinceFirst100InfectionsByProvince
		{
			get; set;
		}
		[Ignore]
		public CovidDayHolder FirstDayWith100InfectionsPerCountry
		{
			get; set;
		}
		[Ignore]
		public CovidDayHolder FirstDayWith100InfectionsPerProvince
		{
			get; set;
		}
	}
}
