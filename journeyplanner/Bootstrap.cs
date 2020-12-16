using System;
using System.Data.SQLite;

namespace JourneyPlanner
{
	class Bootstrap
	{
		static void Main(string[] args)
		{
			DataFetcher Becca = new DataFetcher();
			Becca.InitialiseStationData("C:/Users/alexa/Documents/GitHub/nea-traindisruptionapp/TrainDisruptionHandler/bin/Debug/RailReferences.csv");
		}
	}
}
