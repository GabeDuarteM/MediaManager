using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MediaManager.Forms;
using MediaManager.Model;

namespace MediaManager.Helpers
{
    public class DBHelper
    {
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

        public static bool AddEpisodio(Episode episode)
        {
            try
            {
                using (Context db = new Context())
                {
                    db.Episode.Add(episode);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); return false; }
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
                    foreach (var item in video.AliasNamesStr.Split('|'))
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

        /// <summary>
        /// Adiciona o filme de forma assíncrona no banco.
        /// </summary>
        /// <param name="filme">Filme a ser adicionado.</param>
        /// <returns>True caso o filme tenha sido adicionado com sucesso.</returns>
        public async static Task<bool> AddFilmeAsync(Filme filme)
        {
            throw new NotImplementedException();
            bool retorno = false;
            try
            {
                //if (!Directory.Exists(filme.FolderMetadata))
                //    Directory.CreateDirectory(filme.FolderMetadata);

                //if (!await Helper.DownloadImages(filme))
                //{ MessageBox.Show("Erro ao baixar as imagens."); retorno = false; }

                //using (Context db = new Context())
                //{
                //    filme.Images.Filme = filme;
                //    filme.Ids.Filme = filme;
                //    db.Filmes.Add(filme);
                //    db.Images.Add(filme.Images);
                //    db.Ids.Add(filme.Ids);
                //    db.SaveChanges();
                //    retorno = true;
                //}
                return retorno;
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); return false; }
        }

        public async static Task<bool> AddSerieAsync(Serie serie)
        {
            bool retorno = false;
            try
            {
                if (!Directory.Exists(serie.FolderMetadata))
                    Directory.CreateDirectory(serie.FolderMetadata);

                if (!await Helper.DownloadImages(serie))
                { MessageBox.Show("Erro ao baixar as imagens."); retorno = false; }

                using (Context db = new Context())
                {
                    db.Serie.Add(serie);
                    foreach (var item in serie.Episodes)
                    {
                        item.Serie = serie;
                        db.Episode.Add(item);
                    }

                    db.SaveChanges();

                    if (serie.AliasNamesStr != null)
                        AddSerieAlias(serie);

                    retorno = true;
                }
                return retorno;
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); return false; }
        }

        //internal async static Task<bool> AddSerieAsync(SerieOld serie)
        //{
        //    bool retorno = false;
        //    try
        //    {
        //        if (!Directory.Exists(serie.FolderMetadata))
        //            Directory.CreateDirectory(serie.FolderMetadata);

        //        if (!await Helper.DownloadImages(serie))
        //        { MessageBox.Show("Erro ao baixar as imagens."); retorno = false; }

        //        //using (Context db = new Context())
        //        //{
        //        //    serie.Images.Serie = serie;
        //        //    serie.Ids.Serie = serie;
        //        //    db.Series.Add(serie);
        //        //    db.Images.Add(serie.Images);
        //        //    db.Ids.Add(serie.Ids);
        //        //    db.SaveChanges();
        //        //    retorno = true;
        //        //}
        //        return retorno;
        //    }
        //    catch (Exception e) { Console.WriteLine(e.InnerException); return false; }
        //}

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
                    item.ContentType = item.IsAnime == true ? Helper.Enums.ContentType.anime
                        : Helper.Enums.ContentType.show;
                    item.Estado = Estado.COMPLETO_SEM_EPISODIO;
                }
                return animes;
            }
        }

        /// <summary>
        /// Consulta todos os filmes contidos no banco que tenham a id especificada.
        /// </summary>
        /// <param name="IdBanco">ID do filme.</param>
        /// <returns>Retorna o filme caso este exista no banco, ou null caso não exista.</returns>
        public static Filme GetFilmePorID(int IdBanco)
        {
            //using (Context db = new Context())
            //{
            //    Filme filme = (from filmeDb in db.Filmes
            //                   where filmeDb.ID == IdBanco
            //                   select filmeDb).First();
            //    filme.Ids = (from IdsDb in db.Ids
            //                 where IdsDb.Filme.ID == IdBanco
            //                 select IdsDb).First();
            //    filme.Images = (from ImagesDb in db.Images
            //                    where ImagesDb.Filme.ID == IdBanco
            //                    select ImagesDb).First();
            //    return filme;
            //}
            return null;
        }

        /// <summary>
        /// Consulta todos os filmes contidos no banco.
        /// </summary>
        /// <returns>Retorna um List<Filme> contendo todos os filmes contidos no banco ordenados pelo título.</returns>
        public static List<Filme> GetFilmes()
        {
            using (Context db = new Context())
            {
                //List<Filme> filmes = (from filmeDb in db.Filmes
                //                      orderby filmeDb.Title
                //                      select filmeDb).ToList();
                //foreach (var item in filmes)
                //{
                //    item.Ids = (from IdsDb in db.Ids
                //                where IdsDb.Filme.ID == item.ID
                //                select IdsDb).First();
                //    item.Images = (from ImagesDb in db.Images
                //                   where ImagesDb.Filme.ID == item.ID
                //                   select ImagesDb).First();
                //}
                //return filmes;
                return null; // <<retirar
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
                serie.Estado = Estado.COMPLETO_SEM_EPISODIO;
                return serie;
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
                    item.ContentType = item.IsAnime == true ? Helper.Enums.ContentType.anime
                        : Helper.Enums.ContentType.show;
                    item.Estado = Estado.COMPLETO_SEM_EPISODIO;
                }
                return series;
            }
        }

        /// <summary>
        /// Realiza o update do anime informado de forma assíncrona.
        /// </summary>
        /// <param name="atualizado">Anime atualizado (precisa estar com o ID preenchido).</param>
        /// <returns>Retorna true caso a operação tenha sucesso.</returns>
        //internal async static Task<bool> UpdateAnimeAsync(SerieOld atualizado)
        //{
        //    bool isDiferente = false;
        //    bool retorno = false;
        //    SerieOld original = null;
        //    string originalMetadata = null;
        //    try
        //    {
        //        //using (Context db = new Context())
        //        //{
        //        //    original = db.Series.Find(atualizado.ID);
        //        //    var ids = (from idsDB in db.Ids
        //        //               where idsDB.Serie.ID == original.ID
        //        //               select idsDB).First();
        //        //    var images = (from imagedDB in db.Images
        //        //                  where imagedDB.Serie.ID == original.ID
        //        //                  select imagedDB).First();
        //        //    original.Ids = ids;
        //        //    original.Images = images;
        //        //    originalMetadata = original.MetadataFolder;

        //        //    if (original.Ids.slug != atualizado.Ids.slug)
        //        //        isDiferente = true;

        //        //    if (original != null)
        //        //    {
        //        //        atualizado.Images.ID = original.Images.ID;
        //        //        atualizado.Ids.ID = original.Ids.ID;
        //        //        db.Entry(original).CurrentValues.SetValues(atualizado);
        //        //        db.Entry(original.Images).CurrentValues.SetValues(atualizado.Images);
        //        //        db.Entry(original.Ids).CurrentValues.SetValues(atualizado.Ids);
        //        //        db.SaveChanges();
        //        //    }
        //        //}
        //        retorno = true;
        //    }
        //    catch (Exception e) { Console.WriteLine(e.InnerException); return false; }

        //    if (isDiferente)
        //    {
        //        if (Directory.Exists(originalMetadata))
        //        {
        //            DirectoryInfo metaDir = new DirectoryInfo(originalMetadata);

        //            foreach (FileInfo file in metaDir.GetFiles())
        //            {
        //                file.Delete();
        //            }
        //            foreach (DirectoryInfo dir in metaDir.GetDirectories())
        //            {
        //                dir.Delete(true);
        //            }
        //            Directory.Delete(metaDir.FullName);
        //        }

        //        if (!Directory.Exists(atualizado.FolderMetadata))
        //            Directory.CreateDirectory(atualizado.FolderMetadata);

        //        if (!await Helper.DownloadImages(atualizado))
        //        { MessageBox.Show("Erro ao baixar as imagens."); retorno = false; }
        //    }
        //    return retorno;
        //}

        /// <summary>
        /// Realiza o update do filme informado de forma assíncrona.
        /// </summary>
        /// <param name="atualizado">Filme atualizado (precisa estar com o ID preenchido).</param>
        /// <returns>Retorna true caso a operação tenha sucesso.</returns>
        public async static Task<bool> UpdateFilmeAsync(Filme atualizado)
        {
            //bool isDiferente = false;
            bool retorno = false;
            //Filme original = null;
            //string originalMetadata = null;
            //try
            //{
            //    using (Context db = new Context())
            //    {
            //        original = db.Filmes.Find(atualizado.IDBanco);
            //        var ids = (from idsDB in db.Ids
            //                   where idsDB.Filme.IDBanco == original.IDBanco
            //                   select idsDB).First();
            //        var images = (from imagedDB in db.Images
            //                      where imagedDB.Filme.IDBanco == original.IDBanco
            //                      select imagedDB).First();
            //        original.Ids = ids;
            //        original.Images = images;
            //        originalMetadata = original.FolderMetadata;

            //        if (original.Ids.slug != atualizado.Ids.slug)
            //            isDiferente = true;

            //        if (original != null)
            //        {
            //            atualizado.Images.ID = original.Images.ID;
            //            atualizado.Ids.ID = original.Ids.ID;
            //            db.Entry(original).CurrentValues.SetValues(atualizado);
            //            db.Entry(original.Images).CurrentValues.SetValues(atualizado.Images);
            //            db.Entry(original.Ids).CurrentValues.SetValues(atualizado.Ids);
            //            db.SaveChanges();
            //        }
            //    }
            //    retorno = true;
            //}
            //catch (Exception e) { Console.WriteLine(e.InnerException); return false; }

            //if (isDiferente)
            //{
            //    if (Directory.Exists(originalMetadata))
            //    {
            //        DirectoryInfo metaDir = new DirectoryInfo(originalMetadata);

            //        foreach (FileInfo file in metaDir.GetFiles())
            //        {
            //            file.Delete();
            //        }
            //        foreach (DirectoryInfo dir in metaDir.GetDirectories())
            //        {
            //            dir.Delete(true);
            //        }
            //        Directory.Delete(metaDir.FullName);
            //    }

            //    if (!Directory.Exists(atualizado.FolderMetadata))
            //        Directory.CreateDirectory(atualizado.FolderMetadata);

            //    if (!await Helper.DownloadImages(atualizado))
            //    { MessageBox.Show("Erro ao baixar as imagens."); retorno = false; }
            //}
            return retorno;
        }

        /// <summary>
        /// Realiza o update da série informada de forma assíncrona.
        /// </summary>
        /// <param name="atualizado">Série atualizada (precisa estar com o ID preenchido).</param>
        /// <returns>Retorna true caso a operação tenha sucesso.</returns>
        //internal async static Task<bool> UpdateSerieAsync(SerieOld atualizado)
        //{
        //    bool isDiferente = false;
        //    bool retorno = false;
        //    //SerieOld original = null;
        //    //string originalMetadata = null;
        //    //try
        //    //{
        //    //    using (Context db = new Context())
        //    //    {
        //    //        original = db.Series.Find(atualizado.ID);
        //    //        var ids = from idsDB in db.Ids
        //    //                  where idsDB.Serie.ID == original.ID
        //    //                  select idsDB;
        //    //        var images = from imagedDB in db.Images
        //    //                     where imagedDB.Serie.ID == original.ID
        //    //                     select imagedDB;
        //    //        original.Images = images.First();
        //    //        original.Ids = ids.First();
        //    //        originalMetadata = original.MetadataFolder;
        //    //        if (original.Ids.slug != atualizado.Ids.slug)
        //    //            isDiferente = true;

        //    //        if (original != null)
        //    //        {
        //    //            atualizado.Images.ID = original.Images.ID;
        //    //            atualizado.Ids.ID = original.Ids.ID;
        //    //            db.Entry(original).CurrentValues.SetValues(atualizado);
        //    //            db.Entry(original.Images).CurrentValues.SetValues(atualizado.Images);
        //    //            db.Entry(original.Ids).CurrentValues.SetValues(atualizado.Ids);
        //    //            db.SaveChanges();
        //    //        }
        //    //    }
        //    //    retorno = true;
        //    //}
        //    //catch (Exception e) { Console.WriteLine(e.InnerException); return false; }

        //    //if (isDiferente)
        //    //{
        //    //    if (Directory.Exists(originalMetadata))
        //    //    {
        //    //        DirectoryInfo metaDir = new DirectoryInfo(originalMetadata);

        //    //        foreach (FileInfo file in metaDir.GetFiles())
        //    //        {
        //    //            file.Delete();
        //    //        }
        //    //        foreach (DirectoryInfo dir in metaDir.GetDirectories())
        //    //        {
        //    //            dir.Delete(true);
        //    //        }
        //    //        Directory.Delete(metaDir.FullName);
        //    //    }

        //    //    if (!Directory.Exists(atualizado.MetadataFolder))
        //    //        Directory.CreateDirectory(atualizado.MetadataFolder);

        //    //    if (!await Helper.DownloadImages(atualizado))
        //    //    { MessageBox.Show("Erro ao baixar as imagens."); retorno = false; }
        //    //}
        //    return retorno;
        //}

        public async static Task<bool> UpdateSerieAsync(Serie atualizado)
        {
            bool isDiferente = false;
            bool retorno = false;
            Serie original = null;
            string oldMetadata = null;
            int oldIDApi = -1;
            try
            {
                using (Context db = new Context())
                {
                    original = db.Serie.Find(atualizado.IDBanco);
                    oldMetadata = original.FolderMetadata;
                    oldIDApi = original.IDApi;
                    if (original.IDApi != atualizado.IDApi)
                        isDiferente = true;

                    if (original != null)
                    {
                        db.Entry(original).CurrentValues.SetValues(atualizado);
                        db.SaveChanges();
                    }
                }
                retorno = true;
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); return false; }

            if (isDiferente || oldMetadata != atualizado.FolderMetadata) // Pode acontecer da serie ser a mesma mas o nome ter alterado, alterando tb o folderMetadata.
            {
                if (Directory.Exists(oldMetadata))
                {
                    DirectoryInfo metaDir = new DirectoryInfo(oldMetadata);

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
                { MessageBox.Show("Erro ao baixar as imagens."); retorno = false; }

                using (Context db = new Context())
                {
                    db.Episode.RemoveRange(db.Episode.Where(x => x.IDSeriesTvdb == oldIDApi));
                    foreach (var item in atualizado.Episodes)
                    {
                        item.IDSerie = atualizado.IDBanco;
                        db.Episode.Add(item);
                    }
                    db.SerieAlias.RemoveRange(db.SerieAlias.Where(x => x.IDSerie == atualizado.IDBanco));
                    db.SaveChanges();
                    if (!string.IsNullOrWhiteSpace(atualizado.AliasNamesStr))
                        AddSerieAlias(atualizado);
                }
            }
            return retorno;
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