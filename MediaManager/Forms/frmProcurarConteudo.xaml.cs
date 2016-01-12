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
        public Enums.TipoConteudo TipoConteudo { get; set; }

        public ProcurarConteudoViewModel ProcurarConteudoViewModel { get; set; }

        public frmProcurarConteudo(Enums.TipoConteudo tipoConteudo, Window owner)
        {
            Owner = owner;

            InitializeComponent();

            TipoConteudo = tipoConteudo;

            ProcurarConteudoViewModel = new ProcurarConteudoViewModel(TipoConteudo, Owner);

            ProcurarConteudoViewModel.ActionFechar = new Action(() => { DialogResult = true; Close(); });

            ProcurarConteudoViewModel.Owner = Owner;

            DataContext = ProcurarConteudoViewModel;
        }

        private void btAdicionar_Click(object sender, RoutedEventArgs e)
        {
        }

        private void checkItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void checkTodos_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
                foreach (var item in ProcurarConteudoViewModel.lstConteudos)
                {
                    item.bFlSelecionado = true;
                }
            else
                foreach (var item in ProcurarConteudoViewModel.lstConteudos)
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
                    conteudoAlterado.sDsTitulo = Path.GetFileName(conteudoAlterado.sDsPasta);
                frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(conteudoAlterado.nIdTipoConteudo, conteudoAlterado, true);
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