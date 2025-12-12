using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Sample.Common.Heplers
{
    public enum HttpMethods
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public static class RequestHelper
    {
        public static async Task<HttpResponseMessage> RequestAsync(
            HttpMethods method, string baseUri, string path, object data = null,
            Dictionary<string, string> requestHeaders = null)
        {
            //Create httpClient
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            requestHeaders = requestHeaders ?? new Dictionary<string, string>();
            foreach (var item in requestHeaders)
            {
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
            client.BaseAddress = new Uri(baseUri);

            //Request
            HttpResponseMessage response = null;

            switch (method)
            {
                case HttpMethods.GET:
                    client.BaseAddress = new Uri(baseUri);
                    response = client.GetAsync(path)
                        .ConfigureAwait(false).GetAwaiter().GetResult();
                    break;
                case HttpMethods.POST:
                    response = client
                      .PostAsync(path, new StringContent(JsonConvert.SerializeObject(data),
                          System.Text.Encoding.UTF8, "application/json")).ConfigureAwait(false).GetAwaiter().GetResult();
                    break;
                case HttpMethods.PUT:
                    response = client
                        .PutAsync(path, new StringContent(JsonConvert.SerializeObject(data),
                            System.Text.Encoding.UTF8, "application/json"))
                        .ConfigureAwait(false).GetAwaiter().GetResult();
                    break;
                case HttpMethods.DELETE:
                    client.BaseAddress = new Uri(baseUri);
                    response = client.DeleteAsync(path)
                        .ConfigureAwait(false).GetAwaiter().GetResult();
                    break;
                default:
                    break;
            }

            return response;
        }

        public static async Task<T> RequestAsync<T>(
            HttpMethods method, string baseUri, string path, object data = null,
            Dictionary<string, string> requestHeaders = null) where T : class
        {
            var response = await RequestAsync(method, baseUri, path, data, requestHeaders);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(content))
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            else
                return default(T);
        }

        public static async Task<HttpResponseMessage> HttpPostWithFormDataAsync(string baseUri, string path, Dictionary<string, string> data, Dictionary<string, string> requestHeaders = null)
        {
            //Create httpClient
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            requestHeaders = requestHeaders ?? new Dictionary<string, string>();
            foreach (var item in requestHeaders)
            {
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
            client.BaseAddress = new Uri(baseUri);

            //Request
            HttpResponseMessage response = await client.PostAsync(path, new FormUrlEncodedContent(data));

            return response;
        }

        public static async Task<T> HttpPostWithFormDataAsync<T>(string baseUri, string path, Dictionary<string, string> data, Dictionary<string, string> requestHeaders = null) where T : class
        {
            var response = await HttpPostWithFormDataAsync(baseUri, path, data, requestHeaders);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(content))
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            else
                return default(T);
        }
    }
}
