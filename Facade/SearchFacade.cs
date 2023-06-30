using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Jira.Integration
{
    public class SearchFacade
    {
        private static SearchApi _oSearchApi = new SearchApi();

        public static async Task<JObject> GetResultJql(string strJql)
        {
            return await _oSearchApi.GetResultJql(strJql);
        }
    }

  
}
