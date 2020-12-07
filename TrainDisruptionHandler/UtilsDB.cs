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
		public static SQLiteConnection InitialiseDB()
		{
			string ConnString = "Data Source=.\\data.db; Version=3;";
			return new SQLiteConnection(ConnString);
		}

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
			reader.Read();

			bool passwordMatch = UtilsAuth.PasswordVerify(password, reader.GetString(0));
			if (!passwordMatch)
				return -1;

			SQLiteCommand FetchAccessLevel = new SQLiteCommand("SELECT accessLevel FROM users WHERE UserID = @UserID",dbconn);
			FetchAccessLevel.Parameters.AddWithValue("@UserID", reader.GetInt32(1));
			SQLiteDataReader accessReader = FetchAccessLevel.ExecuteReader();
			int accessLevel;

			return Convert.ToInt32(accessLevel);


		}

		
		public static bool CreateLocalAccount(string username,string email,string password)
		{
			int UserID;
			
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			SQLiteCommand CreateAccountTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS users (UserID INTEGER UNIQUE, accessType INTEGER, accessLevel INTEGER, PRIMARY KEY (UserID AUTOINCREMENT))", dbconn);
			SQLiteCommand CreateLocalAccountTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS users_local (UserID INTEGER UNIQUE, username VARCHAR(64) UNIQUE, email VARCHAR(320), password BLOB)",dbconn);

			CreateAccountTable.ExecuteNonQuery();
			CreateLocalAccountTable.ExecuteNonQuery();

			SQLiteCommand InsertUserAccount = new SQLiteCommand("INSERT INTO users (accessType) VALUES (0)",dbconn);
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
