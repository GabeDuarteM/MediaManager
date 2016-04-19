// Developed by: Gabriel Duarte
// 
// Created at: 20/07/2015 21:10
// Last update: 19/04/2016 02:57

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ConfigurableInputMessageBox;
using MediaManager.Commands;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class AdicionarConteudoViewModel : ViewModelBase
    {
        private readonly Video oVideoBuscaPersonalizada;

        private readonly Video oVideoCarregando;

        private IList<Video> _lstResultPesquisa;

        private Video _oVideoSelecionado;

        public IList<Video> lstVideosQuaseCompletos; // Alguns podem faltar os episódios. Tratar isso ao salvar.

        private int nCdVideo;

        private string sDsPastaEditar;

        public AdicionarConteudoViewModel(Video video, Enums.TipoConteudo tipoConteudo)
        {
            CommandAbrirEpisodios = new AdicionarConteudoCommands.CommandAbrirEpisodios();
            CommandConfigurarConteudo = new AdicionarConteudoCommands.CommandConfigurarConteudo();
            CommandSalvarConteudo = new AdicionarConteudoCommands.CommandSalvarConteudo();
            CommandEscolherPasta = new AdicionarConteudoCommands.CommandEscolherPasta();
            nIdTipoConteudo = tipoConteudo;
            //Data = new SeriesData();
            lstVideosQuaseCompletos = new List<Video>();
            lstResultPesquisa = new ObservableCollection<Video>();
            oVideoBuscaPersonalizada = new Serie()
            {
                sDsTitulo = "Busca personalizada...",
                sDsSinopse = "Carregando sinopse..."
            };
            oVideoCarregando = new Serie() {sDsTitulo = "Carregando...", sDsSinopse = "Carregando sinopse..."};

            getResultPesquisaAsync(video);
        }

        public IList<Video> lstResultPesquisa
        {
            get { return _lstResultPesquisa; }
            set
            {
                _lstResultPesquisa = value;
                OnPropertyChanged();
            }
        }

        public Enums.TipoConteudo nIdTipoConteudo { get; set; }

        public Video oVideoSelecionado
        {
            get { return _oVideoSelecionado; }
            set
            {
                Video oVideoSelecionadoTemp = _oVideoSelecionado;
                _oVideoSelecionado = value;
                AlterarVideoAsync(oVideoSelecionadoTemp);
                OnPropertyChanged();
            }
        }

        public bool bProcurarConteudo { get; set; }

        public bool bFechar { get; set; }

        public Action<bool> ActionClose { get; set; }

        public ICommand CommandAbrirEpisodios { get; set; }

        public ICommand CommandConfigurarConteudo { get; set; }

        public ICommand CommandSalvarConteudo { get; set; }

        public ICommand CommandEscolherPasta { get; set; }

        private async void getResultPesquisaAsync(Video video)
        {
            lstResultPesquisa.Clear();

            if (video.nCdApi != 0 &&
                (video.nIdEstado == Enums.Estado.Completo || video.nIdEstado == Enums.Estado.CompletoSemForeignKeys))
            {
                lstVideosQuaseCompletos.Add(video);
                lstResultPesquisa.Add(video);
            }

            lstResultPesquisa.Add(oVideoCarregando);
            oVideoSelecionado = video.nIdEstado == Enums.Estado.Completo ||
                                video.nIdEstado == Enums.Estado.CompletoSemForeignKeys
                                    ? video
                                    : oVideoCarregando;
            var lstResultPesquisaTemp = new List<Video>();

            // Guarda as informações abaixo para caso for realizada uma Busca personalizada.
            if (string.IsNullOrWhiteSpace(sDsPastaEditar) || nCdVideo == 0)
            {
                sDsPastaEditar = video.sDsPasta;
                nCdVideo = video.nCdVideo;
            }

            if (video.nIdTipoConteudo == Enums.TipoConteudo.Série || video.nIdTipoConteudo == Enums.TipoConteudo.Anime)
            {
                var lstSeries = new List<Serie>();

                if ((video is Serie && !(video as Serie).bFlNaoEncontrado) || !(video is Serie))
                {
                    //data = await APIRequests.GetSeriesAsync(video.Title);
                    lstSeries = await APIRequests.GetSeriesAsync(video.sDsTitulo);
                }
                else if (video is Serie && (video as Serie).bFlNaoEncontrado)
                {
                    var inputMessageBox = new InputMessageBox(inputType.SemResultados);
                    inputMessageBox.InputViewModel.Properties.InputText = video.sDsTitulo;
                    inputMessageBox.ShowDialog();
                    if (inputMessageBox.DialogResult == true)
                    {
                        lstSeries =
                            await APIRequests.GetSeriesAsync(inputMessageBox.InputViewModel.Properties.InputText);
                    }

                    while (lstSeries.Count == 0)
                    {
                        if (
                            Helper.MostrarMensagem("Nenhum resultado encontrado, deseja pesquisar por outro nome?",
                                                   Enums.eTipoMensagem.QuestionamentoSimNao,
                                                   "Nenhum resultado encontrado") ==
                            MessageBoxResult.Yes)
                        {
                            inputMessageBox = new InputMessageBox(inputType.SemResultados);
                            inputMessageBox.InputViewModel.Properties.InputText = video.sDsTitulo;
                            inputMessageBox.ShowDialog();
                            if (inputMessageBox.DialogResult == true)
                            {
                                lstSeries =
                                    await
                                    APIRequests.GetSeriesAsync(inputMessageBox.InputViewModel.Properties.InputText);
                            }
                        }
                        else // TODO Fechar tela ao entrar no else
                        {
                            bFechar = true;
                            return;
                        }
                    }
                }

                foreach (Serie item in lstSeries)
                {
                    bool bFlAdicionado = lstResultPesquisa.Select(x => x.nCdApi).Contains(item.nCdApi);

                    if (!bFlAdicionado)
                    {
                        item.nCdVideo = nCdVideo != 0
                                            ? nCdVideo
                                            : video.nCdVideo;
                        item.sDsSinopse = oVideoCarregando.sDsSinopse;
                        // Seta como default e será carregado quando e se for selecionado no combo.
                        item.sDsPasta = null;

                        lstResultPesquisaTemp.Add(item);
                    }
                }
            }

            lstResultPesquisaTemp.Add(oVideoBuscaPersonalizada);
            lstResultPesquisa.Remove(oVideoCarregando);
            lstResultPesquisaTemp.ForEach(x => lstResultPesquisa.Add(x));

            oVideoSelecionado = video.nCdApi != 0 && lstResultPesquisa.Any(x => x.nCdApi == video.nCdApi)
                                    ? lstResultPesquisa.First(x => x.nCdApi == video.nCdApi)
                                    : lstResultPesquisa[0];
        }

        /*
        private async void getResultPesquisaAsync(string title)
        {
            lstResultPesquisa.Clear();

            lstResultPesquisa.Add(oVideoCarregando);
            oVideoSelecionado = oVideoCarregando;
            List<Video> lstResultPesquisaTemp = new List<Video>();

            if (nIdTipoConteudo == Enums.TipoConteudo.Série || nIdTipoConteudo == Enums.TipoConteudo.Anime)
            {
                List<Serie> lstSeries = await APIRequests.GetSeriesAsync(title);

                if (lstSeries.Count == 0) // Verificar a primeira vez se é null para não exibir a mensagem quando não encontra resultados na tela de procurar conteúdo.
                {
                    InputMessageBox InputMessageBox = new InputMessageBox(inputType.SemResultados);
                    InputMessageBox.InputViewModel.Properties.InputText = title;
                    InputMessageBox.ShowDialog();
                    if (InputMessageBox.DialogResult == true)
                    {
                        lstSeries = await APIRequests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText);
                    }

                    while (lstSeries.Count == 0)
                    {
                        if (MessageBox.Show("Nenhum resultado encontrado, deseja pesquisar por outro nome?", "Nenhum resultado encontrado - " + Properties.Settings.Default.AppName, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            InputMessageBox = new InputMessageBox(inputType.SemResultados);
                            InputMessageBox.InputViewModel.Properties.InputText = title;
                            InputMessageBox.ShowDialog();
                            if (InputMessageBox.DialogResult == true)
                            {
                                lstSeries = await APIRequests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText);
                            }
                        }
                        else // TODO Fechar tela ao entrar no else
                        {
                            bFechar = true;
                            return;
                        }
                    }
                }

                foreach (var item in lstSeries)
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

                    bool bFlAdicionado = lstResultPesquisaTemp.Select(x => x.nCdApi).Contains(item.nCdApi);

                    if (!bFlAdicionado)
                    {
                        item.sDsSinopse = oVideoCarregando.sDsSinopse;
                        lstResultPesquisaTemp.Add(item);
                    }
                }
            }

            lstResultPesquisa.Remove(oVideoCarregando);
            lstResultPesquisaTemp.Add(oVideoBuscaPersonalizada);
            lstResultPesquisaTemp.ForEach(x => lstResultPesquisa.Add(x));
            oVideoSelecionado = lstResultPesquisa[0];
        }
*/

        private async void AlterarVideoAsync(Video oVideoSelecionadoTemp)
        {
            if (oVideoSelecionado == null || oVideoSelecionado == oVideoCarregando ||
                oVideoSelecionado.nIdEstado == Enums.Estado.Completo ||
                oVideoSelecionado.nIdEstado == Enums.Estado.CompletoSemForeignKeys)
            {
                return;
            }

            if (lstResultPesquisa.Count > 0 && oVideoSelecionado == oVideoBuscaPersonalizada)
            {
                var inputMessageBox = new InputMessageBox(inputType.AdicionarConteudo);
                inputMessageBox.ShowDialog();
                if (inputMessageBox.DialogResult == true)
                {
                    getResultPesquisaAsync(new Serie() {sDsTitulo = inputMessageBox.InputViewModel.Properties.InputText});
                    return;
                }
                else
                {
                    _oVideoSelecionado =
                        lstResultPesquisa.Where(x => x.nCdApi == oVideoSelecionadoTemp?.nCdApi).FirstOrDefault();
                    if (oVideoSelecionado == null)
                    {
                        ActionClose(false);
                    }
                    else
                    {
                        OnPropertyChanged(nameof(oVideoSelecionado));
                    }
                    return;
                }
            }
            else if (lstVideosQuaseCompletos != null && lstVideosQuaseCompletos.Count > 0)
            {
                lstVideosQuaseCompletos
                    .Where(
                           x =>
                           x.nCdApi == oVideoSelecionado.nCdApi &&
                           (x.nIdEstado == Enums.Estado.Completo || x.nIdEstado == Enums.Estado.CompletoSemForeignKeys))
                    .ToList()
                    .ForEach(x =>
                    {
                        _oVideoSelecionado = x;
                        OnPropertyChanged(nameof(oVideoSelecionado));
                        return;
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
            Serie serie =
                await
                APIRequests.GetSerieInfoAsync(oVideoSelecionado.nCdApi,
                                              Properties.Settings.Default.pref_IdiomaPesquisa);
            serie.sDsTitulo = oVideoSelecionado.sDsTitulo;
            // Para manter os titulos no idioma original, ao invés das traduções do SBT (tipo "Os Seguidores" pro The Following ¬¬)
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

            lstResultPesquisa.Where(x => x.nCdApi == oVideoSelecionado.nCdApi)
                             .ToList()
                             .ForEach(x => { x.Clone(serie); });

            lstVideosQuaseCompletos.Add(serie);
            _oVideoSelecionado = serie;
            OnPropertyChanged(nameof(oVideoSelecionado));
            CommandManager.InvalidateRequerySuggested();
            // Para forçar a habilitação do botão de configurar conteúdo (As vezes continua desabilitado até que haja interação na UI, com esse método isso não acontece).
        }
    }
}
