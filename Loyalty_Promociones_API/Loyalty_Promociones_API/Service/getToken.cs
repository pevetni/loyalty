using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty_Promociones_API.Service
{
    public class getToken
    {
        public async Task<Token> getTokenAsync(Credenciales credencial)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), credencial.uriToken.ToString()))
                {
                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(credencial.authorization.ToString()+":"));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                    var contentList = new List<string>();
                    contentList.Add("grant_type="+credencial.grant_type.ToString());
                    contentList.Add("username="+credencial.username.ToString());
                    contentList.Add("password="+credencial.password.ToString());
                    contentList.Add("scope="+credencial.scope.ToString());
                    request.Content = new StringContent(string.Join("&", contentList));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    var jsonContent = await response.Content.ReadAsStringAsync();

                    Token tok = JsonConvert.DeserializeObject<Token>(jsonContent);

                    return tok;
                }
            }
        }
    }
}
