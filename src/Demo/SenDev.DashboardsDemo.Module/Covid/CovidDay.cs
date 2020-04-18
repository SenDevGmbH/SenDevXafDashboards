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
		[Ignore]
		public int DaysSinceFirst100InfectionsByCountry
		{
			get; set;
		}
		
		[Name("Country/Region", "Country_Region")]
		public virtual string Country
		{
			get; set;
		}

		[Name("Province/State", "Province_State")]
		public virtual string Province
		{
			get; set;
		}

		[Name("Confirmed")]
		public virtual int ConfirmedCount
		{
			get; set;
		}

		[Name("Deaths")]
		public virtual int DeathCount
		{
			get;
			set;
		}

		[Name("Recovered")]
		public virtual int RecoveredCount
		{
			get; set;
		}

		[Name("Latitude")]
		public virtual double Latitude
		{
			get; set;
		}

		[Name("Longitude")]
		public virtual double Longitude
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
