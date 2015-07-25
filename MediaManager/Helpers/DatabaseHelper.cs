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
        /// <summary>
        /// Adiciona o anime de forma assíncrona no banco.
        /// </summary>
        /// <param name="anime">Anime a ser adicionado.</param>
        /// <returns>True caso o anime tenha sido adicionado com sucesso.</returns>
        internal async static Task<bool> AddAnimeAsync(Serie anime)
        {
            bool retorno = false;
            try
            {
                if (!Directory.Exists(anime.MetadataFolder))
                    Directory.CreateDirectory(anime.MetadataFolder);

                if (!await Helper.DownloadImages(anime))
                { MessageBox.Show("Erro ao baixar as imagens."); retorno = false; }

                using (Context db = new Context())
                {
                    anime.Images.Serie = anime;
                    anime.Ids.Serie = anime;
                    db.Series.Add(anime);
                    db.Images.Add(anime.Images);
                    db.Ids.Add(anime.Ids);
                    db.SaveChanges();
                    retorno = true;
                }
                return retorno;
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); return false; }
        }

        /// <summary>
        /// Adiciona o filme de forma assíncrona no banco.
        /// </summary>
        /// <param name="filme">Filme a ser adicionado.</param>
        /// <returns>True caso o filme tenha sido adicionado com sucesso.</returns>
        internal async static Task<bool> AddFilmeAsync(Filme filme)
        {
            bool retorno = false;
            try
            {
                if (!Directory.Exists(filme.MetadataFolder))
                    Directory.CreateDirectory(filme.MetadataFolder);

                if (!await Helper.DownloadImages(filme))
                { MessageBox.Show("Erro ao baixar as imagens."); retorno = false; }

                using (Context db = new Context())
                {
                    filme.Images.Filme = filme;
                    filme.Ids.Filme = filme;
                    db.Filmes.Add(filme);
                    db.Images.Add(filme.Images);
                    db.Ids.Add(filme.Ids);
                    db.SaveChanges();
                    retorno = true;
                }
                return retorno;
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); return false; }
        }

        /// <summary>
        /// Adiciona a série de forma assíncrona no banco.
        /// </summary>
        /// <param name="serie">Série a ser adicionada.</param>
        /// <returns>True caso a série tenha sido adicionada com sucesso.</returns>
        internal async static Task<bool> AddSerieAsync(Serie serie)
        {
            bool retorno = false;
            try
            {
                if (!Directory.Exists(serie.MetadataFolder))
                    Directory.CreateDirectory(serie.MetadataFolder);

                if (!await Helper.DownloadImages(serie))
                { MessageBox.Show("Erro ao baixar as imagens."); retorno = false; }

                using (Context db = new Context())
                {
                    serie.Images.Serie = serie;
                    serie.Ids.Serie = serie;
                    db.Series.Add(serie);
                    db.Images.Add(serie.Images);
                    db.Ids.Add(serie.Ids);
                    db.SaveChanges();
                    retorno = true;
                }
                return retorno;
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); return false; }
        }

        /// <summary>
        /// Consulta todos os animes contidos no banco que tenham a id especificada.
        /// </summary>
        /// <param name="IdBanco">ID do anime.</param>
        /// <returns>Retorna o anime caso este exista no banco, ou null caso não exista.</returns>
        internal static Serie GetAnimePorId(int IdBanco)
        {
            using (Context db = new Context())
            {
                Serie serie = (from animeDb in db.Series
                               where animeDb.IsAnime && animeDb.ID == IdBanco
                               select animeDb).First();
                serie.Ids = (from IdsDb in db.Ids
                             where IdsDb.Serie.ID == IdBanco
                             select IdsDb).First();
                serie.Images = (from ImagesDb in db.Images
                                where ImagesDb.Serie.ID == IdBanco
                                select ImagesDb).First();
                return serie;
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
                List<Serie> animes = (from animeDb in db.Series
                                      where animeDb.IsAnime
                                      orderby animeDb.Title
                                      select animeDb).ToList();
                foreach (var item in animes)
                {
                    item.Ids = (from IdsDb in db.Ids
                                where IdsDb.Serie.ID == item.ID
                                select IdsDb).First();
                    item.Images = (from ImagesDb in db.Images
                                   where ImagesDb.Serie.ID == item.ID
                                   select ImagesDb).First();
                }
                return animes;
            }
        }

        /// <summary>
        /// Consulta todos os filmes contidos no banco que tenham a id especificada.
        /// </summary>
        /// <param name="IdBanco">ID do filme.</param>
        /// <returns>Retorna o filme caso este exista no banco, ou null caso não exista.</returns>
        internal static Filme GetFilmePorId(int IdBanco)
        {
            using (Context db = new Context())
            {
                Filme filme = (from filmeDb in db.Filmes
                               where filmeDb.ID == IdBanco
                               select filmeDb).First();
                filme.Ids = (from IdsDb in db.Ids
                             where IdsDb.Filme.ID == IdBanco
                             select IdsDb).First();
                filme.Images = (from ImagesDb in db.Images
                                where ImagesDb.Filme.ID == IdBanco
                                select ImagesDb).First();
                return filme;
            }
        }

        /// <summary>
        /// Consulta todos os filmes contidos no banco.
        /// </summary>
        /// <returns>Retorna um List<Filme> contendo todos os filmes contidos no banco ordenados pelo título.</returns>
        internal static List<Filme> GetFilmes()
        {
            using (Context db = new Context())
            {
                List<Filme> filmes = (from filmeDb in db.Filmes
                                      orderby filmeDb.Title
                                      select filmeDb).ToList();
                foreach (var item in filmes)
                {
                    item.Ids = (from IdsDb in db.Ids
                                where IdsDb.Filme.ID == item.ID
                                select IdsDb).First();
                    item.Images = (from ImagesDb in db.Images
                                   where ImagesDb.Filme.ID == item.ID
                                   select ImagesDb).First();
                }
                return filmes;
            }
        }

        /// <summary>
        /// Consulta todos as séries contidas no banco que tenham a id especificada.
        /// </summary>
        /// <param name="IdBanco">ID da série.</param>
        /// <returns>Retorna a série caso esta exista no banco, ou null caso não exista.</returns>
        internal static Serie GetSeriePorId(int IdBanco)
        {
            using (Context db = new Context())
            {
                Serie serie = (from serieDb in db.Series
                               where !serieDb.IsAnime && serieDb.ID == IdBanco
                               select serieDb).First();
                serie.Ids = (from IdsDb in db.Ids
                             where IdsDb.Serie.ID == IdBanco
                             select IdsDb).First();
                serie.Images = (from ImagesDb in db.Images
                                where ImagesDb.Serie.ID == IdBanco
                                select ImagesDb).First();
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
                List<Serie> series = (from serieDb in db.Series
                                      where !serieDb.IsAnime
                                      orderby serieDb.Title
                                      select serieDb).ToList();
                foreach (var item in series)
                {
                    item.Ids = (from IdsDb in db.Ids
                                where IdsDb.Serie.ID == item.ID
                                select IdsDb).First();
                    item.Images = (from ImagesDb in db.Images
                                   where ImagesDb.Serie.ID == item.ID
                                   select ImagesDb).First();
                }
                return series;
            }
        }

        /// <summary>
        /// Realiza o update do anime informado de forma assíncrona.
        /// </summary>
        /// <param name="atualizado">Anime atualizado (precisa estar com o ID preenchido).</param>
        /// <returns>Retorna true caso a operação tenha sucesso.</returns>
        internal async static Task<bool> UpdateAnimeAsync(Serie atualizado)
        {
            bool isDiferente = false;
            bool retorno = false;
            Serie original = null;
            string originalMetadata = null;
            try
            {
                using (Context db = new Context())
                {
                    original = db.Series.Find(atualizado.ID);
                    var ids = (from idsDB in db.Ids
                               where idsDB.Serie.ID == original.ID
                               select idsDB).First();
                    var images = (from imagedDB in db.Images
                                  where imagedDB.Serie.ID == original.ID
                                  select imagedDB).First();
                    original.Ids = ids;
                    original.Images = images;
                    originalMetadata = original.MetadataFolder;

                    if (original.Ids.slug != atualizado.Ids.slug)
                        isDiferente = true;

                    if (original != null)
                    {
                        atualizado.Images.ID = original.Images.ID;
                        atualizado.Ids.ID = original.Ids.ID;
                        db.Entry(original).CurrentValues.SetValues(atualizado);
                        db.Entry(original.Images).CurrentValues.SetValues(atualizado.Images);
                        db.Entry(original.Ids).CurrentValues.SetValues(atualizado.Ids);
                        db.SaveChanges();
                    }
                }
                retorno = true;
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); return false; }

            if (isDiferente)
            {
                if (Directory.Exists(originalMetadata))
                {
                    DirectoryInfo metaDir = new DirectoryInfo(originalMetadata);

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

                if (!Directory.Exists(atualizado.MetadataFolder))
                    Directory.CreateDirectory(atualizado.MetadataFolder);

                if (!await Helper.DownloadImages(atualizado))
                { MessageBox.Show("Erro ao baixar as imagens."); retorno = false; }
            }
            return retorno;
        }

        /// <summary>
        /// Realiza o update do filme informado de forma assíncrona.
        /// </summary>
        /// <param name="atualizado">Filme atualizado (precisa estar com o ID preenchido).</param>
        /// <returns>Retorna true caso a operação tenha sucesso.</returns>
        internal async static Task<bool> UpdateFilmeAsync(Filme atualizado)
        {
            bool isDiferente = false;
            bool retorno = false;
            Filme original = null;
            string originalMetadata = null;
            try
            {
                using (Context db = new Context())
                {
                    original = db.Filmes.Find(atualizado.ID);
                    var ids = (from idsDB in db.Ids
                               where idsDB.Filme.ID == original.ID
                               select idsDB).First();
                    var images = (from imagedDB in db.Images
                                  where imagedDB.Filme.ID == original.ID
                                  select imagedDB).First();
                    original.Ids = ids;
                    original.Images = images;
                    originalMetadata = original.MetadataFolder;

                    if (original.Ids.slug != atualizado.Ids.slug)
                        isDiferente = true;

                    if (original != null)
                    {
                        atualizado.Images.ID = original.Images.ID;
                        atualizado.Ids.ID = original.Ids.ID;
                        db.Entry(original).CurrentValues.SetValues(atualizado);
                        db.Entry(original.Images).CurrentValues.SetValues(atualizado.Images);
                        db.Entry(original.Ids).CurrentValues.SetValues(atualizado.Ids);
                        db.SaveChanges();
                    }
                }
                retorno = true;
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); return false; }

            if (isDiferente)
            {
                if (Directory.Exists(originalMetadata))
                {
                    DirectoryInfo metaDir = new DirectoryInfo(originalMetadata);

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

                if (!Directory.Exists(atualizado.MetadataFolder))
                    Directory.CreateDirectory(atualizado.MetadataFolder);

                if (!await Helper.DownloadImages(atualizado))
                { MessageBox.Show("Erro ao baixar as imagens."); retorno = false; }
            }
            return retorno;
        }

        /// <summary>
        /// Realiza o update da série informada de forma assíncrona.
        /// </summary>
        /// <param name="atualizado">Série atualizada (precisa estar com o ID preenchido).</param>
        /// <returns>Retorna true caso a operação tenha sucesso.</returns>
        internal async static Task<bool> UpdateSerieAsync(Serie atualizado)
        {
            bool isDiferente = false;
            bool retorno = false;
            Serie original = null;
            string originalMetadata = null;
            try
            {
                using (Context db = new Context())
                {
                    original = db.Series.Find(atualizado.ID);
                    var ids = from idsDB in db.Ids
                              where idsDB.Serie.ID == original.ID
                              select idsDB;
                    var images = from imagedDB in db.Images
                                 where imagedDB.Serie.ID == original.ID
                                 select imagedDB;
                    original.Images = images.First();
                    original.Ids = ids.First();
                    originalMetadata = original.MetadataFolder;
                    if (original.Ids.slug != atualizado.Ids.slug)
                        isDiferente = true;

                    if (original != null)
                    {
                        atualizado.Images.ID = original.Images.ID;
                        atualizado.Ids.ID = original.Ids.ID;
                        db.Entry(original).CurrentValues.SetValues(atualizado);
                        db.Entry(original.Images).CurrentValues.SetValues(atualizado.Images);
                        db.Entry(original.Ids).CurrentValues.SetValues(atualizado.Ids);
                        db.SaveChanges();
                    }
                }
                retorno = true;
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); return false; }

            if (isDiferente)
            {
                if (Directory.Exists(originalMetadata))
                {
                    DirectoryInfo metaDir = new DirectoryInfo(originalMetadata);

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

                if (!Directory.Exists(atualizado.MetadataFolder))
                    Directory.CreateDirectory(atualizado.MetadataFolder);

                if (!await Helper.DownloadImages(atualizado))
                { MessageBox.Show("Erro ao baixar as imagens."); retorno = false; }
            }
            return retorno;
        }

        /// <summary>
        /// Pesquisa se o conteúdo ja existe no banco.
        /// </summary>
        /// <param name="traktId">Id do trakt</param>
        /// <returns>Retorna true se o conteúdo for encontrado no banco</returns>
        internal static bool VerificaSeExiste(int traktId)
        {
            using (Context db = new Context())
            {
                var ids = from id in db.Ids select id.trakt;
                if (ids.Contains(traktId))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Pesquisa se o conteúdo ja existe no banco.
        /// </summary>
        /// <param name="path">Diretório do conteúdo</param>
        /// <returns>Retorna true se o conteúdo for encontrado no banco</returns>
        internal static bool VerificaSeExiste(string path)
        {
            using (Context db = new Context())
            {
                var series = from serie in db.Series
                             where serie.FolderPath == path
                             select serie;
                var filmes = from filme in db.Filmes
                             where filme.FolderPath == path
                             select filme;

                if (series.Count() > 0 || filmes.Count() > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}