using MediaManager.Code;
using MediaManager.Code.Modelos;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        private List<Search> resultPesquisa = new List<Search>();

        private Helper.Conteudo conteudo = new Helper.Conteudo();

        public frmAdicionarConteudo(Helper.Conteudo conteudo, List<Search> resultPesquisa)
        {
            // TODO Arrumar imagem da engrenagem que não aparece.
            InitializeComponent();

            this.resultPesquisa = resultPesquisa;
            this.conteudo = conteudo;
            if (conteudo == Helper.Conteudo.Serie || conteudo == Helper.Conteudo.Anime)
            {
                for (int i = 0; i < resultPesquisa.Count; i++)
                {
                    cboListaConteudo.Items.Add(resultPesquisa[i].show.title);
                }
                cboListaConteudo.SelectedIndex = 1;
                cboListaConteudo.SelectionChanged += cboListaConteudo_SelectionChanged;
                AtualizarInformacoes(resultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.slug);
            }
            else if (conteudo == Helper.Conteudo.Filme)
            {
                for (int i = 0; i < resultPesquisa.Count; i++)
                {
                    cboListaConteudo.Items.Add(resultPesquisa[i].movie.title);
                }
                cboListaConteudo.SelectedIndex = 1;
                cboListaConteudo.SelectionChanged += cboListaConteudo_SelectionChanged;
                AtualizarInformacoes(resultPesquisa[cboListaConteudo.SelectedIndex - 1].movie.ids.slug);
            }
            // TODO Resolver exception quando a pesquisa não retorna resultados (resultPesquisa = 0)
        }

        private void AtualizarInformacoes(string slugTrakt)
        {
            if (conteudo == Helper.Conteudo.Serie || conteudo == Helper.Conteudo.Anime)
            {
                Serie serie = new Serie();

                BackgroundWorker workerAtualizarCampos = new BackgroundWorker();
                workerAtualizarCampos.DoWork += (s, e) =>
                {
                    this.Dispatcher.Invoke((Action)(() => { serie = Helper.API_GetSerieInfo(slugTrakt); }));
                };

                workerAtualizarCampos.RunWorkerCompleted += (s, e) =>
                {
                    if (serie.images.poster.thumb != null)
                    {
                        BitmapImage bmpPoster = new BitmapImage();
                        bmpPoster.BeginInit();
                        bmpPoster.UriSource = new Uri(serie.images.poster.thumb);
                        bmpPoster.EndInit();

                        imgPoster.Source = bmpPoster;
                    }
                    else if (serie.images.poster.medium != null)
                    {
                        BitmapImage bmpPoster = new BitmapImage();
                        bmpPoster.BeginInit();
                        bmpPoster.UriSource = new Uri(serie.images.poster.medium);
                        bmpPoster.EndInit();

                        imgPoster.Source = bmpPoster;
                    }
                    else if (serie.images.poster.full != null)
                    {
                        BitmapImage bmpPoster = new BitmapImage();
                        bmpPoster.BeginInit();
                        bmpPoster.UriSource = new Uri(serie.images.poster.full);
                        bmpPoster.EndInit();

                        imgPoster.Source = bmpPoster;
                    }

                    if (serie.images.banner.full != null)
                    {
                        BitmapImage bmpBanner = new BitmapImage();
                        bmpBanner.BeginInit();
                        bmpBanner.UriSource = new Uri(serie.images.banner.full);
                        bmpBanner.EndInit();

                        imgBanner.Source = bmpBanner;
                    } // TODO Colocar imagem padrão caso não tenha banner/poster.

                    if (serie.available_translations.Contains(settings.pref_IdiomaPesquisa))
                    {
                        var vv = Helper.API_GetSerieSinopse(slugTrakt);
                        if (vv.overview != null)
                        {
                            tbxSinopse.Text = vv.overview;
                        }
                        else if (serie.overview != null)
                        {
                            tbxSinopse.Text = serie.overview;
                        }
                        else
                        {
                            tbxSinopse.Text = "Sinopse não encontrada.";
                        };
                    }
                    else if (serie.overview != null)
                    {
                        tbxSinopse.Text = serie.overview;
                    }
                    else
                    {
                        tbxSinopse.Text = "Sinopse não encontrada.";
                    };

                    if (settings.pref_PastaSeries != "")
                    {
                        tbxPasta.Text = System.IO.Path.Combine(settings.pref_PastaSeries, Helper.RetirarCaracteresInvalidos(serie.title));
                    }
                };
                if (workerAtualizarCampos.IsBusy == false)
                {
                    workerAtualizarCampos.RunWorkerAsync();
                }
                else
                {
                    workerAtualizarCampos.CancelAsync();
                    workerAtualizarCampos.RunWorkerAsync();
                }
            }
            else if (conteudo == Helper.Conteudo.Filme)
            {
                Filme filme = new Filme();

                BackgroundWorker workerAtualizarCampos = new BackgroundWorker();
                workerAtualizarCampos.DoWork += (s, e) =>
                {
                    this.Dispatcher.Invoke((Action)(() => { filme = Helper.API_GetFilmeInfo(slugTrakt); }));
                };

                workerAtualizarCampos.RunWorkerCompleted += (s, e) =>
                {
                    if (filme.images.poster.thumb != null)
                    {
                        BitmapImage bmpPoster = new BitmapImage();
                        bmpPoster.BeginInit();
                        bmpPoster.UriSource = new Uri(filme.images.poster.thumb);
                        bmpPoster.EndInit();

                        imgPoster.Source = bmpPoster;
                    }
                    else if (filme.images.poster.medium != null)
                    {
                        BitmapImage bmpPoster = new BitmapImage();
                        bmpPoster.BeginInit();
                        bmpPoster.UriSource = new Uri(filme.images.poster.medium);
                        bmpPoster.EndInit();

                        imgPoster.Source = bmpPoster;
                    }
                    else if (filme.images.poster.full != null)
                    {
                        BitmapImage bmpPoster = new BitmapImage();
                        bmpPoster.BeginInit();
                        bmpPoster.UriSource = new Uri(filme.images.poster.full);
                        bmpPoster.EndInit();

                        imgPoster.Source = bmpPoster;
                    }

                    if (filme.images.banner.full != null)
                    {
                        BitmapImage bmpBanner = new BitmapImage();
                        bmpBanner.BeginInit();
                        bmpBanner.UriSource = new Uri(filme.images.banner.full);
                        bmpBanner.EndInit();

                        imgBanner.Source = bmpBanner;
                    } // TODO Colocar imagem padrão caso não tenha banner/poster.

                    if (filme.available_translations.Contains(settings.pref_IdiomaPesquisa))
                    {
                        Filme filmeTraduzido = Helper.API_GetFilmeSinopse(slugTrakt);
                        if (filmeTraduzido.overview != null)
                        {
                            tbxSinopse.Text = filmeTraduzido.overview;
                        }
                        else if (filme.overview != null)
                        {
                            tbxSinopse.Text = filme.overview;
                        }
                        else
                        {
                            tbxSinopse.Text = "Sinopse não encontrada.";
                        };
                    }
                    else if (filme.overview != null)
                    {
                        tbxSinopse.Text = filme.overview;
                    }
                    else
                    {
                        tbxSinopse.Text = "Sinopse não encontrada.";
                    };

                    if (settings.pref_PastaFilmes != "")
                    {
                        tbxPasta.Text = System.IO.Path.Combine(settings.pref_PastaFilmes, Helper.RetirarCaracteresInvalidos(filme.title));
                    }
                };
                if (workerAtualizarCampos.IsBusy == false)
                {
                    workerAtualizarCampos.RunWorkerAsync();
                }
                else
                {
                    workerAtualizarCampos.CancelAsync();
                    workerAtualizarCampos.RunWorkerAsync();
                }
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
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cboListaConteudo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (conteudo == Helper.Conteudo.Serie || conteudo == Helper.Conteudo.Anime)
            {
                if ((sender as ComboBox).SelectedIndex != 0)
                {
                    AtualizarInformacoes(resultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.slug);
                }
            }
            else if (conteudo == Helper.Conteudo.Filme)
            {
                if ((sender as ComboBox).SelectedIndex != 0)
                {
                    AtualizarInformacoes(resultPesquisa[cboListaConteudo.SelectedIndex - 1].movie.ids.slug);
                }
            }
        }
    }
}