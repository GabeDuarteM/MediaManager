using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using MediaManager.Model;
using MediaManager.Properties;
using Newtonsoft.Json;

namespace MediaManager.Helpers
{
    public class Helper
    {
        private static Settings settings = Settings.Default;

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool CreateHardLink(
          string lpFileName,
          string lpExistingFileName,
          IntPtr lpSecurityAttributes
            );

        public static async Task<bool> DownloadImagesAsync(Video video, Enums.TipoImagem tipoImagem = Enums.TipoImagem.Todos)
        {
            try
            {
                if (tipoImagem == Enums.TipoImagem.Todos || tipoImagem == Enums.TipoImagem.Poster)
                {
                    if (video.sDsImgPoster != new Serie().sDsImgPoster)
                    {
                        using (System.Net.WebClient wc = new System.Net.WebClient())
                        {
                            var path = Path.Combine(video.sDsMetadata, "poster.jpg");
                            await wc.DownloadFileTaskAsync(new Uri(video.sDsImgPoster), path);
                        }
                    }
                }
                if (tipoImagem == Enums.TipoImagem.Todos || tipoImagem == Enums.TipoImagem.Fanart)
                {
                    if (video.sDsImgFanart != new Serie().sDsImgFanart)
                    {
                        using (System.Net.WebClient wc = new System.Net.WebClient())
                        {
                            var path = Path.Combine(video.sDsMetadata, "fanart.jpg");
                            await wc.DownloadFileTaskAsync(new Uri(video.sDsImgFanart), path);
                        }
                    }
                }
                return true;
            }
            catch (Exception e) { TratarException(e, "Ocorreu um erro ao realizar o download das imagens.", true); return false; }
        }

        public static bool DownloadImages(Video video, Enums.TipoImagem tipoImagem = Enums.TipoImagem.Todos)
        {
            try
            {
                if (tipoImagem == Enums.TipoImagem.Todos || tipoImagem == Enums.TipoImagem.Poster)
                {
                    if (video.sDsImgPoster != new Serie().sDsImgPoster)
                    {
                        using (System.Net.WebClient wc = new System.Net.WebClient())
                        {
                            var path = Path.Combine(video.sDsMetadata, "poster.jpg");
                            wc.DownloadFile(new Uri(video.sDsImgPoster), path);
                        }
                    }
                }
                if (tipoImagem == Enums.TipoImagem.Todos || tipoImagem == Enums.TipoImagem.Fanart)
                {
                    if (video.sDsImgFanart != new Serie().sDsImgFanart)
                    {
                        using (System.Net.WebClient wc = new System.Net.WebClient())
                        {
                            var path = Path.Combine(video.sDsMetadata, "fanart.jpg");
                            wc.DownloadFile(new Uri(video.sDsImgFanart), path);
                        }
                    }
                }
                return true;
            }
            catch (Exception e) { TratarException(e, "Ocorreu um erro ao realizar o download das imagens.", true); return false; }
        }

        public static string ListToString(IList<string> lista)
        {
            if (lista != null)
            {
                string strGeneros = "";
                foreach (var item in lista)
                {
                    strGeneros += item + "|";
                }
                if (strGeneros != "")
                    return strGeneros.Remove(strGeneros.Length - 1);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        public static bool RealizarPosProcessamento(Episodio item)
        {
            switch ((Enums.MetodoDeProcessamento)Settings.Default.pref_MetodoDeProcessamento)
            {
                case Enums.MetodoDeProcessamento.HardLink:
                    {
                        if (CreateHardLink(item.sDsFilepath, item.sDsFilepathOriginal, IntPtr.Zero))
                            return true;
                        else { TratarException(new Exception("Código: " + Marshal.GetLastWin32Error() + "\r\nArquivo: " + item.sDsFilepath), "Ocorreu um erro ao criar o " + Settings.Default.pref_MetodoDeProcessamento.ToString()); return false; }
                    }
                case Enums.MetodoDeProcessamento.Copiar:
                    {
                        try
                        {
                            File.Copy(item.sDsFilepathOriginal, item.sDsFilepath);
                            return true;
                        }
                        catch (Exception e) { TratarException(e, "Ocorreu um erro ao criar o " + ((Enums.MetodoDeProcessamento)Settings.Default.pref_MetodoDeProcessamento).ToString(), true); return false; }
                    }
                default:
                    TratarException(new ArgumentException("Método de processamento não reconhecido ou inválido."), "Ocorreu um erro ao realizar o pós processamento.", true);
                    return false;
            }
        }

        public static string RenomearConformePreferencias(Episodio episodio, string formato = null)
        {
            if (formato == null)
            {
                switch (episodio.nIdTipoConteudo)
                {
                    case Enums.TipoConteudo.Filme: // TODO Funcionar com filmes
                        break;

                    case Enums.TipoConteudo.Série:
                        formato = settings.pref_FormatoSeries;
                        break;

                    case Enums.TipoConteudo.Anime:
                        formato = settings.pref_FormatoAnimes;
                        break;

                    default:
                        TratarException(new ArgumentException("Episodio informado é de um tipo inválido."), IsSilencioso: true);
                        return null;
                }
            }

            Regex regex = new Regex("(?:{(?<tag>.*?)})");

            foreach (Match tag in regex.Matches(formato))
            {
                switch (tag.Value)
                {
                    case "{Titulo}":
                        formato = formato.Replace(tag.Value, episodio.oSerie.sDsTitulo);
                        break;

                    case "{TituloEpisodio}":
                        formato = formato.Replace(tag.Value, episodio.sDsEpisodio);
                        break;

                    case "{Temporada}":
                        formato = formato.Replace(tag.Value, episodio.nNrTemporada.ToString("00") + "");
                        break;

                    case "{Episodio}":
                        {
                            string ep = "";
                            foreach (var item in episodio.lstIntEpisodios)
                            {
                                if (ep == "")
                                    ep = item.ToString("00");
                                else
                                    ep += " & " + item.ToString("00");
                            }
                            formato = formato.Replace(tag.Value, ep);
                            break;
                        }
                    case "{Absoluto}":
                        {
                            string ep = "";
                            foreach (var item in episodio.lstIntEpisodiosAbsolutos)
                            {
                                if (ep == "")
                                    ep = item.ToString("00");
                                else
                                    ep += " & " + item.ToString("00");
                            }
                            formato = formato.Replace(tag.Value, ep);
                            break;
                        }
                    case "{SxEE}":
                        {
                            string ep = "";
                            foreach (var item in episodio.lstIntEpisodios)
                            {
                                if (ep == "")
                                    ep = item.ToString("00");
                                else
                                    ep += "x" + item.ToString("00");
                            }
                            formato = formato.Replace(tag.Value, episodio.nNrTemporada + "x" + ep);
                            break;
                        }
                    case "{S00E00}":
                        {
                            string ep = "";
                            foreach (var item in episodio.lstIntEpisodios)
                            {
                                if (ep == "")
                                    ep = item.ToString("00");
                                else
                                    ep += "E" + item.ToString("00");
                            }
                            formato = formato.Replace(tag.Value, "S" + episodio.nNrTemporada.ToString("00") + "E" + ep);
                            break;
                        }
                    default:
                        break;
                }
            }

            return RetirarCaracteresInvalidos(formato, false); // TODO Corrigir quando é anime o SxEE e o S00E00 para retornar o n do ep normal e não o absoluto.
        }

        public static string ColocarVirgula(string frase, List<string> adicional)
        {
            foreach (var item in adicional)
            {
                if (item == adicional[0])
                {
                    frase += item;
                }
                else if (item == adicional.Last())
                {
                    frase += " e " + item;
                }
                else
                {
                    frase += ", " + item;
                }
            }
            return frase;
        }

        public static void TratarException(Exception exception, string mensagem = "Ocorreu um erro na aplicação.", bool IsSilencioso = true)
        {
            if (mensagem.Last() != '.')
            {
                mensagem += ".";
            }
            if (!string.IsNullOrWhiteSpace(exception.Message))
            {
                mensagem += "\r\nDetalhes: " + exception.Message;
            }
            if (exception.StackTrace != null)
            {
                mensagem += "\r\nStackTrace: " + exception.StackTrace;
            }

            if (IsSilencioso)
            {
                LogMessage(mensagem);
            }
            else
            {
                MessageBox.Show(mensagem, Settings.Default.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static int CalcularAlgoritimoLevenshtein(string origem, string destino)
        {
            if (string.IsNullOrEmpty(origem))
            {
                if (string.IsNullOrEmpty(destino)) return 0;
                return destino.Length;
            }
            if (string.IsNullOrEmpty(destino)) return origem.Length;

            if (origem.Length > destino.Length)
            {
                var temp = destino;
                destino = origem;
                origem = temp;
            }

            var m = destino.Length;
            var n = origem.Length;
            var distance = new int[2, m + 1];
            // Initialize the distance 'matrix'
            for (var j = 1; j <= m; j++) distance[0, j] = j;

            var currentRow = 0;
            for (var i = 1; i <= n; ++i)
            {
                currentRow = i & 1;
                distance[currentRow, 0] = i;
                var previousRow = currentRow ^ 1;
                for (var j = 1; j <= m; j++)
                {
                    var cost = (destino[j - 1] == origem[i - 1] ? 0 : 1);
                    distance[currentRow, j] = Math.Min(Math.Min(
                                distance[previousRow, j] + 1,
                                distance[currentRow, j - 1] + 1),
                                distance[previousRow, j - 1] + cost);
                }
            }
            return distance[currentRow, m];
        }

        public static Dictionary<string, int> OrdenarListaUsandoLevenshtein(string origem, IList<string> lstDestinos)
        {
            try
            {
                Dictionary<string, int> retorno = new Dictionary<string, int>();
                foreach (var destino in lstDestinos)
                {
                    retorno.Add(destino, CalcularAlgoritimoLevenshtein(origem, destino));
                }
                return retorno.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception e)
            {
                TratarException(e, "Ocorreu um erro ao ordenar a lista utilizando o algorítimo levenshtein.", true);
                return null;
            }
        }

        public static MessageBoxResult MostrarMensagem(string mensagem, MessageBoxButton messageBoxButton = MessageBoxButton.OK, MessageBoxImage messageBoxImage = MessageBoxImage.Error, string titulo = "")
        {
            titulo = (string.IsNullOrWhiteSpace(titulo)) ? Settings.Default.AppName : titulo + " - " + Settings.Default.AppName;

            return MessageBox.Show(mensagem, titulo, messageBoxButton, messageBoxImage);
        }

        /// <summary>
        /// Adiciona mensagem no log.
        /// </summary>
        /// <param name="message">Mensagem a ser adicionada</param>
        /// <returns>Retorna false se ocorrer um erro</returns>
        public static bool LogMessage(string message)
        {
            string logPath = null;
            // O Try abaixo é só para evitar erros ao rodar os testes unitários.
            try
            {
                logPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), settings.AppName + ".log");
            }
            catch
            {
                return false;
            }
            string data = "## " + DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy") + " ## ";
            string logLine = null;
            message = message.Trim();
            if (message.LastOrDefault() != '.')
                message += ".";

            if (!File.Exists(logPath))
                logLine = "####################################################################################################\r\n" +
                          "########################################### " + settings.AppName + " ##########################################\r\n" +
                          "####################################################################################################\r\n\r\n" +
                          data + message;
            else
                logLine = data + message;

            try
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(logPath, FileMode.Append, FileAccess.Write), Encoding.UTF8))
                {
                    sw.WriteLine(logLine);
                }
                return true;
            }
            catch (Exception e) { TratarException(e, "Ocorreu um erro ao registrar a mensagem no log.", false); return false; }
        }

        public static IEnumerable<FileInfo> PesquisarArquivosPorExtensao(DirectoryInfo dir, params string[] extensao)
        {
            if (extensao == null)
                throw new ArgumentNullException("extensao");
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensao.Contains(f.Extension));
        }

        /// <summary>
        /// Retira os caracteres que o windows não aceita na criação de pastas e arquivos.
        /// </summary>
        /// <param name="nome">Nome do arquivo a ser normalizado.</param>
        /// <returns>Nome sem os caracteres não permitidos.</returns>
        public static string RetirarCaracteresInvalidos(string nome, bool retirarContraBarras = true)
        {
            string nomeNormalizado = nome.Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace("\"", "").Replace("<", "").Replace(">", "").Replace("|", "");
            if (retirarContraBarras)
                nomeNormalizado = nomeNormalizado.Replace("\\", "");
            return nomeNormalizado.Trim();
        }

        /// <summary>
        /// Retorna os subdiretórios do diretório de animes, configurado nas preferências.
        /// </summary>
        /// <returns></returns>
        public static DirectoryInfo[] retornarDiretoriosAnimes()
        {
            if (!string.IsNullOrWhiteSpace(settings.pref_PastaAnimes) && Directory.Exists(settings.pref_PastaAnimes))
            {
                DirectoryInfo dir = new DirectoryInfo(settings.pref_PastaAnimes);
                return dir.GetDirectories();
            }
            else return null;
        }

        /// <summary>
        /// Retorna os subdiretórios do diretório de filmes, configurado nas preferências.
        /// </summary>
        /// <returns></returns>
        public static DirectoryInfo[] retornarDiretoriosFilmes()
        {
            if (!string.IsNullOrWhiteSpace(settings.pref_PastaFilmes) && Directory.Exists(settings.pref_PastaFilmes))
            {
                DirectoryInfo dir = new DirectoryInfo(settings.pref_PastaFilmes);
                return dir.GetDirectories();
            }
            else return null;
        }

        /// <summary>
        /// Retorna os subdiretórios do diretório de séries, configurado nas preferências.
        /// </summary>
        /// <returns></returns>
        public static DirectoryInfo[] retornarDiretoriosSeries()
        {
            if (!string.IsNullOrWhiteSpace(settings.pref_PastaSeries) && Directory.Exists(settings.pref_PastaSeries))
            {
                DirectoryInfo dir = new DirectoryInfo(settings.pref_PastaSeries);
                return dir.GetDirectories();
            }
            else return null;
        }

        public static ObservableCollection<SerieAlias> PopularCampoSerieAlias(Video video)
        {
            if (/*video.IDBanco == 0 && */(video.lstSerieAlias == null || video.lstSerieAlias.Count == 0))
            {
                video.lstSerieAlias = new ObservableCollection<SerieAlias>();
                if (!string.IsNullOrWhiteSpace(video.sAliases))
                {
                    foreach (var item in video.sAliases.Split('|'))
                    {
                        SerieAlias alias = new SerieAlias(item);
                        video.lstSerieAlias.Add(alias);
                    }
                }
            }
            return video.lstSerieAlias;
        }

        public class RegexEpisodio
        {
            // nome.da.serie.S00E00 ou nome.da.serie.S00E00E01E02E03E04 ou nome.da.serie.S00E00-01-02-03-04 -- https://regex101.com/r/zP7aL3/1
            public Regex regex_S00E00 { get; set; } = new Regex(@"^(?i)(?<name>.*?)S(?<season>\d{2,2})E(?<episodes>\d{2,3}(?:(?<separador>[E-])\d{2,3})*)");

            // [Nome do Fansub] Nome da Série - 00 ou [Nome do Fansub] Nome da Série - 0000 -- https://regex101.com/r/jP1zN6/6
            public Regex regex_Fansub0000 { get; set; } = new Regex(@"^(?i)(?:\[(?<fansub>.*?)\](?:\s{0,})?)?(?<name>.*?)?(?:\s{0,})(?:(?:\s{0,})?[-&](?:\s)?)?(?:(?:ep|Episode)(?:\s{0,})?)?(?:\D)(?<episodes>(?:\d{2,3})(?:\D|$)(?:(?<separador>(?:\s*)[\s&-](?:\s*))*\d{2,3})*)");

            // Nome da Série - 0x00 - Nome do episódio -- https://regex101.com/r/rZ5dK1/3
            public Regex regex_0x00 { get; set; } = new Regex(@"^(?i)(?<name>.*?)(?: - )?(?:\s{0,})(?<season>\d{1,2})x(?<episodes>\d{1,3}(?:(?<separador>[-x])\d{1,3})*)");
        }

        #region [ APIs trakt ]

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

        #region [ OLD API Methods ]

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

        #endregion [ OLD API Methods ]

        #endregion [ APIs trakt ]
    }
}