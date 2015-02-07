using MediaManager.Code.Searches;
using MediaManager.Code.Series;
using MediaManager.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Code
{
    internal class Helper
    {
        private static Settings settings = Settings.Default;

        public enum Conteudo
        {
            Serie,
            Filme,
            Anime
        }

        public static List<Search> API_PesquisarConteudo(string query, string type)
        {
            var request = WebRequest.Create(settings.APIBaseUrl + "/search?query=" + query + "&type=" + type) as System.Net.HttpWebRequest;
            request.KeepAlive = true;

            request.Method = "GET";

            request.Headers.Add("trakt-api-version", "2");

            request.Headers.Add("trakt-api-key", "");

            request.ContentType = "application/json";

            string responseContent = null;

            using (var response = request.GetResponse() as System.Net.HttpWebResponse)
            {
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseContent = reader.ReadToEnd();
                    var searchResults = JsonConvert.DeserializeObject<List<Search>>(responseContent);
                    return searchResults;
                }
            }
        }

        public static Serie API_GetSerieImages(string id)
        {
            var request = WebRequest.Create(settings.APIBaseUrl + "/shows/" + id + "?extended=images") as System.Net.HttpWebRequest;
            request.KeepAlive = true;

            request.Method = "GET";

            request.ContentType = "application/json";

            request.Headers.Add("trakt-api-version", "2");

            request.Headers.Add("trakt-api-key", "");

            string responseContent = null;

            using (var response = request.GetResponse() as System.Net.HttpWebResponse)
            {
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseContent = reader.ReadToEnd();
                    var searchResults = JsonConvert.DeserializeObject<Serie>(responseContent);
                    return searchResults;
                }
            }
        }

        public static Translations API_GetSerieSinopse(string id)
        {
            // @TODO Fazer funcionar com o idioma definido nas configurações.
            try
            {
                var request = WebRequest.Create(settings.APIBaseUrl + "/shows/" + id + "/translations/pt") as System.Net.HttpWebRequest;
                request.KeepAlive = true;

                request.Method = "GET";

                request.ContentType = "application/json";

                request.Headers.Add("trakt-api-version", "2");

                request.Headers.Add("trakt-api-key", "");

                string responseContent = null;

                using (var response = request.GetResponse() as System.Net.HttpWebResponse)
                {
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                        var searchResults = JsonConvert.DeserializeObject<List<Translations>>(responseContent);
                        return searchResults[0];
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Auth API_GetAccessToken(string code)
        {
            var request = WebRequest.Create(settings.APIBaseUrl + "/oauth/token") as System.Net.HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";
            request.KeepAlive = true;

            string responseContent = null;
            Auth auth = null;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                var json = "{\"code\": \"" + code + "\", \"client_id\": \"" + settings.ClientID + "\", \"client_secret\": \"" + settings.ClientSecret + "\", \"redirect_uri\": \"" + settings.CallbackUrl + "\", \"grant_type\": \"authorization_code\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                using (var response = request.GetResponse() as System.Net.HttpWebResponse)
                {
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                        auth = JsonConvert.DeserializeObject<Auth>(responseContent);
                    }
                }
            }
            return auth;
        }

        public static Usuario API_GetUserSettings()
        {
            var request = WebRequest.Create(settings.APIBaseUrl + "/users/settings") as System.Net.HttpWebRequest;
            request.KeepAlive = true;

            request.Method = "GET";

            request.ContentType = "application/json";

            request.Headers.Add("authorization", "Bearer " + settings.pref_accessToken);

            request.Headers.Add("trakt-api-version", "2");

            request.Headers.Add("trakt-api-key", settings.ClientID);

            string responseContent = null;

            using (var response = request.GetResponse() as System.Net.HttpWebResponse)
            {
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseContent = reader.ReadToEnd();
                    var user = JsonConvert.DeserializeObject<Usuario>(responseContent);
                    return user;
                }
            }
        }
    }
}