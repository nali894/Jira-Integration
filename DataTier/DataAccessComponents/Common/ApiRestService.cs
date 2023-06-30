using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Jira.Integration
{
    public class ApiRestService
    {
        private readonly Uri _baseUrl;
        private  string _strUserName;
        private  string _strApiToken;
        private const string JsonContentType = "application/json";
        
        public ApiRestService(string baseUrl)
        {           
            _baseUrl = new Uri(baseUrl);        
        }

        /// <summary>
        /// Realizar una solicitud GET a una API.
        /// </summary>
        /// <param name="apiUrl">string: La URL del punto final de la API.</param>
        /// <param name="headers">Dictionary<string, string> Colección de encabezados que se incluirán en la solicitud.</param>
        /// <param name="parameters">Dictionary<string, string>: Colección de parámetros de consulta que se agregarán a la URL.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Get(string apiUrl, Dictionary<string, string> headers, Dictionary<string, string> parameters)
        {
            HttpResponseMessage response;

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = _baseUrl;
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonContentType));
                AddAuthorizationHeader(headers);
                AddHeaders(httpClient, headers);
                apiUrl = BuildUrl(apiUrl, parameters);

                response = await httpClient.GetAsync(apiUrl);
            }

            return response;
        }


        /// <summary>
        /// Realizar una solicitud POST a una API.
        /// </summary>
        /// <param name="apiUrl">string: La URL del punto final de la API.</param>
        /// <param name="headers">Dictionary<string, string> Colección de encabezados que se incluirán en la solicitud.</param>
        /// <param name="parameters">Dictionary<string, string>: Colección de parámetros de consulta que se agregarán a la URL.</param>
        /// <returns>HttpResponseMessage: Respuesta recibida del servidor después de realizar la solicitud</returns>
        public async Task<HttpResponseMessage> Post(string apiUrl, Dictionary<string, string> headers, Dictionary<string, string> parameters, object bodyObject)
        {
            HttpResponseMessage response;

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = _baseUrl;
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonContentType));
                AddAuthorizationHeader(headers);
                AddHeaders(httpClient, headers);
                apiUrl = BuildUrl(apiUrl, parameters);

                string bodyJson = JsonConvert.SerializeObject(bodyObject);
                StringContent body = new StringContent(bodyJson, Encoding.UTF8, JsonContentType);

                response = await httpClient.PostAsync(apiUrl, body);
            }

            return response;
        }


        /// <summary>
        /// Realizar una solicitud PUT a una API.
        /// </summary>
        /// <param name="apiUrl">string: La URL del punto final de la API.</param>
        /// <param name="headers">Dictionary<string, string> Colección de encabezados que se incluirán en la solicitud.</param>
        /// <param name="parameters">Dictionary<string, string>: Colección de parámetros de consulta que se agregarán a la URL.</param>
        /// <returns>HttpResponseMessage: Respuesta recibida del servidor después de realizar la solicitud</returns>
        public async Task<HttpResponseMessage> Put(string apiUrl, Dictionary<string, string> headers, Dictionary<string, string> parameters, object bodyObject)
        {
            HttpResponseMessage response;

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = _baseUrl;
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonContentType));
                AddAuthorizationHeader(headers);
                AddHeaders(httpClient, headers);
                apiUrl = BuildUrl(apiUrl, parameters);

                string bodyJson = JsonConvert.SerializeObject(bodyObject);
                StringContent body = new StringContent(bodyJson, Encoding.UTF8, JsonContentType);

                response = await httpClient.PutAsync(apiUrl, body);
            }

            return response;
        }


        /// <summary>
        /// Agregar  encabezado de autorización con el nombre de usuario y su respectivo token
        /// </summary>
        /// <param name="headers">Dictionary<string, string>: Diccionario de encabezados</param>
        public void AddAuthorizationHeader(Dictionary<string, string> headers)
        {
            if(string.IsNullOrEmpty(_strUserName))
            {
                return;           
            }

            string strUser = $"{_strUserName}:{_strApiToken}";
            string strToken = Utilities.EncodeTo64(strUser);
            headers.Add("Authorization", $"Basic {strToken}");

        }


        /// <summary>
        /// Aregar encabezados personalizados al objeto HttpClient
        /// </summary>
        /// <param name="httpClient">objeto tipo httpClient:solicitudes HTTP</param>
        /// <param name="headers">Dictionary<string, string>: Diccionario de encabezados</param>
        private void AddHeaders(HttpClient httpClient, Dictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }
        }


        /// <summary>
        /// Construir una URL completa para una solicitud HTTP
        /// </summary>
        /// <param name="apiUrl">string: URL base</param>
        /// <param name="parametersDictionary">Dictionary<string, string>:Parámetros de consulta </param>
        /// <returns>string: URL contruida</returns>
        private string BuildUrl(string apiUrl, Dictionary<string, string> parametersDictionary)
        {
            string queryString = BuildQueryString(parametersDictionary);
            apiUrl = !string.IsNullOrEmpty(queryString) ? string.Concat(apiUrl, "?", queryString) : apiUrl;
            return apiUrl;
        }


        /// <summary>
        /// Construir una cadena de consulta (query string) a partir de un diccionario de parámetros (parametersDictionary).
        /// </summary>
        /// <param name="parametersDictionary">Dictionary<string, string>Pparámetros </param>
        /// <returns></returns>
        private string BuildQueryString(Dictionary<string, string> parametersDictionary)
        {
            if (parametersDictionary == null || parametersDictionary.Count == 0)
            {
                return string.Empty;
            }

            List<string> parametersList = parametersDictionary.Select(parameter => parameter.Key + "=" + parameter.Value).ToList();
            string parameters = string.Join("&", parametersList);
            return parameters;
        }


        /// <summary>
        /// Establece las credenciales de autenticación para acceder a la API REST
        /// </summary>
        /// <param name="strUserName">string: Nombre de usuario</param>
        /// <param name="strApiToken">string: Token</param>
        protected void SetCredentials(string strUserName, string strApiToken)
        {
            _strUserName = strUserName;
            _strApiToken = strApiToken;
        }

    }
}
