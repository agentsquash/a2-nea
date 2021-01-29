using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using System.Net.Http;
using System.Data.SQLite;
using Microsoft.VisualBasic.FileIO;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace TrainDisruptionHandler
{
	class UtilsFetching
	{
		private static readonly string darwin_ldb_key = "?accessToken=3be43ffc-b0b8-4e2c-bb24-28060d72e7fb";
		private static readonly string darwin_web_loc = "https://nea-nrapi.apphb.com/";
		private static readonly string nrdata_password = "#Q47-M6#4vty";

		/// <summary>
		/// This requests an API token from the National Rail Open Data system.
		/// </summary>
		/// <returns>PostTokenDTD Class: username, token</returns>
		public static async Task<POSTTokenDTD> POSTRequest()
		{
			var baseAddress = new Uri("https://opendata.nationalrail.co.uk/");

			using (var httpClient = new HttpClient { BaseAddress = baseAddress })
			{
				//username = user1@gmail.com & password = P@55w0rd1
				var values = new Dictionary<string, string>
				{
					{ "username","me@alexashley.xyz" },
					{ "password",nrdata_password }
				};
				var content = new FormUrlEncodedContent(values);

				using (var response = await httpClient.PostAsync("authenticate",content))
				{
					string string_response = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<POSTTokenDTD>(string_response);
				}
			}
		}

		public static async Task<FileStream> FetchRouteingGuide(string token)
		{
			var baseAddress = new Uri("https://opendata.nationalrail.co.uk/api/staticfeeds/2.0/");

			using (var httpClient = new HttpClient { BaseAddress = baseAddress })
			{
				httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
				httpClient.DefaultRequestHeaders.Add("X-Auth-Token", token);

				using (var response = await httpClient.GetAsync("routeing"))
				{
					var content = await response.Content.ReadAsStreamAsync();
					var file = File.OpenWrite(@"APIData\RouteingGuide.zip");
					content.CopyTo(file);
					return file;
				}
			}
		}


		/// <summary>
		/// This function is to fetch requested strings from a URL.
		/// </summary>
		/// <param name="requestURL"></param>
		/// <returns></returns>
		private static string FetchURL(string requestURL)
		{
			using (var webClient = new System.Net.WebClient())
			{
				return webClient.DownloadString(requestURL);
			}
		}
		/// <summary>
		/// This function fetches the Delay Information from the Darwin OpenLDBWS service, and returns it in the DelayInfo class.
		/// </summary>
		/// <param name="crsDep">Verified CRS code for departure station</param>
		/// <param name="crsArr">Verified CRS code for arrival station</param>
		/// <returns></returns>
		public static DelayInfo FetchDarwinLDBDelays(string crsDep, string crsArr)
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
		public static FastestInfo FetchDarwinLDBFastest(string crsDep, string crsArr)
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
		public static BoardInfo FetchDarwinLDBBoard(string boardRequested, string crsDep)
		{
			string requestConstruct = darwin_web_loc + boardRequested + "/" + crsDep + darwin_ldb_key;
			return JsonSerializer.Deserialize<BoardInfo>(FetchURL(requestConstruct));
		}
	}
}
