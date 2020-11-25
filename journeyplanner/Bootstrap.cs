using System;

namespace JourneyPlanner
{
	class Bootstrap
	{
		static void Main(string[] args)
		{
			DataFetcher dataFetch = new DataFetcher();
			dataFetch.FetchCRS("Ulverston");
		}
	}
}
