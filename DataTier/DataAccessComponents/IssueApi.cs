using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BusinessLayer.Jira.Integration
{
    public class IssueApi
    {
        private ServicedeskApi _ServicedeskApi;
        private RestApi _RestApi;
        public IssueApi()
        {
            _ServicedeskApi = new ServicedeskApi();
            _RestApi = new RestApi();
        }

        #region GET
        /// <summary>
        /// GET: https://Jira.atlassian.net/rest/servicedeskapi/request/{IssueKey}
        /// Obtiene la información del requerimiento (Issue) por identificación o clave
        /// </summary>
        /// <param name="strIssueKey">identificación o clave del requerimiento (issue)</param>
        /// <returns></returns>
        public async Task<JObject> GetByKey(string strIssueKey)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            HttpResponseMessage response = await _ServicedeskApi.Get($"request/{strIssueKey}", headers, parameters);
            JObject valueJson = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            return valueJson;
        }

        /// <summary>
        /// Construye el json con el Id del campo y su valor 
        /// para ser usado como body del servicio
        /// </summary>
        /// <param name="dCustomField">Diccionario con el id del campo y su valor</param>
        /// <returns>json con el id del campo y su valor</returns>
        private dynamic GetCustomFieldJson(Dictionary<string, object> dCustomField)
        {
            string strjson = "{\"fields\": {[CUSTOMFIELD]}}";
            string strField = string.Empty;
   
            foreach (var item in dCustomField)
            {
                string strType = item.Value.GetType().FullName;
                switch (strType)
                {
                    case "Newtonsoft.Json.Linq.JObject":
                    case "Newtonsoft.Json.Linq.JArray":
                    case "System.Int64":
                        strField += $"\"{item.Key}\":{item.Value},";
                        break;
                    case "System.DateTime":
                        DateTime dtm = Convert.ToDateTime(item.Value);
                        string strDate = dtm.ToString("o");
                        strField += $"\"{item.Key}\":\"{strDate}\",";
                        break;             
                    default:
                        strField += $"\"{item.Key}\":\"{item.Value}\",";
                        break;
                }

            }

            strField = strField.Substring(0, strField.Length - 1);

            return JsonConvert.DeserializeObject(strjson.Replace("[CUSTOMFIELD]", strField));

        }

        /// <summary>
        /// Construye el json con los comentarios
        /// </summary>
        /// <param name="strComment">texto</param>
        /// <returns></returns>
        private dynamic GetCommentJson(string strComment)
        {
            string strjson = "{\"body\": \"TEXT\"}";
            return JsonConvert.DeserializeObject(strjson.Replace("TEXT", strComment));

        }

        /// <summary>
        /// Construye el json para actualizar el estado del requerimoento/Issue de Jira
        /// </summary>
        /// <param name="strStatusID">Id del estado en Jira</param>
        /// <returns></returns>
        private dynamic GetStatusJson(string strStatusID)
        {
            string strjson = "{\"transition\": {STATUS}}";

            string strStatus = $"\"id\":\"{strStatusID}\"";
            return JsonConvert.DeserializeObject(strjson.Replace("STATUS", strStatus));

        }

        /// <summary>
        /// GET: https://Jira.atlassian.net/rest/api/2/issue/{IssueKey} 
        /// lista los Id de campos personalizados, con su respectivo valor
        /// </summary>
        /// <param name="strIssueKey">identificación o clave del requerimiento (issue)</param>
        /// <returns>true/false</returns>
        public async Task<JObject> GetValuesCustomFieldByID(string strIssueKey)
        {

            if (string.IsNullOrEmpty(strIssueKey))
            {
                return null;
            }

            Dictionary<string, string> headers = new Dictionary<string, string>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            JObject valueJson = new JObject();

            HttpResponseMessage response = await _RestApi.Get($"issue/{strIssueKey}", headers, parameters);

            if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                 valueJson = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            }

            return valueJson;
        }
        #endregion

        #region UPDATE

        /// <summary>
        /// POST: https://Jira.atlassian.net/rest/api/2/issue/{IssueKey}/comment 
        /// </summary>
        /// <param name="strIssueKey">identificación o clave del requerimiento (issue)</param>
        /// <param name="strComment">Texto</param>
        /// <returns>true/false</returns>
        public async Task<bool> AddComment(string strIssueKey, string strComment)
        {

            if (string.IsNullOrEmpty(strComment))
            {
                return false;
            }

            Dictionary<string, string> headers = new Dictionary<string, string>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            dynamic json = GetCommentJson(strComment);

            HttpResponseMessage response = await _RestApi.Post($"issue/{strIssueKey}/comment", headers, parameters, json);

            if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// PUT: https://Jira.atlassian.net/rest/api/2/issue/{IssueKey} 
        /// Actualiza los campos del Jira
        /// </summary>
        /// <param name="strIssueKey">identificación o clave del requerimiento (issue)</param>
        /// <param name="dCustomField">Diccionario con el id del campo y su valor</param>
        /// <returns>true/false</returns>
        public async Task<bool> Update(string strIssueKey, Dictionary<string, object> dCustomField)
        {

            if (dCustomField.Count <= 0)
            {
                return false;
            }

            Dictionary<string, string> headers = new Dictionary<string, string>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            dynamic json = GetCustomFieldJson(dCustomField);

            HttpResponseMessage response = await _RestApi.Put($"issue/{strIssueKey}", headers, parameters, json);

            if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
        }



        /// <summary>
        /// POST: https://Jira.atlassian.net/rest/api/2/issue/{IssueKey}/transitions?expand=transitions.fields 
        /// Actualizar el estado del requerimoento/Issue de Jira
        /// </summary>
        /// <param name="strIssueKey">identificación o clave del requerimiento (issue)</param>
        /// <param name="strStatusID">Id del estado en Jira</param>
        /// <returns>true/false</returns>
        public async Task<bool> UpdateStatus(string strIssueKey, string strStatusID)
        {
            try
            {
                if (string.IsNullOrEmpty(strIssueKey) || string.IsNullOrEmpty(strStatusID))
                {
                    return false;
                }

                Dictionary<string, string> headers = new Dictionary<string, string>();
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                dynamic json = GetStatusJson(strStatusID);

                HttpResponseMessage response = await _RestApi.Post($"issue/{strIssueKey}/transitions?expand=transitions", headers, parameters, json);

                if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }          
        
        }
        #endregion


    }
}
