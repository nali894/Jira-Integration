using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Jira.Integration
{
    public class ProjectFacade
    {
        private static QueueApi _oQueueApi;
        public ProjectFacade(string strQueueID)
        {
            _oQueueApi = new QueueApi(strQueueID);
        }
        public static  async Task<JObject> GetAllIssues()
        {
            return await _oQueueApi.GetAllIssues();
        }
    }
}
