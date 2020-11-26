using System;
using System.Data.Sqlite;

namespace JourneyPlanner
{
	class Bootstrap
	{
		static void Main(string[] args)
		{
			DataFetcher dataFetch = new DataFetcher();
			dataFetch.FetchCRSData("Ulverston");
		}
	}
}
