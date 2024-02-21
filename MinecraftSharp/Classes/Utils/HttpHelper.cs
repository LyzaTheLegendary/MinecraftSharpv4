using MinecraftSharp.Classes.Json;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace MinecraftSharp.Classes.Utils
{
    public static class HttpHelper
    {
        public static T Get<T>(string url)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.GetAsync(url).GetAwaiter().GetResult();
            
            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
        //public static ApiResonse Post<T>(string url, T data)
        //{
        //    HttpClient httpClient = new HttpClient();
        //    HttpContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.ASCII, "application/json");
            
        //    HttpResponseMessage response = httpClient.PostAsync(url, content).GetAwaiter().GetResult();
        //    if (response.StatusCode == HttpStatusCode.NoContent)
        //        return new ApiResonse { name = "none", result = "none" };

        //    return JsonConvert.DeserializeObject<ApiResonse>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        //}
    }
}
