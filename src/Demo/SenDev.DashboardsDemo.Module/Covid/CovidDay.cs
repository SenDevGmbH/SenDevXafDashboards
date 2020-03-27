using CsvHelper.Configuration.Attributes;
using System;
using System.Net;
using System.Text;

namespace SenDev.DashboardsDemo.Module.Covid
{
	public class CovidDay : ICovidDay
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
		public virtual string Country
		{
			get; set;
		}

		[Index(0)]
		public virtual string Province
		{
			get; set;
		}

		[Index(3)]
		public virtual int ConfirmedCount
		{
			get; set;
		}

		[Index(4)]
		public virtual int DeathCount
		{
			get;
			set;
		}

		[Index(5)]
		public virtual int RecoveredCount
		{
			get; set;
		}

		[Index(6)]
		public virtual double Latitude
		{
			get; set;
		}

		[Index(7)]
		public virtual double Longitude
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
