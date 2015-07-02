using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace MediaManager.ViewModel
{
    public class ProcurarConteudoViewModel
    {
        public ObservableCollection<Conteudo> Conteudos { get; set; }

        public void Init()
        {
            ObservableCollection<Conteudo> conteudos = new ObservableCollection<Conteudo>();

            conteudos.Add(new Conteudo { Nome = "Carregando...", Pasta = "Carregando...", Tipo = "Carregando...", IsSelected = false });

            Conteudos = conteudos;
        }

        public async void LoadConteudos(Helper.TipoConteudo conteudo)
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
                    List<Search> series = null;
                    foreach (var dir in dirSeries)
                    {
                        series = await Helper.API_PesquisarConteudoAsync(dir.Name, Helper.TipoConteudo.show.ToString());
                        conteudos.Add(new Conteudo { Nome = series[0].show.title, Pasta = dir.FullName, Tipo = "Show", TraktSlug = series[0].show.ids.slug, IsSelected = true });
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