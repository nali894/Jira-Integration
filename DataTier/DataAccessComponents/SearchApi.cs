using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BusinessLayer.Jira.Integration
{
    public class SearchApi
    {
        private RestApi _RestApi;
        public SearchApi()
        {
            _RestApi = new RestApi();
        }

        /// <summary>
        /// https://Jira.atlassian.net/rest/api/2/search?jql=project={NombreProyecto} and status !=closed&fields=key&maxResults=3000&startAt=0
        /// </summary>
        /// <param name="strJql">filtro Jql (Jira Query Language)</param>
        /// <returns></returns>
        public async Task<JObject> GetResultJql(string strJql)
        {  
            Dictionary<string, string> headers = new Dictionary<string, string>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            HttpResponseMessage response = await _RestApi.Get($"search?jql={strJql}", headers, parameters);
            JObject valueJson = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            return valueJson;
        }
    }
}
