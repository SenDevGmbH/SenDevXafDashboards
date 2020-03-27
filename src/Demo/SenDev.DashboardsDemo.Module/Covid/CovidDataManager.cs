using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SenDev.DashboardsDemo.Module.Covid
{
	public class CovidDataManager
	{
		private CovidDayHolder fistDayWith100InfectionsPerCountry = new CovidDayHolder();
		private CovidDayHolder firstDayWith100InfectionsPerProvince = new CovidDayHolder();

		public IList<ICovidDay> GetData()
		{

			var tasks = new List<Task<IEnumerable<ICovidDay>>>();
			for (var date = new DateTime(2020, 1, 22); date <= DateTime.Today; date = date.AddDays(1))
			{
				tasks.Add(RetrieveDayDataAsync(date));
			}

			return tasks.SelectMany(t => t.Result).ToList();

		}

		private async Task<IEnumerable<ICovidDay>> RetrieveDayDataAsync(DateTime date)
		{
			using (HttpClient httpClient = new HttpClient())
			{
				var response = await httpClient.GetAsync(GetUri(date), HttpCompletionOption.ResponseContentRead);
				if (response.IsSuccessStatusCode)
				{
					using (var stream = await response.Content.ReadAsStreamAsync())
					{
						return GetRecords(date, stream);
					}
				}

			}

			return Array.Empty<ICovidDay>();
		}

		public static IList<ICovidDay> GetRecords(DateTime date, Stream stream)
		{
			if (stream is null)
				throw new ArgumentNullException(nameof(stream));

			CsvReader reader = CreateCsvReader(stream);
			ICovidDay[] records ;
			if (date < new DateTime(2020, 3, 22))
				records = reader.GetRecords<CovidDay>().ToArray();
			else
				records = reader.GetRecords<CovidDayNewFormat>().ToArray();

			foreach (var record in records)
			{
				record.Date = date;
			}
			return records;
		}

		private static CsvConfiguration configuration;
		private static CsvConfiguration Configuration => configuration ?? (configuration = CreateConfiguration());

		private static CsvReader CreateCsvReader(Stream stream)
		{
			return new CsvReader(new StreamReader(stream), Configuration);
		}

		private static CsvConfiguration CreateConfiguration()
		{
			var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HeaderValidated = null,
				MissingFieldFound = null
			};
			configuration.TypeConverterCache.AddConverter<int>(new Int32EmptyStringConverter());
			configuration.TypeConverterCache.AddConverter<double>(new DoubleEmptyStringConverter());
			return configuration;
		}

		private string GetUri(DateTime date)
		{
			return string.Format(
				CultureInfo.InvariantCulture,
				"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/{0:MM-dd-yyyy}.csv",
				date);
		}
	}
	class Int32EmptyStringConverter : Int32Converter
	{
		public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
		{
			if (string.IsNullOrWhiteSpace(text))
				return 0;
			!return base.ConvertFromString(text, row, memberMapData);
		}
	}

	class DoubleEmptyStringConverter : DoubleConverter
	{
		public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
		{
			if (string.IsNullOrWhiteSpace(text))
				return 0.0;
			return base.ConvertFromString(text, row, memberMapData);
		}
	}
}
