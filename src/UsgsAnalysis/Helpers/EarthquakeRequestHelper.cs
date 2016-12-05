using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UsgsAnalysis.Models.Earthquake;

namespace UsgsAnalysis.Helpers
{
    public class EarthquakeRequestHelper
    {
        private const string baseUrl = "http://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson";

        public async Task<RootObject> GetByDate(DateTime starttime, DateTime endtime)
        {
            if(starttime > endtime)
            {
                throw new Exception("starttime must be less than endtime");
            }

            return JsonConvert.DeserializeObject<RootObject>(await GetRequest(new Uri($"{baseUrl}&starttime={starttime.ToUniversalTime()}&endtime={endtime.ToUniversalTime()}")));
        }

        private async Task<string> GetRequest(Uri webAddress)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(webAddress);
                if(response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception($"{response.StatusCode.ToString()} - {response.RequestMessage}");
                }
            }
        }
    }
}
