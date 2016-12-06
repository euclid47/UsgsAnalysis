using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using UsgsAnalysis.Models.Earthquake;

namespace UsgsAnalysis.Helpers
{
    /// <summary>
    /// Get requests from USGS JSON Api
    /// </summary>
    public class EarthquakeRequestHelper
    {
        /// <summary>
        /// Base USGS JSON Api url
        /// </summary>
        private const string baseUrl = "http://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson";

        /// <summary>
        /// Request earthquakes between two dates. All times use ISO8601 Date/Time format. Unless a timezone is specified, UTC is assumed.
        /// </summary>
        /// <param name="starttime">Limit to events on or after the specified end time.</param>
        /// <param name="endtime">Limit to events on or before the specified end time.</param>
        /// <param name="minmagnitude">Limit to events with a magnitude larger than the specified minimum</param>
        /// <returns>RootObject</returns>
        public async Task<RootObject> GetByDate(DateTime starttime, DateTime endtime, int minmagnitude = 0)
        {
            if(starttime > endtime)
            {
                throw new Exception("starttime must be less than endtime");
            }

            var requestUrl = new Uri($"{baseUrl}&starttime={starttime.ToUniversalTime()}&endtime={endtime.ToUniversalTime()}&minmagnitude={minmagnitude}");

            return JsonConvert.DeserializeObject<RootObject>(await GetRequest(requestUrl));
        }

        /// <summary>
        /// Requests that use both rectangle and circle will return the intersection, which may be empty, use with caution.
        /// </summary>
        /// <param name="minlatitude">Limit to events with a latitude larger than the specified minimum.</param>
        /// <param name="minlongitude">Limit to events with a longitude larger than the specified minimum.</param>
        /// <param name="maxlatitude">Limit to events with a latitude smaller than the specified maximum.</param>
        /// <param name="maxlongitude">Limit to events with a longitude smaller than the specified maximum.</param>
        /// <param name="minmagnitude">Limit to events with a magnitude larger than the specified minimum</param>
        /// <returns>RootObject</returns>
        public async Task<RootObject> GetByRectangle(decimal minlatitude, decimal minlongitude, decimal maxlatitude, decimal maxlongitude, int minmagnitude = 0)
        {
            if (minlatitude > maxlatitude)
            {
                throw new Exception("minlatitude must be less than maxlatitude");
            }

            if (minlongitude > maxlongitude)
            {
                throw new Exception("minlongitude must be less than maxlongitude");
            }

            if (minlatitude < -90 || minlatitude > 90)
            {
                throw new Exception("minlatitude must be between -90 and 90");
            }

            if (maxlatitude < -90 || maxlatitude > 90)
            {
                throw new Exception("maxlatitude must be between -90 and 90");
            }

            if (minlongitude < -180 || minlongitude > 180)
            {
                throw new Exception("minlongitude must be between -180 and 180");
            }

            if (maxlongitude < -180 || maxlongitude > 180)
            {
                throw new Exception("maxlongitude must be between -180 and 180");
            }

            var requestUrl = new Uri($"{baseUrl}&minlatitude={minlatitude}&minlongitude={minlongitude}&maxlatitude={maxlatitude}&maxlongitude={maxlongitude}&minmagnitude={minmagnitude}");

            return JsonConvert.DeserializeObject<RootObject>(await GetRequest(requestUrl));
        }

        /// <summary>
        /// Get earthquakes by radius degrees
        /// </summary>
        /// <param name="latitude">The latitude to be used for a radius search</param>
        /// <param name="longitude">The longitude to be used for a radius search</param>
        /// <param name="maxradius">Limit to events within the specified maximum number of degrees from the geographic point defined by the latitude and longitude parameters.</param>
        /// <param name="maxradiuskm">Limit to events within the specified maximum number of kilometers from the geographic point defined by the latitude and longitude parameters.</param>
        /// <param name="minmagnitude">Limit to events with a magnitude larger than the specified minimum</param>
        /// <returns>RootObject</returns>
        public async Task<RootObject> GetByCircleRadiusDegree(decimal latitude, decimal longitude, decimal maxradius, int minmagnitude = 0)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new Exception("latitude must be between -90 and 90");
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new Exception("longitude must be between -180 and 180");
            }

            if(maxradius < 0 || maxradius > 180)
            {
                throw new Exception("maxradius must be between 0 and 180");
            }

            var requestUrl = new Uri($"{baseUrl}&latitude={latitude}&longitude={longitude}&maxradius={maxradius}&minmagnitude={minmagnitude}");

            return JsonConvert.DeserializeObject<RootObject>(await GetRequest(requestUrl));
        }

        /// <summary>
        /// Get earthquakes by radius kilometers
        /// </summary>
        /// <param name="latitude">The latitude to be used for a radius search</param>
        /// <param name="longitude">The longitude to be used for a radius search</param>
        /// <param name="maxradiuskm">Limit to events within the specified maximum number of kilometers from the geographic point defined by the latitude and longitude parameters.</param>
        /// <param name="minmagnitude">Limit to events with a magnitude larger than the specified minimum</param>
        /// <returns>RootObject</returns>
        public async Task<RootObject> GetByCircleRadiusKm(decimal latitude, decimal longitude, decimal maxradiuskm, int minmagnitude = 0)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new Exception("latitude must be between -90 and 90");
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new Exception("longitude must be between -180 and 180");
            }

            if (maxradiuskm < 0 || maxradiuskm > 20001.6M)
            {
                throw new Exception("maxradiuskm must be between 0 and 20001.6");
            }

            var requestUrl = new Uri($"{baseUrl}&latitude={latitude}&longitude={longitude}&maxradiuskm={maxradiuskm}&minmagnitude={minmagnitude}");

            return JsonConvert.DeserializeObject<RootObject>(await GetRequest(requestUrl));
        }

        /// <summary>
        /// Get request to resource uri
        /// </summary>
        /// <param name="webAddress">Resource Uri</param>
        /// <returns>Response content string</returns>
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
