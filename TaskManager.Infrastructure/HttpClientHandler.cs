using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TaskManager.Core.Interfaces;

namespace TaskManager.Infrastructure
{
    public class HttpClientHandler : IHttpCommandHandlers, IDisposable
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly IConfiguration _config;
        public HttpClientHandler(IConfiguration config)
        {
            _config = config;
            _client.BaseAddress = new Uri(_config["ConnectionStrings:baseUrl"]);
        }
        #region Api Request handlers
        /// <summary>
        /// this is a generic async post request method. it accept generic request and response type
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <typeparam name="TReq"></typeparam>
        /// <param name="requestModel"></param>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public async Task<TRes> PostRequest<TRes, TReq>(TReq requestModel, string requestUrl)
            where TRes : class
            where TReq : class
        {
            return await PostRequestAsync<TReq, TRes>(requestUrl, requestModel);
        }
        /// <summary>
        /// this is a generic async get request method. it takes in the URL as a parameter and return 
        /// a generic reponse
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public async Task<TRes> GetRequest<TRes>(string requestUrl)
           where TRes : class
        {
            return await GetRequestAsync<TRes>(requestUrl);
        }
        /// <summary>
        /// this method Handle delete request
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TRes> DeleteRequest<TRes>(string id)
           where TRes : class
        {
            return await DeleteRequestAsync<TRes>(id);
        }
        #endregion

        #region Implementatioin details
        /// <summary>
        /// Handles the get request implementation 
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        private async Task<TRes> GetRequestAsync<TRes>(string requestUrl) where TRes : class
        {
            var client = CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            return await GetResponseResultAsync<TRes>(client, request);
        }
        /// <summary>
        /// Handles delete request implementation
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<TRes> DeleteRequestAsync<TRes>(string id) where TRes : class
        {
            var client = CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, id.ToString());
            return await GetResponseResultAsync<TRes>(client, request);
        }
        /// <summary>
        /// Handles post request implementation
        /// </summary>
        /// <typeparam name="TReq"></typeparam>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="requestUrl"></param>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        private async Task<TRes> PostRequestAsync<TReq, TRes>(string requestUrl, TReq requestModel)
            where TReq : class
            where TRes : class
        {
            var caller = CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json")
            };
            return await GetResponseResultAsync<TRes>(caller, request);
        }
        /// <summary>
        /// Gets the deserialize object.
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="clientCaller"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task<TRes> GetResponseResultAsync<TRes>(HttpClient clientCaller, HttpRequestMessage request) where TRes : class
        {
            var response = await clientCaller.SendAsync(request);
            if (!response.IsSuccessStatusCode) throw new ArgumentException(response.ReasonPhrase);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TRes>(responseString);
            return result;
        }
        private HttpClient CreateClient()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return _client;
        }
        /// <summary>
        /// closed the opened connections
        /// </summary>
        public void Dispose()
        {
            _client.Dispose();
        }

        #endregion
    }
}
