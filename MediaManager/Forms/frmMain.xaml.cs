using MediaManager.Code;
using Microsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void menuItProcurarConteudo_Click(object sender, RoutedEventArgs e)
        {
        }

        private void menuItRenomearTudo_Click(object sender, RoutedEventArgs e)
        {
        }

        private void menuItRenomearSerie_Click(object sender, RoutedEventArgs e)
        {
        }

        private void menuItRenomearFilmes_Click(object sender, RoutedEventArgs e)
        {
        }

        private void menuItRenomearAnimes_Click(object sender, RoutedEventArgs e)
        {
        }

        private void menuItPreferencias_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmPreferencias frmPreferencias = new Forms.frmPreferencias();
            frmPreferencias.ShowDialog();
        }

        private void menuItSair_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnPesquisarSerie_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmAdicionarConteudo frmAdicionarConteudo = new Forms.frmAdicionarConteudo(tbxPesquisarSerie.Text, "show");
            frmAdicionarConteudo.ShowDialog();
            if (frmAdicionarConteudo.DialogResult == true)
            {
                // @TODO Adicionar conteudo
            }
        }

        private void btnPesquisarFilme_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnPesquisarAnime_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}