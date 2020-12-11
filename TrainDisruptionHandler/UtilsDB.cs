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
			
			InsertUserLocalAccount.ExecuteNonQuery();
			dbconn.Close();
			return true;
		}
		/// <summary>
		/// This method converts the station name provided by the user to the CRS (3Alpha) format.
		/// </summary>
		/// <param name="stationName"> User provided code.</param>
		/// <returns></returns>
		public string FetchCRSCode(string stationName)
		{
			SQLiteConnection dbconn = UtilsDB.InitialiseDB();
			dbconn.Open();

			// This code checks if a user has already entered a CRS code.
			if (stationName.Length == 3)
			{
				SQLiteCommand CheckIfCRS = new SQLiteCommand("SELECT crsCode FROM stationdata WHERE crsCode = '" + stationName + "'", dbconn);
				SQLiteDataReader reader = CheckIfCRS.ExecuteReader();
				while (reader.Read())
				{

				}
			}
			return "";
		}

		/// <summary>
		/// This method is used to convert the NaPTAN RailReferences.csv file for usage by the system.
		/// </summary>
		/// <param name="filename">Filename as provided from the textbox.</param>
		/// <returns></returns>
		public bool ConvertRailReferences(string filename)
		{
			// Initialises database connection.
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();
			// Initialises create table statements prior to executing them.
			SQLiteCommand CreateTIPLOCTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS tiploc_data (crsID INTEGER, tiploc TEXT UNIQUE)",dbconn);
			SQLiteCommand CreateStationTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS stations_data (crsID INTEGER UNIQUE, crsCode TEXT UNIQUE, stationName TEXT, PRIMARY KEY(crsID AUTOINCREMENT)", dbconn);
			CreateTIPLOCTable.ExecuteNonQuery();
			CreateStationTable.ExecuteNonQuery();
			dbconn.Close();
			// Begin conversion of data to the database.

			using (TextFieldParser parser = new TextFieldParser(filename))
			{
				int rowno = 0;
				int crsID = 0;
				bool crsAdded = false;
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
				while (!parser.EndOfData)
				{
					string[] fields = parser.ReadFields();

					if (rowno != 0)
					{
						// Firstly, add CRS data
						crsAdded = AddToStationData(fields[2], fields[3]);
						// Then, fetch CRS ID and add TIPLOC data.
						if (crsAdded)
							crsID++;
						AddToTIPLOCData(rowno, crsID, fields[1]);
					}
					rowno++;
				}
				Console.WriteLine("Initialisation completed! {0} stations changed.", crsID);
			}
			dbconn.Close();
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
		private bool AddToStationData(string crs, string station)
		{
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			station = station.Replace("'", "''").Replace(" Rail Station", "").Replace(" Railway Station", "");
			SQLiteCommand AddStationData = new SQLiteCommand("INSERT INTO stations_data (crsCode, stationName) VALUES (@crs, @station)", dbconn);
			SQLiteCommand CheckStationUnique = new SQLiteCommand("SELECT crsCode FROM stations_data WHERE crsCode = @crs", dbconn);
			int unique = Convert.ToInt32(CheckStationUnique.ExecuteScalar());
			if (unique == 0)
			{
				AddStationData.Parameters.AddWithValue("@crs", crs);
				AddStationData.Parameters.AddWithValue("@station", station);
				AddStationData.ExecuteNonQuery();
				return true;
			}
			return false;
		}

		/// <summary>
		/// This private method is used to add a new TIPLOC and CRSId to the tiploc_data database.
		/// This method is used in combination with ConvertRailReferences.
		/// </summary>
		/// <param name="rowno">Current row in the CSV file.</param>
		/// <param name="crsID"></param>
		/// <param name="TIPLOC">Station TIPLOC code.</param>
		private void AddToTIPLOCData(int rowno, int crsID, string TIPLOC)
		{
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			SQLiteCommand AddTIPLOCData = new SQLiteCommand("INSERT INTO tiploc_data (crsID, tiploc) VALUES (@crsID, @tiploc)", dbconn);
			AddTIPLOCData.Parameters.AddWithValue("@crsID", crsID);
			AddTIPLOCData.Parameters.AddWithValue("@tiploc", TIPLOC);
			AddTIPLOCData.ExecuteNonQuery();
			dbconn.Close();
		}


	}
}
