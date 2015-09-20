using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MediaManager.Forms;
using MediaManager.Model;
using MediaManager.ViewModel;

namespace MediaManager.Helpers
{
    public class DBHelper
    {
        /// <summary>
        /// Consulta todos os animes contidos no banco.
        /// </summary>
        /// <returns>Retorna um List<Serie> contendo todos os animes que tenham serie.isAnime == true ordenados pelo título.</returns>
        public static List<Serie> GetAnimes()
        {
            using (Context db = new Context())
            {
                var animesDB = (from animeDB in db.Serie
                                where animeDB.IsAnime
                                orderby animeDB.Title
                                select animeDB);
                List<Serie> animes = animesDB.ToList();
                foreach (var item in animes)
                {
                    item.ContentType = item.IsAnime == true ? Enums.ContentType.anime
                        : Enums.ContentType.show;
                    item.Estado = Estado.CompletoSemForeignKeys;
                }
                return animes;
            }
        }

        public static List<Serie> GetAnimesComForeignKeys()
        {
            using (Context db = new Context())
            {
                var animesDB = (from animeDB in db.Serie.Include("Episodes").Include("SerieAlias")
                                where animeDB.IsAnime
                                orderby animeDB.Title
                                select animeDB);
                List<Serie> animes = animesDB.ToList();
                foreach (var item in animes)
                {
                    item.ContentType = item.IsAnime == true ? Enums.ContentType.anime
                        : Enums.ContentType.show;
                    item.Estado = Estado.Completo;
                }
                return animes;
            }
        }

        public static Serie GetAnimePorIDApi(int IDApi)
        {
            using (Context db = new Context())
            {
                var animesDB = (from animeDB in db.Serie
                                where animeDB.IsAnime && animeDB.IDApi == IDApi
                                select animeDB);
                Serie anime = animesDB.First();
                return anime;
            }
        }

        /// <summary>
        /// Consulta todas as séries contidas no banco.
        /// </summary>
        /// <returns>Retorna um List<Serie> contendo todas as séries que tenham serie.isAnime == false ordenados pelo título.</returns>
        public static List<Serie> GetSeries()
        {
            using (Context db = new Context())
            {
                var seriesDB = (from serieDB in db.Serie
                                where !serieDB.IsAnime
                                orderby serieDB.Title
                                select serieDB);
                List<Serie> series = seriesDB.ToList();
                foreach (var item in series)
                {
                    item.ContentType = item.IsAnime == true ? Enums.ContentType.anime
                        : Enums.ContentType.show;
                    item.Estado = Estado.CompletoSemForeignKeys;
                }
                return series;
            }
        }

        public static List<Serie> GetSeriesComForeignKeys()
        {
            using (Context db = new Context())
            {
                var seriesDB = (from serieDB in db.Serie.Include("Episodes").Include("SerieAlias")
                                where !serieDB.IsAnime
                                orderby serieDB.Title
                                select serieDB);
                List<Serie> series = seriesDB.ToList();
                foreach (var item in series)
                {
                    item.ContentType = item.IsAnime == true ? Enums.ContentType.anime
                        : Enums.ContentType.show;
                    item.Estado = Estado.Completo;
                }
                return series;
            }
        }

        /// <summary>
        /// Consulta todos as séries contidas no banco que tenham a id especificada.
        /// </summary>
        /// <param name="IDBanco">ID da série.</param>
        /// <returns>Retorna a série caso esta exista no banco, ou null caso não exista.</returns>
        public static Serie GetSeriePorID(int IDBanco)
        {
            using (Context db = new Context())
            {
                var seriesDB = (from serieDB in db.Serie
                                where serieDB.IDBanco == IDBanco
                                select serieDB);
                Serie serie = seriesDB.First();
                serie.Estado = Estado.CompletoSemForeignKeys;
                return serie;
            }
        }

        public static List<Serie> GetSeriesEAnimes()
        {
            using (Context db = new Context())
            {
                var seriesDB = (from serieDB in db.Serie
                                orderby serieDB.Title
                                select serieDB);
                List<Serie> series = seriesDB.ToList();
                foreach (var item in series)
                {
                    item.ContentType = item.IsAnime == true ? Enums.ContentType.anime
                        : Enums.ContentType.show;
                    item.Estado = Estado.CompletoSemForeignKeys;
                }
                return series;
            }
        }

        public static List<Serie> GetSeriesEAnimesComForeignKeys()
        {
            using (Context db = new Context())
            {
                var seriesDB = (from serieDB in db.Serie.Include("Episodes").Include("SerieAlias")
                                orderby serieDB.Title
                                select serieDB);
                List<Serie> series = seriesDB.ToList();
                foreach (var item in series)
                {
                    item.ContentType = item.IsAnime == true ? Enums.ContentType.anime
                        : Enums.ContentType.show;
                    item.Estado = Estado.Completo;
                }
                return series;
            }
        }

        /// <summary>
        /// Pesquisa por séries ou animes que contenham a string informada
        /// </summary>
        /// <param name="titulo">Título inteiro ou parte dele</param>
        /// <returns>Lista de séries que contenham a string informada</returns>
        public static List<Serie> GetSerieOuAnimePorTitulo(string titulo, bool removerCaracteresEspeciais)
        {
            if (!removerCaracteresEspeciais)
            {
                using (Context db = new Context())
                {
                    var seriesDB = (from serieDB in db.Serie
                                    where serieDB.Title.Contains(titulo)
                                    select serieDB);
                    List<Serie> series = seriesDB.ToList();
                    return series;
                }
            }
            else
            {
                using (Context db = new Context())
                {
                    var seriesDB = (from serieDB in db.Serie
                                    where serieDB.Title.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim().Contains(titulo)
                                    select serieDB);
                    List<Serie> series = seriesDB.ToList();
                    return series;
                }
            }
        }

        public static Serie GetSerieOuAnimePorIDApi(int IDApi)
        {
            using (Context db = new Context())
            {
                var seriesDB = (from serieDB in db.Serie
                                where serieDB.IDApi == IDApi
                                select serieDB);
                Serie serie = seriesDB.First();
                return serie;
            }
        }

        public static List<Episode> GetEpisodes()
        {
            using (Context db = new Context())
            {
                var todosEpisodiosDB = from episodioDB in db.Episode
                                       select episodioDB;
                var listaEpisodios = todosEpisodiosDB.ToList();
                return listaEpisodios;
            }
        }

        public static List<Episode> GetEpisodes(Serie serie)
        {
            using (Context db = new Context())
            {
                var episodios = from episodiosDB in db.Episode
                                where episodiosDB.IDSerie == serie.IDBanco
                                select episodiosDB;
                var listaEpisodios = episodios.ToList();
                return listaEpisodios;
            }
        }

        public static Episode GetEpisode(int IDSerie, int seasonNumber, int episodeNumber)
        {
            using (Context db = new Context())
            {
                var todosEpisodiosDB = from episodioDB in db.Episode
                                       where episodioDB.IDSerie == IDSerie && episodioDB.SeasonNumber == seasonNumber && episodioDB.EpisodeNumber == episodeNumber
                                       select episodioDB;
                return todosEpisodiosDB.Count() != 0 ? todosEpisodiosDB.First() : null;
            }
        }

        public static Episode GetEpisode(int IDSerie, int absoluteNumber)
        {
            using (Context db = new Context())
            {
                var todosEpisodiosDB = from episodioDB in db.Episode
                                       where episodioDB.IDSerie == IDSerie && episodioDB.AbsoluteNumber == absoluteNumber
                                       select episodioDB;
                return todosEpisodiosDB.Count() != 0 ? todosEpisodiosDB.First() : null;
            }
        }

        public static Episode GetEpisode(int IDApi)
        {
            using (Context db = new Context())
            {
                var todosEpisodiosDB = from episodioDB in db.Episode
                                       where episodioDB.IDTvdb == IDApi
                                       select episodioDB;
                return todosEpisodiosDB.Count() != 0 ? todosEpisodiosDB.First() : null;
            }
        }

        public static List<Episode> GetEpisodesToRename()
        {
            using (Context db = new Context())
            {
                var allEpisodesDB = from episodeDB in db.Episode
                                    where !episodeDB.IsRenamed
                                    select episodeDB;

                var allEpisodes = allEpisodesDB.ToList();
                foreach (var item in allEpisodes)
                {
                    item.Serie = db.Serie.Find(item.IDSerie);
                }
                return allEpisodes;
            }
        }

        public static List<SerieAlias> GetSerieAliases(Video video)
        {
            using (Context db = new Context())
            {
                var aliases = from aliasDB in db.SerieAlias
                              where aliasDB.IDSerie == video.IDBanco
                              select aliasDB;
                return aliases != null ? aliases.ToList() : null;
            }
        }

        public static List<SerieAlias> GetAllAliases()
        {
            using (Context db = new Context())
            {
                var aliases = from aliasDB in db.SerieAlias
                              select aliasDB;
                return aliases != null ? aliases.ToList() : null;
            }
        }

        public async static Task<bool> AddSerieAsync(Serie serie)
        {
            try
            {
                if (!Directory.Exists(serie.FolderMetadata))
                    Directory.CreateDirectory(serie.FolderMetadata);

                if (!await Helper.DownloadImages(serie))
                { MessageBox.Show("Erro ao baixar as imagens."); return false; }

                using (Context db = new Context())
                {
                    db.Serie.Add(serie);
                    db.SaveChanges();
                }
                await VerificaEpisodiosNoDiretorioAsync(serie);
                return true;
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao adicionar o conteúdo.", true); return false; }
        }

        private static async Task<bool> VerificaEpisodiosNoDiretorioAsync(Serie serie)
        {
            try
            {
                if (Directory.Exists(serie.FolderPath))
                {
                    var arquivos = new DirectoryInfo(serie.FolderPath).EnumerateFiles("*.*", SearchOption.AllDirectories);
                    string[] extensoesPermitidas = Properties.Settings.Default.ExtensoesRenomeioPermitidas.Split('|');

                    foreach (var item in arquivos)
                    {
                        if (extensoesPermitidas.Contains(item.Extension))
                        {
                            EpisodeToRename episode = new EpisodeToRename();
                            episode.Filename = item.Name;
                            episode.FolderPath = item.DirectoryName;
                            episode.Serie = serie;

                            if (await episode.GetEpisodeAsync())
                            {
                                episode.OriginalFilePath = item.FullName;
                                episode.FilenameRenamed = episode.Serie.IsAnime
                                    ? episode.ParentTitle + " - " + string.Format("{0:00}", episode.AbsoluteNumber) + " - " + episode.EpisodeName + item.Extension
                                    : episode.ParentTitle + " - " + episode.SeasonNumber + "x" + string.Format("{0:00}", episode.EpisodeNumber) + " - " + episode.EpisodeName + item.Extension;
                                episode.FilenameRenamed = Helper.RetirarCaracteresInvalidos(episode.FilenameRenamed);
                                if (episode.Filename == episode.FilenameRenamed)
                                    episode.IsRenamed = true;

                                using (Context db = new Context())
                                {
                                    Episode episodeDB = GetEpisode(serie.IDBanco, episode.SeasonNumber, episode.EpisodeNumber);
                                    episodeDB = db.Episode.Find(episodeDB.IDBanco);
                                    episodeDB.FilePath = item.FullName;
                                    episodeDB.FolderPath = episode.FolderPath;
                                    episodeDB.IsRenamed = episode.IsRenamed;
                                    episodeDB.OriginalFilePath = episode.OriginalFilePath;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao verificar os episódios no diretório."); return false; }
            return true;
        }

        private static bool AddEpisodios(Serie atualizado)
        {
            using (Context db = new Context())
            {
                foreach (var item in atualizado.Episodes)
                {
                    item.IDSerie = atualizado.IDBanco;
                    db.Episode.Add(item);
                }
                db.SaveChanges();
            }
            return true;
        }

        public static bool AddEpisodio(Episode episode)
        {
            try
            {
                using (Context db = new Context())
                {
                    if (episode.IDBanco > 0)
                        db.Episode.Add(episode);
                    else
                    {
                        Serie serie = GetSerieOuAnimePorIDApi(episode.IDSeriesTvdb);
                        episode.IDSerie = serie.IDBanco;
                        db.Episode.Add(episode);
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); return false; }
        }

        public static bool AddSerieAlias(SerieAlias alias)
        {
            try
            {
                using (Context db = new Context())
                {
                    db.SerieAlias.Add(alias);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool AddSerieAlias(Video video)
        {
            try
            {
                using (Context db = new Context())
                {
                    foreach (var item in video.SerieAliasStr.Split('|'))
                    {
                        SerieAlias alias = new SerieAlias(item);
                        alias.Episodio = 1;
                        alias.Temporada = 1;
                        alias.IDSerie = video.IDBanco;
                        db.SerieAlias.Add(alias);
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return false;
            }
        }

        public async static Task<bool> UpdateSerieAsync(Serie atualizado)
        {
            bool isDiferente = false;
            Serie original = null;
            Serie serieOld = new Serie();
            try
            {
                using (Context db = new Context())
                {
                    original = db.Serie.Find(atualizado.IDBanco);
                    serieOld.Clone(original);
                    if (original.IDApi != atualizado.IDApi)
                        isDiferente = true;

                    if (original != null)
                    {
                        db.Entry(original).CurrentValues.SetValues(atualizado);
                        if (isDiferente)
                        {
                            db.Episode.RemoveRange(db.Episode.Where(x => x.IDSeriesTvdb == serieOld.IDApi));
                            db.SerieAlias.RemoveRange(db.SerieAlias.Where(x => x.IDSerie == atualizado.IDBanco));
                            AddEpisodios(atualizado);
                            AddSerieAlias(atualizado);
                        }
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocorreu um erro ao tentar atualizar a série no banco de dados.\r\nErro:" + e.Message, Properties.Settings.Default.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (isDiferente || serieOld.FolderMetadata != atualizado.FolderMetadata) // Pode acontecer da serie ser a mesma mas o nome ter alterado, alterando tb o folderMetadata.
            {
                if (Directory.Exists(serieOld.FolderMetadata))
                {
                    DirectoryInfo metaDir = new DirectoryInfo(serieOld.FolderMetadata);

                    foreach (FileInfo file in metaDir.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in metaDir.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    Directory.Delete(metaDir.FullName);
                }

                if (!Directory.Exists(atualizado.FolderMetadata))
                    Directory.CreateDirectory(atualizado.FolderMetadata);

                if (!await Helper.DownloadImages(atualizado))
                { MessageBox.Show("Erro ao baixar as imagens."); return false; }
            }
            return true;
        }

        public static bool UpdateEpisodio(Episode atualizado)
        {
            Episode original = null;
            try
            {
                using (Context db = new Context())
                {
                    original = db.Episode.Find(atualizado.IDBanco);

                    if (original != null)
                    {
                        original.AbsoluteNumber = atualizado.AbsoluteNumber;
                        //original.AirsAfterSeason = atualizado.AirsAfterSeason;
                        //original.AirsBeforeEpisode = atualizado.AirsBeforeEpisode;
                        //original.AirsBeforeSeason = atualizado.AirsBeforeSeason;
                        original.Artwork = atualizado.Artwork;
                        original.EpisodeName = atualizado.EpisodeName;
                        original.EpisodeNumber = atualizado.EpisodeNumber;
                        original.FirstAired = atualizado.FirstAired;
                        original.IDSeasonTvdb = atualizado.IDSeasonTvdb;
                        original.IDSeriesTvdb = atualizado.IDSeriesTvdb;
                        original.IDTvdb = atualizado.IDTvdb;
                        original.Language = atualizado.Language;
                        original.LastUpdated = atualizado.LastUpdated;
                        original.Overview = atualizado.Overview;
                        original.Rating = atualizado.Rating;
                        //original.RatingCount = atualizado.RatingCount;
                        original.SeasonNumber = atualizado.SeasonNumber;
                        original.ThumbAddedDate = atualizado.ThumbAddedDate;

                        db.SaveChanges();
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); return false; }
        }

        public static bool UpdateEpisodioRenomeado(Episode atualizado)
        {
            Episode original;
            using (Context db = new Context())
            {
                original = db.Episode.Find(atualizado.IDBanco);
                if (original != null)
                {
                    original.FolderPath = atualizado.Serie.FolderPath;
                    original.OriginalFilePath = atualizado.OriginalFilePath;
                    original.FilePath = atualizado.FilePath;
                    original.IsRenamed = atualizado.IsRenamed;
                    db.SaveChanges();
                    return true;
                }
                else return false;
            }
        }

        public static bool RemoveSerieAlias(SerieAlias alias)
        {
            try
            {
                using (Context db = new Context())
                {
                    db.SerieAlias.Attach(alias);
                    db.SerieAlias.Remove(alias);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool VerificarSeEpisodioJaFoiRenomeado(string filePath)
        {
            using (Context db = new Context())
            {
                var episodios = from episodiosDB in db.Episode
                                where episodiosDB.OriginalFilePath == filePath
                                select episodiosDB;
                return episodios.Count() > 0 ? true : false;
            }
        }

        public static bool VerificaSeExiste(int IDApi)
        {
            using (Context db = new Context())
            {
                var series = from seriesDB in db.Serie where seriesDB.IDApi == IDApi select seriesDB;
                return series.Count() > 0 ? true : false;
            }
        }

        public static bool VerificaSeExiste(string folderPath)
        {
            using (Context db = new Context())
            {
                var series = from serie in db.Serie
                             where serie.FolderPath == folderPath
                             select serie;
                //var filmes = from filme in db.Filmes
                //             where filme.FolderPath == folderPath
                //             select filme;

                return (series.Count() > 0 /*|| filmes.Count() > 0*/) ? true : false;
            }
        }
    }
}