using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace MediaManager.ViewModel
{
    public class ProcurarConteudoViewModel
    {
        public ProcurarConteudoViewModel()
        {
            ObservableCollection<Conteudo> conteudos = new ObservableCollection<Conteudo>();

            conteudos.Add(new Conteudo { Nome = "Carregando...", Pasta = "Carregando...", Tipo = "Carregando...", IsSelected = false });

            Conteudos = conteudos;
        }

        public ObservableCollection<Conteudo> Conteudos { get; set; }

        public async Task LoadConteudos(Helper.TipoConteudo conteudo)
        {
            ObservableCollection<Conteudo> conteudos = new ObservableCollection<Conteudo>();

            switch (conteudo)
            {
                case Helper.TipoConteudo.movie:
                    break;

                case Helper.TipoConteudo.show:
                    break;

                case Helper.TipoConteudo.anime:
                    break;

                case Helper.TipoConteudo.movieShowAnime:
                    DirectoryInfo[] dirSeries = Helper.retornarDiretoriosSeries();
                    DirectoryInfo[] dirAnimes = Helper.retornarDiretoriosAnimes();
                    DirectoryInfo[] dirFilmes = Helper.retornarDiretoriosFilmes();

                    List<Search> series = null;
                    List<Search> filmes = null;
                    List<Search> animes = null;

                    foreach (var dir in dirSeries)
                    {
                        if (!DatabaseHelper.VerificaSeExiste(dir.FullName))
                        {
                            series = await Helper.API_PesquisarConteudoAsync(dir.Name, Helper.TipoConteudo.show.ToString());
                            if (series.Count != 0 && !DatabaseHelper.VerificaSeExiste(series[0].show.ids.trakt))
                                conteudos.Add(new Conteudo { Nome = series[0].show.title, Pasta = dir.FullName, Tipo = "Show", TraktSlug = series[0].show.ids.slug, IsSelected = true });
                        }
                    }

                    foreach (var dir in dirAnimes)
                    {
                        if (!DatabaseHelper.VerificaSeExiste(dir.FullName))
                        {
                            animes = await Helper.API_PesquisarConteudoAsync(dir.Name, Helper.TipoConteudo.show.ToString());
                            if (animes.Count != 0 && !DatabaseHelper.VerificaSeExiste(animes[0].show.ids.trakt))
                                conteudos.Add(new Conteudo { Nome = animes[0].show.title, Pasta = dir.FullName, Tipo = "Anime", TraktSlug = animes[0].show.ids.slug, IsSelected = true });
                        }
                    }

                    foreach (var dir in dirFilmes)
                    {
                        if (!DatabaseHelper.VerificaSeExiste(dir.FullName))
                        {
                            filmes = await Helper.API_PesquisarConteudoAsync(dir.Name, Helper.TipoConteudo.movie.ToString());
                            if (filmes.Count != 0 && !DatabaseHelper.VerificaSeExiste(filmes[0].movie.ids.trakt))
                                conteudos.Add(new Conteudo { Nome = filmes[0].movie.title, Pasta = dir.FullName, Tipo = "Filme", TraktSlug = filmes[0].movie.ids.slug, IsSelected = true });
                        }
                    }
                    break;

                default:
                    break;
            }

            Conteudos.Clear();

            foreach (var item in conteudos)
            {
                Conteudos.Add(item);
            }
        }
    }
}