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

		public static int AuthenticateLocal(string username, string password)
		{
			int UserID = -1;
			var dbconn = InitialiseDB();
			dbconn.Open();
			return UserID;
		}

		public static bool CreateLocalAccount(string username,string email,string password, string salt)
		{
			int UserID;
			
			SQLiteConnection dbconn = InitialiseDB();
			dbconn.Open();

			SQLiteCommand CreateAccountTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS users (UserID INTEGER UNIQUE, accessType INTEGER, accessLevel INTEGER PRIMARY KEY (UserID AUTOINCREMENT))", dbconn);
			SQLiteCommand CreateLocalAccountTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS users_local (UserID INTEGER UNIQUE, username VARCHAR(64), email VARCHAR(320), password BLOB, salt BLOB)",dbconn);

			CreateAccountTable.ExecuteNonQuery();
			CreateLocalAccountTable.ExecuteNonQuery();

			SQLiteCommand InsertUserAccount = new SQLiteCommand("INSERT INTO users (accessType) VALUES (0)",dbconn);
			InsertUserAccount.ExecuteNonQuery();
			SQLiteCommand FetchUserID = new SQLiteCommand("SELECT MAX(UserID) FROM users",dbconn);
			SQLiteDataReader readerUsers = FetchUserID.ExecuteReader();
			readerUsers.Read();
			UserID = readerUsers.GetInt32(0);

			SQLiteCommand InsertUserLocalAccount = new SQLiteCommand("INSERT INTO users_local (UserID, username, email, password, salt, accessLevel) VALUES (" + UserID + ",'" + username + "','" + email + "','" + password +"','"+ salt + "',0)", dbconn);
			InsertUserLocalAccount.ExecuteNonQuery();
			return true;
		}

	}
}
