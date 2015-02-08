using MediaManager.Code;
using MediaManager.Code.Modelos;
using MediaManager.Code.Series;
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
using System.Windows.Threading;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for frmAdicionarSerie.xaml
    /// </summary>
    public partial class frmAdicionarConteudo : Window
    {
        private Properties.Settings settings = Properties.Settings.Default;
        private static List<Serie> resultPesquisa;

        public frmAdicionarConteudo(string query, Helper.Conteudo conteudo)
        {
            // TODO Arrumar imagem da engrenagem que não aparece.
            InitializeComponent();
            string type = "";
            switch (conteudo)
            {
                case Helper.Conteudo.Serie:
                    type = "show";
                    break;

                case Helper.Conteudo.Filme:
                    type = "movie";
                    break;

                case Helper.Conteudo.Anime:
                    type = "show";
                    break;
            }
            resultPesquisa = Helper.API_PesquisarConteudo(query, type);
            for (int i = 0; i < resultPesquisa.Count; i++)
            {
                cboListaConteudo.Items.Add(resultPesquisa[i].show.title);
            }

            cboListaConteudo.SelectedIndex = 1;
            cboListaConteudo.SelectionChanged += cboListaConteudo_SelectionChanged;
            AtualizarInformacoes2(resultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.slug);
            // TODO Resolver exception quando a pesquisa não retorna resultados (resultPesquisa = 0)
        }

        private void AtualizarInformacoes(string slugTrakt)
        {
            var midiaEscolhida = resultPesquisa.Find(x => x.show.ids.slug == slugTrakt);
            var v = Helper.API_GetSerieInfo(slugTrakt);
            var midiaEscolhidaImagens = Helper.API_GetSerieImages(slugTrakt);
            var midiaEscolhidaSinopse = Helper.API_GetSerieSinopse(midiaEscolhidaImagens.ids.slug);

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
            } // TODO Colocar imagem padrão caso não tenha banner/poster.

            if (midiaEscolhidaSinopse != null)
            {
                if (midiaEscolhidaSinopse.overview != null)
                {
                    tbxSinopse.Text = midiaEscolhidaSinopse.overview;
                }
                else
                {
                    tbxSinopse.Text = "Sinopse não encontrada.";
                };
            }
            else if (midiaEscolhidaSinopse == null)
            {
                if (midiaEscolhida.show.overview != null)
                {
                    tbxSinopse.Text = midiaEscolhida.show.overview;
                }
                else
                {
                    tbxSinopse.Text = "Sinopse não encontrada.";
                }
            }
            else
            {
                tbxSinopse.Text = "Sinopse não encontrada.";
            }

            if (settings.pref_PastaSeries != "")
            {
                tbxPasta.Text = System.IO.Path.Combine(settings.pref_PastaSeries, Helper.RetirarCaracteresInvalidos(midiaEscolhida.show.title));
            }
        }

        private void AtualizarInformacoes2(string slugTrakt)
        {
            var midiaEscolhida = resultPesquisa.Find(x => x.show.ids.slug == slugTrakt);
            Serie2 v = Helper.API_GetSerieInfo(slugTrakt);
            //var midiaEscolhidaImagens = Helper.API_GetSerieImages(slugTrakt);
            //var midiaEscolhidaSinopse = Helper.API_GetSerieSinopse(midiaEscolhidaImagens.ids.slug);

            if (v.images.poster.thumb != null)
            {
                BitmapImage bmpPoster = new BitmapImage();
                bmpPoster.BeginInit();
                bmpPoster.UriSource = new Uri(v.images.poster.thumb);
                bmpPoster.EndInit();

                imgPoster.Source = bmpPoster;
            }
            else if (v.images.poster.medium != null)
            {
                BitmapImage bmpPoster = new BitmapImage();
                bmpPoster.BeginInit();
                bmpPoster.UriSource = new Uri(v.images.poster.medium);
                bmpPoster.EndInit();

                imgPoster.Source = bmpPoster;
            }
            else if (v.images.poster.full != null)
            {
                BitmapImage bmpPoster = new BitmapImage();
                bmpPoster.BeginInit();
                bmpPoster.UriSource = new Uri(v.images.poster.full);
                bmpPoster.EndInit();

                imgPoster.Source = bmpPoster;
            }

            if (v.images.banner.full != null)
            {
                BitmapImage bmpBanner = new BitmapImage();
                bmpBanner.BeginInit();
                bmpBanner.UriSource = new Uri(v.images.banner.full);
                bmpBanner.EndInit();

                imgBanner.Source = bmpBanner;
            } // TODO Colocar imagem padrão caso não tenha banner/poster.

            if (v.available_translations.Contains(settings.pref_IdiomaPesquisa))
            {
                var vv = Helper.API_GetSerieSinopse(slugTrakt);
                if (vv.overview != null)
                {
                    tbxSinopse.Text = vv.overview;
                }
                else if (v.overview != null)
                {
                    tbxSinopse.Text = v.overview;
                }
                else
                {
                    tbxSinopse.Text = "Sinopse não encontrada.";
                };
            }
            else if (v.overview != null)
            {
                tbxSinopse.Text = v.overview;
            }
            else
            {
                tbxSinopse.Text = "Sinopse não encontrada.";
            };

            if (settings.pref_PastaSeries != "")
            {
                tbxPasta.Text = System.IO.Path.Combine(settings.pref_PastaSeries, Helper.RetirarCaracteresInvalidos(midiaEscolhida.show.title));
            }
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            Forms.frmConfigConteudo frmConfigConteudo = new frmConfigConteudo();
            frmConfigConteudo.ShowDialog();
            if (frmConfigConteudo.DialogResult == true)
            {
                // TODO Salvar configuração do conteúdo
            }
        }

        private void btnPasta_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cboListaConteudo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex != 0)
            {
                var timer = new DispatcherTimer();
                timer.Tick += new EventHandler(timer_Tick);
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Start();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            AtualizarInformacoes2(resultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.slug);
        }
    }
}