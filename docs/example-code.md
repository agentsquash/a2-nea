    class UtilsDb
    {
        private static string ConnString = "Data Source=.\\login.db; Version=3;";

        public static int Login(string Username, string Password)
        {
            int Auth_level;
            SQLiteConnection Conn = new SQLiteConnection(ConnString);
            Conn.Open();

            SQLiteCommand QueryLogin = new SQLiteCommand(@"SELECT Username, Auth_Level
                                                           FROM User
                                                           WHERE Username = $Username
                                                           AND Password = $Password", Conn);
            QueryLogin.Parameters.AddWithValue("$Username", Username);
            QueryLogin.Parameters.AddWithValue("$Password", Password);

            SQLiteDataReader Reader = QueryLogin.ExecuteReader();

            if (Reader.Read())
                Auth_level = Convert.ToInt32(Reader["Auth_Level"]);
            else
                Auth_level = -1;

            Conn.Close();
            return Auth_level;
        }
    }
}
