using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

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

        public FastestInfo FetchDarwinLDBFastest(string crsDep, string crsArr)
		{
            string requestConstruct = darwin_web_loc + "fastest/" + crsDep + "/to/" + crsArr + darwin_ldb_key;
            return JsonSerializer.Deserialize<FastestInfo>(FetchURL(requestConstruct));
		}

        public BoardInfo FetchDarwinLDBBoard (string boardRequested, string crsDep)
		{
            string requestConstruct = darwin_web_loc + boardRequested +"/" + crsDep + darwin_ldb_key;
            return JsonSerializer.Deserialize<BoardInfo>(FetchURL(requestConstruct));
		}

        public void ConvertRailReferences()
        {

        }



    }
}
