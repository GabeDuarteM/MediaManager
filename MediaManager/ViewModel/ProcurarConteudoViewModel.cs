// Developed by: Gabriel Duarte
// 
// Created at: 20/07/2015 21:10
// Last update: 19/04/2016 02:47

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Autofac;
using MediaManager.Commands;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.Services;

namespace MediaManager.ViewModel
{
    public class ProcurarConteudoViewModel : ViewModelBase
    {
        private bool? _bFlSelecionarTodos;

        private ObservableCollection<Video> _lstConteudos;

        public ProcurarConteudoViewModel(Enums.TipoConteudo tipoConteudo = Enums.TipoConteudo.Selecione,
                                         Window owner = null)
        {
            lstConteudos = new ObservableCollection<Video>();
            lstConteudos.Add(new Serie {sDsTitulo = "Carregando...", sDsPasta = "Carregando...", bFlSelecionado = false});
            Owner = owner;
            CommandAdicionar = new ProcurarConteudoCommands.CommandAdicionar();
            CommandSelecionar = new ProcurarConteudoCommands.CommandSelecionar();
            CommandSelecionarTodos = new ProcurarConteudoCommands.CommandSelecionarTodos();
            LoadConteudos(tipoConteudo);
            CommandSelecionar.Execute(this);
        }

        public ObservableCollection<Video> lstConteudos
        {
            get { return _lstConteudos; }
            set
            {
                _lstConteudos = value;
                OnPropertyChanged();
            }
        }

        public bool? bFlSelecionarTodos
        {
            get { return _bFlSelecionarTodos; }
            set
            {
                _bFlSelecionarTodos = value;
                OnPropertyChanged();
            }
        }

        public ICommand CommandAdicionar { get; set; }

        public ICommand CommandSelecionar { get; set; }

        public ICommand CommandSelecionarTodos { get; set; }

        public Action ActionFechar { get; set; }

        public Window Owner { get; set; }

        public void LoadConteudos(Enums.TipoConteudo contentType)
        {
            var frmBarraProgresso = new frmBarraProgresso();
            frmBarraProgresso.BarraProgressoViewModel.sDsTarefa = "Procurando pastas...";
            frmBarraProgresso.BarraProgressoViewModel.Worker.DoWork += (sender, e) =>
            {
                var conteudos = new ObservableCollection<Video>();

                var seriesService = App.Container.Resolve<SeriesService>();

                switch (contentType)
                {
                    case Enums.TipoConteudo.AnimeFilmeSérie:
                        var dirSeries = Helper.retornarDiretoriosSeries();
                        var dirAnimes = Helper.retornarDiretoriosAnimes();
                        var dirFilmes = Helper.retornarDiretoriosFilmes();
                        frmBarraProgresso.BarraProgressoViewModel.dNrProgressoMaximo = (dirSeries != null
                                                                                            ? dirSeries.Length
                                                                                            : 0) + (dirAnimes != null
                                                                                                        ? dirAnimes
                                                                                                              .Length
                                                                                                        : 0) +
                                                                                       (dirFilmes != null
                                                                                            ? dirFilmes.Length
                                                                                            : 0);

                        if (dirSeries != null)
                        {
                            foreach (DirectoryInfo dir in dirSeries)
                            {
                                frmBarraProgresso.BarraProgressoViewModel.dNrProgressoAtual++;
                                frmBarraProgresso.BarraProgressoViewModel.sDsTexto = dir.FullName;
                                if (!seriesService.VerificarSeExiste(dir.FullName))
                                {
                                    var lstSeries = APIRequests.GetSeries(dir.Name);
                                    if (lstSeries.Count == 0)
                                    {
                                        var conteudo = new Serie();
                                        conteudo.nIdTipoConteudo = Enums.TipoConteudo.Série;
                                        conteudo.sDsPasta = dir.FullName;
                                        conteudo.bFlNaoEncontrado = true;
                                        conteudos.Add(conteudo);
                                    }
                                    else if (lstSeries.Count > 0 && !seriesService.VerificarSeExiste(lstSeries[0].nCdApi))
                                    {
                                        Serie conteudo = lstSeries[0];
                                        conteudo.nIdTipoConteudo = Enums.TipoConteudo.Série;
                                        conteudo.sDsPasta = dir.FullName;
                                        conteudo.bFlSelecionado = true;

                                        if (!string.IsNullOrWhiteSpace(conteudo.sAliases))
                                        {
                                            foreach (var item in conteudo.sAliases.Split('|'))
                                            {
                                                var alias = new SerieAlias(item);
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
                            foreach (DirectoryInfo dir in dirAnimes)
                            {
                                frmBarraProgresso.BarraProgressoViewModel.dNrProgressoAtual++;
                                frmBarraProgresso.BarraProgressoViewModel.sDsTexto = dir.FullName;
                                if (!seriesService.VerificarSeExiste(dir.FullName))
                                {
                                    var lstSeries = APIRequests.GetSeries(dir.Name);
                                    if (lstSeries == null || lstSeries.Count == 0)
                                    {
                                        var conteudo = new Serie();
                                        conteudo.nIdTipoConteudo = Enums.TipoConteudo.Anime;
                                        conteudo.sDsPasta = dir.FullName;
                                        conteudo.bFlNaoEncontrado = true;
                                        conteudos.Add(conteudo);
                                    }
                                    else if (lstSeries.Count > 0 && !seriesService.VerificarSeExiste(lstSeries[0].nCdApi))
                                    {
                                        Serie conteudo = lstSeries[0];
                                        conteudo.nIdTipoConteudo = Enums.TipoConteudo.Anime;
                                        conteudo.sDsPasta = dir.FullName;
                                        conteudo.bFlSelecionado = true;

                                        if (!string.IsNullOrWhiteSpace(conteudo.sAliases))
                                        {
                                            foreach (var item in conteudo.sAliases.Split('|'))
                                            {
                                                var alias = new SerieAlias(item);
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
                    Helper.MostrarMensagem("Nenhum novo conteúdo foi encontrado.", Enums.eTipoMensagem.Informativa);
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
    }
}
