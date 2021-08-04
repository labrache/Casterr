using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CasterrLib.classes;
using Newtonsoft.Json;

namespace CastService.Classes
{
    public class appAuth
    {
        private HttpClient client;
        private String _hostname;
        public CookieContainer container = new CookieContainer();
        private string _username;
        private string _password;

        public appAuth(String hostname, String username, String password)
        {
            client = new HttpClient();
            _hostname = hostname;
            client.BaseAddress = new Uri(hostname);
            _username = username;
            _password = password;
        }
        public async Task<AuthenticateRes> auth()
        {
            AuthenticateRequest auth = new AuthenticateRequest()
            {
                Username = _username,
                Password = _password
            };
            var content = new StringContent(JsonConvert.SerializeObject(auth), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("ApiAuth/authenticate", content);
            response.EnsureSuccessStatusCode();
            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
                container.SetCookies(new Uri(_hostname), cookies.First());
            return await response.Content.ReadAsAsync<AuthenticateRes>();
        }
    }
}
