using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;
using Ookii.Dialogs.Wpf;

namespace MediaManager.ViewModel
{
    public class ProcurarConteudoViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Video> _Conteudos;
        public ObservableCollection<Video> Conteudos { get { return _Conteudos; } set { _Conteudos = value; OnPropertyChanged(); } }

        public Window Owner { get; set; }

        public ProcurarConteudoViewModel(Enums.ContentType tipoConteudo = Enums.ContentType.Selecione, Window owner = null)
        {
            Conteudos = new ObservableCollection<Video>();
            Conteudos.Add(new Serie { Title = "Carregando...", FolderPath = "Carregando...", bFlSelecionado = false });
            Owner = owner;
            LoadConteudos(tipoConteudo);
        }

        public void LoadConteudos(Enums.ContentType contentType)
        {
            frmBarraProgresso frmBarraProgresso = new frmBarraProgresso();
            frmBarraProgresso.BarraProgressoViewModel.sDsTarefa = "Procurando pastas...";
            frmBarraProgresso.BarraProgressoViewModel.Worker.DoWork += (sender, e) =>
            {
                ObservableCollection<Video> conteudos = new ObservableCollection<Video>();

                switch (contentType)
                {
                    case Enums.ContentType.AnimeFilmeSérie:
                        DirectoryInfo[] dirSeries = Helper.retornarDiretoriosSeries();
                        DirectoryInfo[] dirAnimes = Helper.retornarDiretoriosAnimes();
                        DirectoryInfo[] dirFilmes = Helper.retornarDiretoriosFilmes();
                        frmBarraProgresso.BarraProgressoViewModel.dNrProgressoMaximo = ((dirSeries != null) ? dirSeries.Length : 0) + ((dirAnimes != null) ? dirAnimes.Length : 0) + ((dirFilmes != null) ? dirFilmes.Length : 0);

                        if (dirSeries != null)
                        {
                            foreach (var dir in dirSeries)
                            {
                                frmBarraProgresso.BarraProgressoViewModel.dNrProgressoAtual++;
                                frmBarraProgresso.BarraProgressoViewModel.sDsTexto = dir.FullName;
                                if (!DBHelper.VerificaSeSerieOuAnimeExiste(dir.FullName))
                                {
                                    SeriesData data = APIRequests.GetSeries(dir.Name);
                                    if (data.Series.Length == 0)
                                    {
                                        Serie conteudo = new Serie();
                                        conteudo.ContentType = Enums.ContentType.Série;
                                        conteudo.FolderPath = dir.FullName;
                                        conteudo.bFlNaoEncontrado = true;
                                        conteudos.Add(conteudo);
                                    }
                                    else if (data.Series.Length > 0 && !DBHelper.VerificaSeSerieOuAnimeExiste(data.Series[0].IDApi))
                                    {
                                        Serie conteudo = data.Series[0];
                                        conteudo.ContentType = Enums.ContentType.Série;
                                        conteudo.FolderPath = dir.FullName;
                                        conteudo.bFlSelecionado = true;

                                        if (!string.IsNullOrWhiteSpace(conteudo.SerieAliasStr))
                                        {
                                            foreach (var item in conteudo.SerieAliasStr.Split('|'))
                                            {
                                                SerieAlias alias = new SerieAlias(item);
                                                if (conteudo.SerieAlias == null)
                                                {
                                                    conteudo.SerieAlias = new ObservableCollection<SerieAlias>();
                                                }
                                                conteudo.SerieAlias.Add(alias);
                                            }
                                        }

                                        conteudos.Add(conteudo);
                                    }
                                }
                            }
                        }

                        if (dirAnimes != null)
                        {
                            foreach (var dir in dirAnimes)
                            {
                                frmBarraProgresso.BarraProgressoViewModel.dNrProgressoAtual++;
                                frmBarraProgresso.BarraProgressoViewModel.sDsTexto = dir.FullName;
                                if (!DBHelper.VerificaSeSerieOuAnimeExiste(dir.FullName))
                                {
                                    SeriesData data = APIRequests.GetSeries(dir.Name);
                                    if (data.Series == null || data.Series.Length == 0)
                                    {
                                        Serie conteudo = new Serie();
                                        conteudo.ContentType = Enums.ContentType.Anime;
                                        conteudo.FolderPath = dir.FullName;
                                        conteudo.bFlNaoEncontrado = true;
                                        conteudos.Add(conteudo);
                                    }
                                    else if (data.Series.Length > 0 && !DBHelper.VerificaSeSerieOuAnimeExiste(data.Series[0].IDApi))
                                    {
                                        Serie conteudo = data.Series[0];
                                        conteudo.ContentType = Enums.ContentType.Anime;
                                        conteudo.FolderPath = dir.FullName;
                                        conteudo.bFlSelecionado = true;

                                        if (!string.IsNullOrWhiteSpace(conteudo.SerieAliasStr))
                                        {
                                            foreach (var item in conteudo.SerieAliasStr.Split('|'))
                                            {
                                                SerieAlias alias = new SerieAlias(item);
                                                if (conteudo.SerieAlias == null)
                                                {
                                                    conteudo.SerieAlias = new ObservableCollection<SerieAlias>();
                                                }
                                                conteudo.SerieAlias.Add(alias);
                                            }
                                        }

                                        conteudos.Add(conteudo);
                                    }
                                }
                            }
                        }

                        //if (dirFilmes != null) {
                        //    foreach (var dir in dirFilmes) // TODO Fazer funfar
                        //{
                        //        if (!DatabaseHelper.VerificaSeExiste(dir.FullName))
                        //        {
                        //            filmes = await Helper.API_PesquisarConteudoAsync(dir.Name, Enums.TipoConteudo.movie.ToString(), false);
                        //            if (filmes.Count != 0 && !DatabaseHelper.VerificaSeExiste(filmes[0].Video.ids.trakt))
                        //                conteudos.Add(new ConteudoGrid { Nome = filmes[0].Video.title, Pasta = dir.FullName, TipoConteudo = Enums.TipoConteudo.movie, TraktSlug = filmes[0].Video.ids.slug, IsSelected = true });
                        //        }
                        //    }
                        //}
                        break;

                    default:
                        throw new InvalidEnumArgumentException();
                }

                Conteudos = conteudos;

                if (Conteudos.Count == 0)
                {
                    Helper.MostrarMensagem("Nenhum novo conteúdo foi encontrado.", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }

                //Conteudos.Clear();

                //foreach (var item in conteudos)
                //{
                //    Conteudos.Add(item);
                //}
            };
            frmBarraProgresso.BarraProgressoViewModel.Worker.RunWorkerAsync();
            frmBarraProgresso.ShowDialog(Owner);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members
    }
}