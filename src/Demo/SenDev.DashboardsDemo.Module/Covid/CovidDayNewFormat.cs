using System;
using CsvHelper.Configuration.Attributes;

namespace SenDev.DashboardsDemo.Module.Covid
{
	public class CovidDayNewFormat : CovidDay
	{


		[Name("Country_Region")]
		public override string Country
		{
			get; set;
		}

		[Name("Lat")]
		public override double Latitude
		{
			get; set;
		}

		[Name("Long_")]
		public override double Longitude
		{
			get; set;
		}


		[Name("Recovered")]
		public override int RecoveredCount
		{
			get; set;
		}

	}
}
