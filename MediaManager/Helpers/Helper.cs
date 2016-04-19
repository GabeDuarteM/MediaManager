// Developed by: Gabriel Duarte
// 
// Created at: 20/09/2015 17:53
// Last update: 19/04/2016 02:57

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MediaManager.Model;
using MediaManager.Properties;
using Newtonsoft.Json;

namespace MediaManager.Helpers
{
    public class Helper
    {
        private static readonly Settings Settings = Settings.Default;

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool CreateHardLink(
            string lpFileName,
            string lpExistingFileName,
            IntPtr lpSecurityAttributes
            );

        public static async Task<bool> DownloadImagesAsync(Video video,
                                                           Enums.TipoImagem tipoImagem = Enums.TipoImagem.Todos)
        {
            try
            {
                if (tipoImagem == Enums.TipoImagem.Todos || tipoImagem == Enums.TipoImagem.Poster)
                {
                    if (video.sDsImgPoster != new Serie().sDsImgPoster)
                    {
                        using (var wc = new WebClient())
                        {
                            string path = Path.Combine(video.sDsMetadata, "poster.jpg");
                            await wc.DownloadFileTaskAsync(new Uri(video.sDsImgPoster), path);
                        }
                    }
                }
                if (tipoImagem == Enums.TipoImagem.Todos || tipoImagem == Enums.TipoImagem.Fanart)
                {
                    if (video.sDsImgFanart != new Serie().sDsImgFanart)
                    {
                        using (var wc = new WebClient())
                        {
                            string path = Path.Combine(video.sDsMetadata, "fanart.jpg");
                            await wc.DownloadFileTaskAsync(new Uri(video.sDsImgFanart), path);
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException("Ocorreu um erro ao realizar o download das imagens.");
                return false;
            }
        }

        public static bool DownloadImages(Video video, Enums.TipoImagem tipoImagem = Enums.TipoImagem.Todos)
        {
            try
            {
                if (tipoImagem == Enums.TipoImagem.Todos || tipoImagem == Enums.TipoImagem.Poster)
                {
                    if (video.sDsImgPoster != new Serie().sDsImgPoster)
                    {
                        using (var wc = new WebClient())
                        {
                            string path = Path.Combine(video.sDsMetadata, "poster.jpg");
                            wc.DownloadFile(new Uri(video.sDsImgPoster), path);
                        }
                    }
                }
                if (tipoImagem == Enums.TipoImagem.Todos || tipoImagem == Enums.TipoImagem.Fanart)
                {
                    if (video.sDsImgFanart != new Serie().sDsImgFanart)
                    {
                        using (var wc = new WebClient())
                        {
                            string path = Path.Combine(video.sDsMetadata, "fanart.jpg");
                            wc.DownloadFile(new Uri(video.sDsImgFanart), path);
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException("Ocorreu um erro ao realizar o download das imagens.");
                return false;
            }
        }

        public static string ListToString(IList<string> lista)
        {
            if (lista != null)
            {
                var strGeneros = "";
                foreach (string item in lista)
                {
                    strGeneros += item + "|";
                }
                if (strGeneros != "")
                {
                    return strGeneros.Remove(strGeneros.Length - 1);
                }

                return null;
            }
            return null;
        }

        public static bool RealizarPosProcessamento(Episodio item)
        {
            switch ((Enums.MetodoDeProcessamento) Settings.Default.pref_MetodoDeProcessamento)
            {
                case Enums.MetodoDeProcessamento.HardLink:
                {
                    if (CreateHardLink(item.sDsFilepath, item.sDsFilepathOriginal, IntPtr.Zero))
                        return true;
                    else
                    {
                        new MediaManagerException(
                            new Exception("Código: " + Marshal.GetLastWin32Error() + "\r\nArquivo: " + item.sDsFilepath))
                            .TratarException("Ocorreu um erro ao criar o " +
                                             Settings.Default.pref_MetodoDeProcessamento.ToString());
                        return false;
                    }
                }
                case Enums.MetodoDeProcessamento.Copiar:
                {
                    try
                    {
                        File.Copy(item.sDsFilepathOriginal, item.sDsFilepath);
                        return true;
                    }
                    catch (Exception e)
                    {
                        new MediaManagerException(e).TratarException(
                                                                     "Ocorreu um erro ao criar o " +
                                                                     ((Enums.MetodoDeProcessamento)
                                                                      Settings.Default.pref_MetodoDeProcessamento)
                                                                         .ToString(), true);
                        return false;
                    }
                }
                default:
                    new MediaManagerException(
                        new ArgumentException("Método de processamento não reconhecido ou inválido.")).TratarException(
                                                                                                                       "Ocorreu um erro ao realizar o pós processamento.",
                                                                                                                       true);
                    return false;
            }
        }

        public static string RenomearConformePreferencias(Episodio episodio)
        {
            var formato = "";

            if (!string.IsNullOrWhiteSpace(episodio.oSerie.sFormatoRenomeioPersonalizado))
            {
                formato = episodio.oSerie.sFormatoRenomeioPersonalizado;
            }
            else
            {
                switch (episodio.nIdTipoConteudo)
                {
                    case Enums.TipoConteudo.Filme: // TODO Funcionar com filmes
                        break;

                    case Enums.TipoConteudo.Série:
                        formato = Settings.pref_FormatoSeries;
                        break;

                    case Enums.TipoConteudo.Anime:
                        formato = Settings.pref_FormatoAnimes;
                        break;

                    default:
                        new MediaManagerException(new ArgumentException("Episodio informado é de um tipo inválido."))
                            .TratarException(bIsSilencioso: true);
                        return null;
                }
            }

            var regex = new Regex("(?:{(?<tag>.*?)})");

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
                        var ep = "";
                        foreach (int item in episodio.lstIntEpisodios)
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
                        var ep = "";
                        foreach (int item in episodio.lstIntEpisodiosAbsolutos)
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
                        var ep = "";
                        foreach (int item in episodio.lstIntEpisodios)
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
                        var ep = "";
                        foreach (int item in episodio.lstIntEpisodios)
                        {
                            if (ep == "")
                                ep = item.ToString("00");
                            else
                                ep += "E" + item.ToString("00");
                        }
                        formato = formato.Replace(tag.Value, "S" + episodio.nNrTemporada.ToString("00") + "E" + ep);
                        break;
                    }
                }
            }

            return RetirarCaracteresInvalidos(formato, false);
            // TODO Corrigir quando é anime o SxEE e o S00E00 para retornar o n do ep normal e não o absoluto.
        }

        internal static void BaixarEpisodio(Episodio episodio, Uri link)
        {
            throw new NotImplementedException();
        }

        public static string ColocarVirgula(string frase, List<string> adicional)
        {
            foreach (string item in adicional)
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

        public static int CalcularAlgoritimoLevenshtein(string origem, string destino)
        {
            if (string.IsNullOrEmpty(origem))
            {
                return string.IsNullOrEmpty(destino)
                           ? 0
                           : destino.Length;
            }

            if (string.IsNullOrEmpty(destino))
                return origem.Length;

            if (origem.Length > destino.Length)
            {
                string temp = destino;
                destino = origem;
                origem = temp;
            }

            int m = destino.Length;
            int n = origem.Length;
            var distance = new int[2, m + 1];
            // Initialize the distance 'matrix'
            for (var j = 1; j <= m; j++)
                distance[0, j] = j;

            var currentRow = 0;
            for (var i = 1; i <= n; ++i)
            {
                currentRow = i & 1;
                distance[currentRow, 0] = i;
                int previousRow = currentRow ^ 1;
                for (var j = 1; j <= m; j++)
                {
                    int cost = destino[j - 1] == origem[i - 1]
                                   ? 0
                                   : 1;
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
                var retorno = new Dictionary<string, int>();
                foreach (string destino in lstDestinos)
                {
                    retorno.Add(destino, CalcularAlgoritimoLevenshtein(origem, destino));
                }
                return retorno.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(
                                                             "Ocorreu um erro ao ordenar a lista utilizando o algorítimo levenshtein.",
                                                             true);
                return null;
            }
        }

        public static MessageBoxResult MostrarMensagem(string mensagem, Enums.eTipoMensagem eTipoMensagem,
                                                       string titulo = "")
        {
            switch (eTipoMensagem)
            {
                case Enums.eTipoMensagem.Alerta:
                    return MessageBox.Show(mensagem,
                                           string.IsNullOrWhiteSpace(titulo)
                                               ? Settings.Default.AppName
                                               : titulo + " - " + Settings.Default.AppName, MessageBoxButton.OK,
                                           MessageBoxImage.Warning);

                case Enums.eTipoMensagem.AlertaSimNao:
                    return MessageBox.Show(mensagem,
                                           string.IsNullOrWhiteSpace(titulo)
                                               ? Settings.Default.AppName
                                               : titulo + " - " + Settings.Default.AppName, MessageBoxButton.YesNo,
                                           MessageBoxImage.Warning);

                case Enums.eTipoMensagem.AlertaSimNaoCancela:
                    return MessageBox.Show(mensagem,
                                           string.IsNullOrWhiteSpace(titulo)
                                               ? Settings.Default.AppName
                                               : titulo + " - " + Settings.Default.AppName, MessageBoxButton.YesNoCancel,
                                           MessageBoxImage.Warning);

                case Enums.eTipoMensagem.Informativa:
                    return MessageBox.Show(mensagem,
                                           string.IsNullOrWhiteSpace(titulo)
                                               ? Settings.Default.AppName
                                               : titulo + " - " + Settings.Default.AppName, MessageBoxButton.OK,
                                           MessageBoxImage.Information);

                case Enums.eTipoMensagem.QuestionamentoSimNao:
                    return MessageBox.Show(mensagem,
                                           string.IsNullOrWhiteSpace(titulo)
                                               ? Settings.Default.AppName
                                               : titulo + " - " + Settings.Default.AppName, MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                case Enums.eTipoMensagem.QuestionamentoSimNaoCancela:
                    return MessageBox.Show(mensagem,
                                           string.IsNullOrWhiteSpace(titulo)
                                               ? Settings.Default.AppName
                                               : titulo + " - " + Settings.Default.AppName, MessageBoxButton.YesNoCancel,
                                           MessageBoxImage.Question);

                case Enums.eTipoMensagem.Erro:
                    return MessageBox.Show(mensagem,
                                           string.IsNullOrWhiteSpace(titulo)
                                               ? Settings.Default.AppName
                                               : titulo + " - " + Settings.Default.AppName, MessageBoxButton.OK,
                                           MessageBoxImage.Error);

                default:
                    throw new ArgumentOutOfRangeException(nameof(eTipoMensagem), eTipoMensagem, null);
            }

            //titulo = (string.IsNullOrWhiteSpace(titulo)) ? Settings.Default.AppName : titulo + " - " + Settings.Default.AppName;

            //return MessageBox.Show(mensagem, titulo, messageBoxButton, messageBoxImage);
        }

        /// <summary>
        ///     Adiciona mensagem no log.
        /// </summary>
        /// <param name="message">Mensagem a ser adicionada</param>
        /// <returns>Retorna false se ocorrer um erro</returns>
        public static bool LogMessage(string message)
        {
            string logPath = null;
            string data = "## " + DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy") + " ## ";
            string espacamento = "## " + new string(' ', data.Length - 3);
            string logLine = null;

            // O Try abaixo é só para evitar erros ao rodar os testes unitários.
            try
            {
                logPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),
                                       Settings.AppName + ".log");
            }
            catch
            {
                return false;
            }
            message = message.Trim();
            if (message.LastOrDefault() != '.')
                message += ".";

            if (!File.Exists(logPath))
                logLine =
                    $@"####################################################################################################
########################################### {Settings
                        .AppName} ##########################################
####################################################################################################
##
{data}{message
                            .Replace(Environment.NewLine, Environment.NewLine + espacamento)}";
            else
                logLine = "## " + Environment.NewLine + data +
                          message.Replace(Environment.NewLine, Environment.NewLine + espacamento);

            try
            {
                using (
                    var sw = new StreamWriter(new FileStream(logPath, FileMode.Append, FileAccess.Write), Encoding.UTF8)
                    )
                {
                    sw.WriteLine(logLine);
                }
                return true;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException("Ocorreu um erro ao registrar a mensagem no log.", false);
                return false;
            }
        }

        public static IEnumerable<FileInfo> PesquisarArquivosPorExtensao(DirectoryInfo dir, params string[] extensao)
        {
            if (extensao == null)
                throw new ArgumentNullException("extensao");
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensao.Contains(f.Extension));
        }

        /// <summary>
        ///     Retira os caracteres que o windows não aceita na criação de pastas e arquivos.
        /// </summary>
        /// <param name="nome">Nome do arquivo a ser normalizado.</param>
        /// <returns>Nome sem os caracteres não permitidos.</returns>
        public static string RetirarCaracteresInvalidos(string nome, bool retirarContraBarras = true)
        {
            string nomeNormalizado =
                nome.Replace("/", "")
                    .Replace(":", "")
                    .Replace("*", "")
                    .Replace("?", "")
                    .Replace("\"", "")
                    .Replace("<", "")
                    .Replace(">", "")
                    .Replace("|", "");
            if (retirarContraBarras)
                nomeNormalizado = nomeNormalizado.Replace("\\", "");
            return nomeNormalizado.Trim();
        }

        /// <summary>
        ///     Retorna os subdiretórios do diretório de animes, configurado nas preferências.
        /// </summary>
        /// <returns></returns>
        public static DirectoryInfo[] retornarDiretoriosAnimes()
        {
            if (!string.IsNullOrWhiteSpace(Settings.pref_PastaAnimes) && Directory.Exists(Settings.pref_PastaAnimes))
            {
                var dir = new DirectoryInfo(Settings.pref_PastaAnimes);
                return dir.GetDirectories();
            }
            else
                return null;
        }

        /// <summary>
        ///     Retorna os subdiretórios do diretório de filmes, configurado nas preferências.
        /// </summary>
        /// <returns></returns>
        public static DirectoryInfo[] retornarDiretoriosFilmes()
        {
            if (!string.IsNullOrWhiteSpace(Settings.pref_PastaFilmes) && Directory.Exists(Settings.pref_PastaFilmes))
            {
                var dir = new DirectoryInfo(Settings.pref_PastaFilmes);
                return dir.GetDirectories();
            }
            else
                return null;
        }

        /// <summary>
        ///     Retorna os subdiretórios do diretório de séries, configurado nas preferências.
        /// </summary>
        /// <returns></returns>
        public static DirectoryInfo[] retornarDiretoriosSeries()
        {
            if (!string.IsNullOrWhiteSpace(Settings.pref_PastaSeries) && Directory.Exists(Settings.pref_PastaSeries))
            {
                var dir = new DirectoryInfo(Settings.pref_PastaSeries);
                return dir.GetDirectories();
            }
            else
                return null;
        }

        public static ObservableCollection<SerieAlias> PopularCampoSerieAlias(Video video)
        {
            if ( /*video.IDBanco == 0 && */video.lstSerieAlias == null || video.lstSerieAlias.Count == 0)
            {
                video.lstSerieAlias = new ObservableCollection<SerieAlias>();
                if (!string.IsNullOrWhiteSpace(video.sAliases))
                {
                    foreach (string item in video.sAliases.Split('|'))
                    {
                        var alias = new SerieAlias(item);
                        video.lstSerieAlias.Add(alias);
                    }
                }
            }
            return video.lstSerieAlias;
        }

        /// <summary>
        ///     Tenta executar novamente o método caso haja algum exception.
        /// </summary>
        /// <param name="action">Método</param>
        /// <param name="retryInterval">Intervalo de execução entre os métodos</param>
        /// <param name="retryCount">Número de tentativas de execução do método</param>
        public static void Retry(Action action, TimeSpan retryInterval, int retryCount = 3)
        {
            Retry<object>(() =>
            {
                action();
                return null;
            }, retryInterval, retryCount);
        }

        public static T Retry<T>(Func<T> action, TimeSpan retryInterval, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (var retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    if (retry > 0)
                        Thread.Sleep(retryInterval);
                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }

        public class RegexEpisodio
        {
            // nome.da.serie.S00E00 ou nome.da.serie.S00E00E01E02E03E04 ou nome.da.serie.S00E00-01-02-03-04 -- https://regex101.com/r/zP7aL3/1
            public static Regex regex_S00E00 { get; } =
                new Regex(@"^(?i)(?<name>.*?)S(?<season>\d{2,2})E(?<episodes>\d{2,3}(?:(?<separador>[E-])\d{2,3})*)");

            // [Nome do Fansub] Nome da Série - 00 ou [Nome do Fansub] Nome da Série - 0000 -- https://regex101.com/r/jP1zN6/8
            public static Regex regex_Fansub0000 { get; } =
                new Regex(
                    @"^(?i)(?:\s*)(?:\[(?:\s*)(?<fansub>.*?)(?:\s*)\](?:\s*))?(?<name>.*?(?:\(\d{4,4}\).*?)?)(?:\s*)(?:[\s-])?(?:(?:ep|Episode)(?:\D*))?(?:\s*)(?<episodes>(?:\d{2,4})(?:(?<separador>(?:\s*)?[\s&-](?:\s*))*\d{2,4})*)")
                ;

            // Nome da Série - 0x00 - Nome do episódio -- https://regex101.com/r/rZ5dK1/4
            public static Regex regex_0x00 { get; } =
                new Regex(
                    @"^(?i)(?<name>.*?)(?:[\s-])*(?:\s{0,})(?:\D)(?<season>\d{1,2})x(?<episodes>\d{1,3}(?:(?<separador>[-x])\d{1,3})*)")
                ;

            public static Regex regex_Qualidades { get; } = new Regex(@".*?(720p|1280x720|960x720|1080p|1920x1080|480p)")
                ;

            public static Regex regex_QualidadesProblematicas { get; } = new Regex(@".*?(720|1080|480|HDTV)");
        }

        public class MyWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                var request = base.GetWebRequest(address) as HttpWebRequest;
                if (request != null)
                {
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                }
                return request;
            }
        }

        #region [ APIs trakt ]

        /// <summary>
        ///     Realiza a troca do código do trakt pelo access token necessário para realizar as transações específicas de usuário.
        /// </summary>
        /// <param name="code">Código a ser trocado.</param>
        /// <returns>Access token</returns>
        public static async Task<UserAuth> API_GetAccessTokenAsync(string code)
        {
            var responseData = "";

            using (var httpClient = new HttpClient {BaseAddress = new Uri(Settings.APIBaseUrl)})
            {
                using (
                    var content =
                        new StringContent(
                            "{  \"code\": " + code + ",  \"client_id\": " + Settings.ClientID + ",  \"client_secret\": " +
                            Settings.ClientSecret + ",  \"redirect_uri\": " + Settings.CallbackUrl +
                            ",  \"grant_type\": \"authorization_code\"}", Encoding.Default, "application/json"))
                {
                    using (HttpResponseMessage response = await httpClient.PostAsync("oauth/token", content))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return JsonConvert.DeserializeObject<UserAuth>(responseData);
        }

        /// <summary>
        ///     Pega as configurações da conta no trakt.tv.
        /// </summary>
        /// <returns>Objeto com todas as configurações</returns>
        public static async Task<UserInfo> API_GetUserSettingsAsync()
        {
            var responseData = "";

            using (var httpClient = new HttpClient {BaseAddress = new Uri(Settings.APIBaseUrl)})
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", Settings.ClientID);

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization",
                                                                         "Bearer " + Settings.user_accessToken);

                using (HttpResponseMessage response = await httpClient.GetAsync("users/settings"))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            return JsonConvert.DeserializeObject<UserInfo>(responseData);
        }

        #endregion [ APIs trakt ]
    }
}
