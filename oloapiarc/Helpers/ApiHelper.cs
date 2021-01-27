using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using oloapiarc.DataModels;


namespace oloapiarc.HelperFiles
{
    public class ApiHelper
    {
        private readonly string _apiUrl;
        private CookieContainer cookieContainer;
        private HttpClientHandler handler;
        private HttpClient client;
        private bool _ensureSuccessStatus;

        public ApiHelper(string ApiUrl, bool ensureSuccessStatus = true)
        {
            _apiUrl = ApiUrl;
            cookieContainer = new CookieContainer();
            handler = new HttpClientHandler() { CookieContainer = cookieContainer, UseDefaultCredentials = true };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri(_apiUrl);
            _ensureSuccessStatus = ensureSuccessStatus;
        }

        public void Disconnect()
        {
            client.Dispose();
        }

        public async Task<string> RunEndpoint(BaseAPIModel apiModel, RequestType request)
        {
            var response = "";

            switch(request)
            {
                case RequestType.GET:
                    response = await GET(apiModel);
                    break;
                case RequestType.POST:
                    response = await POST(apiModel);
                    break;
                case RequestType.PUT:
                    //response = await PUT(apiModel);
                    break;
                case RequestType.PATCH:
                    //response = await GPATCHET(apiModel);
                    break;
                case RequestType.DELETE:
                    //response = await DELETE(apiModel);
                    break;
            }

            return response;
        }

        //POST
        public async Task<string> POST(BaseAPIModel apiModel)
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (apiModel.Authentication != null)
                {
                    if (apiModel.Authentication.Keys.ToString() == "SessionId")
                        cookieContainer.Add(client.BaseAddress, new Cookie(apiModel.Authentication.Keys.ToString(), apiModel.Authentication.Values.ToString()));
                    if (apiModel.Authentication.Keys.ToString() == "Bearer")
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(apiModel.Authentication.Keys.ToString(), apiModel.Authentication.Values.ToString());
                }

                HttpResponseMessage res;
                res = await client.PostAsJsonAsync(apiModel.GetFullUrl(), apiModel.body);

                if (_ensureSuccessStatus && !res.IsSuccessStatusCode)
                {
                    var errorContent = await res.Content.ReadAsStringAsync();
                    throw new HttpRequestException(errorContent);
                }

                return await res.Content.ReadAsStringAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        //GET
        private async Task<string> GET(BaseAPIModel apiModel)
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (apiModel.Authentication != null)
                {
                    if(apiModel.Authentication.Keys.ToString() == "SessionId")
                        cookieContainer.Add(client.BaseAddress, new Cookie(apiModel.Authentication.Keys.ToString(), apiModel.Authentication.Values.ToString()));
                    if(apiModel.Authentication.Keys.ToString() == "Bearer")
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(apiModel.Authentication.Keys.ToString(), apiModel.Authentication.Values.ToString());
                }

                HttpResponseMessage res = new HttpResponseMessage();
                res = await client.GetAsync(apiModel.GetFullUrl());

                if (_ensureSuccessStatus)
                    res.EnsureSuccessStatusCode();

                if (res.IsSuccessStatusCode)
                    return res.Content.ReadAsStringAsync().Result;
                else
                    return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //PUT
        public async Task<string> PUT(BaseAPIModel apiModel)
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (apiModel.Authentication != null)
                {
                    if (apiModel.Authentication.Keys.ToString() == "SessionId")
                        cookieContainer.Add(client.BaseAddress, new Cookie(apiModel.Authentication.Keys.ToString(), apiModel.Authentication.Values.ToString()));
                    if (apiModel.Authentication.Keys.ToString() == "Bearer")
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(apiModel.Authentication.Keys.ToString(), apiModel.Authentication.Values.ToString());
                }

                HttpResponseMessage res;
                res = await client.PutAsJsonAsync(apiModel.GetFullUrl(), apiModel.body);

                if (_ensureSuccessStatus)
                    res.EnsureSuccessStatusCode();

                return res.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //PATCH
        public async Task<string> PATCH(BaseAPIModel apiModel)
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (apiModel.Authentication != null)
                {
                    if (apiModel.Authentication.Keys.ToString() == "SessionId")
                        cookieContainer.Add(client.BaseAddress, new Cookie(apiModel.Authentication.Keys.ToString(), apiModel.Authentication.Values.ToString()));
                    if (apiModel.Authentication.Keys.ToString() == "Bearer")
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(apiModel.Authentication.Keys.ToString(), apiModel.Authentication.Values.ToString());
                }
                
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), apiModel.GetFullUrl());

                var dataString = "";
                dataString = JsonConvert.SerializeObject(apiModel.body);
                request.Content = new StringContent(dataString, Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.SendAsync(request);
                if (_ensureSuccessStatus)
                    res.EnsureSuccessStatusCode();

                return res.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        //DELETE
        public async Task<string> DELETE(BaseAPIModel apiModel)
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (apiModel.Authentication != null)
                {
                    if (apiModel.Authentication.Keys.ToString() == "SessionId")
                        cookieContainer.Add(client.BaseAddress, new Cookie(apiModel.Authentication.Keys.ToString(), apiModel.Authentication.Values.ToString()));
                    if (apiModel.Authentication.Keys.ToString() == "Bearer")
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(apiModel.Authentication.Keys.ToString(), apiModel.Authentication.Values.ToString());
                }

                var res = await client.DeleteAsync(apiModel.GetFullUrl());
                if (_ensureSuccessStatus)
                    res.EnsureSuccessStatusCode();

                return res.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
