using MediaManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Helpers
{
    public class DatabaseHelper
    {
        public static bool adicionarSerie(Serie serie)
        {
            serie.metadataFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppName, "Metadata", "Séries", Helpers.Helper.RetirarCaracteresInvalidos(serie.title));

            if (!System.IO.Directory.Exists(serie.metadataFolder))
                System.IO.Directory.CreateDirectory(serie.metadataFolder);

            if (serie.images.poster.medium != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(serie.metadataFolder, "poster.jpg");
                    wc.DownloadFile(serie.images.poster.medium, path);
                }
            }
            else if (serie.images.poster.thumb != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(serie.metadataFolder, "poster.jpg");
                    wc.DownloadFile(serie.images.poster.thumb, path);
                }
            }
            else if (serie.images.poster.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(serie.metadataFolder, "poster.jpg");
                    wc.DownloadFile(serie.images.poster.full, path);
                }
            }

            if (serie.images.banner.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(serie.metadataFolder, "banner.jpg");
                    wc.DownloadFile(serie.images.banner.full, path);
                }
            }

            using (Context db = new Context())
            {
                db.Series.Add(serie);
                db.SaveChanges();
            }
            return true;
        }

        public static bool adicionarFilme(Filme filme)
        {
            filme.metadataFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Properties.Settings.Default.AppName, "Metadata", "Filmes", Helpers.Helper.RetirarCaracteresInvalidos(filme.title));

            if (!System.IO.Directory.Exists(filme.metadataFolder))
                System.IO.Directory.CreateDirectory(filme.metadataFolder);

            if (filme.images.poster.medium != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(filme.metadataFolder, "poster.jpg");
                    wc.DownloadFile(filme.images.poster.medium, path);
                }
            }
            else if (filme.images.poster.thumb != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(filme.metadataFolder, "poster.jpg");
                    wc.DownloadFile(filme.images.poster.thumb, path);
                }
            }
            else if (filme.images.poster.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(filme.metadataFolder, "poster.jpg");
                    wc.DownloadFile(filme.images.poster.full, path);
                }
            }

            if (filme.images.banner.full != null)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var path = System.IO.Path.Combine(filme.metadataFolder, "banner.jpg");
                    wc.DownloadFile(filme.images.banner.full, path);
                }
            }

            using (Context db = new Context())
            {
                db.Filmes.Add(filme);
                db.SaveChanges();
            }
            return true;
        }
    }
}