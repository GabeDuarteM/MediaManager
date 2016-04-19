// Developed by: Gabriel Duarte
// 
// Created at: 16/08/2015 22:33
// Last update: 19/04/2016 02:46

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    ///     Interaction logic for frmProcurarConteudo.xaml
    /// </summary>
    public partial class frmProcurarConteudo : Window
    {
        public frmProcurarConteudo(Enums.TipoConteudo tipoConteudo, Window owner)
        {
            Owner = owner;

            InitializeComponent();

            TipoConteudo = tipoConteudo;

            ProcurarConteudoViewModel = new ProcurarConteudoViewModel(TipoConteudo, Owner);

            ProcurarConteudoViewModel.ActionFechar = new Action(() =>
            {
                DialogResult = true;
                Close();
            });

            ProcurarConteudoViewModel.Owner = Owner;

            DataContext = ProcurarConteudoViewModel;
        }

        public Enums.TipoConteudo TipoConteudo { get; set; }

        public ProcurarConteudoViewModel ProcurarConteudoViewModel { get; set; }

        private void btAdicionar_Click(object sender, RoutedEventArgs e)
        {
        }

        private void checkItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void checkTodos_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
                foreach (Video item in ProcurarConteudoViewModel.lstConteudos)
                {
                    item.bFlSelecionado = true;
                }
            else
                foreach (Video item in ProcurarConteudoViewModel.lstConteudos)
                {
                    item.bFlSelecionado = false;
                }
        }

        private void dgAllRowClick_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgAll.SelectedItem != null)
            {
                var conteudo = dgAll.SelectedItem as Serie;
                var conteudoAlterado = new Serie();
                // Para não alterar as informações na grid e tb pra cair no for abaixo quando o resultado nao tiver sido encontrado.
                conteudoAlterado.Clone(conteudo);
                if (conteudoAlterado.bFlNaoEncontrado)
                    conteudoAlterado.sDsTitulo = Path.GetFileName(conteudoAlterado.sDsPasta);
                var frmAdicionarConteudo = new frmAdicionarConteudo(conteudoAlterado.nIdTipoConteudo,
                                                                    conteudoAlterado, true);
                frmAdicionarConteudo.ShowDialog(this);
                if (frmAdicionarConteudo.DialogResult == true)
                {
                    Video video = frmAdicionarConteudo.AdicionarConteudoViewModel.oVideoSelecionado;
                    conteudo.Clone(video);
                    conteudo.bFlSelecionado = true;
                    ProcurarConteudoViewModel.CommandSelecionar.Execute(ProcurarConteudoViewModel);
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
