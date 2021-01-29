using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Microsoft.VisualBasic.FileIO;

namespace TrainDisruptionHandler
{
	class UtilsDB
	{
		/// <summary>
		/// This method initialises the SQLiteConnection for all database purposes within the system.
		/// </summary>
		/// <returns></returns>
		public static SQLiteConnection InitialiseDB()
		{
			string ConnString = "Data Source=.\\data.db; Version=3;";
			return new SQLiteConnection(ConnString);
		}

		/// <summary>
		/// This method checks user provided details for login against the main database
		/// and returns an authentication level (or error) for local system accounts.
		/// </summary>
		/// <param name="username">User provided username</param>
		/// <param name="password">User provided password (not hashed/salted)</param>
		/// <returns></returns>
		public static int AuthLocalAccount(string username, string password)
		{
			bool usernameValid = UtilsAuth.UsernameValidation(username);
			if (!usernameValid)
				return -1;

			bool passwordValid = UtilsAuth.PasswordValidation(password, password);
			if (!passwordValid)
				return -1;

			SQLiteConnection dbconn = InitialiseDB();
			SQLiteCommand FetchUsername = new SQLiteCommand("SELECT password,userID FROM users_local WHERE username = @username", dbconn);
			FetchUsername.Parameters.AddWithValue("@username", username);

			dbconn.Open();
			SQLiteDataReader reader = FetchUsername.ExecuteReader();
			try
			{
				reader.Read();
				bool passwordMatch = UtilsAuth.PasswordVerify(password, reader.GetString(0));
				if (!passwordMatch)
					return -1;
			}
			catch { return -1; }

			SQLiteCommand FetchAccessLevel = new SQLiteCommand("SELECT accessLevel FROM users WHERE UserID = @UserID", dbconn);
			FetchAccessLevel.Parameters.AddWithValue("@UserID", reader.GetInt32(1));
			return Convert.ToInt32(FetchAccessLevel.ExecuteScalar());
		}

		/// <summary>
		/// This method is used to create a local system account.
		/// </summary>
		/// <param name="username">User provided username</param>
		/// <param name="email">User provided email</param>
		/// <param name="password">User provided password</param>
		/// <returns></returns>
		public static bool CreateLocalAccount(string username, string email, string password)
		{
			int UserID;

			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			SQLiteCommand CreateAccountTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS users (UserID INTEGER UNIQUE, accessType INTEGER, accessLevel INTEGER, PRIMARY KEY (UserID AUTOINCREMENT))", dbconn);
			SQLiteCommand CreateLocalAccountTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS users_local (UserID INTEGER UNIQUE, username VARCHAR(64) UNIQUE, email VARCHAR(320), password BLOB)", dbconn);

			CreateAccountTable.ExecuteNonQuery();
			CreateLocalAccountTable.ExecuteNonQuery();

			// Check if username exists already before inserting - to satisfy unique constraint.
			SQLiteCommand CheckUsername = new SQLiteCommand("SELECT COUNT(*) FROM users_local WHERE username = @username", dbconn);
			CheckUsername.Parameters.AddWithValue("@username", username);
			int usernameUnique = Convert.ToInt32(CheckUsername.ExecuteScalar());
			if (usernameUnique == 1)
				return false;

			// Inserts user account into the 'user' database
			SQLiteCommand InsertUserAccount = new SQLiteCommand("INSERT INTO users (accessType,accessLevel) VALUES (0,0)", dbconn);
			InsertUserAccount.ExecuteNonQuery();
			SQLiteCommand FetchUserID = new SQLiteCommand("SELECT MAX(UserID) FROM users", dbconn);
			UserID = Convert.ToInt32(FetchUserID.ExecuteScalar());

			//Insert local user account.
			SQLiteCommand InsertUserLocalAccount = new SQLiteCommand("INSERT INTO users_local (UserID, username, email, password) VALUES (@userid,@username,@email,@password)", dbconn);

			InsertUserLocalAccount.Parameters.AddWithValue("@userid", UserID);
			InsertUserLocalAccount.Parameters.AddWithValue("@username", username);
			InsertUserLocalAccount.Parameters.AddWithValue("@email", email);
			InsertUserLocalAccount.Parameters.AddWithValue("@password", password);

			InsertUserLocalAccount.ExecuteNonQuery();
			dbconn.Close();
			return true;
		}

		/// <summary>
		/// This method is used for the Admin Panel to check if the Station Data table exists.
		/// </summary>
		/// <returns></returns>
		public static bool CheckStationDataExists()
		{
			string table_name = "station_data";
			return CheckTableExists(table_name);
		}

		/// <summary>
		/// This method handles any requests to check if a table exists for GUI purposes.
		/// </summary>
		/// <param name="tablename">The name of the table to be checked.</param>
		/// <returns></returns>
		private static bool CheckTableExists(string tablename)
		{
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			SQLiteCommand CheckTableExists = new SQLiteCommand("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@tablename", dbconn);
			CheckTableExists.Parameters.AddWithValue("@tablename", tablename);
			int exists = Convert.ToInt32(CheckTableExists.ExecuteScalar());

			if (exists == 1)
				return true;
			return false;
		}

		/// <summary>
		/// This method removes any existing station data prior to the import of a new Station Codes file.
		/// </summary>
		/// <returns></returns>
		public static void ResetStationData()
		{
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			// Setting up statements
			SQLiteCommand ResetStationDataTable = new SQLiteCommand("DROP TABLE IF EXISTS station_data", dbconn);
			SQLiteCommand ResetTiplocTable = new SQLiteCommand("DROP TABLE IF EXISTS tiploc_data", dbconn);
			SQLiteCommand ResetConnectionTable = new SQLiteCommand("DROP TABLE IF EXISTS connection_data", dbconn);
			SQLiteCommand ResetFixedLinkTable = new SQLiteCommand("DROP TABLE IF EXISTS fixed_links", dbconn);

			// Executing SQL statements
			ResetStationDataTable.ExecuteNonQuery();
			ResetTiplocTable.ExecuteNonQuery();
			ResetConnectionTable.ExecuteNonQuery();
			ResetFixedLinkTable.ExecuteNonQuery();
		}

		/// <summary>
		/// This method is used to convert the Station Codes data from the National Rail website into a format acceptable
		/// to the Train Disruption Handler. This method is ran before any other data is imported.
		/// </summary>
		/// <param name="filename">User selected file</param>
		/// <returns>True if successful</returns>
		public static bool ConvertStationCodes(string filename)
		{
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			SQLiteCommand CreateStationTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS station_data (crsID INTEGER UNIQUE, crsCode TEXT UNIQUE, stationName TEXT, PRIMARY KEY(crsID AUTOINCREMENT))", dbconn);
			CreateStationTable.ExecuteNonQuery();

			using (TextFieldParser parser = InitialiseParser(filename))
			{
				string[] fields;
				SQLiteCommand AddStationData = new SQLiteCommand("INSERT INTO station_data (crsCode, stationName) VALUES (@crs, @station)", dbconn);
				while (!parser.EndOfData)
				{
					fields = parser.ReadFields();
					if (fields[1].Length > 3 | fields[1].Length < 3)
						return false;
					AddStationData.Parameters.AddWithValue("@crs", fields[1]);
					AddStationData.Parameters.AddWithValue("@station", fields[0]);
					AddStationData.ExecuteNonQuery();
				}
				dbconn.Close();
				return true;
			}
		}

		/// <summary>
		/// This method is used to check whether a station exists in the Station Data table.
		/// The method will also return the station's unique ID.
		/// </summary>
		/// <param name="station"></param>
		/// <returns></returns>
		public static int CheckStationExists(string station)
		{
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();
			int exists = 0;

			// Define SQL commands.
			SQLiteCommand CheckCRSExists = new SQLiteCommand("SELECT COUNT(*) FROM station_data WHERE crsCode = @crs", dbconn);
			SQLiteCommand CheckStationExists = new SQLiteCommand("SELECT COUNT(*) FROM station_data WHERE stationName LIKE @station+'%'", dbconn);
			SQLiteCommand FetchCRSIDusingCRS = new SQLiteCommand("SELECT crsID FROM station_data WHERE crsCode = @crs", dbconn);
			SQLiteCommand FetchCRSIDusingStation = new SQLiteCommand("SELECT crsID FROM station_data WHERE stationName LIKE @station+'%'", dbconn);

			// Check initially against CRS data.
			if (station.Length == 3)
			{
				string crs = station.Substring(0, 3);
				CheckCRSExists.Parameters.AddWithValue("@crs", crs);
				exists = Convert.ToInt32(CheckCRSExists.ExecuteScalar());
				if (exists == 1)
				{
					FetchCRSIDusingCRS.Parameters.AddWithValue("@crs", crs);
					return Convert.ToInt32(FetchCRSIDusingCRS.ExecuteScalar());
				}
			}
			// Then check against station name.
			else
			{
				CheckStationExists.Parameters.AddWithValue("@station", station);
				exists = Convert.ToInt32(CheckStationExists.ExecuteScalar());
				if (exists == 1)
				{
					FetchCRSIDusingStation.Parameters.AddWithValue("@station", station);
					return Convert.ToInt32(FetchCRSIDusingStation.ExecuteScalar());
				}
			}
			return -1;
		}

		/// <summary>
		/// This method is used to convert the NaPTAN RailReferences.csv file for usage by the system.
		/// </summary>
		/// <param name="filename">User selected file</param>
		/// <returns></returns>
		public static bool ConvertRailReferences(string filename)
		{
			// Initialises database connection.
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			// Initialises create table statements prior to executing them.
			SQLiteCommand CreateTIPLOCTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS tiploc_data (crsID INTEGER, tiploc TEXT UNIQUE)", dbconn);
			SQLiteCommand ClearTIPLOCTable = new SQLiteCommand("DELETE FROM tiploc_data", dbconn);
			CreateTIPLOCTable.ExecuteNonQuery();
			ClearTIPLOCTable.ExecuteNonQuery();

			//Create additional statements.
			SQLiteCommand AddTIPLOCData = new SQLiteCommand("INSERT INTO tiploc_data (crsID, tiploc) VALUES (@crsID, @tiploc)", dbconn);

			// Begin conversion of data to the database.
			using (TextFieldParser parser = InitialiseParser(filename))
			{
				string[] fields;
				while (!parser.EndOfData)
				{
					fields = parser.ReadFields();
					int crsID = CheckStationExists(fields[2]);
					if (crsID != -1)
					{
						AddTIPLOCData.Parameters.AddWithValue("@crsID", crsID);
						AddTIPLOCData.Parameters.AddWithValue("@tiploc", fields[1]);
						AddTIPLOCData.ExecuteNonQuery();
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Used to convert stationID to a 3 digit CRS code for public use.
		/// </summary>
		/// <param name="stationID"></param>
		/// <returns></returns>
		public static string FetchCRSCode(int stationID)
		{
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			SQLiteCommand FetchCRSCode = new SQLiteCommand("SELECT crsCode FROM station_data WHERE crsID = @crsID");
			FetchCRSCode.Parameters.AddWithValue("@crsID", stationID);
			return Convert.ToString(FetchCRSCode.ExecuteScalar());
		}

		/// <summary>
		/// Use to convert station ID to the station name.
		/// </summary>
		/// <param name="stationID"></param>
		/// <returns></returns>
		public static string FetchStationName(int stationID)
		{
			SQLiteCommand FetchStationName = new SQLiteCommand("SELECT stationName FROM station_data WHERE crsID = @crsID");
			FetchStationName.Parameters.AddWithValue("@crsID", stationID);
			return Convert.ToString(FetchStationName.ExecuteScalar());
		}

		/// <summary>
		/// This method is used to convert connection time data for usage by the system.
		/// </summary>
		/// <param name="filename">User selected file</param>
		/// <returns></returns>
		public static bool ConvertConnectionData(string filename)
		{
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			// Create table statements, and clear the table if already exists.
			SQLiteCommand CreateConnectionsTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS connection_data (crsID INTEGER UNIQUE, connTime INT)",dbconn);
			SQLiteCommand ClearConnectionTable = new SQLiteCommand("DELETE FROM connection_data", dbconn);
			CreateConnectionsTable.ExecuteNonQuery();
			ClearConnectionTable.ExecuteNonQuery();

			// Creating statement to add.
			SQLiteCommand AddConnectionData = new SQLiteCommand("INSERT INTO connection_data (crsID, connTime) VALUES (@crsID,@connTime)");
			string[] fields;

			using (TextFieldParser parser = InitialiseParser(filename))
			{
				while (!parser.EndOfData)
				{
					fields = parser.ReadFields();
					int crsID = CheckStationExists(fields[0]);
					AddConnectionData.Parameters.AddWithValue("@crsID", crsID);
					AddConnectionData.Parameters.AddWithValue("@connTime", fields[1]);
					AddConnectionData.ExecuteNonQuery();
				}
			}
			return true;
		}

		/// <summary>
		/// This method is used to convert fixed link connection data for usage by the system.
		/// </summary>
		/// <param name="filename">User selected file</param>
		/// <returns></returns>
		public static bool ConvertFixedLinks(string filename)
		{
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			// Create table statements, and clear the table if already exists.
			SQLiteCommand CreateFixedLinksTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS fixed_links (crsID_dep INTEGER, crsID_arr INTEGER, connTime INT)", dbconn);
			SQLiteCommand ClearFixedLinksTable = new SQLiteCommand("DELETE FROM fixed_links", dbconn);
			CreateFixedLinksTable.ExecuteNonQuery();
			ClearFixedLinksTable.ExecuteNonQuery();

			// Creating statement to add.
			SQLiteCommand AddFixedLinks = new SQLiteCommand("INSERT INTO fixed_links (crsID_dep, crsID_arr, connTime) VALUES (@crsID_dep, @crsID_arr, @connTime)");
			string[] fields;

			using (TextFieldParser parser = InitialiseParser(filename))
			{
				while (!parser.EndOfData)
				{
					fields = parser.ReadFields();
					int crsID_dep = CheckStationExists(fields[0]);
					int crsID_arr = CheckStationExists(fields[1]);
					AddFixedLinks.Parameters.AddWithValue("@crsID_dep", crsID_dep);
					AddFixedLinks.Parameters.AddWithValue("@crsID_arr", crsID_arr);
					AddFixedLinks.Parameters.AddWithValue("@connTime", fields[1]);
					AddFixedLinks.ExecuteNonQuery();
				}
			}
			return true;
		}

		/// <summary>
		/// This method is used to initialise the TextFieldParser for CSV files.
		/// </summary>
		/// <param name="filename">CSV file</param>
		/// <returns></returns>
		private static TextFieldParser InitialiseParser(string filename)
		{
			TextFieldParser parser = new TextFieldParser(filename);
			parser.TextFieldType = FieldType.Delimited;
			parser.SetDelimiters(",");
			return parser;
		}

	}
}
