using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaManager.Model;
using MediaManager.Properties;
using Newtonsoft.Json;

namespace MediaManager.Helpers
{
    public class Helper
    {
        private static Settings settings = Settings.Default;

        /// <summary>
        /// Define o tipo de conteúdo a ser usado.
        /// </summary>
        public enum TipoConteudo
        {
            movie,
            show,
            anime,
            season,
            episode,
            person,
            movieShowAnime
        }

        #region { APIs trakt }

        /// <summary>
        /// Realiza a troca do código do trakt pelo access token necessário para realizar as transações específicas de usuário.
        /// </summary>
        /// <param name="code">Código a ser trocado.</param>
        /// <returns>Access token</returns>
        public async static Task<UserAuth> API_GetAccessTokenAsync(string code)
        {
            string responseData = "";

            using (var httpClient = new HttpClient { BaseAddress = new Uri(settings.APIBaseUrl) })
            {
                using (var content = new StringContent("{  \"code\": " + code + ",  \"client_id\": " + settings.ClientID + ",  \"client_secret\": " + settings.ClientSecret + ",  \"redirect_uri\": " + settings.CallbackUrl + ",  \"grant_type\": \"authorization_code\"}", System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("oauth/token", content))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return JsonConvert.DeserializeObject<UserAuth>(responseData);
        }

        /// <summary>
        /// Realiza uma pesquisa pelo filme no Trakt.tv baseado no slug (Id) do trakt
        /// </summary>
        /// <param name="slugTrakt">Slug (id) do trakt. Ex.: breaking-bad</param>
        /// <returns>Objeto contendo as informações do filme</returns>
        public async static Task<Filme> API_GetFilmeInfoAsync(string slugTrakt)
        {
            string responseData = "";

            Filme filme = new Filme();

            using (var httpClient = new HttpClient { BaseAddress = new Uri(settings.APIBaseUrl) })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "");

                using (var response = await httpClient.GetAsync("movies/" + slugTrakt + "?extended=full,images"))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            filme = JsonConvert.DeserializeObject<Filme>(responseData);
            var traducoes = new { available_translations = JsonConvert.DeserializeObject(responseData) };
            filme.MetadataFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Properties.Settings.Default.AppName, "Metadata", "Filmes", Helpers.Helper.RetirarCaracteresInvalidos(filme.Title));
            if (settings.pref_PastaFilmes != "")
                filme.FolderPath = System.IO.Path.Combine(settings.pref_PastaFilmes, Helper.RetirarCaracteresInvalidos(filme.Title));
            filme.Traducoes = ListToString(filme.AvailableTranslations.ToList());
            filme.Generos = ListToString(filme.Genres.ToList());
            return filme;
        }

        /// <summary>
        /// Realiza uma pesquisa pelo filme no Trakt.tv baseado no slug (Id) do trakt para retornar a sinopse traduzida do filme
        /// </summary>
        /// <param name="slugTrakt">Slug (id) do trakt. Ex.: breaking-bad</param>
        /// <returns>Objeto contendo as informações traduzidas do filme</returns>
        public async static Task<Filme> API_GetFilmeSinopseAsync(string slugTrakt)
        {
            string responseData = "";

            using (var httpClient = new HttpClient { BaseAddress = new Uri(settings.APIBaseUrl) })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "");

                using (var response = await httpClient.GetAsync("movies/" + slugTrakt + "/translations/pt"))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            return JsonConvert.DeserializeObject<List<Filme>>(responseData)[0];
        }

        public async static Task<Serie> API_GetSerieInfoAsync(string slugTrakt, Helper.TipoConteudo tipoConteudo)
        {
            string responseData = "";

            Serie serie = new Serie();

            using (var httpClient = new HttpClient { BaseAddress = new Uri(settings.APIBaseUrl) })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "");

                using (var response = await httpClient.GetAsync("shows/" + slugTrakt + "?extended=full,images"))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            serie = JsonConvert.DeserializeObject<Serie>(responseData);

            if (tipoConteudo == TipoConteudo.anime)
            {
                serie.IsAnime = true;
                serie.MetadataFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    Properties.Settings.Default.AppName, "Metadata", "Animes", Helpers.Helper.RetirarCaracteresInvalidos(serie.Title));
                if (settings.pref_PastaAnimes != "")
                    serie.FolderPath = System.IO.Path.Combine(settings.pref_PastaAnimes, Helper.RetirarCaracteresInvalidos(serie.Title));
            }
            else if (tipoConteudo == TipoConteudo.show)
            {
                serie.MetadataFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    Properties.Settings.Default.AppName, "Metadata", "Séries", RetirarCaracteresInvalidos(serie.Title));
                if (settings.pref_PastaSeries != "")
                    serie.FolderPath = System.IO.Path.Combine(settings.pref_PastaSeries, Helper.RetirarCaracteresInvalidos(serie.Title));
            }
            serie.Traducoes = ListToString(serie.AvailableTranslations.ToList());
            serie.Generos = ListToString(serie.Genres.ToList());
            return serie;
        }

        public async static Task<Serie> API_GetSerieSinopseAsync(string slugTrakt)
        {
            string responseData = "";

            List<Serie> series = new List<Serie>();

            using (var httpClient = new HttpClient { BaseAddress = new Uri(settings.APIBaseUrl) })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "");

                using (var response = await httpClient.GetAsync("shows/" + slugTrakt + "/translations/pt"))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            series = JsonConvert.DeserializeObject<List<Serie>>(responseData);
            return (series.Count > 0) ? series[0] : new Serie(); ;
        }

        /// <summary>
        /// Pega as configurações da conta no trakt.tv.
        /// </summary>
        /// <returns>Objeto com todas as configurações</returns>
        public async static Task<UserInfo> API_GetUserSettingsAsync()
        {
            string responseData = "";

            using (var httpClient = new HttpClient { BaseAddress = new Uri(settings.APIBaseUrl) })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", settings.ClientID);

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + settings.user_accessToken);

                using (var response = await httpClient.GetAsync("users/settings"))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            return JsonConvert.DeserializeObject<UserInfo>(responseData);
        }

        public static async Task<List<Search>> API_PesquisarConteudoAsync(string query, string type)
        {
            string responseData = "";

            if (type == Helper.TipoConteudo.anime.ToString())
                type = Helper.TipoConteudo.show.ToString();

            using (var httpClient = new HttpClient { BaseAddress = new Uri(settings.APIBaseUrl) })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "");

                using (var response = await httpClient.GetAsync("search?query=" + query + "&type=" + type))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            return JsonConvert.DeserializeObject<List<Search>>(responseData);
        }

        #region { OLD API Methods }

        //public static List<Search> API_PesquisarConteudo(string query, string type)
        //{
        //    var request = WebRequest.Create(settings.APIBaseUrl + "/search?query=" + query + "&type=" + type) as System.Net.HttpWebRequest;
        //    request.KeepAlive = true;

        //    request.Method = "GET";

        //    request.Headers.Add("trakt-api-version", "2");

        //    request.Headers.Add("trakt-api-key", "");

        //    request.ContentType = "application/json";

        //    string responseContent = null;

        //    using (var response = request.GetResponse() as System.Net.HttpWebResponse)
        //    {
        //        using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
        //        {
        //            responseContent = reader.ReadToEnd();
        //            var searchResults = JsonConvert.DeserializeObject<List<Search>>(responseContent);
        //            return searchResults;
        //        }
        //    }
        //}

        //public static Serie API_GetSerieInfo(string slugTrakt)
        //{
        //    var request = WebRequest.Create(settings.APIBaseUrl + "/shows/" + slugTrakt + "?extended=full,images") as System.Net.HttpWebRequest;
        //    request.KeepAlive = true;

        //    request.Method = "GET";

        //    request.ContentType = "application/json";

        //    request.Headers.Add("trakt-api-version", "2");

        //    request.Headers.Add("trakt-api-key", "");

        //    string responseContent = null;

        //    using (var response = request.GetResponse() as System.Net.HttpWebResponse)
        //    {
        //        using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
        //        {
        //            responseContent = reader.ReadToEnd();
        //            var searchResults = JsonConvert.DeserializeObject<Serie>(responseContent);
        //            return searchResults;
        //        }
        //    }
        //}

        //public static Serie API_GetSerieSinopse(string slugTrakt)
        //{
        //    // TODO Fazer funcionar com o idioma definido nas configurações.
        //    try
        //    {
        //        var request = WebRequest.Create(settings.APIBaseUrl + "/shows/" + slugTrakt + "/translations/pt") as System.Net.HttpWebRequest;
        //        request.KeepAlive = true;

        //        request.Method = "GET";

        //        request.ContentType = "application/json";

        //        request.Headers.Add("trakt-api-version", "2");

        //        request.Headers.Add("trakt-api-key", "");

        //        string responseContent = null;

        //        using (var response = request.GetResponse() as System.Net.HttpWebResponse)
        //        {
        //            using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
        //            {
        //                responseContent = reader.ReadToEnd();
        //                var searchResults = JsonConvert.DeserializeObject<List<Serie>>(responseContent);
        //                return searchResults[0];
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //public static Filme API_GetFilmeInfo(string slugTrakt)
        //{
        //    var request = WebRequest.Create(settings.APIBaseUrl + "/movies/" + slugTrakt + "?extended=full,images") as System.Net.HttpWebRequest;
        //    request.KeepAlive = true;

        //    request.Method = "GET";

        //    request.ContentType = "application/json";

        //    request.Headers.Add("trakt-api-version", "2");

        //    request.Headers.Add("trakt-api-key", "");

        //    string responseContent = null;

        //    using (var response = request.GetResponse() as System.Net.HttpWebResponse)
        //    {
        //        using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
        //        {
        //            responseContent = reader.ReadToEnd();
        //            var searchResults = JsonConvert.DeserializeObject<Filme>(responseContent);
        //            return searchResults;
        //        }
        //    }
        //}

        //public static Filme API_GetFilmeSinopse(string slugTrakt)
        //{
        //    // TODO Fazer funcionar com o idioma definido nas configurações.
        //    try
        //    {
        //        var request = WebRequest.Create(settings.APIBaseUrl + "/movies/" + slugTrakt + "/translations/pt") as System.Net.HttpWebRequest;
        //        request.KeepAlive = true;

        //        request.Method = "GET";

        //        request.ContentType = "application/json";

        //        request.Headers.Add("trakt-api-version", "2");

        //        request.Headers.Add("trakt-api-key", "");

        //        string responseContent = null;

        //        using (var response = request.GetResponse() as System.Net.HttpWebResponse)
        //        {
        //            using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
        //            {
        //                responseContent = reader.ReadToEnd();
        //                var searchResults = JsonConvert.DeserializeObject<List<Filme>>(responseContent);
        //                return searchResults[0];
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //public static UserAuth API_GetAccessToken(string code)
        //{
        //    var request = WebRequest.Create(settings.APIBaseUrl + "/oauth/token") as System.Net.HttpWebRequest;
        //    request.ContentType = "application/json";
        //    request.Method = "POST";
        //    request.KeepAlive = true;

        //    string responseContent = null;
        //    UserAuth auth = null;

        //    var streamWriter = new StreamWriter(request.GetRequestStream());

        //    var json = "{\"code\": \"" + code + "\", \"client_id\": \"" + settings.ClientID + "\", \"client_secret\": \"" + settings.ClientSecret + "\", \"redirect_uri\": \"" + settings.CallbackUrl + "\", \"grant_type\": \"authorization_code\"}";

        //    streamWriter.Write(json);
        //    streamWriter.Flush();
        //    streamWriter.Close();

        //    using (var response = request.GetResponse() as System.Net.HttpWebResponse)
        //    {
        //        using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
        //        {
        //            responseContent = reader.ReadToEnd();
        //            auth = JsonConvert.DeserializeObject<UserAuth>(responseContent);
        //        }
        //    }

        //    return auth;
        //}

        ///// <summary>
        ///// Pega as configurações da conta no trakt.tv.
        ///// </summary>
        ///// <returns>Objeto com todas as configurações</returns>
        //public static UserInfo API_GetUserSettings()
        //{
        //    var request = WebRequest.Create(settings.APIBaseUrl + "/users/settings") as System.Net.HttpWebRequest;
        //    request.KeepAlive = true;

        //    request.Method = "GET";

        //    request.ContentType = "application/json";

        //    request.Headers.Add("authorization", "Bearer " + settings.user_accessToken);

        //    request.Headers.Add("trakt-api-version", "2");

        //    request.Headers.Add("trakt-api-key", settings.ClientID);

        //    string responseContent = null;

        //    UserInfo user = null;

        //    using (var response = request.GetResponse() as System.Net.HttpWebResponse)
        //    {
        //        using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
        //        {
        //            responseContent = reader.ReadToEnd();
        //            user = JsonConvert.DeserializeObject<UserInfo>(responseContent);
        //        }
        //    }
        //    return user;
        //}

        #endregion { OLD API Methods }

        #endregion { APIs trakt }

        /// <summary>
        /// Adiciona mensagem no log.
        /// </summary>
        /// <param name="message">Mensagem a ser adicionada</param>
        /// <returns>Retorna false se ocorrer um erro</returns>
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

        public static IEnumerable<FileInfo> PesquisarArquivosPorExtensao(DirectoryInfo dir, params string[] extensao)
        {
            if (extensao == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensao.Contains(f.Extension));
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

        /// <summary>
        /// Retorna os subdiretórios do diretório de séries, configurado nas preferências.
        /// </summary>
        /// <returns></returns>
        public static DirectoryInfo[] retornarDiretoriosSeries()
        {
            // TODO Validação para quando não tem pasta nas preferências.
            DirectoryInfo dir = new DirectoryInfo(settings.pref_PastaSeries);
            return dir.GetDirectories();
        }

        /// <summary>
        /// Retorna os subdiretórios do diretório de animes, configurado nas preferências.
        /// </summary>
        /// <returns></returns>
        public static DirectoryInfo[] retornarDiretoriosAnimes()
        {
            DirectoryInfo dir = new DirectoryInfo(settings.pref_PastaAnimes);
            return dir.GetDirectories();
        }

        /// <summary>
        /// Retorna os subdiretórios do diretório de filmes, configurado nas preferências.
        /// </summary>
        /// <returns></returns>
        public static DirectoryInfo[] retornarDiretoriosFilmes()
        {
            DirectoryInfo dir = new DirectoryInfo(settings.pref_PastaFilmes);
            return dir.GetDirectories();
        }

        public static string ListToString(IList<string> list)
        {
            var s = "";
            foreach (var item in list)
            {
                s += item + "|";
            }
            if (s != "")
                return s.Remove(s.Length - 1);
            else
                return null;
        }
    }
}