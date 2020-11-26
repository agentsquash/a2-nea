using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using SQL

namespace JourneyPlanner
{
	class DataFetcher
	{

		private string darwin_ldb_key = "?accessToken=3be43ffc-b0b8-4e2c-bb24-28060d72e7fb";
		private string darwin_web_loc = "https://nea-nrapi.apphb.com/";

		public DataFetcher()
		{
			
		}

		public void FetchRouteingGuide()
		{

		}

		/// <summary>
		/// This function is to fetch requested strings from a URL.
		/// </summary>
		/// <param name="requestURL"></param>
		/// <returns></returns>
        private string FetchURL(string requestURL)
		{
            using (var webClient = new System.Net.WebClient())
			{
                return webClient.DownloadString(requestURL);
			}
		}

		public DelayInfo FetchDarwinLDBDelays(string crsDep, string crsArr)
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
        public FastestInfo FetchDarwinLDBFastest(string crsDep, string crsArr)
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
        public BoardInfo FetchDarwinLDBBoard (string boardRequested, string crsDep)
		{
            string requestConstruct = darwin_web_loc + boardRequested +"/" + crsDep + darwin_ldb_key;
            return JsonSerializer.Deserialize<BoardInfo>(FetchURL(requestConstruct));
		}

		public void FetchCRSData (string stationName)
		{
		}

		public bool CheckCRSData(CRSData crs)
		{
			return true;
		}
		
		public void ConvertRailReferences()
        {

        }
	}
}
