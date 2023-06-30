using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Jira.Integration
{
    public class IssueFacade
    {
        //TODO: Validar cambio a singleton
        private static IssueApi _IssueApi = new IssueApi();

        /// <summary>
        /// Obtiene la información del requerimiento (Issue) por identificación o clave
        /// </summary>
        /// <param name="strIssueKey">identificación o clave del requerimiento (issue)</param>
        /// <returns></returns>
        public static async Task<JObject> GetByKey(string strIssueKey)
        {
            return await _IssueApi.GetByKey(strIssueKey);
        }


        /// <summary>       
        /// lista los Id de campos personalizados, con su respectivo valor
        /// </summary>
        /// <param name="strIssueKey">identificación o clave del requerimiento (issue)</param>
        /// <returns>true/false</returns>
        public static async Task<JObject> GetValuesCustomFieldByID(string strIssueKey)
        {
            return await _IssueApi.GetValuesCustomFieldByID(strIssueKey);
        }

        /// <summary>
        /// Actualiza los campos del Jira
        /// </summary>
        /// <param name="strIssueKey">identificación o clave del requerimiento (issue)</param>
        /// <param name="dCustomField">Diccionario con el id del campo y su valor</param>
        /// <returns>true/false</returns>
        public static async Task<bool> Update(string strIssueKey, Dictionary<string, object> dCustomField)
        {
            return await _IssueApi.Update(strIssueKey, dCustomField);
        }

        /// <summary>
        /// </summary>
        /// <param name="strIssueKey">identificación o clave del requerimiento (issue)</param>
        /// <param name="strComment">Texto</param>
        /// <returns>true/false</returns>
        public static async Task<bool> AddComment(string strIssueKey, string strComment)
        {
            return await _IssueApi.AddComment(strIssueKey, strComment);
        }

        /// <summary>      
        /// Actualizar el estado del requerimoento/Issue de Jira
        /// </summary>
        /// <param name="strIssueKey">identificación o clave del requerimiento (issue)</param>
        /// <param name="strStatusID">Id del estado en Jira</param>
        /// <returns>true/false</returns>
        public static async Task<bool> UpdateStatus(string strIssueKey, string strStatusID)
        {
            return await _IssueApi.UpdateStatus(strIssueKey, strStatusID);
        }
    }
}
