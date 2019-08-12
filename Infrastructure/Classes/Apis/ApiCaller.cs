using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Infrastructure.Classes.Apis
{
   public static class ApiCaller
    {
        public static object ExecutePostRequest(object data, string requestedUrl)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent request = new StringContent(JsonConvert.SerializeObject(data))
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                };

                response = httpClient.PostAsync(requestedUrl, request).Result;
            }

            return response.Content.ReadAsAsync(typeof(object)).Result;
        }
    }
}
