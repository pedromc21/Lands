namespace Lands.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    public class ApiService
    {
        //Valida la conexion del telefono, para esto se aprega la Nuget Xamarin.conecction = Plugin.Connectivity
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "por favor encienda su internet en ajustes de su dispositivo.",
                };
            }
            //ping a la pagina
            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "compruebe su conexión a internet",
                };
            }
            return new Response
            {
                IsSuccess = true,
                Message = "Ok",
            };
        }
        public async Task<TokenResponse> GetToken(
            string urlBase,
            string username,
            string password)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync("Token",
                    new StringContent(string.Format(
                        "grant_type=password&username={0}&password){1}", username, password),
                        Encoding.UTF8, "application/x-www-form-urlencoded"));
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenResponse>(
                    resultJSON);
                return result;
            }
            catch
            {
                return null;
            }
        }
        public async Task<Response> GetList<T>(
            string urlBase,
            string servicePrefix,
            string controller
            )
            //string tokenType, string accessToken
        {
            try
            {
                var client = new HttpClient();
                //client.DefaultRequestHeaders.Authorization =
                //    new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                //Dispara la consulta
                var response = await client.GetAsync(url);
                //Recibe la respuesta
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode )
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true ,
                    Message = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message, 
                };
            }
        }
    }
}
