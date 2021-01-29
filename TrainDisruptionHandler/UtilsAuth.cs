using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace TrainDisruptionHandler
{
	/// <summary>
	/// This class handles activities relating to authentication.
	/// </summary>
	class UtilsAuth
	{
		/// <summary>
		/// This method is used to validate usernames entered by the user.
		/// </summary>
		/// <param name="username">User entered username</param>
		/// <returns>TRUE: Username valid. | FALSE: Username invalid. </returns>
		public static bool UsernameValidation(string username)
		{
			if (username.Length > 0 | username.Length <= 20)
				return true;
			return false;
		}
		
		/// <summary>
		/// This method is used to validate emails in compliance with RFC 3696.
		/// This method does not check if an email address will actually work!
		/// </summary>
		/// <param name="email">User entered email address.</param>
		/// <returns>TRUE: Email valid. | FALSE: Email invalid.</returns>
		public static bool EmailValidation(string email)
		{
			if (!(email.Contains('@')))
				return false;
			if (!(email.Contains('.')))
				return false;

			string[] emailSplit = email.Split('@');
			if (emailSplit[0].Length == 0 | emailSplit[0].Length > 64)
				return false;
			string[] domainSplit = emailSplit[1].Split('.');
			if (domainSplit[0].Length == 0 | domainSplit[0].Length >= 252)
				return false;
			if (domainSplit[1].Length < 2)
				return false;
			return true;
		}

		/// <summary>
		/// This method verifies that the user entered passwords are matching, and neither field is not null. 
		/// The method does NOT handle passwords from the database.
		/// </summary>
		/// <param name="password">User entered password (top field)</param>
		/// <param name="confirm_pass">User entered password (confirm field)</param>
		/// <returns></returns>
		public static bool PasswordValidation(string password, string confirm_pass)
		{
			if (password.Length == 0 | confirm_pass.Length == 0)
				return false;
			if (password != confirm_pass)
				return false;
			return true;
		}

		/// <summary>
		/// This method hashes and salts passwords on signup, using the RFC2898 standard.
		/// I have adapted this method from the link below: https://medium.com/@mehanix/lets-talk-security-salted-password-hashing-in-c-5460be5c3aae
		/// Demonstrates Group A technical skill: Hashing
		/// </summary>
		/// <param name="password">User entered password.</param>
		/// <returns>Hashed/salted password.</returns>
		public static string PasswordHash(string password)
		{
			// Create salt
			byte[] _salt;
			new RNGCryptoServiceProvider().GetBytes(_salt = new byte[16]);
			// Hash + salt user password
			var pbkdf2 = new Rfc2898DeriveBytes(password, _salt, 10000);
			byte[] hash = pbkdf2.GetBytes(20);
			byte[] hashBytes = new byte[36];
			// Combine salt and hashed password
			Array.Copy(_salt, 0, hashBytes, 0, 16);
			Array.Copy(hash, 0, hashBytes, 16, 20);

			return Convert.ToBase64String(hashBytes);
		}

		/// <summary>
		/// This method verifies the password entered by the user against that in the database.
		/// The method hashes and salts the user entered, before verifying byte by byte the user password.
		/// This method has been adapted from this link: https://medium.com/@mehanix/lets-talk-security-salted-password-hashing-in-c-5460be5c3aae
		/// 
		/// </summary>
		/// <param name="password"></param>
		/// <param name="dbpassword"></param>
		/// <returns></returns>
		public static bool PasswordVerify(string password, string dbpassword)
		{
			byte[] hashBytes = Convert.FromBase64String(dbpassword);
			// Remove salt
			byte[] salt = new byte[16];
			Array.Copy(hashBytes, 0, salt, 0, 16);
			// Hash user password
			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
			byte[] hash = pbkdf2.GetBytes(20);

			for (int i = 0; i < 20; i++)
				if (hashBytes[i + 16] != hash[i])
					return false;
			return true;


		}
	}



}
