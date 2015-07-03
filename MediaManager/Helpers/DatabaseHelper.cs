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
        public async static Task<bool> adicionarAnimeAsync(Serie anime)
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

        public async static Task<bool> adicionarFilmeAsync(Filme filme)
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

        public async static Task<bool> adicionarSerieAsync(Serie serie)
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
    }
}