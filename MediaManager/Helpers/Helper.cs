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

        public static async Task<bool> DownloadImages(Video video, Enums.TipoImagem tipoImagem = Enums.TipoImagem.Todos)
        {
            try
            {
                if (tipoImagem == Enums.TipoImagem.Todos || tipoImagem == Enums.TipoImagem.Poster)
                {
                    if (video.ImgPoster != new Serie().ImgPoster)
                    {
                        using (System.Net.WebClient wc = new System.Net.WebClient())
                        {
                            var path = Path.Combine(video.FolderMetadata, "poster.jpg");
                            await wc.DownloadFileTaskAsync(new Uri(video.ImgPoster), path);
                        }
                    }
                }
                if (tipoImagem == Enums.TipoImagem.Todos || tipoImagem == Enums.TipoImagem.Fanart)
                {
                    if (video.ImgFanart != new Serie().ImgFanart)
                    {
                        using (System.Net.WebClient wc = new System.Net.WebClient())
                        {
                            var path = Path.Combine(video.FolderMetadata, "fanart.jpg");
                            await wc.DownloadFileTaskAsync(new Uri(video.ImgFanart), path);
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

        public static bool RealizarPosProcessamento(EpisodeToRename item)
        {
            switch ((Enums.MetodoDeProcessamento)Settings.Default.pref_MetodoDeProcessamento)
            {
                case Enums.MetodoDeProcessamento.HardLink:
                    {
                        if (CreateHardLink(Path.Combine(item.Serie.FolderPath, item.FilenameRenamed), Path.Combine(item.FolderPath, item.Filename), IntPtr.Zero))
                            return true;
                        else { TratarException(new Exception("Código: " + Marshal.GetLastWin32Error() + "\r\nArquivo: " + Path.Combine(item.FolderPath, item.Filename)), "Ocorreu um erro ao criar o " + ((Enums.MetodoDeProcessamento)Settings.Default.pref_MetodoDeProcessamento).ToString(), true); return false; }
                    }
                case Enums.MetodoDeProcessamento.Copiar:
                    {
                        try
                        {
                            File.Copy(Path.Combine(item.FolderPath, item.Filename), Path.Combine(item.Serie.FolderPath, item.FilenameRenamed));
                            return true;
                        }
                        catch (Exception e) { TratarException(e, "Ocorreu um erro ao criar o " + ((Enums.MetodoDeProcessamento)Settings.Default.pref_MetodoDeProcessamento).ToString(), true); return false; }
                    }
                default:
                    TratarException(new ArgumentException("Método de processamento não reconhecido ou inválido."), "Ocorreu um erro ao realizar o pós processamento.", true);
                    return false;
            }
        }

        public static string RenomearConformePreferencias(EpisodeToRename episodio)
        {
            string formato = null;
            switch (episodio.ContentType)
            {
                case Enums.ContentType.movie: // TODO Funcionar com filmes
                    break;

                case Enums.ContentType.show:
                    formato = settings.pref_FormatoSeries;
                    break;

                case Enums.ContentType.anime:
                    formato = settings.pref_FormatoAnimes;
                    break;

                default:
                    TratarException(new ArgumentException("Episodio informado é de um tipo inválido."), IsSilencioso: true);
                    return null;
            }

            Regex regex = new Regex("(?:{(?<tag>.*?)})");

            foreach (Match tag in regex.Matches(formato))
            {
                switch (tag.Value)
                {
                    case "{Titulo}":
                        formato = formato.Replace(tag.Value, episodio.ParentTitle);
                        break;

                    case "{TituloEpisodio}":
                        formato = formato.Replace(tag.Value, episodio.EpisodeName);
                        break;

                    case "{Temporada}":
                        formato = formato.Replace(tag.Value, episodio.SeasonNumber.ToString("00") + "");
                        break;

                    case "{Episodio}":
                        {
                            string ep = "";
                            foreach (var item in episodio.EpisodeArray)
                            {
                                int nItem;
                                int.TryParse(item, out nItem);
                                if (ep == "")
                                    ep = nItem.ToString("00");
                                else
                                    ep += " & " + nItem.ToString("00");
                            }
                            formato = formato.Replace(tag.Value, ep);
                            break;
                        }
                    case "{Absoluto}":
                        {
                            string ep = "";
                            foreach (var item in episodio.AbsoluteArray)
                            {
                                int nItem;
                                int.TryParse(item, out nItem);
                                if (ep == "")
                                        ep = nItem.ToString("00");
                                    else
                                        ep += " & " + nItem.ToString("00");
                            }
                            formato = formato.Replace(tag.Value, ep);
                            break;
                        }
                    case "{SxEE}":
                        {
                            string ep = "";
                            foreach (var item in episodio.EpisodeArray)
                            {
                                int nItem;
                                int.TryParse(item, out nItem);
                                if (ep == "")
                                    ep = nItem.ToString("00");
                                else
                                    ep += "x" + nItem.ToString("00");
                            }
                            formato = formato.Replace(tag.Value, episodio.SeasonNumber + "x" + ep);
                            break;
                        }
                    case "{S00E00}":
                        {
                            string ep = "";
                            foreach (var item in episodio.EpisodeArray)
                            {
                                int nItem;
                                int.TryParse(item, out nItem);
                                if (ep == "")
                                    ep = nItem.ToString("00");
                                else
                                    ep += "E" + nItem.ToString("00");
                            }
                            formato = formato.Replace(tag.Value, "S" + episodio.SeasonNumber.ToString("00") + "E" + ep);
                            break;
                        }
                    default:
                        break;
                }
            }

            return RetirarCaracteresInvalidos(formato, false); // TODO Corrigir quando é anime o SxEE e o S00E00 para retornar o n do ep normal e não o absoluto.
        }

        public static void TratarException(Exception exception, string mensagem = "Ocorreu um erro na aplicação.", bool IsSilencioso = false)
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
            if (!string.IsNullOrWhiteSpace(settings.pref_PastaAnimes))
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
            if (!string.IsNullOrWhiteSpace(settings.pref_PastaFilmes))
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
            if (!string.IsNullOrWhiteSpace(settings.pref_PastaSeries))
            {
                DirectoryInfo dir = new DirectoryInfo(settings.pref_PastaSeries);
                return dir.GetDirectories();
            }
            else return null;
        }

        public static ObservableCollection<SerieAlias> PopularCampoSerieAlias(Video video)
        {
            if (/*video.IDBanco == 0 && */(video.SerieAlias == null || video.SerieAlias.Count == 0))
            {
                video.SerieAlias = new ObservableCollection<SerieAlias>();
                if (!string.IsNullOrWhiteSpace(video.SerieAliasStr))
                {
                    foreach (var item in video.SerieAliasStr.Split('|'))
                    {
                        SerieAlias alias = new SerieAlias(item);
                        video.SerieAlias.Add(alias);
                    }
                }
            }
            return video.SerieAlias;
        }

        public class RegexEpisodio
        {
            // nome.da.serie.S00E00 ou nome.da.serie.S00E00E01E02E03E04 ou nome.da.serie.S00E00-01-02-03-04 -- https://regex101.com/r/zP7aL3/1
            public Regex regex_S00E00 { get; set; } = new Regex(@"^(?i)(?<name>.*?)S(?<season>\d{2,2})E(?<episodes>\d{2,3}(?:(?<separador>[E-])\d{2,3})*)");

            // [Nome do Fansub] Nome da Série - 00 ou [Nome do Fansub] Nome da Série - 0000 -- https://regex101.com/r/jP1zN6/4
            public Regex regex_Fansub0000 { get; set; } = new Regex(@"^(?i)(?:\[(?<fansub>.*?)\](?:\s{0,})?)?(?<name>.*?)(?:\s{0,})(?:(?:\s{0,})?[-&](?:\s)?)?(?:(?:ep|Episode)(?:\s{0,})?)?(?<episodes>(?:\d{2,4})(?:(?<separador>(?:\s*)[\s&-](?:\s*))*\d{2,3})*)");

            // Nome da Série - 0x00 - Nome do episódio -- https://regex101.com/r/rZ5dK1/1
            public Regex regex_0x00 { get; set; } = new Regex(@"^(?i)(?<name>.*) - (?<season>\d{1,2})x(?<episodes>\d{1,3}(?:(?<separador>[-])\d{1,3})*)");
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

            if (filme.AvailableTranslations.Contains(settings.pref_IdiomaPesquisa))
            {
                string responseDataSinopse = "";

                List<Filme> traducoes = new List<Filme>();

                using (var httpClient = new HttpClient { BaseAddress = new Uri(settings.APIBaseUrl) })
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "");

                    using (var response = await httpClient.GetAsync("movies/" + slugTrakt + "/translations/" + settings.pref_IdiomaPesquisa))
                    {
                        responseDataSinopse = await response.Content.ReadAsStringAsync();
                    }
                }
                traducoes = JsonConvert.DeserializeObject<List<Filme>>(responseDataSinopse);

                var sinopseTraduzida = traducoes.Count > 0 ? traducoes.First().Overview : null;
                if (!string.IsNullOrWhiteSpace(sinopseTraduzida))
                    filme.Overview = sinopseTraduzida;
            }

            filme.FolderMetadata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Settings.Default.AppName, "Metadata", "Filmes", RetirarCaracteresInvalidos(filme.Title));
            if (settings.pref_PastaFilmes != "")
                filme.FolderPath = Path.Combine(settings.pref_PastaFilmes, RetirarCaracteresInvalidos(filme.Title));
            return filme;
        }

        public async static Task<SerieOld> API_GetSerieInfoAsync(string slugTrakt, Enums.ContentType tipoConteudo)
        {
            string responseData = "";

            SerieOld serie = new SerieOld();

            using (var httpClient = new HttpClient { BaseAddress = new Uri(settings.APIBaseUrl) })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "");

                using (var response = await httpClient.GetAsync("shows/" + slugTrakt + "?extended=full,images"))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            serie = JsonConvert.DeserializeObject<SerieOld>(responseData);

            if (serie.AvailableTranslations.Contains(settings.pref_IdiomaPesquisa))
            {
                string responseDataSinopse = "";

                List<SerieOld> traducoes = new List<SerieOld>();

                using (var httpClient = new HttpClient { BaseAddress = new Uri(settings.APIBaseUrl) })
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "");

                    using (var response = await httpClient.GetAsync("shows/" + slugTrakt + "/translations/" + settings.pref_IdiomaPesquisa))
                    {
                        responseDataSinopse = await response.Content.ReadAsStringAsync();
                    }
                }
                traducoes = JsonConvert.DeserializeObject<List<SerieOld>>(responseDataSinopse);

                var sinopseTraduzida = traducoes.Count > 0 ? traducoes.First().Overview : null;
                if (!string.IsNullOrWhiteSpace(sinopseTraduzida))
                    serie.Overview = sinopseTraduzida;
            }

            if (tipoConteudo == Enums.ContentType.anime)
            {
                serie.IsAnime = true;
                serie.FolderMetadata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    Settings.Default.AppName, "Metadata", "Animes", RetirarCaracteresInvalidos(serie.Title));
                if (settings.pref_PastaAnimes != "")
                    serie.FolderPath = Path.Combine(settings.pref_PastaAnimes, RetirarCaracteresInvalidos(serie.Title));
            }
            else if (tipoConteudo == Enums.ContentType.show)
            {
                serie.FolderMetadata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    Settings.Default.AppName, "Metadata", "Séries", RetirarCaracteresInvalidos(serie.Title));
                if (settings.pref_PastaSeries != "")
                    serie.FolderPath = Path.Combine(settings.pref_PastaSeries, RetirarCaracteresInvalidos(serie.Title));
            }
            return serie;
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

        public static async Task<List<Search>> API_PesquisarConteudoAsync(string query, string type, bool traduzirSinopse = true)
        {
            string responseData = "";
            List<Search> responseList = null;

            if (type == Enums.ContentType.anime.ToString())
                type = Enums.ContentType.show.ToString();

            using (var httpClient = new HttpClient { BaseAddress = new Uri(settings.APIBaseUrl) })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "");

                using (var response = await httpClient.GetAsync("search?query=" + query + "&type=" + type))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            responseList = JsonConvert.DeserializeObject<List<Search>>(responseData);

            if (traduzirSinopse)
            {
                foreach (var item in responseList)
                {
                    var itemVideo = item.ToVideo();
                    string responseDataSinopse = "";

                    List<SerieOld> traducoes = new List<SerieOld>();

                    using (var httpClient = new HttpClient { BaseAddress = new Uri(settings.APIBaseUrl) })
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");

                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "");

                        //using (var response = await httpClient.GetAsync(type + "s/" + itemVideo.Ids.slug + "/translations/" + settings.pref_IdiomaPesquisa))
                        //{
                        //    responseDataSinopse = await response.Content.ReadAsStringAsync();
                        //}
                    }
                    traducoes = JsonConvert.DeserializeObject<List<SerieOld>>(responseDataSinopse);

                    var sinopseTraduzida = traducoes.Count > 0 ? traducoes.First().Overview : null;
                    if (!string.IsNullOrWhiteSpace(sinopseTraduzida))
                        item.Video.overview = sinopseTraduzida;
                }
            }

            return responseList;
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