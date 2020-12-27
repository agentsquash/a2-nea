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
			SQLiteCommand FetchUsername = new SQLiteCommand("SELECT password,userID FROM users_local WHERE username = @username",dbconn);
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


			SQLiteCommand FetchAccessLevel = new SQLiteCommand("SELECT accessLevel FROM users WHERE UserID = @UserID",dbconn);
			FetchAccessLevel.Parameters.AddWithValue("@UserID", reader.GetInt32(1));
			SQLiteDataReader accessReader = FetchAccessLevel.ExecuteReader();
			while (accessReader.Read())
			{
				int accessLevel = accessReader.GetInt32(0);
				dbconn.Close();
				return accessLevel;
			}
			return -1;

		}

		/// <summary>
		/// This method is used to create a local system account.
		/// </summary>
		/// <param name="username">User provided username</param>
		/// <param name="email">User provided email</param>
		/// <param name="password">User provided password</param>
		/// <returns></returns>
		public static bool CreateLocalAccount(string username,string email,string password)
		{
			int UserID;
			
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			SQLiteCommand CreateAccountTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS users (UserID INTEGER UNIQUE, accessType INTEGER, accessLevel INTEGER, PRIMARY KEY (UserID AUTOINCREMENT))", dbconn);
			SQLiteCommand CreateLocalAccountTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS users_local (UserID INTEGER UNIQUE, username VARCHAR(64) UNIQUE, email VARCHAR(320), password BLOB)",dbconn);

			CreateAccountTable.ExecuteNonQuery();
			CreateLocalAccountTable.ExecuteNonQuery();

			// Check if username exists already before inserting - to satisfy unique constraint.
			SQLiteCommand CheckUsername = new SQLiteCommand("SELECT username FROM users_local WHERE username = @username",dbconn);
			CheckUsername.Parameters.AddWithValue("@username", username);
			SQLiteDataReader usernameReader = CheckUsername.ExecuteReader();

			bool unique = true;
			while (usernameReader.Read())
				unique = false;
			if (!unique)
				return false;

			// Inserts user account into the 'user' database
			SQLiteCommand InsertUserAccount = new SQLiteCommand("INSERT INTO users (accessType,accessLevel) VALUES (0,0)",dbconn);
			InsertUserAccount.ExecuteNonQuery();
			SQLiteCommand FetchUserID = new SQLiteCommand("SELECT MAX(UserID) FROM users",dbconn);
			SQLiteDataReader readerUsers = FetchUserID.ExecuteReader();
			// Fetches unique UserID from user database.
			readerUsers.Read();
			UserID = readerUsers.GetInt32(0);

			//Insert local user account.
			SQLiteCommand InsertUserLocalAccount = new SQLiteCommand("INSERT INTO users_local (UserID, username, email, password) VALUES (" + UserID + ",@username,@email,@password)", dbconn);
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

			SQLiteCommand CheckTableExists = new SQLiteCommand("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@tablename",dbconn);
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
			SQLiteCommand ResetStationDataTable = new SQLiteCommand("DROP TABLE IF EXISTS station_data",dbconn);
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

			using (TextFieldParser parser = new TextFieldParser(filename))
			{
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
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
		public static (bool,int) CheckStationExists(string station)
		{
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();
			int exists = 0;

			// Define SQL commands.
			SQLiteCommand CheckCRSExists = new SQLiteCommand("SELECT COUNT(*) FROM station_data WHERE crsCode = @crs", dbconn);
			SQLiteCommand CheckStationExists = new SQLiteCommand("SELECT COUNT(*) FROM station_data WHERE stationName LIKE @station+'%'");
			SQLiteCommand FetchCRSIDusingCRS = new SQLiteCommand("SELECT crsID FROM station_data WHERE crsCode = @crs");

			// Check initially against CRS data.
			if (station.Length == 3)
			{
				string crs = station.Substring(0, 3);
				CheckCRSExists.Parameters.AddWithValue("@crs", crs);
				exists = Convert.ToInt32(CheckCRSExists.ExecuteScalar());
				if (exists == 1)
				{
					FetchCRSIDusingCRS.Parameters.AddWithValue("@crs", crs);
					return (true, Convert.ToInt32(FetchCRSIDusingCRS.ExecuteScalar()));
				}
			}
			// Then check against station name.
			else
			{
				CheckStationExists.Parameters.AddWithValue("@station",station);
				SQLiteDataReader reader = CheckStationExists.ExecuteReader();
				while (reader.Read())
				{
					exists++;
				}
				if (exists == 1)
				{
					//TODO.
				}
			}

			return (false, -1);


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
			SQLiteCommand CreateTIPLOCTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS tiploc_data (crsID INTEGER, tiploc TEXT UNIQUE)",dbconn);
			SQLiteCommand ClearTIPLOCTable = new SQLiteCommand("DELETE FROM tiploc_data",dbconn);
			CreateTIPLOCTable.ExecuteNonQuery();
			ClearTIPLOCTable.ExecuteNonQuery();

			// Begin conversion of data to the database.
			using (TextFieldParser parser = new TextFieldParser(filename))
			{
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
				string[] fields;
				while (!parser.EndOfData)
				{
					fields = parser.ReadFields();

				}
			}
			return true;
		}

		/// <summary>
		/// This private method is used to add the station CRS and name to the station_data database.
		/// This method is used in combination with ConvertRailReferences.
		/// </summary>
		/// <param name="rowno">Current row in the CSV file.</param>
		/// <param name="crs">The station's CRS (3Alpha) code, as found in RailReferences.</param>
		/// <param name="station">The station's name, with excess data trimmed.</param>
		/// <returns>If TRUE, then a new CRS code has been added and the CRS Code needs to be incremented.</returns>
		private static bool AddToStationData(string crs, string station)
		{
			SQLiteConnection dbconn = InitialiseDB();
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
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			SQLiteCommand AddTIPLOCData = new SQLiteCommand("INSERT INTO tiploc_data (crsID, tiploc) VALUES (@crsID, @tiploc)", dbconn);
			AddTIPLOCData.Parameters.AddWithValue("@crsID", crsID);
			AddTIPLOCData.Parameters.AddWithValue("@tiploc", TIPLOC);
			AddTIPLOCData.ExecuteNonQuery();
			dbconn.Close();
		}

		public static bool ConvertRailConnections(string filename)
		{
			return true;
		}


	}
}
