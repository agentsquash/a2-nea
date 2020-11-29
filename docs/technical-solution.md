	1. Copy your code (ensure it has comments)
	2. It can be presented as an overview guide, including:
		a. directing the marker to the particularly more sophisticated algorithms
		b. explanations of particularly difficult-to-understand code sections; a careful division of the presentation of the code listing into appropriately labelled sections to make navigation as easy as possible for the marker.
	3. Highlight where you have used Group A/B technical skills (pg. 95-96 spec)
  Highlight where you have used excellent coding styles (pg. 97 spec)

# DataFetcher.cs
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

		public void FetchCRSData(string stationName)
		{

		}

		public bool CheckCRSData(CRSData crs)
		{
			return true;
		}
		/// <summary>
		/// This function initialises the StationData database, using the RailReferences.csv provided by the DfT's NaPTAN.
		/// </summary>
		public void InitialiseStationData()
		{
			string ConnString = "Data Source=.\\data.db; Version=3;";

			SQLiteConnection dbconn = new SQLiteConnection(ConnString);
			dbconn.Open();

			SQLiteCommand DeleteStationTable = new SQLiteCommand("DROP TABLE stationdata", dbconn);
			SQLiteCommand CreateStationTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS stationdata (tiplocCode VARCHAR(7) PRIMARY KEY UNIQUE, crsCode VARCHAR(3), stationName VARCHAR(64), connTime INT)", dbconn);


			DeleteStationTable.ExecuteNonQuery();
			CreateStationTable.ExecuteNonQuery();

			using (TextFieldParser parser = new TextFieldParser("C:/Users/alexa/Documents/GitHub/nea-traindisruptionapp/journeyplanner/RailReferences.csv"))
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

		public void InitialiseConnectionData()
		{
			string ConnString = "Data Source=.\\data.db; Version=3;";

			SQLiteConnection dbconn = new SQLiteConnection(ConnString);
			dbconn.Open();

			SQLiteCommand CreateConnectionTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS connectiondata (crsCode VARCHAR(3) PRIMARY KEY, connectionType INT, connTime INT, connFrom VARCHAR(2), connTo VARCHAR(3))",dbconn);
			CreateConnectionTable.ExecuteNonQuery();
		}

		
	}