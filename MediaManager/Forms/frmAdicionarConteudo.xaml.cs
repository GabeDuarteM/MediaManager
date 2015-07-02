using MediaManager.Helpers;
using MediaManager.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for frmAdicionarSerie.xaml
    /// </summary>
    public partial class frmAdicionarConteudo : Window
    {
        private Properties.Settings Settings = Properties.Settings.Default;

        private List<Search> ResultPesquisa = new List<Search>();

        private Helper.TipoConteudo Conteudo = new Helper.TipoConteudo();

        private Filme Filme = new Filme();

        private Filme FilmeTraduzido = new Filme();

        private Serie Serie = new Serie();

        private Serie SerieTraduzida = new Serie();

        public frmAdicionarConteudo(Helper.TipoConteudo conteudo, List<Search> resultPesquisa)
        {
            // TODO Arrumar imagem da engrenagem que não aparece.
            InitializeComponent();

            ResultPesquisa = resultPesquisa;
            Conteudo = conteudo;
            if (conteudo == Helper.TipoConteudo.show || conteudo == Helper.TipoConteudo.anime)
            {
                foreach (var item in resultPesquisa)
                {
                    cboListaConteudo.Items.Add(item.show.title);
                }

                cboListaConteudo.SelectedIndex = 1;
                cboListaConteudo.SelectionChanged += cboListaConteudo_SelectionChanged;
                AtualizarInformacoesAsync(resultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.slug);
            }
            else if (conteudo == Helper.TipoConteudo.movie)
            {
                foreach (var item in resultPesquisa)
                {
                    cboListaConteudo.Items.Add(item.movie.title);
                }

                cboListaConteudo.SelectedIndex = 1;
                cboListaConteudo.SelectionChanged += cboListaConteudo_SelectionChanged;
                AtualizarInformacoesAsync(resultPesquisa[cboListaConteudo.SelectedIndex - 1].movie.ids.slug);
            }
            // TODO Resolver exception quando a pesquisa não retorna resultados (resultPesquisa = 0)
        }

        private async void AtualizarInformacoesAsync(string slugTrakt)
        {
            // TODO Fazer funcionar com filmes/animes
            if (Conteudo == Helper.TipoConteudo.show || Conteudo == Helper.TipoConteudo.anime)
            {
                Serie = await Helper.API_GetSerieInfoAsync(slugTrakt);

                if (Serie.images.poster.thumb != null)
                {
                    BitmapImage bmpPoster = new BitmapImage();
                    bmpPoster.BeginInit();
                    bmpPoster.UriSource = new Uri(Serie.images.poster.thumb);
                    bmpPoster.EndInit();

                    imgPoster.Source = bmpPoster;
                }
                else if (Serie.images.poster.medium != null)
                {
                    BitmapImage bmpPoster = new BitmapImage();
                    bmpPoster.BeginInit();
                    bmpPoster.UriSource = new Uri(Serie.images.poster.medium);
                    bmpPoster.EndInit();

                    imgPoster.Source = bmpPoster;
                }
                else if (Serie.images.poster.full != null)
                {
                    BitmapImage bmpPoster = new BitmapImage();
                    bmpPoster.BeginInit();
                    bmpPoster.UriSource = new Uri(Serie.images.poster.full);
                    bmpPoster.EndInit();

                    imgPoster.Source = bmpPoster;
                }

                if (Serie.images.banner.full != null)
                {
                    BitmapImage bmpBanner = new BitmapImage();
                    bmpBanner.BeginInit();
                    bmpBanner.UriSource = new Uri(Serie.images.banner.full);
                    bmpBanner.EndInit();

                    imgBanner.Source = bmpBanner;
                } // TODO Colocar imagem padrão caso não tenha banner/poster.

                if (Serie.available_translations.Contains(Settings.pref_IdiomaPesquisa))
                {
                    SerieTraduzida = await Helper.API_GetSerieSinopseAsync(slugTrakt);
                    if (SerieTraduzida.overview != null)
                    {
                        tbxSinopse.Text = SerieTraduzida.overview;
                    }
                    else if (Serie.overview != null)
                    {
                        tbxSinopse.Text = Serie.overview;
                    }
                    else
                    {
                        tbxSinopse.Text = "Sinopse não encontrada.";
                    };
                }
                else if (Serie.overview != null)
                {
                    tbxSinopse.Text = Serie.overview;
                }
                else
                {
                    tbxSinopse.Text = "Sinopse não encontrada.";
                };

                if (Settings.pref_PastaSeries != "")
                {
                    tbxPasta.Text = System.IO.Path.Combine(Settings.pref_PastaSeries, Helper.RetirarCaracteresInvalidos(Serie.title));
                }
            }
        }

        #region { OLD AtualizarInformações }

        //private void AtualizarInformacoes(string slugTrakt)
        //{
        //    if (conteudo == Helper.Conteudo.Serie || conteudo == Helper.Conteudo.Anime)
        //    {
        //        BackgroundWorker workerAtualizarCampos = new BackgroundWorker();
        //        workerAtualizarCampos.DoWork += (s, e) =>
        //        {
        //            this.Dispatcher.Invoke((Action)(() => { serie = Helper.API_GetSerieInfo(slugTrakt); }));
        //        };

        //        workerAtualizarCampos.RunWorkerCompleted += (s, e) =>
        //        {
        //            if (serie.images.poster.thumb != null)
        //            {
        //                BitmapImage bmpPoster = new BitmapImage();
        //                bmpPoster.BeginInit();
        //                bmpPoster.UriSource = new Uri(serie.images.poster.thumb);
        //                bmpPoster.EndInit();

        //                imgPoster.Source = bmpPoster;
        //            }
        //            else if (serie.images.poster.medium != null)
        //            {
        //                BitmapImage bmpPoster = new BitmapImage();
        //                bmpPoster.BeginInit();
        //                bmpPoster.UriSource = new Uri(serie.images.poster.medium);
        //                bmpPoster.EndInit();

        //                imgPoster.Source = bmpPoster;
        //            }
        //            else if (serie.images.poster.full != null)
        //            {
        //                BitmapImage bmpPoster = new BitmapImage();
        //                bmpPoster.BeginInit();
        //                bmpPoster.UriSource = new Uri(serie.images.poster.full);
        //                bmpPoster.EndInit();

        //                imgPoster.Source = bmpPoster;
        //            }

        //            if (serie.images.banner.full != null)
        //            {
        //                BitmapImage bmpBanner = new BitmapImage();
        //                bmpBanner.BeginInit();
        //                bmpBanner.UriSource = new Uri(serie.images.banner.full);
        //                bmpBanner.EndInit();

        //                imgBanner.Source = bmpBanner;
        //            } // TODO Colocar imagem padrão caso não tenha banner/poster.

        //            if (serie.available_translations.Contains(settings.pref_IdiomaPesquisa))
        //            {
        //                serieTraduzida = Helper.API_GetSerieSinopse(slugTrakt);
        //                if (serieTraduzida.overview != null)
        //                {
        //                    tbxSinopse.Text = serieTraduzida.overview;
        //                }
        //                else if (serie.overview != null)
        //                {
        //                    tbxSinopse.Text = serie.overview;
        //                }
        //                else
        //                {
        //                    tbxSinopse.Text = "Sinopse não encontrada.";
        //                };
        //            }
        //            else if (serie.overview != null)
        //            {
        //                tbxSinopse.Text = serie.overview;
        //            }
        //            else
        //            {
        //                tbxSinopse.Text = "Sinopse não encontrada.";
        //            };

        //            if (settings.pref_PastaSeries != "")
        //            {
        //                tbxPasta.Text = System.IO.Path.Combine(settings.pref_PastaSeries, Helper.RetirarCaracteresInvalidos(serie.title));
        //            }
        //        };
        //        if (workerAtualizarCampos.IsBusy == false)
        //        {
        //            workerAtualizarCampos.RunWorkerAsync();
        //        }
        //        else
        //        {
        //            workerAtualizarCampos.CancelAsync();
        //            workerAtualizarCampos.RunWorkerAsync();
        //        }
        //    }
        //    else if (conteudo == Helper.Conteudo.Filme)
        //    {
        //        BackgroundWorker workerAtualizarCampos = new BackgroundWorker();
        //        workerAtualizarCampos.DoWork += (s, e) =>
        //        {
        //            this.Dispatcher.Invoke((Action)(() => { filme = Helper.API_GetFilmeInfo(slugTrakt); }));
        //        };

        //        workerAtualizarCampos.RunWorkerCompleted += (s, e) =>
        //        {
        //            if (filme.images.poster.thumb != null)
        //            {
        //                BitmapImage bmpPoster = new BitmapImage();
        //                bmpPoster.BeginInit();
        //                bmpPoster.UriSource = new Uri(filme.images.poster.thumb);
        //                bmpPoster.EndInit();

        //                imgPoster.Source = bmpPoster;
        //            }
        //            else if (filme.images.poster.medium != null)
        //            {
        //                BitmapImage bmpPoster = new BitmapImage();
        //                bmpPoster.BeginInit();
        //                bmpPoster.UriSource = new Uri(filme.images.poster.medium);
        //                bmpPoster.EndInit();

        //                imgPoster.Source = bmpPoster;
        //            }
        //            else if (filme.images.poster.full != null)
        //            {
        //                BitmapImage bmpPoster = new BitmapImage();
        //                bmpPoster.BeginInit();
        //                bmpPoster.UriSource = new Uri(filme.images.poster.full);
        //                bmpPoster.EndInit();

        //                imgPoster.Source = bmpPoster;
        //            }

        //            if (filme.images.banner.full != null)
        //            {
        //                BitmapImage bmpBanner = new BitmapImage();
        //                bmpBanner.BeginInit();
        //                bmpBanner.UriSource = new Uri(filme.images.banner.full);
        //                bmpBanner.EndInit();

        //                imgBanner.Source = bmpBanner;
        //            } // TODO Colocar imagem padrão caso não tenha banner/poster.

        //            if (filme.available_translations.Contains(settings.pref_IdiomaPesquisa))
        //            {
        //                filmeTraduzido = Helper.API_GetFilmeSinopse(slugTrakt);
        //                if (filmeTraduzido.overview != null)
        //                {
        //                    tbxSinopse.Text = filmeTraduzido.overview;
        //                }
        //                else if (filme.overview != null)
        //                {
        //                    tbxSinopse.Text = filme.overview;
        //                }
        //                else
        //                {
        //                    tbxSinopse.Text = "Sinopse não encontrada.";
        //                };
        //            }
        //            else if (filme.overview != null)
        //            {
        //                tbxSinopse.Text = filme.overview;
        //            }
        //            else
        //            {
        //                tbxSinopse.Text = "Sinopse não encontrada.";
        //            };

        //            if (settings.pref_PastaFilmes != "")
        //            {
        //                tbxPasta.Text = System.IO.Path.Combine(settings.pref_PastaFilmes, Helper.RetirarCaracteresInvalidos(filme.title));
        //            }
        //        };
        //        if (workerAtualizarCampos.IsBusy == false)
        //        {
        //            workerAtualizarCampos.RunWorkerAsync();
        //        }
        //        else
        //        {
        //            workerAtualizarCampos.CancelAsync();
        //            workerAtualizarCampos.RunWorkerAsync();
        //        }
        //    }
        //}

        #endregion { OLD AtualizarInformações }

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
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tbxPasta.Text = dialog.SelectedPath;
            }
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (tbxPasta.Text == string.Empty || cboListaConteudo.SelectedIndex == 0)
            {
                MessageBox.Show("Favor preencher todos os campos antes de salvar.");
            }
            else if (Conteudo == Helper.TipoConteudo.show)
            {
                Serie.folderPath = tbxPasta.Text;
                DatabaseHelper.adicionarSerie(Serie);
                this.DialogResult = true;
                this.Close();
            }
            else if (Conteudo == Helper.TipoConteudo.movie)
            {
                Filme.folderPath = tbxPasta.Text;
                DatabaseHelper.adicionarFilme(Filme);
                this.DialogResult = true;
                this.Close();
            }
        }

        private void cboListaConteudo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Conteudo == Helper.TipoConteudo.show || Conteudo == Helper.TipoConteudo.anime)
            {
                if ((sender as ComboBox).SelectedIndex != 0)
                {
                    AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.slug);
                }
            }
            else if (Conteudo == Helper.TipoConteudo.movie)
            {
                if ((sender as ComboBox).SelectedIndex != 0)
                {
                    AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].movie.ids.slug);
                }
            }
        }
    }
}