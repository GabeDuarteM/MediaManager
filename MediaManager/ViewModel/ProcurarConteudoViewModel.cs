using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class ProcurarConteudoViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Video> _lstConteudos;
        public ObservableCollection<Video> lstConteudos { get { return _lstConteudos; } set { _lstConteudos = value; OnPropertyChanged(); } }

        private bool? _bFlSelecionarTodos;

        public bool? bFlSelecionarTodos { get { return _bFlSelecionarTodos; } set { _bFlSelecionarTodos = value; OnPropertyChanged(); } }

        public ICommand CommandAdicionar { get; set; }

        public ICommand CommandSelecionar { get; set; }

        public ICommand CommandSelecionarTodos { get; set; }

        public Action ActionFechar { get; set; }

        public Window Owner { get; set; }

        public ProcurarConteudoViewModel(Enums.TipoConteudo tipoConteudo = Enums.TipoConteudo.Selecione, Window owner = null)
        {
            lstConteudos = new ObservableCollection<Video>();
            lstConteudos.Add(new Serie { sDsTitulo = "Carregando...", sDsPasta = "Carregando...", bFlSelecionado = false });
            Owner = owner;
            CommandAdicionar = new ProcurarConteudoCommands.CommandAdicionar();
            CommandSelecionar = new ProcurarConteudoCommands.CommandSelecionar();
            CommandSelecionarTodos = new ProcurarConteudoCommands.CommandSelecionarTodos();
            LoadConteudos(tipoConteudo);
            CommandSelecionar.Execute(this);
        }

        public void LoadConteudos(Enums.TipoConteudo contentType)
        {
            frmBarraProgresso frmBarraProgresso = new frmBarraProgresso();
            frmBarraProgresso.BarraProgressoViewModel.sDsTarefa = "Procurando pastas...";
            frmBarraProgresso.BarraProgressoViewModel.Worker.DoWork += (sender, e) =>
            {
                ObservableCollection<Video> conteudos = new ObservableCollection<Video>();

                switch (contentType)
                {
                    case Enums.TipoConteudo.AnimeFilmeSérie:
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
                                        conteudo.nIdTipoConteudo = Enums.TipoConteudo.Série;
                                        conteudo.sDsPasta = dir.FullName;
                                        conteudo.bFlNaoEncontrado = true;
                                        conteudos.Add(conteudo);
                                    }
                                    else if (data.Series.Length > 0 && !DBHelper.VerificaSeSerieOuAnimeExiste(data.Series[0].nCdApi))
                                    {
                                        Serie conteudo = data.Series[0];
                                        conteudo.nIdTipoConteudo = Enums.TipoConteudo.Série;
                                        conteudo.sDsPasta = dir.FullName;
                                        conteudo.bFlSelecionado = true;

                                        if (!string.IsNullOrWhiteSpace(conteudo.sAliases))
                                        {
                                            foreach (var item in conteudo.sAliases.Split('|'))
                                            {
                                                SerieAlias alias = new SerieAlias(item);
                                                if (conteudo.lstSerieAlias == null)
                                                {
                                                    conteudo.lstSerieAlias = new ObservableCollection<SerieAlias>();
                                                }
                                                conteudo.lstSerieAlias.Add(alias);
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
                                        conteudo.nIdTipoConteudo = Enums.TipoConteudo.Anime;
                                        conteudo.sDsPasta = dir.FullName;
                                        conteudo.bFlNaoEncontrado = true;
                                        conteudos.Add(conteudo);
                                    }
                                    else if (data.Series.Length > 0 && !DBHelper.VerificaSeSerieOuAnimeExiste(data.Series[0].nCdApi))
                                    {
                                        Serie conteudo = data.Series[0];
                                        conteudo.nIdTipoConteudo = Enums.TipoConteudo.Anime;
                                        conteudo.sDsPasta = dir.FullName;
                                        conteudo.bFlSelecionado = true;

                                        if (!string.IsNullOrWhiteSpace(conteudo.sAliases))
                                        {
                                            foreach (var item in conteudo.sAliases.Split('|'))
                                            {
                                                SerieAlias alias = new SerieAlias(item);
                                                if (conteudo.lstSerieAlias == null)
                                                {
                                                    conteudo.lstSerieAlias = new ObservableCollection<SerieAlias>();
                                                }
                                                conteudo.lstSerieAlias.Add(alias);
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

                lstConteudos = conteudos;

                if (lstConteudos.Count == 0)
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