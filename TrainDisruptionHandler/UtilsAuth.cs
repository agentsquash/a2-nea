using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace TrainDisruptionHandler
{
	class UtilsAuth
	{
		public static bool UsernameValidation(string username)
		{
			if (username.Length > 0 | username.Length <= 64)
				return true;
			return false;
		}
		
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

		public static bool PasswordValidation(string password, string confirm_pass)
		{
			if (password.Length == 0 | confirm_pass.Length == 0)
				return false;
			if (password != confirm_pass)
				return false;
			return true;
		}

		// https://medium.com/@mehanix/lets-talk-security-salted-password-hashing-in-c-5460be5c3aae
		public static string[] PasswordHash(string password)
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

			string[] data = new string[] { Convert.ToBase64String(hashBytes), Convert.ToBase64String(_salt) };
			return data;
		}

		public static int FetchAccessLevel()
		{
			return -1;
		}
	}



}
