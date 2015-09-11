using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Ionic.Zip;
using MediaManager.Model;
using MediaManager.Properties;
using Newtonsoft.Json.Linq;

namespace MediaManager.Helpers
{
    public class APIRequests
    {
        public async static Task<bool> GetAtualizacoes()
        {
            string responseData = null;

            using (var httpClient = new HttpClient { BaseAddress = new Uri(Settings.Default.API_UrlTheTVDB) })
            {
                using (var response = await httpClient.GetAsync("/api/Updates.php?type=all&time=" + Settings.Default.API_UltimaDataAtualizacaoTVDB))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            XmlDocument xml = new XmlDocument();

            xml.LoadXml(responseData);

            XmlNodeList nodesSeries = xml.SelectNodes("/Items/Series");
            XmlNodeList nodesEpisodios = xml.SelectNodes("/Items/Episode");
            XmlNodeList nodesHoraServidorTVDB = xml.SelectNodes("/Items/Time");

            List<Serie> listaSeriesAnimes = DBHelper.GetSeries();
            List<Serie> tempListaAnimes = DBHelper.GetAnimes();
            List<Episode> listaEpisodios = DBHelper.GetEpisodes();
            List<string> listaSeriesAnimesIDApi = new List<string>();
            List<string> listaEpisodiosIDApi = new List<string>();
            foreach (var item in tempListaAnimes)
            {
                listaSeriesAnimes.Add(item);
            }
            foreach (var item in listaSeriesAnimes)
            {
                listaSeriesAnimesIDApi.Add(item.IDApi + "");
            }
            foreach (var item in listaEpisodios)
            {
                listaEpisodiosIDApi.Add(item.IDTvdb + "");
            }

            foreach (XmlNode item in nodesSeries)
            {
                if (listaSeriesAnimesIDApi.Contains(item.InnerText))
                {
                    int IDApi = 0;
                    int.TryParse(item.InnerText, out IDApi);
                    SeriesData serie = await GetSerieInfoAsync(IDApi, Settings.Default.pref_IdiomaPesquisa);
                    serie.Series[0].Episodes = serie.Episodes;

                    Serie serieDB = DBHelper.GetSerieOuAnimePorIDApi(IDApi);
                    serie.Series[0].IDBanco = serieDB.IDBanco;
                    serie.Series[0].FolderPath = serieDB.FolderPath;
                    serie.Series[0].IsAnime = serieDB.IsAnime;
                    serie.Series[0].ContentType = serieDB.ContentType;
                    serie.Series[0].Title = serieDB.Title;
                    serie.Series[0].AliasNamesStr = serieDB.AliasNamesStr;

                    await DBHelper.UpdateSerieAsync(serie.Series[0]);
                }
            }

            foreach (XmlNode item in nodesEpisodios)
            {
                int IDApi = -1;
                int.TryParse(item.InnerText, out IDApi);
                Episode episodio = await GetEpisodeInfoAsync(IDApi, Settings.Default.pref_IdiomaPesquisa);
                if (listaSeriesAnimesIDApi.Contains(episodio.IDSeriesTvdb + ""))
                {
                    if (listaEpisodiosIDApi.Contains(item.InnerText))
                    {
                        Episode episodioDB = DBHelper.GetEpisode(episodio.IDTvdb);
                        episodio.IDBanco = episodioDB.IDBanco;
                        DBHelper.UpdateEpisodio(episodio);
                    }
                    else
                    {
                        DBHelper.AddEpisodio(episodio);
                    }
                }
            }
            Settings.Default.API_UltimaDataAtualizacaoTVDB = int.Parse(nodesHoraServidorTVDB[0].InnerText);
            return true;
        }

        /// <summary>
        /// Retorna um Tuple, onde o primeiro valor é a fanart e o segundo é o poster.
        /// </summary>
        /// <param name="idApi"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async static Task<Tuple<string, string>> GetImagesAsync(int idApi, Helper.Enums.ContentType contentType)
        {
            if (idApi == 0)
                throw new ArgumentException("Não foi possível baixar as imagens pois o id fornecido é inválido.");

            string responseData = "";
            Tuple<string, string> urlImages;

            if (contentType == Helper.Enums.ContentType.anime || contentType == Helper.Enums.ContentType.show)
            {
                using (var httpClient = new HttpClient { BaseAddress = new Uri(Settings.Default.API_UrlFanartTv) })
                {
                    using (var response = await httpClient.GetAsync("v3/tv/" + idApi + "?api_key=" + Settings.Default.API_KeyFanartTv))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }

                JObject jsonObject = JObject.Parse(responseData);
                if (jsonObject.First.Path != "status" && (string)jsonObject["status"] != "error")
                {
                    var imgFanart = (string)jsonObject["showbackground"][0]["url"];
                    var imgPoster = (string)jsonObject["tvposter"][0]["url"];
                    urlImages = new Tuple<string, string>(imgFanart, imgPoster);
                }
                else
                {
                    urlImages = new Tuple<string, string>(null, null);
                }

                return urlImages;
            }
            throw new ArgumentException("Não foi possível efetuar a operação, pois o tipo do parâmetro informado é inválido.");
            //string

            //if (filme.AvailableTranslations.Contains(settings.pref_IdiomaPesquisa))
            //{
            //    string responseDataSinopse = "";

            //    List<Filme> traducoes = new List<Filme>();

            //    using (var httpClient = new HttpClient { BaseAddress = new Uri(settings.APIBaseUrl) })
            //    {
            //        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");

            //        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "");

            //        using (var response = await httpClient.GetAsync("movies/" + slugTrakt + "/translations/" + settings.pref_IdiomaPesquisa))
            //        {
            //            responseDataSinopse = await response.Content.ReadAsStringAsync();
            //        }
            //    }
            //    traducoes = JsonConvert.DeserializeObject<List<Filme>>(responseDataSinopse);

            //    var sinopseTraduzida = traducoes.Count > 0 ? traducoes.First().Overview : null;
            //    if (!string.IsNullOrWhiteSpace(sinopseTraduzida))
            //        filme.Overview = sinopseTraduzida;
            //}

            //filme.FolderMetadata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            //    Settings.Default.AppName, "Metadata", "Filmes", RetirarCaracteresInvalidos(filme.Title));
            //if (settings.pref_PastaFilmes != "")
            //    filme.FolderPath = Path.Combine(settings.pref_PastaFilmes, RetirarCaracteresInvalidos(filme.Title));
            //return filme;
        }

        public async static Task<SeriesData> GetSerieInfoAsync(int IDTvdb, string lang)
        {
            string xmlString = null;

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Settings.Default.AppName, "Metadata", "temp.zip");
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (WebClient wc = new WebClient())
                {
                    await wc.DownloadFileTaskAsync(new Uri(Settings.Default.API_UrlTheTVDB + "/api/" + Settings.Default.API_KeyTheTVDB + "/series/" + IDTvdb + "/all/" + lang + ".zip"), path);
                }

                using (ZipFile zip = ZipFile.Read(path))
                {
                    ZipEntry xmlFileEntry = zip[lang + ".xml"];
                    using (var ms = new MemoryStream())
                    {
                        xmlFileEntry.Extract(ms);
                        var sr = new StreamReader(ms);
                        ms.Position = 0;
                        xmlString = sr.ReadToEnd();
                    }
                }
            }
            finally
            {
                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Settings.Default.AppName, "Metadata", "temp.zip")))
                    File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Settings.Default.AppName, "Metadata", "temp.zip"));
            }

            SeriesData data = new SeriesData();
            XmlSerializer serializer = new XmlSerializer(typeof(SeriesData));

            using (var reader = new StringReader(xmlString))
            {
                data = (SeriesData)serializer.Deserialize(reader);
            }

            foreach (var item in data.Series)
            {
                item.Estado = Estado.COMPLETO;
            }

            return data;
        }

        public async static Task<SeriesData> GetSeriesAsync(string query, bool traduzir)
        {
            string responseData = null;

            using (var httpClient = new HttpClient { BaseAddress = new Uri(Settings.Default.API_UrlTheTVDB) })
            {
                using (var response = await httpClient.GetAsync("/api/GetSeries.php?seriesname=" + query/* + "&language=" + Settings.Default.pref_IdiomaPesquisa*/))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }

            // Valida quando o xml possui a tag <Language> em com o 'L' minúsculo.
            responseData = Regex.Replace(responseData, @"(?:<language>)([\w\W]{0,2})(?:<\/language>)", "<Language>$1</Language>");

            SeriesData data = new SeriesData();
            XmlSerializer serializer = new XmlSerializer(typeof(SeriesData));

            using (var reader = new StringReader(responseData))
            {
                data = (SeriesData)serializer.Deserialize(reader);
            }

            List<Serie> series = new List<Serie>();
            if (data.Series != null)
            {
                foreach (var itemData in data.Series)
                {
                    if (traduzir)
                    {
                        var serieDataFull = await GetSerieInfoAsync(itemData.IDApi, /*itemData.Language*/Settings.Default.pref_IdiomaPesquisa);
                        serieDataFull.Series[0].Episodes = serieDataFull.Episodes;
                        if (string.IsNullOrWhiteSpace(serieDataFull.Series[0].AliasNamesStr))
                        {
                            foreach (var item in data.Series)
                            {
                                if (item.IDApi == serieDataFull.Series[0].IDApi && !string.IsNullOrWhiteSpace(item.AliasNamesStr))
                                {
                                    serieDataFull.Series[0].AliasNamesStr = item.AliasNamesStr;
                                    break;
                                }
                            }
                        }
                        series.Add(serieDataFull.Series[0]);
                    }
                    else
                    {
                        foreach (var item in data.Series)
                        {
                            var isExistente = false;
                            item.Estado = Estado.SIMPLES;
                            foreach (var itemListaSeries in series)
                            {
                                if (item.IDApi == itemListaSeries.IDApi)
                                {
                                    isExistente = true;
                                    break;
                                }
                            }
                            if (!isExistente)
                            {
                                series.Add(item);
                            }
                        }
                    }
                    //foreach (var item in series)
                    //{
                    //    if (item.IDApi == itemData.IDApi)
                    //    {
                    //        isExistente = true;
                    //        break;
                    //    }
                    //}
                    //if (!isExistente)
                    //    series.Add(itemData);
                    //break;
                }
                data.Series = series.ToArray();
            }

            return data;
        }

        private static async Task<Episode> GetEpisodeInfoAsync(int IDApi, string pref_IdiomaPesquisa)
        {
            string responseData = null;

            using (var httpClient = new HttpClient { BaseAddress = new Uri(Settings.Default.API_UrlTheTVDB) })
            {
                using (var response = await httpClient.GetAsync("/api/" + Settings.Default.API_KeyTheTVDB + "/episodes/" + IDApi + "/" + Settings.Default.pref_IdiomaPesquisa + ".xml"))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }

            Episode episode = new Episode();
            XmlSerializer serializer = new XmlSerializer(typeof(SeriesData));

            using (var reader = new StringReader(responseData))
            {
                var data = (SeriesData)serializer.Deserialize(reader);
                episode = data.Episodes[0];
            }
            return episode;
        }
    }
}