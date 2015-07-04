using MediaManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Helpers
{
    public class DatabaseHelper
    {
        public async static Task<bool> AddAnimeAsync(Serie anime)
        {
            if (!System.IO.Directory.Exists(anime.metadataFolder))
                System.IO.Directory.CreateDirectory(anime.metadataFolder);

            if (anime.images.poster.medium != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(anime.metadataFolder, "poster.jpg.temp");
                    await wc.DownloadFileTaskAsync(new Uri(anime.images.poster.medium), path);
                    File.Move(path, path.Remove(path.Length - 5));
                }
            }
            else if (anime.images.poster.thumb != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(anime.metadataFolder, "poster.jpg.temp");
                    await wc.DownloadFileTaskAsync(new Uri(anime.images.poster.thumb), path);
                    File.Move(path, path.Remove(path.Length - 5));
                }
            }
            else if (anime.images.poster.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(anime.metadataFolder, "poster.jpg.temp");
                    await wc.DownloadFileTaskAsync(new Uri(anime.images.poster.full), path);
                    File.Move(path, path.Remove(path.Length - 5));
                }
            }

            if (anime.images.banner.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(anime.metadataFolder, "banner.jpg.temp");
                    await wc.DownloadFileTaskAsync(new Uri(anime.images.banner.full), path);
                    File.Move(path, path.Remove(path.Length - 5));
                }
            }

            using (Context db = new Context())
            {
                db.Series.Add(anime);
                db.SaveChanges();
            }
            return true;
        }

        public async static Task<bool> AddFilmeAsync(Filme filme)
        {
            if (!System.IO.Directory.Exists(filme.metadataFolder))
                System.IO.Directory.CreateDirectory(filme.metadataFolder);

            if (filme.images.poster.medium != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(filme.metadataFolder, "poster.jpg.temp");
                    await wc.DownloadFileTaskAsync(new Uri(filme.images.poster.medium), path);
                    File.Move(path, path.Remove(path.Length - 5));
                }
            }
            else if (filme.images.poster.thumb != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(filme.metadataFolder, "poster.jpg.temp");
                    await wc.DownloadFileTaskAsync(new Uri(filme.images.poster.thumb), path);
                    File.Move(path, path.Remove(path.Length - 5));
                }
            }
            else if (filme.images.poster.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(filme.metadataFolder, "poster.jpg.temp");
                    await wc.DownloadFileTaskAsync(new Uri(filme.images.poster.full), path);
                    File.Move(path, path.Remove(path.Length - 5));
                }
            }

            if (filme.images.banner.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(filme.metadataFolder, "banner.jpg.temp");
                    await wc.DownloadFileTaskAsync(new Uri(filme.images.banner.full), path);
                    File.Move(path, path.Remove(path.Length - 5));
                }
            }

            using (Context db = new Context())
            {
                db.Filmes.Add(filme);
                db.SaveChanges();
            }
            return true;
        }

        public async static Task<bool> AddSerieAsync(Serie serie)
        {
            if (!System.IO.Directory.Exists(serie.metadataFolder))
                System.IO.Directory.CreateDirectory(serie.metadataFolder);

            if (serie.images.poster.medium != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(serie.metadataFolder, "poster.jpg.temp");
                    await wc.DownloadFileTaskAsync(new Uri(serie.images.poster.medium), path);
                    File.Move(path, path.Remove(path.Length - 5));
                }
            }
            else if (serie.images.poster.thumb != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(serie.metadataFolder, "poster.jpg.temp");
                    await wc.DownloadFileTaskAsync(new Uri(serie.images.poster.thumb), path);
                    File.Move(path, path.Remove(path.Length - 5));
                }
            }
            else if (serie.images.poster.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(serie.metadataFolder, "poster.jpg.temp");
                    await wc.DownloadFileTaskAsync(new Uri(serie.images.poster.full), path);
                    File.Move(path, path.Remove(path.Length - 5));
                }
            }

            if (serie.images.banner.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(serie.metadataFolder, "banner.jpg.temp");
                    await wc.DownloadFileTaskAsync(new Uri(serie.images.banner.full), path);
                    File.Move(path, path.Remove(path.Length - 5));
                }
            }

            using (Context db = new Context())
            {
                db.Series.Add(serie);
                db.SaveChanges();
            }
            return true;
        }

        internal static Serie GetAnimePorId(int IdBanco)
        {
            using (Context db = new Context())
            {
                var animes = from anime in db.Series.Include("Images").Include("Ids")
                             where anime.isAnime && anime.IDSerie == IdBanco
                             select anime;
                if (animes.Count() > 0)
                    return animes.ToList()[0];
                else
                    return null;
            }
        }

        /// <summary>
        /// Consulta todos os animes contidos no banco.
        /// </summary>
        /// <returns>Retorna um List<Serie> contendo todos os animes que tenham serie.isAnime == true</returns>
        internal static List<Serie> GetAnimes()
        {
            using (Context db = new Context())
            {
                var animes = from anime in db.Series.Include("Images").Include("Ids")
                             where anime.isAnime == true
                             select anime;
                return animes.ToList();
            }
        }

        internal static Filme GetFilmePorId(int IdBanco)
        {
            using (Context db = new Context())
            {
                var filmes = from filme in db.Filmes.Include("Images").Include("Ids")
                             where filme.IDFilme == IdBanco
                             select filme;
                if (filmes.Count() > 0)
                    return filmes.ToList()[0];
                else
                    return null;
            }
        }

        /// <summary>
        /// Consulta todos os filmes contidos no banco.
        /// </summary>
        /// <returns>Retorna um List<Filme> contendo todos os filmes contidos no banco.</returns>
        internal static List<Filme> GetFilmes()
        {
            using (Context db = new Context())
            {
                var filmes = from filme in db.Filmes.Include("Images").Include("Ids")
                             select filme;
                return filmes.ToList();
            }
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

        internal static Serie GetSeriePorId(int IdBanco)
        {
            using (Context db = new Context())
            {
                var series = from serie in db.Series.Include("Images").Include("Ids")
                             where !serie.isAnime && serie.IDSerie == IdBanco
                             select serie;
                if (series.Count() > 0)
                    return series.ToList()[0];
                else
                    return null;
            }
        }

        /// <summary>
        /// Consulta todas as séries contidas no banco.
        /// </summary>
        /// <returns>Retorna um List<Serie> contendo todas as séries que tenham serie.isAnime == false</returns>
        internal static List<Serie> GetSeries()
        {
            using (Context db = new Context())
            {
                var series = from serie in db.Series.Include("Images").Include("Ids")
                             where serie.isAnime == false
                             select serie;
                return series.ToList();
            }
        }
    }
}