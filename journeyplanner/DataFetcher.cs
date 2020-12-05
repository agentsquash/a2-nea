using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using System.Data.SQLite;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;

namespace JourneyPlanner
{
	class DataFetcher
	{
		
		private string darwin_ldb_key = "?accessToken=3be43ffc-b0b8-4e2c-bb24-28060d72e7fb";
		private string darwin_web_loc = "https://nea-nrapi.apphb.com/";

		public DataFetcher()
		{

		}

		public void FetchRouteingGuide()
		{

		}

		/// <summary>
		/// This function is to fetch requested strings from a URL.
		/// </summary>
		/// <param name="requestURL"></param>
		/// <returns></returns>
		private string FetchURL(string requestURL)
		{
			using (var webClient = new System.Net.WebClient())
			{
				return webClient.DownloadString(requestURL);
			}
		}

		public DelayInfo FetchDarwinLDBDelays(string crsDep, string crsArr)
		{
			string requestConstruct = darwin_web_loc + "delays/" + crsArr + "/from/" + crsDep + "/20" + darwin_ldb_key;
			return JsonSerializer.Deserialize<DelayInfo>(FetchURL(requestConstruct));
		}
		/// <summary>
		/// This function is to fetch the fastest service from the OpenLDBWS service. All CRS codes should be verified via VerifyCRS.
		/// </summary>
		/// <param name="crsDep"></param>
		/// <param name="crsArr"></param>
		/// <returns></returns>
		public FastestInfo FetchDarwinLDBFastest(string crsDep, string crsArr)
		{
			string requestConstruct = darwin_web_loc + "fastest/" + crsDep + "/to/" + crsArr + darwin_ldb_key;
			return JsonSerializer.Deserialize<FastestInfo>(FetchURL(requestConstruct));
		}

		/// <summary>
		/// This function is to fetch Darwin live departure data from the OpenLDBWS service.
		/// </summary>
		/// <param name="boardRequested">
		/// Either "dep", "arr", "next" or "all"</param>
		/// <param name="crsDep"></param>
		/// A valid CRS point - should be verified through VerifyCRSCode first.
		/// <returns></returns>
		public BoardInfo FetchDarwinLDBBoard(string boardRequested, string crsDep)
		{
			string requestConstruct = darwin_web_loc + boardRequested + "/" + crsDep + darwin_ldb_key;
			return JsonSerializer.Deserialize<BoardInfo>(FetchURL(requestConstruct));
		}

		/// <summary>
		/// This function attempts to find a CRS (3Alpha) code for 
		/// </summary>
		/// <param name="stationName"></param>
		/// <returns></returns>
		public string FetchCRSCode(string stationName)
		{
			List<string> StationsFound = new List<string>();
			
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			if (stationName.Length == 3)
			{
				bool match = true;
				SQLiteCommand CheckIfCRS = new SQLiteCommand("SELECT crsCode FROM stationdata WHERE crsCode = '" + stationName + "'",dbconn);
				SQLiteDataReader crsReader = CheckIfCRS.ExecuteReader();

				while (crsReader.Read())
				{
					return crsReader.GetString(0);
				}
			}
			SQLiteCommand StationSearch = new SQLiteCommand("SELECT crsCode,stationName FROM stationdata WHERE stationName LIKE '" + stationName + "'", dbconn);
			SQLiteDataReader stationReader = StationSearch.ExecuteReader();

			while (stationReader.Read())
			{
				Console.WriteLine(stationReader.GetString(0)+" "+stationReader.GetString(1));
			}

			return "";

		}
		/// <summary>
		/// This function handles the initial connection to the data.db.
		/// </summary>
		/// <returns></returns>
		private SQLiteConnection InitialiseDB()
		{
			string ConnString = "Data Source=.\\data.db; Version=3;";
			return new SQLiteConnection(ConnString);
		}
		/// <summary>
		/// This function initialises the StationData database, using the RailReferences.csv provided by the DfT's NaPTAN.
		/// </summary>
		public void InitialiseStationData()
		{
			var dbconn = InitialiseDB();
			dbconn.Open();
			SQLiteCommand DeleteStationTable = new SQLiteCommand("DROP TABLE stationdata", dbconn);
			SQLiteCommand CreateStationTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS stationdata (tiplocCode VARCHAR(7) PRIMARY KEY UNIQUE, crsCode VARCHAR(3), stationName VARCHAR(64), connTime INT)", dbconn);


			DeleteStationTable.ExecuteNonQuery();
			CreateStationTable.ExecuteNonQuery();

			using (TextFieldParser parser = new TextFieldParser(".\\RailReferences.csv"))
			{
				int rowno = 0;
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
				while (!parser.EndOfData)
				{
					string[] fields = parser.ReadFields();
					fields[3] = fields[3].Replace("'", "''").Replace(" Rail Station", "").Replace(" Railway Station", "");

					// Add station data
					if (rowno != 0)
					{
						string addstation = "INSERT INTO stationdata (tiplocCode, crsCode, stationName, connTime) values ('" + fields[1] + "','" + fields[2] + "','" + fields[3] + "','5')";
						SQLiteCommand AddStation = new SQLiteCommand(addstation, dbconn);
						Console.WriteLine("{1}: Adding {0}...", fields[3], rowno);
						AddStation.ExecuteNonQuery();
					}
					rowno++;
				}
				Console.WriteLine("Initialisation completed! {0} stations changed.", rowno);
			}
			dbconn.Close();
		}

		/// <summary>
		/// This function creates the connection database if it does not already exist.
		/// </summary>
		public void InitialiseConnectionData()
		{
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			SQLiteCommand CreateConnectionTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS connectiondata (crsCode VARCHAR(3) PRIMARY KEY, connectionType INT, connTime INT, connFrom VARCHAR(2), connTo VARCHAR(3))",dbconn);
			CreateConnectionTable.ExecuteNonQuery();
		}

		
	}
}
