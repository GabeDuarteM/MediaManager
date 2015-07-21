using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaManager.Forms;
using MediaManager.Model;

namespace MediaManager.Helpers
{
    public class DatabaseHelper
    {
        /// <summary>
        /// Adiciona o filme de forma assíncrona no banco.
        /// </summary>
        /// <param name="filme">Filme a ser adicionado.</param>
        /// <returns>True caso o filme tenha sido adicionado com sucesso.</returns>
        public async static Task<bool> AddFilmeAsync(Filme filme)
        {
            if (!System.IO.Directory.Exists(filme.MetadataFolder))
                System.IO.Directory.CreateDirectory(filme.MetadataFolder);

            if (filme.Images.poster.thumb != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(filme.MetadataFolder, "poster.jpg");
                    await wc.DownloadFileTaskAsync(new Uri(filme.Images.poster.thumb), path);
                }
            }
            else if (filme.Images.poster.medium != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(filme.MetadataFolder, "poster.jpg");
                    await wc.DownloadFileTaskAsync(new Uri(filme.Images.poster.medium), path);
                }
            }
            else if (filme.Images.poster.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(filme.MetadataFolder, "poster.jpg");
                    await wc.DownloadFileTaskAsync(new Uri(filme.Images.poster.full), path);
                }
            }

            if (filme.Images.banner.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(filme.MetadataFolder, "banner.jpg");
                    await wc.DownloadFileTaskAsync(new Uri(filme.Images.banner.full), path);
                }
            }

            using (Context db = new Context())
            {
                db.Filmes.Add(filme);
                db.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// Adiciona o anime de forma assíncrona no banco.
        /// </summary>
        /// <param name="anime">Anime a ser adicionado.</param>
        /// <returns>True caso o anime tenha sido adicionado com sucesso.</returns>
        public async static Task<bool> AddAnimeAsync(Serie anime)
        {
            if (!System.IO.Directory.Exists(anime.MetadataFolder))
                System.IO.Directory.CreateDirectory(anime.MetadataFolder);

            if (anime.Images.poster.thumb != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(anime.MetadataFolder, "poster.jpg");
                    await wc.DownloadFileTaskAsync(new Uri(anime.Images.poster.thumb), path);
                }
            }
            else if (anime.Images.poster.medium != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(anime.MetadataFolder, "poster.jpg");
                    await wc.DownloadFileTaskAsync(new Uri(anime.Images.poster.medium), path);
                }
            }
            else if (anime.Images.poster.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(anime.MetadataFolder, "poster.jpg");
                    await wc.DownloadFileTaskAsync(new Uri(anime.Images.poster.full), path);
                }
            }

            if (anime.Images.banner.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(anime.MetadataFolder, "banner.jpg");
                    await wc.DownloadFileTaskAsync(new Uri(anime.Images.banner.full), path);
                }
            }

            using (Context db = new Context())
            {
                db.Series.Add(anime);
                db.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// Adiciona a série de forma assíncrona no banco.
        /// </summary>
        /// <param name="serie">Série a ser adicionada.</param>
        /// <returns>True caso a série tenha sido adicionada com sucesso.</returns>
        public async static Task<bool> AddSerieAsync(Serie serie)
        {
            if (!System.IO.Directory.Exists(serie.MetadataFolder))
                System.IO.Directory.CreateDirectory(serie.MetadataFolder);

            if (serie.Images.poster.thumb != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(serie.MetadataFolder, "poster.jpg");
                    await wc.DownloadFileTaskAsync(new Uri(serie.Images.poster.thumb), path);
                }
            }
            else if (serie.Images.poster.medium != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(serie.MetadataFolder, "poster.jpg");
                    await wc.DownloadFileTaskAsync(new Uri(serie.Images.poster.medium), path);
                }
            }
            else if (serie.Images.poster.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(serie.MetadataFolder, "poster.jpg");
                    await wc.DownloadFileTaskAsync(new Uri(serie.Images.poster.full), path);
                }
            }

            if (serie.Images.banner.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(serie.MetadataFolder, "banner.jpg");
                    await wc.DownloadFileTaskAsync(new Uri(serie.Images.banner.full), path);
                }
            }

            using (Context db = new Context())
            {
                db.Series.Add(serie);
                db.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// Consulta todos os filmes contidos no banco.
        /// </summary>
        /// <returns>Retorna um List<Filme> contendo todos os filmes contidos no banco ordenados pelo título.</returns>
        internal static List<Filme> GetFilmes()
        {
            using (Context db = new Context())
            {
                var filmes = from filme in db.Filmes.Include("Images").Include("Ids")
                             orderby filme.Title
                             select filme;
                var filmesList = filmes.ToList();
                foreach (var item in filmesList)
                {
                    item.AvailableTranslations = item.Traducoes.Split('|').ToList();
                    item.Genres = item.Generos.Split('|').ToList();
                }
                return filmesList;
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
                var animes = from anime in db.Series.Include("Images").Include("Ids")
                             where anime.IsAnime == true
                             orderby anime.Title
                             select anime;
                var animesList = animes.ToList();
                foreach (var item in animesList)
                {
                    if (item.Traducoes != null)
                        item.AvailableTranslations = item.Traducoes.Split('|').ToList();
                    if (item.Generos != null)
                        item.Genres = item.Generos.Split('|').ToList();
                }
                return animesList;
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
                var series = from serie in db.Series.Include("Images").Include("Ids")
                             where serie.IsAnime == false
                             orderby serie.Title
                             select serie;
                var seriesList = series.ToList();
                foreach (var item in seriesList)
                {
                    if (item.Traducoes != null)
                        item.AvailableTranslations = item.Traducoes.Split('|').ToList();
                    if (item.Generos != null)
                        item.Genres = item.Generos.Split('|').ToList();
                }
                return seriesList;
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
                var filmes = from filmeDB in db.Filmes.Include("Images").Include("Ids")
                             where filmeDB.IDFilme == IdBanco
                             select filmeDB;
                var filme = filmes.ToList()[0];
                if (filme != null)
                {
                    filme.AvailableTranslations = filme.Traducoes.Split('|').ToList();
                    filme.Genres = filme.Generos.Split('|').ToList();
                    return filme;
                }
                else
                    return null;
            }
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
                var animes = from animeDb in db.Series.Include("Images").Include("Ids")
                             where animeDb.IsAnime && animeDb.IDSerie == IdBanco
                             select animeDb;
                var anime = animes.ToList()[0];
                if (anime != null)
                {
                    if (anime.Traducoes != null)
                        anime.AvailableTranslations = anime.Traducoes.Split('|').ToList();
                    if (anime.Generos != null)
                        anime.Genres = anime.Generos.Split('|').ToList();
                    return anime;
                }
                else
                    return null;
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
                var series = from serieDB in db.Series.Include("Images").Include("Ids")
                             where !serieDB.IsAnime && serieDB.IDSerie == IdBanco
                             select serieDB;
                var serie = series.ToList()[0];
                if (serie != null)
                {
                    if (serie.Traducoes != null)
                        serie.AvailableTranslations = serie.Traducoes.Split('|').ToList();
                    if (serie.Generos != null)
                        serie.Genres = serie.Generos.Split('|').ToList();
                    return serie;
                }
                else
                    return null;
            }
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
                    original = db.Filmes.Find(atualizado.IDFilme);
                    originalMetadata = original.MetadataFolder;

                    if (original.Ids.slug != atualizado.Ids.slug)
                        isDiferente = true;
                    if (original != null)
                    {
                        atualizado.Images.IDImages = original.Images.IDImages;
                        atualizado.Ids.IDIds = original.Ids.IDIds;
                        db.Entry(original).CurrentValues.SetValues(atualizado);
                        db.Entry(original.Images).CurrentValues.SetValues(atualizado.Images);
                        db.Entry(original.Ids).CurrentValues.SetValues(atualizado.Ids);
                        db.SaveChanges();
                    }
                }
                retorno = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return false;
            }

            if (isDiferente)
            {
                if (Directory.Exists(originalMetadata))
                {
                    System.IO.DirectoryInfo metaDir = new DirectoryInfo(originalMetadata);

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

                if (!System.IO.Directory.Exists(atualizado.MetadataFolder))
                    System.IO.Directory.CreateDirectory(atualizado.MetadataFolder);

                if (atualizado.Images.poster.thumb != null)
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        var path = Path.Combine(atualizado.MetadataFolder, "poster.jpg");
                        await wc.DownloadFileTaskAsync(new Uri(atualizado.Images.poster.thumb), path);
                    }
                }
                else if (atualizado.Images.poster.medium != null)
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        var path = Path.Combine(atualizado.MetadataFolder, "poster.jpg");
                        await wc.DownloadFileTaskAsync(new Uri(atualizado.Images.poster.medium), path);
                    }
                }
                else if (atualizado.Images.poster.full != null)
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        var path = Path.Combine(atualizado.MetadataFolder, "poster.jpg");
                        await wc.DownloadFileTaskAsync(new Uri(atualizado.Images.poster.full), path);
                    }
                }

                if (atualizado.Images.banner.full != null)
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        var path = Path.Combine(atualizado.MetadataFolder, "banner.jpg");
                        await wc.DownloadFileTaskAsync(new Uri(atualizado.Images.banner.full), path);
                    }
                }
            }
            return retorno;
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
                    original = db.Series.Find(atualizado.IDSerie);
                    originalMetadata = original.MetadataFolder;

                    if (original.Ids.slug != atualizado.Ids.slug)
                        isDiferente = true;
                    if (original != null)
                    {
                        atualizado.Images.IDImages = original.Images.IDImages;
                        atualizado.Ids.IDIds = original.Ids.IDIds;
                        db.Entry(original).CurrentValues.SetValues(atualizado);
                        db.Entry(original.Images).CurrentValues.SetValues(atualizado.Images);
                        db.Entry(original.Ids).CurrentValues.SetValues(atualizado.Ids);
                        db.SaveChanges();
                    }
                }
                retorno = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return false;
            }
            if (isDiferente)
            {
                if (Directory.Exists(originalMetadata))
                {
                    System.IO.DirectoryInfo metaDir = new DirectoryInfo(originalMetadata);

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

                if (!System.IO.Directory.Exists(atualizado.MetadataFolder))
                    System.IO.Directory.CreateDirectory(atualizado.MetadataFolder);

                if (atualizado.Images.poster.thumb != null)
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        var path = Path.Combine(atualizado.MetadataFolder, "poster.jpg");
                        await wc.DownloadFileTaskAsync(new Uri(atualizado.Images.poster.thumb), path);
                    }
                }
                else if (atualizado.Images.poster.medium != null)
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        var path = Path.Combine(atualizado.MetadataFolder, "poster.jpg");
                        await wc.DownloadFileTaskAsync(new Uri(atualizado.Images.poster.medium), path);
                    }
                }
                else if (atualizado.Images.poster.full != null)
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        var path = Path.Combine(atualizado.MetadataFolder, "poster.jpg");
                        await wc.DownloadFileTaskAsync(new Uri(atualizado.Images.poster.full), path);
                    }
                }

                if (atualizado.Images.banner.full != null)
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        var path = Path.Combine(atualizado.MetadataFolder, "banner.jpg");
                        await wc.DownloadFileTaskAsync(new Uri(atualizado.Images.banner.full), path);
                    }
                }
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
            Serie originalDB = null;
            string originalMetadata = null;
            try
            {
                using (Context db = new Context())
                {
                    originalDB = db.Series.Find(atualizado.IDSerie);
                    originalMetadata = originalDB.MetadataFolder;

                    if (originalDB.Ids.slug != atualizado.Ids.slug)
                        isDiferente = true;
                    if (originalDB != null)
                    {
                        atualizado.Images.IDImages = originalDB.Images.IDImages;
                        atualizado.Ids.IDIds = originalDB.Ids.IDIds;
                        db.Entry(originalDB).CurrentValues.SetValues(atualizado);
                        db.Entry(originalDB.Images).CurrentValues.SetValues(atualizado.Images);
                        db.Entry(originalDB.Ids).CurrentValues.SetValues(atualizado.Ids);
                        db.SaveChanges();
                    }
                }
                retorno = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return false;
            }
            if (isDiferente)
            {
                if (Directory.Exists(originalMetadata))
                {
                    System.IO.DirectoryInfo metaDir = new DirectoryInfo(originalMetadata);

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

                if (!System.IO.Directory.Exists(atualizado.MetadataFolder))
                    System.IO.Directory.CreateDirectory(atualizado.MetadataFolder);

                if (atualizado.Images.poster.thumb != null)
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        var path = Path.Combine(atualizado.MetadataFolder, "poster.jpg");
                        await wc.DownloadFileTaskAsync(new Uri(atualizado.Images.poster.thumb), path);
                    }
                }
                else if (atualizado.Images.poster.medium != null)
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        var path = Path.Combine(atualizado.MetadataFolder, "poster.jpg");
                        await wc.DownloadFileTaskAsync(new Uri(atualizado.Images.poster.medium), path);
                    }
                }
                else if (atualizado.Images.poster.full != null)
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        var path = Path.Combine(atualizado.MetadataFolder, "poster.jpg");
                        await wc.DownloadFileTaskAsync(new Uri(atualizado.Images.poster.full), path);
                    }
                }

                if (atualizado.Images.banner.full != null)
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        var path = Path.Combine(atualizado.MetadataFolder, "banner.jpg");
                        await wc.DownloadFileTaskAsync(new Uri(atualizado.Images.banner.full), path);
                    }
                }
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