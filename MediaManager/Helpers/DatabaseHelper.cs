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
    public class DatabaseHelper
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

        /// <summary>
        /// Adiciona o filme de forma assíncrona no banco.
        /// </summary>
        /// <param name="filme">Filme a ser adicionado.</param>
        /// <returns>True caso o filme tenha sido adicionado com sucesso.</returns>
        internal async static Task<bool> AddFilmeAsync(Filme filme)
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

        internal async static Task<bool> AddSerieAsync(Serie serie)
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
        /// Consulta todos os animes contidos no banco que tenham a id especificada.
        /// </summary>
        /// <param name="IDBanco">ID do anime.</param>
        /// <returns>Retorna o anime caso este exista no banco, ou null caso não exista.</returns>
        internal static Serie GetAnimePorID(int IDBanco)
        {
            using (Context db = new Context())
            {
                var animesDB = (from animeDB in db.Serie
                                where animeDB.IsAnime && animeDB.IDBanco == IDBanco
                                select animeDB);
                Serie anime = animesDB.First();
                return anime;
            }
        }

        /// <summary>
        /// Consulta todos os animes contidos no banco.
        /// </summary>
        /// <returns>Retorna um List<Serie> contendo todos os animes que tenham serie.isAnime == true ordenados pelo título.</returns>
        internal static List<Serie> GetAnimes()
        {
            using (Context db = new Context())
            {
                var animesDB = (from animeDB in db.Serie
                                where animeDB.IsAnime
                                orderby animeDB.Title
                                select animeDB);
                List<Serie> animes = animesDB.ToList();
                return animes;
            }
        }

        /// <summary>
        /// Consulta todos os filmes contidos no banco que tenham a id especificada.
        /// </summary>
        /// <param name="IdBanco">ID do filme.</param>
        /// <returns>Retorna o filme caso este exista no banco, ou null caso não exista.</returns>
        internal static Filme GetFilmePorID(int IdBanco)
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
        internal static List<Filme> GetFilmes()
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
        internal static Serie GetSeriePorID(int IDBanco)
        {
            using (Context db = new Context())
            {
                var seriesDB = (from serieDB in db.Serie
                                where !serieDB.IsAnime && serieDB.IDBanco == IDBanco
                                select serieDB);
                Serie serie = seriesDB.First();
                return serie;
            }
        }

        /// <summary>
        /// Consulta todas as séries contidas no banco.
        /// </summary>
        /// <returns>Retorna um List<Serie> contendo todas as séries que tenham serie.isAnime == false ordenados pelo título.</returns>
        internal static List<Serie> GetSeries()
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
        internal async static Task<bool> UpdateFilmeAsync(Filme atualizado)
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

        internal async static Task<bool> UpdateSerieAsync(Serie atualizado)
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

            if (isDiferente)
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
                    var serieDoEpisodio = db.Serie.Find(original.IDBanco);
                    foreach (var item in atualizado.Episodes)
                    {
                        item.Serie = serieDoEpisodio;
                        db.Episode.Add(item);
                    }
                    db.SaveChanges();
                }
            }
            return retorno;
        }

        internal static bool VerificaSeExiste(int IDApi)
        {
            using (Context db = new Context())
            {
                var series = from seriesDB in db.Serie where seriesDB.IDApi == IDApi select seriesDB;
                return series.Count() > 0 ? true : false;
            }
        }

        internal static bool VerificaSeExiste(string folderPath)
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