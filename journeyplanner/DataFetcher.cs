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

		public string FetchCRSCode(string stationName)
		{
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			if (stationName.Length == 3)
			{
				SQLiteCommand CheckIfCRS = new SQLiteCommand("SELECT crsCode FROM stationdata WHERE crsCode = '" + stationName + "'",dbconn);
				SQLiteDataReader reader = CheckIfCRS.ExecuteReader();
				
				while (reader.Read())
				{

				}
			}
			return "";
		}

		public SQLiteConnection InitialiseDB()
		{
			string ConnString = "Data Source=.\\data.db; Version=3;";
			return new SQLiteConnection(ConnString);
		}
		/// <summary>
		/// This function initialises the StationData database, using the RailReferences.csv provided by the DfT's NaPTAN.
		/// </summary>
		public void InitialiseStationData(string filename)
		{
			// Initialises database connection.
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			// Initialises create table statements prior to executing them.
			SQLiteCommand CreateTIPLOCTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS tiploc_data (crsID INTEGER, tiploc TEXT UNIQUE)", dbconn);
			SQLiteCommand ClearTIPLOCTable = new SQLiteCommand("DELETE FROM tiploc_data", dbconn);
			SQLiteCommand CreateStationTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS stations_data (crsID INTEGER UNIQUE, crsCode TEXT UNIQUE, stationName TEXT, PRIMARY KEY(crsID AUTOINCREMENT))", dbconn);
			SQLiteCommand ClearStationTable = new SQLiteCommand("DELETE FROM stations_data", dbconn);
			CreateTIPLOCTable.ExecuteNonQuery();
			CreateStationTable.ExecuteNonQuery();
			ClearTIPLOCTable.ExecuteNonQuery();
			ClearStationTable.ExecuteNonQuery();
			dbconn.Close();

			// Begin conversion of data to the database.
			using (TextFieldParser parser = new TextFieldParser(filename))
			{
				int rowno = 0;
				int crsID = 0;
				bool crsAdded = false;
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
				string[] fields;
				while (!parser.EndOfData)
				{
					fields = parser.ReadFields();

					if (rowno != 0)
					{
						// Firstly, add CRS data
						crsAdded = AddToStationData(fields[2], fields[3]);
						// Then, handle CRS ID and add TIPLOC data.
						if (crsAdded)
							crsID++;
						AddToTIPLOCData(crsID, fields[1]);
					}
					rowno++;
				}
			}
		}

		/// <summary>
		/// This private method is used to add the station CRS and name to the station_data database.
		/// This method is used in combination with ConvertRailReferences.
		/// </summary>
		/// <param name="rowno">Current row in the CSV file.</param>
		/// <param name="crs">The station's CRS (3Alpha) code, as found in RailReferences.</param>
		/// <param name="station">The station's name, with excess data trimmed.</param>
		/// <returns>If TRUE, then a new CRS code has been added and the CRS Code needs to be incremented.</returns>
		static bool AddToStationData(string crs, string station)
		{
			string ConnString = "Data Source=.\\data.db; Version=3;";
			SQLiteConnection dbconn = new SQLiteConnection(ConnString);
			dbconn.Open();

			station = station.Replace("'", "''").Replace(" Rail Station", "").Replace(" Railway Station", "");
			SQLiteCommand AddStationData = new SQLiteCommand("INSERT INTO stations_data (crsCode, stationName) VALUES (@crs, @station)", dbconn);
			SQLiteCommand CheckStationUnique = new SQLiteCommand("SELECT COUNT(crsCode) FROM stations_data WHERE crsCode = @crs", dbconn);
			CheckStationUnique.Parameters.AddWithValue("@crs", crs);
			int unique = Convert.ToInt32(CheckStationUnique.ExecuteScalar());

			if (unique == 0)
			{
				AddStationData.Parameters.AddWithValue("@crs", crs);
				AddStationData.Parameters.AddWithValue("@station", station);
				AddStationData.ExecuteNonQuery();
				dbconn.Close();
				return true;
			}
			dbconn.Close();
			return false;
		}

		/// <summary>
		/// This private method is used to add a new TIPLOC and CRSId to the tiploc_data database.
		/// This method is used in combination with ConvertRailReferences.
		/// </summary>
		/// <param name="rowno">Current row in the CSV file.</param>
		/// <param name="crsID"></param>
		/// <param name="TIPLOC">Station TIPLOC code.</param>
		private static void AddToTIPLOCData(int crsID, string TIPLOC)
		{
			string ConnString = "Data Source=.\\data.db; Version=3;";
			SQLiteConnection dbconn = new SQLiteConnection(ConnString);
			dbconn.Open();

			SQLiteCommand AddTIPLOCData = new SQLiteCommand("INSERT INTO tiploc_data (crsID, tiploc) VALUES (@crsID, @tiploc)", dbconn);
			AddTIPLOCData.Parameters.AddWithValue("@crsID", crsID);
			AddTIPLOCData.Parameters.AddWithValue("@tiploc", TIPLOC);
			AddTIPLOCData.ExecuteNonQuery();
			dbconn.Close();
		}
	}
}
