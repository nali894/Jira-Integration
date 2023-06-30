using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BusinessLayer.Jira.Integration
{
    public class CustomFieldApi
    {
        private RestApi _RestApi;
        public CustomFieldApi()
        {
            _RestApi = new RestApi();        
        }

        /// <summary>
        /// Obtiene el Id del campo consultado
        /// </summary>
        /// <param name="strIssueKey">identificación o clave del requerimiento (issue)</param>
        /// <param name="strLabelName">Nombre del campo</param>
        /// <returns></returns>
        public async Task<string> GetIDByLabelName(string strIssueKey,string strLabelName)
        {
            if (string.IsNullOrEmpty(strLabelName))
            {
                return null;
            }
            JObject joIssue=await new IssueApi().GetByKey(strIssueKey);
            string issueList = (from item in joIssue["requestFieldValues"]
                                      where (string)item["label"] == strLabelName
                                      select (string)item["fieldId"]).FirstOrDefault();
            return issueList;
        }

        /// <summary>
        /// https://Jira.atlassian.net/rest/api/2/customField/{customFieldID}/option
        /// Trae las opciones de un campo tipo  dropdownlist
        /// </summary>
        /// <param name="intCustomFielID">Identificación única del campo en jira</param>
        /// <returns></returns>
        public async Task<JObject> GetdropdownlistOptions(int intCustomFielID,int intMaxResults, int intStartAt)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            Dictionary<string, string> parameters = new Dictionary<string, string>() { 
                { "maxResults", intMaxResults.ToString() },
                { "startAt", intStartAt.ToString() }
            };
            HttpResponseMessage response = await _RestApi.Get($"customField/{intCustomFielID}/option", headers, parameters);
            JObject valueJson = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            return valueJson;
        }
    }
}
