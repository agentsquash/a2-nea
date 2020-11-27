using System;
using System.Data.SQLite;

namespace JourneyPlanner
{
	class Bootstrap
	{
		static void Main(string[] args)
		{
			
			DataFetcher dataFetch = new DataFetcher();
			dataFetch.ConvertRailReferences();
		}
	}
}
