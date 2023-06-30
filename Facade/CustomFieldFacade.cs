using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Jira.Integration
{
    public class CustomFieldFacade
    {
        private static  CustomFieldApi _oCustomFieldApi=new CustomFieldApi();

        /// <summary>
        /// Obtiene el Id del campo consultado
        /// </summary>
        /// <param name="strIssueKey">identificación o clave del requerimiento (issue)</param>
        /// <param name="strLabelName">Nombre del campo</param>
        /// <returns></returns>
        public static async Task<string> GetIDByLabelName(string strIssueKey, string strLabelName)
        {

            return await _oCustomFieldApi.GetIDByLabelName(strIssueKey, strLabelName);           
        }

        /// <summary>
        /// Trae las opciones de un campo tipo  dropdownlist
        /// </summary>
        /// <param name="intCustomFielID"></param>
        /// <returns></returns>
        public static async Task<JObject> GetdropdownlistOptions(int intCustomFielID, int intMaxResults = 1000, int intStartAt = 0)
        {          
            
            return await _oCustomFieldApi.GetdropdownlistOptions(intCustomFielID,  intMaxResults, intStartAt);
        }
    }
}
