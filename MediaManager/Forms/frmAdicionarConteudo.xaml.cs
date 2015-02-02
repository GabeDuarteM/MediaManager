using MediaManager.Code;
using MediaManager.Code.Searches;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for frmAdicionarSerie.xaml
    /// </summary>
    public partial class frmAdicionarConteudo : Window
    {
        private static List<Search> resultPesquisa;

        public frmAdicionarConteudo(string query, string type)
        {
            resultPesquisa = Helper.API_PesquisarConteudo(query,type);
            InitializeComponent();
            
            for (int i = 0; i < resultPesquisa.Count; i++)
            {
                cboListaConteudo.Items.Add(resultPesquisa[i].show.title);
            }
            
            cboListaConteudo.SelectedIndex = 1;
            AtualizarInformacoes(resultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.trakt);
        }

        private void AtualizarInformacoes(string traktID)
        {
        	// @TODO Fazer atualizar também a pasta, de acordo com as opções do programa.
            var midiaEscolhidaImagens = Helper.API_GetSerieImages(traktID);

            if (midiaEscolhidaImagens.images.poster.thumb != null)
            {
                BitmapImage bmpPoster = new BitmapImage();
                bmpPoster.BeginInit();
                bmpPoster.UriSource = new Uri(midiaEscolhidaImagens.images.poster.thumb);
                bmpPoster.EndInit();

                imgPoster.Source = bmpPoster;
            }
            else if (midiaEscolhidaImagens.images.poster.medium != null)
            {
                BitmapImage bmpPoster = new BitmapImage();
                bmpPoster.BeginInit();
                bmpPoster.UriSource = new Uri(midiaEscolhidaImagens.images.poster.medium);
                bmpPoster.EndInit();

                imgPoster.Source = bmpPoster;
            }
            else if (midiaEscolhidaImagens.images.poster.full != null)
            {
                BitmapImage bmpPoster = new BitmapImage();
                bmpPoster.BeginInit();
                bmpPoster.UriSource = new Uri(midiaEscolhidaImagens.images.poster.full);
                bmpPoster.EndInit();

                imgPoster.Source = bmpPoster;
            }
            
            if (midiaEscolhidaImagens.images.banner.full != null)
            {
                BitmapImage bmpBanner = new BitmapImage();
                bmpBanner.BeginInit();
                bmpBanner.UriSource = new Uri(midiaEscolhidaImagens.images.banner.full);
                bmpBanner.EndInit();

                imgBanner.Source = bmpBanner;
            }
            
            var midiaEscolhidaSinopse = Helper.API_GetSerieSinopse(midiaEscolhidaImagens.ids.slug);
            tbxSinopse.Text = midiaEscolhidaSinopse.overview;
            
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmConfigConteudo frmConfigConteudo = new frmConfigConteudo();
            frmConfigConteudo.ShowDialog();
            if (frmConfigConteudo.DialogResult == true)
            {
                // @TODO Salvar configuração do conteúdo
            }
        }

        private void btnPasta_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cboListaConteudo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex != 0)
            {
                AtualizarInformacoes(resultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.trakt);
            }
        }
    }
}