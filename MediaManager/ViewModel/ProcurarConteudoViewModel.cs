using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class ProcurarConteudoViewModel
    {
        public ObservableCollection<ConteudoGrid> Conteudos { get; set; }

        public ProcurarConteudoViewModel()
        {
            ObservableCollection<ConteudoGrid> conteudos = new ObservableCollection<ConteudoGrid>();

            conteudos.Add(new ConteudoGrid { Nome = "Carregando...", Pasta = "Carregando...", IsSelected = false });

            Conteudos = conteudos;
        }

        public async Task LoadConteudos(Helper.Enums.TipoConteudo conteudo)
        {
            ObservableCollection<ConteudoGrid> conteudos = new ObservableCollection<ConteudoGrid>();

            switch (conteudo)
            {
                case Helper.Enums.TipoConteudo.movie:
                    break;

                case Helper.Enums.TipoConteudo.show:
                    break;

                case Helper.Enums.TipoConteudo.anime:
                    break;

                case Helper.Enums.TipoConteudo.movieShowAnime:
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
                            series = await Helper.API_PesquisarConteudoAsync(dir.Name, Helper.Enums.TipoConteudo.show.ToString());
                            if (series.Count != 0 && !DatabaseHelper.VerificaSeExiste(series[0].show.ids.trakt))
                                conteudos.Add(new ConteudoGrid { Nome = series[0].show.title, Pasta = dir.FullName, TipoConteudo = Helper.Enums.TipoConteudo.show, TraktSlug = series[0].show.ids.slug, IsSelected = true });
                        }
                    }

                    foreach (var dir in dirAnimes)
                    {
                        if (!DatabaseHelper.VerificaSeExiste(dir.FullName))
                        {
                            animes = await Helper.API_PesquisarConteudoAsync(dir.Name, Helper.Enums.TipoConteudo.show.ToString());
                            if (animes.Count != 0 && !DatabaseHelper.VerificaSeExiste(animes[0].show.ids.trakt))
                                conteudos.Add(new ConteudoGrid { Nome = animes[0].show.title, Pasta = dir.FullName, TipoConteudo = Helper.Enums.TipoConteudo.anime, TraktSlug = animes[0].show.ids.slug, IsSelected = true });
                        }
                    }

                    foreach (var dir in dirFilmes)
                    {
                        if (!DatabaseHelper.VerificaSeExiste(dir.FullName))
                        {
                            filmes = await Helper.API_PesquisarConteudoAsync(dir.Name, Helper.Enums.TipoConteudo.movie.ToString());
                            if (filmes.Count != 0 && !DatabaseHelper.VerificaSeExiste(filmes[0].movie.ids.trakt))
                                conteudos.Add(new ConteudoGrid { Nome = filmes[0].movie.title, Pasta = dir.FullName, TipoConteudo = Helper.Enums.TipoConteudo.movie, TraktSlug = filmes[0].movie.ids.slug, IsSelected = true });
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