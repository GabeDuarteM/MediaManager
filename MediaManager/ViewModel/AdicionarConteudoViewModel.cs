using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ConfigurableInputMessageBox;
using MediaManager.Commands;
using MediaManager.Helpers;
using MediaManager.Model;
using Ookii.Dialogs;

namespace MediaManager.ViewModel
{
    public class AdicionarConteudoViewModel : INotifyPropertyChanged
    {
        private string sDsPastaEditar;

        private int nCdVideo;

        private Video oVideoBuscaPersonalizada;

        private Video oVideoCarregando;

        public List<Video> listaVideosQuaseCompletos; // Alguns podem faltar os episódios. Tratar isso ao salvar.

        private List<Video> _ListaResultPesquisa;

        public List<Video> ListaResultPesquisa { get { return _ListaResultPesquisa; } set { _ListaResultPesquisa = value; OnPropertyChanged(); } }

        private Enums.TipoConteudo _nIdTipoConteudo;

        public Enums.TipoConteudo nIdTipoConteudo { get { return _nIdTipoConteudo; } set { _nIdTipoConteudo = value; } }

        private Video _oVideoSelecionado;

        public Video oVideoSelecionado { get { return _oVideoSelecionado; } set { _oVideoSelecionado = value; OnPropertyChanged(); AlterarVideoAsync(); } }

        public ICommand CommandAbrirEpisodios { get; set; }

        public AdicionarConteudoViewModel(Video video, Enums.TipoConteudo tipoConteudo)
        {
            CommandAbrirEpisodios = new AdicionarConteudoCommands.CommandAbrirEpisodios();
            nIdTipoConteudo = tipoConteudo;
            //Data = new SeriesData();
            listaVideosQuaseCompletos = new List<Video>();
            ListaResultPesquisa = new List<Video>();
            oVideoBuscaPersonalizada = new Serie() { sDsTitulo = "Busca personalizada...", sDsSinopse = "Carregando sinopse..." };
            oVideoCarregando = new Serie() { sDsTitulo = "Carregando...", sDsSinopse = "Carregando sinopse..." };
            getResultPesquisaAsync(video);
        }

        private async void getResultPesquisaAsync(Video video)
        {
            ListaResultPesquisa = new List<Video>();

            if (video.nCdApi != 0 && (video.nIdEstado == Enums.Estado.Completo || video.nIdEstado == Enums.Estado.CompletoSemForeignKeys))
            {
                listaVideosQuaseCompletos.Add(video);
                ListaResultPesquisa.Add(video);
            }

            ListaResultPesquisa.Add(oVideoCarregando);
            oVideoSelecionado = (video.nIdEstado == Enums.Estado.Completo || video.nIdEstado == Enums.Estado.CompletoSemForeignKeys) ? video : oVideoCarregando;
            var listaResultPesquisaTemp = new List<Video>();

            // Guarda as informações abaixo para caso for realizada uma Busca personalizada.
            if (string.IsNullOrWhiteSpace(sDsPastaEditar) || nCdVideo == 0)
            {
                sDsPastaEditar = video.sDsPasta;
                nCdVideo = video.nCdVideo;
            }

            if (video.nIdTipoConteudo == Enums.TipoConteudo.Série || video.nIdTipoConteudo == Enums.TipoConteudo.Anime)
            {
                //SeriesData data = new SeriesData();
                List<Serie> listaSeries = new List<Serie>();

                if ((video is Serie && !(video as Serie).bFlNaoEncontrado) || !(video is Serie))
                {
                    //data = await APIRequests.GetSeriesAsync(video.Title);
                    listaSeries = await APIRequests.GetSeriesAsync(video.sDsTitulo);
                }
                else if (video is Serie && (video as Serie).bFlNaoEncontrado)
                {
                    InputMessageBox InputMessageBox = new InputMessageBox(inputType.SemResultados);
                    InputMessageBox.InputViewModel.Properties.InputText = video.sDsTitulo;
                    InputMessageBox.ShowDialog();
                    if (InputMessageBox.DialogResult == true)
                    {
                        listaSeries = await APIRequests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText);
                    }

                    while (listaSeries.Count == 0)
                    {
                        if (Helper.MostrarMensagem("Nenhum resultado encontrado, deseja pesquisar por outro nome?", MessageBoxButton.YesNo, MessageBoxImage.Question, "Nenhum resultado encontrado") == MessageBoxResult.Yes)
                        {
                            InputMessageBox = new InputMessageBox(inputType.SemResultados);
                            InputMessageBox.InputViewModel.Properties.InputText = video.sDsTitulo;
                            InputMessageBox.ShowDialog();
                            if (InputMessageBox.DialogResult == true)
                            {
                                listaSeries = await APIRequests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText);
                            }
                        }
                        else // TODO Fechar tela ao entrar no else
                        {
                            return;
                        }
                    }
                }

                foreach (var item in listaSeries)
                {
                    bool bFlAdicionado = ListaResultPesquisa.Select(x => x.nCdApi).Contains(item.nCdApi);

                    if (!bFlAdicionado)
                    {
                        item.nCdVideo = (nCdVideo != 0) ? nCdVideo : video.nCdVideo;
                        item.sDsSinopse = oVideoCarregando.sDsSinopse; // Seta como default e será carregado quando e se for selecionado no combo.
                        item.sDsPasta = null;

                        listaResultPesquisaTemp.Add(item);
                    }
                }
            }

            listaResultPesquisaTemp.Add(oVideoBuscaPersonalizada);
            ListaResultPesquisa.Remove(oVideoCarregando);
            listaResultPesquisaTemp.ForEach(x => ListaResultPesquisa.Add(x));

            oVideoSelecionado = (video.nCdApi != 0 && ListaResultPesquisa.Where(x => x.nCdApi == video.nCdApi).Count() > 0) ? ListaResultPesquisa.Where(x => x.nCdApi == video.nCdApi).First() : ListaResultPesquisa[0];
        }

        private async void getResultPesquisaAsync(string title)
        {
            ListaResultPesquisa.Add(oVideoCarregando);
            oVideoSelecionado = oVideoCarregando;
            List<Video> listaResultPesquisaTemp = new List<Video>();

            if (nIdTipoConteudo == Enums.TipoConteudo.Série || nIdTipoConteudo == Enums.TipoConteudo.Anime)
            {
                List<Serie> listaSeries = await APIRequests.GetSeriesAsync(title);

                if (listaSeries.Count == 0) // Verificar a primeira vez se é null para não exibir a mensagem quando não encontra resultados na tela de procurar conteúdo.
                {
                    InputMessageBox InputMessageBox = new InputMessageBox(inputType.SemResultados);
                    InputMessageBox.InputViewModel.Properties.InputText = title;
                    InputMessageBox.ShowDialog();
                    if (InputMessageBox.DialogResult == true)
                    {
                        listaSeries = await APIRequests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText);
                    }

                    while (listaSeries.Count == 0)
                    {
                        if (MessageBox.Show("Nenhum resultado encontrado, deseja pesquisar por outro nome?", "Nenhum resultado encontrado - " + Properties.Settings.Default.AppName, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            InputMessageBox = new InputMessageBox(inputType.SemResultados);
                            InputMessageBox.InputViewModel.Properties.InputText = title;
                            InputMessageBox.ShowDialog();
                            if (InputMessageBox.DialogResult == true)
                            {
                                listaSeries = await APIRequests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText);
                            }
                        }
                        else // TODO Fechar tela ao entrar no else
                        {
                            return;
                        }
                    }
                }

                foreach (var item in listaSeries)
                {
                    if (!string.IsNullOrWhiteSpace(sDsPastaEditar)) // Verifica se é edição para setar o folderpath igual.
                    {
                        item.sDsPasta = sDsPastaEditar;
                    }
                    else
                    {
                        item.SetDefaultFolderPath();
                    }

                    item.nCdVideo = nCdVideo;
                    item.nIdTipoConteudo = nIdTipoConteudo;

                    bool bFlAdicionado = listaResultPesquisaTemp.Select(x => x.nCdApi).Contains(item.nCdApi);

                    if (!bFlAdicionado)
                    {
                        item.sDsSinopse = oVideoCarregando.sDsSinopse;
                        listaResultPesquisaTemp.Add(item);
                    }
                }
            }

            listaResultPesquisaTemp.Add(oVideoBuscaPersonalizada);
            ListaResultPesquisa = listaResultPesquisaTemp;
            oVideoSelecionado = ListaResultPesquisa[0];
        }

        private async void AlterarVideoAsync()
        {
            if (oVideoSelecionado == null || oVideoSelecionado == oVideoCarregando || oVideoSelecionado.nIdEstado == Enums.Estado.Completo || oVideoSelecionado.nIdEstado == Enums.Estado.CompletoSemForeignKeys)
            {
                return;
            }

            if (ListaResultPesquisa.Count > 0 && oVideoSelecionado == oVideoBuscaPersonalizada)
            {
                InputMessageBox inputMessageBox = new InputMessageBox(inputType.AdicionarConteudo);
                inputMessageBox.ShowDialog();
                if (inputMessageBox.DialogResult == true)
                {
                    getResultPesquisaAsync(new Serie() { sDsTitulo = inputMessageBox.InputViewModel.Properties.InputText });
                    return;
                }
            }
            else if (listaVideosQuaseCompletos != null && listaVideosQuaseCompletos.Count > 0)
            {
                listaVideosQuaseCompletos
                    .Where(x => x.nCdApi == oVideoSelecionado.nCdApi && (x.nIdEstado == Enums.Estado.Completo || x.nIdEstado == Enums.Estado.CompletoSemForeignKeys)).ToList()
                    .ForEach(x =>
                    {
                        _oVideoSelecionado = x; OnPropertyChanged("SelectedVideo"); return;
                    });
                //foreach (var item in listaVideosQuaseCompletos)
                //{
                //    if (item.IDApi == SelectedVideo.IDApi && (item.Estado == Enums.Estado.Completo || item.Estado == Enums.Estado.CompletoSemForeignKeys))
                //    {
                //        //Data = item;
                //        _SelectedVideo = item; OnPropertyChanged("SelectedVideo");
                //        return;
                //    }
                //}
            }
            Serie serie = await APIRequests.GetSerieInfoAsync(oVideoSelecionado.nCdApi, Properties.Settings.Default.pref_IdiomaPesquisa);
            serie.sDsTitulo = oVideoSelecionado.sDsTitulo; // Para manter os titulos no idioma original, ao invés das traduções do SBT (tipo "Os Seguidores" pro The Following ¬¬)
            serie.nIdTipoConteudo = nIdTipoConteudo;

            if (oVideoSelecionado.sDsPasta != null)
            {
                serie.sDsPasta = oVideoSelecionado.sDsPasta;
            }
            else if (sDsPastaEditar != null)
            {
                serie.sDsPasta = sDsPastaEditar;
            }
            else
            {
                serie.SetDefaultFolderPath();
            }

            serie.sAliases = oVideoSelecionado.sAliases;

            if (nCdVideo > 0)
            {
                serie.nCdVideo = nCdVideo;
            }

            ListaResultPesquisa.Where(x => x.nCdApi == oVideoSelecionado.nCdApi).ToList().ForEach(x =>
            {
                x.Clone(serie);
            });

            listaVideosQuaseCompletos.Add(serie);
            _oVideoSelecionado = serie;
            OnPropertyChanged("SelectedVideo");
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