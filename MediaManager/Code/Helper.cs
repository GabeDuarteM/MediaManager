using MediaManager.Code.Searches;
using MediaManager.Code.Series;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Code
{
    internal class Helper
    {
        public enum Conteudo
        {
            Serie,
            Filme,
            Anime
        }

        public static List<Search> API_PesquisarConteudo(string query, string type)
        {
            var request = WebRequest.Create("https://api.trakt.tv/search?query=" + query + "&type=" + type) as System.Net.HttpWebRequest;
            request.KeepAlive = true;

            request.Method = "GET";

            request.Headers.Add("trakt-api-version", "2");

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

        public static Serie API_GetSerieInfo(string id)
        {
            var request = WebRequest.Create("https://api.trakt.tv/shows/" + id + "?extended=images") as System.Net.HttpWebRequest;
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
    }
}