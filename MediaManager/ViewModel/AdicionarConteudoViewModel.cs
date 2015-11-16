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
        private List<Video> _ResultPesquisa;

        private Enums.ContentType _TipoConteudo;

        private Video _SelectedVideo;

        private string folderPathEditar;

        private int IDBanco;

        private Video videoBuscaPersonalizada;

        private Video videoCarregando;

        public List<Video> listaVideosQuaseCompletos; // Alguns podem faltar os episódios. Tratar isso ao salvar.

        public List<Video> ResultPesquisa { get { return _ResultPesquisa; } set { _ResultPesquisa = value; OnPropertyChanged(); } }

        public Enums.ContentType TipoConteudo { get { return _TipoConteudo; } set { _TipoConteudo = value; } }

        public Video SelectedVideo { get { return _SelectedVideo; } set { _SelectedVideo = value; OnPropertyChanged(); AlterarVideoAsync(); } }

        public ICommand CommandAbrirEpisodios { get; set; }

        public AdicionarConteudoViewModel(Video video, Enums.ContentType tipoConteudo)
        {
            CommandAbrirEpisodios = new AdicionarConteudoCommands.CommandAbrirEpisodios();
            TipoConteudo = tipoConteudo;
            //Data = new SeriesData();
            listaVideosQuaseCompletos = new List<Video>();
            ResultPesquisa = new List<Video>();
            videoBuscaPersonalizada = new Serie() { Title = "Busca personalizada...", Overview = "Carregando sinopse..." };
            videoCarregando = new Serie() { Title = "Carregando...", Overview = "Carregando sinopse..." };
            getResultPesquisaAsync(video);
        }

        private async void getResultPesquisaAsync(Video video)
        {
            ResultPesquisa = new List<Video>();

            if (video.IDApi != 0 && (video.Estado == Enums.Estado.Completo || video.Estado == Enums.Estado.CompletoSemForeignKeys))
            {
                listaVideosQuaseCompletos.Add(video);
                ResultPesquisa.Add(video);
            }

            ResultPesquisa.Add(videoCarregando);
            SelectedVideo = (video.Estado == Enums.Estado.Completo || video.Estado == Enums.Estado.CompletoSemForeignKeys) ? video : videoCarregando;
            var listaResultPesquisaTemp = new List<Video>();

            // Guarda as informações abaixo para caso for realizada uma Busca personalizada.
            if (string.IsNullOrWhiteSpace(folderPathEditar) || IDBanco == 0)
            {
                folderPathEditar = video.FolderPath;
                IDBanco = video.IDBanco;
            }

            if (video.ContentType == Enums.ContentType.Série || video.ContentType == Enums.ContentType.Anime)
            {
                SeriesData data = new SeriesData();

                if ((video is Serie && !(video as Serie).bFlNaoEncontrado) || !(video is Serie))
                {
                    data = await APIRequests.GetSeriesAsync(video.Title);
                }
                else if (video is Serie && (video as Serie).bFlNaoEncontrado)
                {
                    InputMessageBox InputMessageBox = new InputMessageBox(inputType.SemResultados);
                    InputMessageBox.InputViewModel.Properties.InputText = video.Title;
                    InputMessageBox.ShowDialog();
                    if (InputMessageBox.DialogResult == true)
                    {
                        data = await APIRequests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText);
                    }

                    while (data.Series == null)
                    {
                        if (Helper.MostrarMensagem("Nenhum resultado encontrado, deseja pesquisar por outro nome?", MessageBoxButton.YesNo, MessageBoxImage.Question, "Nenhum resultado encontrado") == MessageBoxResult.Yes)
                        {
                            InputMessageBox = new InputMessageBox(inputType.SemResultados);
                            InputMessageBox.InputViewModel.Properties.InputText = video.Title;
                            InputMessageBox.ShowDialog();
                            if (InputMessageBox.DialogResult == true)
                            {
                                data = await APIRequests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                foreach (var item in data.Series)
                {
                    bool bFlAdicionado = false;

                    if (ResultPesquisa.Select(x => x.IDApi).Contains(item.IDApi))
                    {
                        bFlAdicionado = true;
                    }

                    if (!bFlAdicionado)
                    {
                        //if (video.ContentType == Enums.ContentType.Anime)
                        //{
                        //    item.IsAnime = true;
                        //}
                        item.IDBanco = (IDBanco != 0) ? IDBanco : video.IDBanco;
                        item.Overview = videoCarregando.Overview; // Seta como default e será carregado quando e se for selecionado no combo.
                        item.FolderPath = null;

                        listaResultPesquisaTemp.Add(item);
                    }
                }
            }

            listaResultPesquisaTemp.Add(videoBuscaPersonalizada);
            ResultPesquisa.Remove(videoCarregando);
            listaResultPesquisaTemp.ForEach(x => ResultPesquisa.Add(x));

            SelectedVideo = (video.IDApi != 0 && ResultPesquisa.Where(x => x.IDApi == video.IDApi).Count() > 0) ? ResultPesquisa.Where(x => x.IDApi == video.IDApi).First() : ResultPesquisa[0];
        }

        private async void getResultPesquisaAsync(string title)
        {
            ResultPesquisa.Add(videoCarregando);
            SelectedVideo = videoCarregando;
            List<Video> listaResultPesquisaTemp = new List<Video>();

            if (TipoConteudo == Enums.ContentType.Série || TipoConteudo == Enums.ContentType.Anime)
            {
                SeriesData data = await APIRequests.GetSeriesAsync(title);

                if (data.Series == null) // Verificar a primeira vez se é null para não exibir a mensagem quando não encontra resultados na tela de procurar conteúdo.
                {
                    InputMessageBox InputMessageBox = new InputMessageBox(inputType.SemResultados);
                    InputMessageBox.InputViewModel.Properties.InputText = title;
                    InputMessageBox.ShowDialog();
                    if (InputMessageBox.DialogResult == true)
                    {
                        data = await APIRequests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText);
                    }

                    while (data.Series == null)
                    {
                        if (MessageBox.Show("Nenhum resultado encontrado, deseja pesquisar por outro nome?", "Nenhum resultado encontrado - " + Properties.Settings.Default.AppName, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            InputMessageBox = new InputMessageBox(inputType.SemResultados);
                            InputMessageBox.InputViewModel.Properties.InputText = title;
                            InputMessageBox.ShowDialog();
                            if (InputMessageBox.DialogResult == true)
                            {
                                data = await APIRequests.GetSeriesAsync(InputMessageBox.InputViewModel.Properties.InputText);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                foreach (var item in data.Series)
                {
                    if (!string.IsNullOrWhiteSpace(folderPathEditar)) // Verifica se é edição para setar o folderpath igual.
                    {
                        item.FolderPath = folderPathEditar;
                    }
                    else
                    {
                        item.SetDefaultFolderPath();
                    }

                    item.IDBanco = IDBanco;
                    item.ContentType = TipoConteudo;
                    bool bFlAdicionado = false;
                    if (listaResultPesquisaTemp.Select(x => x.IDApi).Contains(item.IDApi))
                    {
                        bFlAdicionado = true;
                    }

                    if (!bFlAdicionado)
                    {
                        item.Overview = videoCarregando.Overview;
                        listaResultPesquisaTemp.Add(item);
                    }
                }
            }

            listaResultPesquisaTemp.Add(videoBuscaPersonalizada);
            ResultPesquisa = listaResultPesquisaTemp;
            SelectedVideo = ResultPesquisa[0];
        }

        private async void AlterarVideoAsync()
        {
            if (SelectedVideo == null || SelectedVideo == videoCarregando || SelectedVideo.Estado == Enums.Estado.Completo || SelectedVideo.Estado == Enums.Estado.CompletoSemForeignKeys)
            {
                return;
            }

            if (ResultPesquisa.Count > 0 && SelectedVideo == videoBuscaPersonalizada)
            {
                InputMessageBox inputMessageBox = new InputMessageBox(inputType.AdicionarConteudo);
                inputMessageBox.ShowDialog();
                if (inputMessageBox.DialogResult == true)
                {
                    getResultPesquisaAsync(new Serie() { Title = inputMessageBox.InputViewModel.Properties.InputText });
                    return;
                }
            }
            else if (listaVideosQuaseCompletos != null && listaVideosQuaseCompletos.Count > 0)
            {
                listaVideosQuaseCompletos
                    .Where(x => x.IDApi == SelectedVideo.IDApi && (x.Estado == Enums.Estado.Completo || x.Estado == Enums.Estado.CompletoSemForeignKeys)).ToList()
                    .ForEach(x =>
                    {
                        _SelectedVideo = x; OnPropertyChanged("SelectedVideo"); return;
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
            SeriesData data = await APIRequests.GetSerieInfoAsync(SelectedVideo.IDApi, Properties.Settings.Default.pref_IdiomaPesquisa);
            data.Series[0].Title = SelectedVideo.Title; // Para manter os titulos no idioma original, ao invés das traduções do SBT (tipo "Os Seguidores" pro The Following ¬¬)
            data.Series[0].ContentType = TipoConteudo;

            if (SelectedVideo.FolderPath != null)
            {
                data.Series[0].FolderPath = SelectedVideo.FolderPath;
            }
            else if (folderPathEditar != null)
            {
                data.Series[0].FolderPath = folderPathEditar;
            }
            else
            {
                data.Series[0].SetDefaultFolderPath();
            }

            data.Series[0].SerieAliasStr = SelectedVideo.SerieAliasStr;

            if (IDBanco > 0)
            {
                data.Series[0].IDBanco = IDBanco;
            }

            ResultPesquisa.Where(x => x.IDApi == SelectedVideo.IDApi).ToList().ForEach(x =>
            {
                x.Clone(data.Series[0]);
            });

            listaVideosQuaseCompletos.Add(data.Series[0]);
            _SelectedVideo = data.Series[0];
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