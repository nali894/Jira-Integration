namespace BusinessLayer.Jira.Integration
{

    /// <summary>
    ///  Acceder a la raíz de la API REST de Jira
    /// </summary>
    public class RestApi : ApiRestService
    {
        public RestApi() : base($"https://Jira.atlassian.net/rest/api/2/")
        {
            SetCredentials("username@domain.com", "token");
        }
    }
    
}
