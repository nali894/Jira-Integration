namespace BusinessLayer.Jira.Integration
{
    public class ServicedeskApi: ApiRestService
    {
        /// <summary>
        /// Acceder a la API de Service Desk de Jira
        /// </summary>
        public ServicedeskApi() : base($"https://Jira.atlassian.net/rest/servicedeskapi/")
        {            
            SetCredentials("username@domain.com", "token");
        }

     
    }
}
