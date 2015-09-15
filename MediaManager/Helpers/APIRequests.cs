using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Ionic.Zip;
using MediaManager.Model;
using MediaManager.Properties;

namespace MediaManager.Helpers
{
    public class APIRequests
    {
        public async static Task<bool> GetAtualizacoes()
        {
            if (Settings.Default.API_UltimaDataAtualizacaoTVDB == default(DateTime))
            {
                Settings.Default.API_UltimaDataAtualizacaoTVDB = DateTime.Now.AddDays(-5);
                Settings.Default.Save();
            }

            DateTime dataAtualizacao = DateTime.Now;
            int dias = (Settings.Default.API_UltimaDataAtualizacaoTVDB - dataAtualizacao).Days;
            string url = Settings.Default.API_UrlTheTVDB + "/api/" + Settings.Default.API_KeyTheTVDB + "/updates/";
            string nomeArquivo = "updates_";
            string xmlString = null;
            var randomNum = new Random().Next(1, 55555);
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Settings.Default.AppName, "Metadata", "temp" + randomNum, "updatesTemp.zip");

            if (dias == 0)
            {
                nomeArquivo += "day";
            }
            else if (dias < 0 && dias > -7)
            {
                nomeArquivo += "week";
            }
            else if (dias <= -7 && dias > -30)
            {
                nomeArquivo += "month";
            }
            else
            {
                nomeArquivo += "all";
            }
            url += nomeArquivo + ".zip";

            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                using (WebClient wc = new WebClient())
                {
                    await wc.DownloadFileTaskAsync(new Uri(url), path);
                }

                using (ZipFile zip = ZipFile.Read(path))
                {
                    ZipEntry xmlFileEntry = zip[nomeArquivo + ".xml"];
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
                if (File.Exists(path))
                    File.Delete(path);
                if (Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.Delete(Path.GetDirectoryName(path));
            }
            XmlDocument xml = new XmlDocument();

            xml.LoadXml(xmlString);

            XmlNodeList nodesSeries = xml.SelectNodes("/Data/Series");
            XmlNodeList nodesEpisodios = xml.SelectNodes("/Data/Episode");
            XmlNodeList nodesBanners = xml.SelectNodes("/Data/Banner");

            List<Serie> listaSeriesAnimes = DBHelper.GetSeriesEAnimes();
            List<Episode> listaEpisodios = DBHelper.GetEpisodes();
            List<string> listaSeriesAnimesIDApi = new List<string>();
            List<string> listaEpisodiosIDApi = new List<string>();
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
                if (listaSeriesAnimesIDApi.Contains(item.SelectSingleNode("id").InnerText))
                {
                    int IDApi = 0;
                    int.TryParse(item.SelectSingleNode("id").InnerText, out IDApi);

                    Serie serieDB = DBHelper.GetSerieOuAnimePorIDApi(IDApi);

                    //data.Series[0].Episodes = new List<Episode>(data.Episodes);

                    if (int.Parse(serieDB.LastUpdated) < int.Parse(item.SelectSingleNode("time").InnerText))
                    {
                        SeriesData data = await GetSerieInfoAsync(IDApi, Settings.Default.pref_IdiomaPesquisa);

                        data.Series[0].IDBanco = serieDB.IDBanco;
                        data.Series[0].FolderPath = serieDB.FolderPath;
                        data.Series[0].IsAnime = serieDB.IsAnime;
                        data.Series[0].ContentType = serieDB.ContentType;
                        data.Series[0].Title = serieDB.Title;
                        data.Series[0].SerieAliasStr = serieDB.SerieAliasStr;

                        await DBHelper.UpdateSerieAsync(data.Series[0]);
                    }
                }
            }

            foreach (XmlNode item in nodesEpisodios)
            {
                int IDApi = 0;
                int.TryParse(item.SelectSingleNode("id").InnerText, out IDApi);
                if (listaEpisodiosIDApi.Contains(IDApi + ""))
                {
                    Episode episodioDB = DBHelper.GetEpisode(IDApi);

                    if (int.Parse(episodioDB.LastUpdated) < int.Parse(item.SelectSingleNode("time").InnerText))
                    {
                        Episode episodio = await GetEpisodeInfoAsync(IDApi, Settings.Default.pref_IdiomaPesquisa);
                        episodio.IDBanco = episodioDB.IDBanco;
                        DBHelper.UpdateEpisodio(episodio);
                    }
                }
                else if (listaSeriesAnimesIDApi.Contains(item.SelectSingleNode("Series").InnerText))
                {
                    Episode episodio = await GetEpisodeInfoAsync(IDApi, Settings.Default.pref_IdiomaPesquisa);
                    DBHelper.AddEpisodio(episodio);
                }
            }

            foreach (XmlNode item in nodesBanners)
            {
                if ((item.SelectSingleNode("type").InnerText == Enums.TipoImagem.Fanart.ToString().ToLower() || item.SelectSingleNode("type").InnerText == Enums.TipoImagem.Poster.ToString().ToLower()))
                {
                    if (listaSeriesAnimesIDApi.Contains(item.SelectSingleNode("Series").InnerText))
                    {
                        var IDApi = int.Parse(item.SelectSingleNode("Series").InnerText);
                        var urlImagem = Settings.Default.API_UrlTheTVDB + "/banners/" + item.SelectSingleNode("path").InnerText;
                        var tipo = item.SelectSingleNode("type").InnerText;
                        if (tipo == Enums.TipoImagem.Fanart.ToString().ToLower())
                        {
                            using (Context db = new Context())
                            {
                                var serie = (from series in db.Serie
                                             where series.IDApi == IDApi
                                             select series).First();
                                if (urlImagem != serie.ImgFanart)
                                {
                                    serie.ImgFanart = urlImagem;
                                    db.SaveChanges();
                                    await Helper.DownloadImages(serie, Enums.TipoImagem.Fanart);
                                }
                            }
                        }
                        else
                        {
                            using (Context db = new Context())
                            {
                                var serie = (from series in db.Serie
                                             where series.IDApi == IDApi
                                             select series).First();
                                if (urlImagem != serie.ImgPoster)
                                {
                                    serie.ImgPoster = urlImagem;
                                    db.SaveChanges();
                                    await Helper.DownloadImages(serie, Enums.TipoImagem.Poster);
                                }
                            }
                        }
                    }
                }
            }

            Settings.Default.API_UltimaDataAtualizacaoTVDB = dataAtualizacao;
            Settings.Default.Save();
            return true;
        }

        public async static Task<SeriesData> GetSerieInfoAsync(int IDTvdb, string lang)
        {
            string xmlString = null;
            Random rnd = new Random();
            var randomNum = rnd.Next(1, 55555);
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Settings.Default.AppName, "Metadata", "temp" + randomNum, "temp.zip");
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
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
                if (File.Exists(path))
                    File.Delete(path);
                if (Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.Delete(Path.GetDirectoryName(path));
            }

            SeriesData data = new SeriesData();
            XmlSerializer serializer = new XmlSerializer(typeof(SeriesData));

            using (var reader = new StringReader(xmlString))
            {
                data = (SeriesData)serializer.Deserialize(reader);
            }

            foreach (var item in data.Series)
            {
                item.Estado = Estado.Completo;
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
                        serieDataFull.Series[0].Episodes = new List<Episode>(serieDataFull.Episodes);
                        if (string.IsNullOrWhiteSpace(serieDataFull.Series[0].SerieAliasStr))
                        {
                            foreach (var item in data.Series)
                            {
                                if (item.IDApi == serieDataFull.Series[0].IDApi && !string.IsNullOrWhiteSpace(item.SerieAliasStr))
                                {
                                    serieDataFull.Series[0].SerieAliasStr = item.SerieAliasStr;
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
                            item.Estado = Estado.Simples;
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