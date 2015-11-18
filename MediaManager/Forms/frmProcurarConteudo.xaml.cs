using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.ViewModel;
using Ookii.Dialogs.Wpf;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for frmProcurarConteudo.xaml
    /// </summary>
    public partial class frmProcurarConteudo : Window
    {
        public Enums.ContentType TipoConteudo { get; set; }

        public ProcurarConteudoViewModel ProcurarConteudoViewModel { get; set; }

        public frmProcurarConteudo(Enums.ContentType tipoConteudo, Window owner)
        {
            Owner = owner;

            InitializeComponent();

            TipoConteudo = tipoConteudo;

            ProcurarConteudoViewModel = new ProcurarConteudoViewModel(TipoConteudo, Owner);

            DataContext = ProcurarConteudoViewModel;

            btAdicionar.IsEnabled = true;
        }

        private void btAdicionar_Click(object sender, RoutedEventArgs e)
        {
            //btAdicionar.IsEnabled = false;
            frmBarraProgresso frmBarraProgresso = new frmBarraProgresso();
            frmBarraProgresso.BarraProgressoViewModel.dNrProgressoMaximo = ProcurarConteudoViewModel.Conteudos.Count;
            frmBarraProgresso.BarraProgressoViewModel.sDsTarefa = "Salvando...";
            frmBarraProgresso.BarraProgressoViewModel.Worker.DoWork += (s, ev) =>
            {
                if (ProcurarConteudoViewModel.Conteudos.Where(x => x.bFlSelecionado).Count() == 0)
                {
                    Helper.MostrarMensagem("Para realizar a operação, selecione ao menos um registro.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                foreach (var item in ProcurarConteudoViewModel.Conteudos)
                {
                    if (item.bFlSelecionado == true)
                    {
                        switch (item.ContentType)
                        {
                            case Enums.ContentType.Série:
                                {
                                    if (item.Estado != Enums.Estado.Completo)
                                    {
                                        frmBarraProgresso.BarraProgressoViewModel.sDsTexto = "Salvando " + item.Title + "...";
                                        Serie serie = APIRequests.GetSerieInfoAsync(item.IDApi, Properties.Settings.Default.pref_IdiomaPesquisa).Result;
                                        serie.FolderPath = item.FolderPath;
                                        serie.SerieAliasStr = item.SerieAliasStr;
                                        serie.SerieAlias = item.SerieAlias;
                                        serie.Title = item.Title;
                                        DBHelper.AddSerie(serie);
                                        frmBarraProgresso.BarraProgressoViewModel.dNrProgressoAtual++;
                                    }
                                    else
                                    {
                                        frmBarraProgresso.BarraProgressoViewModel.sDsTexto = "Salvando " + item.Title + "...";
                                        DBHelper.AddSerie((Serie)item);
                                        frmBarraProgresso.BarraProgressoViewModel.dNrProgressoAtual++;
                                    }
                                    break;
                                }
                            case Enums.ContentType.Anime:
                                {
                                    if (item.Estado != Enums.Estado.Completo)
                                    {
                                        frmBarraProgresso.BarraProgressoViewModel.sDsTexto = "Salvando " + item.Title + "...";
                                        Serie anime = APIRequests.GetSerieInfoAsync(item.IDApi, /*item.Language*/Properties.Settings.Default.pref_IdiomaPesquisa).Result;
                                        anime.ContentType = item.ContentType;
                                        anime.FolderPath = item.FolderPath;
                                        anime.SerieAliasStr = item.SerieAliasStr;
                                        anime.SerieAlias = item.SerieAlias;
                                        anime.Title = item.Title;

                                        DBHelper.AddSerie(anime);
                                        frmBarraProgresso.BarraProgressoViewModel.dNrProgressoAtual++;
                                    }
                                    else
                                    {
                                        frmBarraProgresso.BarraProgressoViewModel.sDsTexto = "Salvando " + item.Title + "...";
                                        DBHelper.AddSerie((Serie)item);
                                        frmBarraProgresso.BarraProgressoViewModel.dNrProgressoAtual++;
                                    }
                                    break;
                                }
                            case Enums.ContentType.Filme:
                                // TODO Fazer funfar
                                //Filme filme = await Helper.API_GetFilmeInfoAsync(item.TraktSlug);
                                //filme.FolderPath = item.Pasta;
                                //await DatabaseHelper.AddFilmeAsync(filme);
                                break;

                            default:
                                break;
                        }
                    }
                }

                Helper.MostrarMensagem("Séries inseridas com sucesso.", MessageBoxButton.OK, MessageBoxImage.Information);
            };

            Action actionFechar = new Action(() => { DialogResult = true; Close(); });

            frmBarraProgresso.BarraProgressoViewModel.Worker.RunWorkerCompleted += (s, ev) =>
            {
                actionFechar();
            };
            frmBarraProgresso.BarraProgressoViewModel.Worker.RunWorkerAsync();
            frmBarraProgresso.ShowDialog(this);
        }

        private void checkItem_Click(object sender, RoutedEventArgs e)
        {
            // TODO Verificar se todos os checks estão checados e checar/deschecar o checkAllItems

            //int selecionado = 0;
            //foreach (var item in contViewModel.Conteudos)
            //{
            //    if (item.IsSelected == true)
            //    {
            //        selecionado++;
            //    }
            //}
        }

        private void checkTodos_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
                foreach (var item in ProcurarConteudoViewModel.Conteudos)
                {
                    item.bFlSelecionado = true;
                }
            else
                foreach (var item in ProcurarConteudoViewModel.Conteudos)
                {
                    item.bFlSelecionado = false;
                }
        }

        private void dgAllRowClick_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgAll.SelectedItem != null)
            {
                Serie conteudo = dgAll.SelectedItem as Serie;
                Serie conteudoAlterado = new Serie(); // Para não alterar as informações na grid e tb pra cair no for abaixo quando o resultado nao tiver sido encontrado.
                conteudoAlterado.Clone(conteudo);
                if (conteudoAlterado.bFlNaoEncontrado)
                    conteudoAlterado.Title = Path.GetFileName(conteudoAlterado.FolderPath);
                frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(conteudoAlterado.ContentType, conteudoAlterado);
                frmAdicionarConteudo.IsProcurarConteudo = true;
                frmAdicionarConteudo.ShowDialog(this);
                if (frmAdicionarConteudo.DialogResult == true)
                {
                    Video video = frmAdicionarConteudo.AdicionarConteudoViewModel.SelectedVideo;
                    conteudo.Clone(video);
                    conteudo.bFlSelecionado = true;
                    //if
                    //int i;
                    //for (i = 0; i < ProcurarConteudoViewModel.Conteudos.Count; i++)
                    //{
                    //    if (ProcurarConteudoViewModel.Conteudos[i] == conteudo)
                    //    {
                    //        break;
                    //    }
                    //}
                    //if (video is Serie)
                    //{
                    //    ProcurarConteudoViewModel.Conteudos[i] = (Serie)video;
                    //    ProcurarConteudoViewModel.Conteudos[i].bFlSelecionado = true;
                    //}
                    //else
                    //    throw new InvalidCastException();
                }
            }
        }

        public void ShowDialog(Window owner)
        {
            Owner = owner;
            ShowDialog();
        }
    }
}