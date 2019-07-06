using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gravityzero.Console.Utility.Tools
{
    public static class WinApiConnector
    {
        private static HttpClient client = new HttpClient();

        public async static Task<ConnectorResult<TResult>> RequestPost<TData, TResult>(string path, TData data) where TResult : new(){ 
            if (string.IsNullOrEmpty(path)) return new ConnectorResult<TResult>($"Path is empty.");
            string output = JsonConvert.SerializeObject(data);
            var content = new StringContent(output, Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PostAsync(path, content);
                if (response.IsSuccessStatusCode)
                {
                    var serverResponse = await response.Content.ReadAsStringAsync();
                    return SerializationHandler<TResult>(serverResponse);   
                }
                else
                {
                    return new ConnectorResult<TResult>($"Occurred code: {response.StatusCode.ToString()}");
                }
            }
            catch(ArgumentNullException ex)
            {
                return new ConnectorResult<TResult>(ex.ToString());
            }
            catch(HttpRequestException ex)
            {
                return new ConnectorResult<TResult>(ex.ToString());
            }
        }

        public async static Task<ConnectorResult<TResult>> RequestPut<TData, TResult>(string path, TData data) where TResult : new(){
            if (string.IsNullOrEmpty(path)) return new ConnectorResult<TResult>($"Path is empty.");
            try
            {
                string output = JsonConvert.SerializeObject(data);
                var requestContent = new StringContent(output, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(path, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return SerializationHandler<TResult>(content);
                }else{
                    return new ConnectorResult<TResult>($"Occurred code: {response.StatusCode.ToString()}");
                }
            }
            catch(ArgumentNullException ex)
            {
                return new ConnectorResult<TResult>(ex.ToString());
            }
            catch(HttpRequestException ex)
            {
                return new ConnectorResult<TResult>(ex.ToString());
            }
        }

        public async static Task<ConnectorResult<TResult>> RequestDelete<TResult>(string path) where TResult : new(){
            if (string.IsNullOrEmpty(path)) return new ConnectorResult<TResult>($"Path is empty.");
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return SerializationHandler<TResult>(content);   
                }else{
                    return new ConnectorResult<TResult>($"Occurred code: {response.StatusCode.ToString()}");
                }
            }
            catch(ArgumentNullException ex)
            {
                return new ConnectorResult<TResult>(ex.ToString());
            }
            catch(HttpRequestException ex)
            {
                return new ConnectorResult<TResult>(ex.ToString());
            }
        }

        public async static Task<ConnectorResult<TResult>> RequestGet<TResult>(string path) where TResult : new()
        {
            if (string.IsNullOrEmpty(path)) return new ConnectorResult<TResult>($"Path is empty.");
            try
            {
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return SerializationHandler<TResult>(content);                    
                }else{
                    return new ConnectorResult<TResult>($"Occurred code: {response.StatusCode.ToString()}");
                }
            }
            catch(ArgumentNullException ex)
            {
                return new ConnectorResult<TResult>(ex.ToString());
            }
            catch(HttpRequestException ex)
            {
                return new ConnectorResult<TResult>(ex.ToString());
            }
        }

        private static ConnectorResult<TResult> SerializationHandler<TResult>(string serializedContent) where TResult : new()
        {
            try
            {
                var result = JsonConvert.DeserializeObject<TResult>(serializedContent);
                return new ConnectorResult<TResult>(result);
            }
            catch(Exception ex)
            {
                return new ConnectorResult<TResult>(ex.ToString());
            }
        }

    }
}
