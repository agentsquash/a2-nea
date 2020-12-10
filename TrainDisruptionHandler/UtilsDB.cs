using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

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

			SQLiteCommand InsertUserAccount = new SQLiteCommand("INSERT INTO users (accessType,accessLevel) VALUES (0,0)",dbconn);
			InsertUserAccount.ExecuteNonQuery();
			SQLiteCommand FetchUserID = new SQLiteCommand("SELECT MAX(UserID) FROM users",dbconn);
			SQLiteDataReader readerUsers = FetchUserID.ExecuteReader();
			readerUsers.Read();
			UserID = readerUsers.GetInt32(0);

			SQLiteCommand InsertUserLocalAccount = new SQLiteCommand("INSERT INTO users_local (UserID, username, email, password) VALUES (" + UserID + ",'" + username + "','" + email + "','" + password + "')", dbconn);
			InsertUserLocalAccount.ExecuteNonQuery();
			return true;
		}

	}
}
