using MediaManager.Code.Modelos;
using MediaManager.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaManager.Code
{
    public class Helper
    {
        private static Settings settings = Settings.Default;

        /// <summary>
        /// Define o tipo de conteúdo a ser usado.
        /// </summary>
        public enum Conteudo
        {
            Serie,
            Filme,
            Anime,
            Tudo
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

        public static Serie API_GetSerieInfo(string slugTrakt)
        {
            var request = WebRequest.Create(settings.APIBaseUrl + "/shows/" + slugTrakt + "?extended=full,images") as System.Net.HttpWebRequest;
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

        public static Serie API_GetSerieSinopse(string slugTrakt)
        {
            // TODO Fazer funcionar com o idioma definido nas configurações.
            try
            {
                var request = WebRequest.Create(settings.APIBaseUrl + "/shows/" + slugTrakt + "/translations/pt") as System.Net.HttpWebRequest;
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
                        var searchResults = JsonConvert.DeserializeObject<List<Serie>>(responseContent);
                        return searchResults[0];
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Filme API_GetFilmeInfo(string slugTrakt)
        {
            var request = WebRequest.Create(settings.APIBaseUrl + "/movies/" + slugTrakt + "?extended=full,images") as System.Net.HttpWebRequest;
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
                    var searchResults = JsonConvert.DeserializeObject<Filme>(responseContent);
                    return searchResults;
                }
            }
        }

        public static Filme API_GetFilmeSinopse(string slugTrakt)
        {
            // TODO Fazer funcionar com o idioma definido nas configurações.
            try
            {
                var request = WebRequest.Create(settings.APIBaseUrl + "/movies/" + slugTrakt + "/translations/pt") as System.Net.HttpWebRequest;
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
                        var searchResults = JsonConvert.DeserializeObject<List<Filme>>(responseContent);
                        return searchResults[0];
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static UserAuth API_GetAccessToken(string code)
        {
            var request = WebRequest.Create(settings.APIBaseUrl + "/oauth/token") as System.Net.HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";
            request.KeepAlive = true;

            string responseContent = null;
            UserAuth auth = null;

            var streamWriter = new StreamWriter(request.GetRequestStream());

            var json = "{\"code\": \"" + code + "\", \"client_id\": \"" + settings.ClientID + "\", \"client_secret\": \"" + settings.ClientSecret + "\", \"redirect_uri\": \"" + settings.CallbackUrl + "\", \"grant_type\": \"authorization_code\"}";

            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();

            using (var response = request.GetResponse() as System.Net.HttpWebResponse)
            {
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseContent = reader.ReadToEnd();
                    auth = JsonConvert.DeserializeObject<UserAuth>(responseContent);
                }
            }

            return auth;
        }

        /// <summary>
        /// Pega as configurações da conta no trakt.tv.
        /// </summary>
        /// <returns>Objeto com todas as configurações</returns>
        public static UserInfo API_GetUserSettings()
        {
            var request = WebRequest.Create(settings.APIBaseUrl + "/users/settings") as System.Net.HttpWebRequest;
            request.KeepAlive = true;

            request.Method = "GET";

            request.ContentType = "application/json";

            request.Headers.Add("authorization", "Bearer " + settings.user_accessToken);

            request.Headers.Add("trakt-api-version", "2");

            request.Headers.Add("trakt-api-key", settings.ClientID);

            string responseContent = null;

            UserInfo user = null;

            using (var response = request.GetResponse() as System.Net.HttpWebResponse)
            {
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseContent = reader.ReadToEnd();
                    user = JsonConvert.DeserializeObject<UserInfo>(responseContent);
                }
            }
            return user;
        }

        /// <summary>
        /// Retira os caracteres que o windows não aceita na criação de pastas e arquivos.
        /// </summary>
        /// <param name="nome">Nome do arquivo a ser normalizado.</param>
        /// <returns>Nome sem os caracteres não permitidos.</returns>
        public static string RetirarCaracteresInvalidos(string nome)
        {
            string nomeNormalizado = nome.Replace("\\", "").Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace("\"", "").Replace("<", "").Replace(">", "").Replace("|", "");
            return nomeNormalizado;
        }

        public static bool LogMessageToFile(string message)
        {
            bool sucesso = false;
            StreamWriter sw = File.AppendText(Environment.CurrentDirectory + "//" + settings.AppName + ".log");
            try
            {
                string logLine = "## " + System.DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy") + " ## " + message;
                sw.WriteLine(logLine);
                sucesso = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possivel registrar a mensagem no log.\n" + ex.Message, settings.AppName + " - Erro ao registrar log", MessageBoxButtons.OK, MessageBoxIcon.Information);
                sucesso = false;
                Environment.Exit(0);
            }
            finally
            {
                sw.Close();
            }
            return sucesso;
        }

        public static bool LogMessageToFile(string message, bool start)
        {
            bool sucesso = false;
            string logPath = Environment.CurrentDirectory + "//" + settings.AppName + ".log";
            StringBuilder logLine;
            if (File.Exists(logPath))
                logLine = new StringBuilder("\n");
            else
                logLine = new StringBuilder("");
            StreamWriter sw = File.AppendText(logPath);
            try
            {
                logLine.Append("####################################################################################################\n");
                logLine.Append("############################################ " + settings.AppName + " ###########################################\n");
                logLine.Append("####################################################################################################\n");
                logLine.Append("\n");
                logLine.Append("## " + System.DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy") + " ## " + message);
                sw.WriteLine(logLine);
                sucesso = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possivel registrar a mensagem no log.\n" + ex.Message, settings.AppName + " - Erro ao registrar log", MessageBoxButtons.OK, MessageBoxIcon.Information);
                sucesso = false;
            }
            finally
            {
                sw.Close();
            }
            return sucesso;
        }
    }
}