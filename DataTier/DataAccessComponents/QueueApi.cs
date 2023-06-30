using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BusinessLayer.Jira.Integration
{
    public class QueueApi
    {
        private string _strQueueID;
        private ServicedeskApi _ServicedeskApi;
        public QueueApi(string strQueueID)
        {
            _strQueueID = strQueueID;
            _ServicedeskApi = new ServicedeskApi();
        }      

        public async Task<JObject>  GetAllIssues()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            HttpResponseMessage response = await _ServicedeskApi.Get($"servicedesk/2/queue/{_strQueueID}/issue", headers, parameters);
            JObject valueJson = JObject.Parse(response.Content.ReadAsStringAsync().Result); 
            return valueJson;
        }
    }
}
