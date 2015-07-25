using System;
using System.Collections.Generic;
using System.Windows;
using ConfigurableInputMessageBox;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.ViewModel;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for frmAdicionarSerie.xaml
    /// </summary>
    public partial class frmAdicionarConteudo : Window
    {
        public AdicionarConteudoViewModel AdicionarConteudoViewModel { get; set; }

        public bool IsEdicao { get; set; }

        public bool IsProcurarConteudo { get; set; }

        public frmAdicionarConteudo(Helper.Enums.TipoConteudo tipoConteudo)
        {
            InitializeComponent();

            InputMessageBox inputMessageBox = new InputMessageBox(inputType.AdicionarConteudo);
            inputMessageBox.ShowDialog();

            if (inputMessageBox.DialogResult == true)
            {
                AdicionarConteudoViewModel = new AdicionarConteudoViewModel(inputMessageBox.InputViewModel.Properties.InputText, tipoConteudo);
            }

            DataContext = AdicionarConteudoViewModel;
        }

        public frmAdicionarConteudo(Helper.Enums.TipoConteudo tipoConteudo, Video video)
        {
            InitializeComponent();

            AdicionarConteudoViewModel = new AdicionarConteudoViewModel(video, tipoConteudo);

            DataContext = AdicionarConteudoViewModel;
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            frmConfigConteudo frmConfigConteudo = new frmConfigConteudo();
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

        private async void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (AdicionarConteudoViewModel.Video == null || AdicionarConteudoViewModel.Video.FolderPath == null)
            {
                MessageBox.Show("Favor preencher todos os campos antes de salvar.");
                return;
            }
            else if (IsProcurarConteudo)
            {
                DialogResult = true;
            }
            else
            {
                switch (AdicionarConteudoViewModel.TipoConteudo)
                {
                    case Helper.Enums.TipoConteudo.movie:
                        Filme filme = await Helper.API_GetFilmeInfoAsync(AdicionarConteudoViewModel.Video.Ids.slug);
                        filme.FolderPath = AdicionarConteudoViewModel.Video.FolderPath;

                        if (IsEdicao)
                        {
                            filme.ID = AdicionarConteudoViewModel.Video.ID;
                            try { await DatabaseHelper.UpdateFilmeAsync(filme); }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message + " Detalhes: " + ex.InnerException);
                                DialogResult = false;
                            }
                        }
                        else
                        {
                            try { await DatabaseHelper.AddFilmeAsync(filme); }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message + " Detalhes: " + ex.InnerException);
                                DialogResult = false;
                            }
                        }
                        break;

                    case Helper.Enums.TipoConteudo.show:
                        {
                            Serie serie = await Helper.API_GetSerieInfoAsync(AdicionarConteudoViewModel.Video.Ids.slug, Helper.Enums.TipoConteudo.show);
                            serie.FolderPath = AdicionarConteudoViewModel.Video.FolderPath;

                            if (IsEdicao)
                            {
                                serie.ID = AdicionarConteudoViewModel.Video.ID;
                                try { await DatabaseHelper.UpdateSerieAsync(serie); }
                                catch (Exception ex)
                                {
                                    Console.Write(ex.Message + " Detalhes: " + ex.InnerException);
                                    DialogResult = false;
                                }
                            }
                            else
                            {
                                try { await DatabaseHelper.AddSerieAsync(serie); }
                                catch (Exception ex)
                                {
                                    Console.Write(ex.Message + " Detalhes: " + ex.InnerException);
                                    DialogResult = false;
                                }
                            }
                            break;
                        }

                    case Helper.Enums.TipoConteudo.anime:
                        {
                            Serie anime = await Helper.API_GetSerieInfoAsync(AdicionarConteudoViewModel.Video.Ids.slug, Helper.Enums.TipoConteudo.anime);
                            anime.FolderPath = AdicionarConteudoViewModel.Video.FolderPath;

                            if (IsEdicao)
                            {
                                anime.ID = AdicionarConteudoViewModel.Video.ID;
                                try { await DatabaseHelper.UpdateAnimeAsync(anime); }
                                catch (Exception ex)
                                {
                                    Console.Write(ex.Message + " Detalhes: " + ex.InnerException);
                                    DialogResult = false;
                                }
                            }
                            else
                            {
                                try { await DatabaseHelper.AddAnimeAsync(anime); }
                                catch (Exception ex)
                                {
                                    Console.Write(ex.Message + " Detalhes: " + ex.InnerException);
                                    DialogResult = false;
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
                DialogResult = true;
                Close();
            }
        }

        #region [ Métodos antigos ]

        //private async void AtualizarInformacoesAsync(string slugTrakt)
        //{
        //    if (TipoConteudo == Helper.Enums.TipoConteudo.show || TipoConteudo == Helper.Enums.TipoConteudo.anime)
        //    {
        //        int idSerieTemp = Serie.IDSerie;
        //        Serie = await Helper.API_GetSerieInfoAsync(slugTrakt, TipoConteudo);
        //        Serie.IDSerie = idSerieTemp;

        //        //tbxPasta.Text = Serie.folderPath;

        //        if (Serie.Images.poster.thumb != null)
        //        {
        //            BitmapImage bmpPoster = new BitmapImage();
        //            bmpPoster.BeginInit();
        //            bmpPoster.UriSource = new Uri(Serie.Images.poster.thumb);
        //            bmpPoster.EndInit();

        //            imgPoster.Source = bmpPoster;
        //        }
        //        else if (Serie.Images.poster.medium != null)
        //        {
        //            BitmapImage bmpPoster = new BitmapImage();
        //            bmpPoster.BeginInit();
        //            bmpPoster.UriSource = new Uri(Serie.Images.poster.medium);
        //            bmpPoster.EndInit();

        //            imgPoster.Source = bmpPoster;
        //        }
        //        else if (Serie.Images.poster.full != null)
        //        {
        //            BitmapImage bmpPoster = new BitmapImage();
        //            bmpPoster.BeginInit();
        //            bmpPoster.UriSource = new Uri(Serie.Images.poster.full);
        //            bmpPoster.EndInit();

        //            imgPoster.Source = bmpPoster;
        //        }
        //        else
        //        {
        //            BitmapImage bmpPoster = new BitmapImage();
        //            bmpPoster.BeginInit();
        //            bmpPoster.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png");
        //            bmpPoster.EndInit();

        //            imgPoster.Source = bmpPoster;
        //        }

        //        if (Serie.Images.banner.full != null)
        //        {
        //            BitmapImage bmpBanner = new BitmapImage();
        //            bmpBanner.BeginInit();
        //            bmpBanner.UriSource = new Uri(Serie.Images.banner.full);
        //            bmpBanner.EndInit();

        //            imgBanner.Source = bmpBanner;
        //        }
        //        else
        //        {
        //            BitmapImage bmpBanner = new BitmapImage();
        //            bmpBanner.BeginInit();
        //            bmpBanner.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_BannerDefault.png");
        //            bmpBanner.EndInit();

        //            imgBanner.Source = bmpBanner;
        //        }

        //        if (Serie.AvailableTranslations != null && Serie.AvailableTranslations.Contains(Settings.pref_IdiomaPesquisa))
        //        {
        //            SerieTraduzida = await Helper.API_GetSerieSinopseAsync(slugTrakt);
        //            if (SerieTraduzida.Overview != null)
        //            {
        //                tbxSinopse.Text = SerieTraduzida.Overview;
        //            }
        //        }
        //        else if (Serie.Overview != null)
        //        {
        //            tbxSinopse.Text = Serie.Overview;
        //        }
        //        else
        //        {
        //            tbxSinopse.Text = "Sinopse não encontrada.";
        //        }
        //    }
        //    else if (TipoConteudo == Helper.Enums.TipoConteudo.movie)
        //    {
        //        int idFilmeTemp = Filme.IDFilme;
        //        Filme = await Helper.API_GetFilmeInfoAsync(slugTrakt);
        //        Filme.IDFilme = idFilmeTemp;

        //        if (Filme.Images.poster.thumb != null)
        //        {
        //            BitmapImage bmpPoster = new BitmapImage();
        //            bmpPoster.BeginInit();
        //            bmpPoster.UriSource = new Uri(Filme.Images.poster.thumb);
        //            bmpPoster.EndInit();

        //            imgPoster.Source = bmpPoster;
        //        }
        //        else if (Filme.Images.poster.medium != null)
        //        {
        //            BitmapImage bmpPoster = new BitmapImage();
        //            bmpPoster.BeginInit();
        //            bmpPoster.UriSource = new Uri(Filme.Images.poster.medium);
        //            bmpPoster.EndInit();

        //            imgPoster.Source = bmpPoster;
        //        }
        //        else if (Filme.Images.poster.full != null)
        //        {
        //            BitmapImage bmpPoster = new BitmapImage();
        //            bmpPoster.BeginInit();
        //            bmpPoster.UriSource = new Uri(Filme.Images.poster.full);
        //            bmpPoster.EndInit();

        //            imgPoster.Source = bmpPoster;
        //        }
        //        else
        //        {
        //            BitmapImage bmpPoster = new BitmapImage();
        //            bmpPoster.BeginInit();
        //            bmpPoster.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png");
        //            bmpPoster.EndInit();

        //            imgPoster.Source = bmpPoster;
        //        }

        //        if (Filme.Images.banner.full != null)
        //        {
        //            BitmapImage bmpBanner = new BitmapImage();
        //            bmpBanner.BeginInit();
        //            bmpBanner.UriSource = new Uri(Filme.Images.banner.full);
        //            bmpBanner.EndInit();

        //            imgBanner.Source = bmpBanner;
        //        }
        //        else
        //        {
        //            BitmapImage bmpBanner = new BitmapImage();
        //            bmpBanner.BeginInit();
        //            bmpBanner.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_BannerDefault.png");
        //            bmpBanner.EndInit();

        //            imgBanner.Source = bmpBanner;
        //        }

        //        if (Filme.AvailableTranslations != null && Filme.AvailableTranslations.Contains(Settings.pref_IdiomaPesquisa))
        //        {
        //            FilmeTraduzido = await Helper.API_GetFilmeSinopseAsync(slugTrakt);
        //            if (FilmeTraduzido.Overview != null)
        //            {
        //                tbxSinopse.Text = FilmeTraduzido.Overview;
        //            }
        //        }
        //        else if (Filme.Overview != null)
        //        {
        //            tbxSinopse.Text = Filme.Overview;
        //        }
        //        else
        //        {
        //            tbxSinopse.Text = "Sinopse não encontrada.";
        //        };

        //        if (Settings.pref_PastaFilmes != "")
        //        {
        //            //tbxPasta.Text = Filme.folderPath;
        //        }
        //    }
        //}

        //private async void AtualizarInformacoesAsync(Serie serie)
        //{
        //    tbxPasta.Text = serie.FolderPath;

        //    if (serie.Images.poster.thumb != null)
        //    {
        //        BitmapImage bmpPoster = new BitmapImage();
        //        bmpPoster.BeginInit();
        //        bmpPoster.UriSource = new Uri(serie.Images.poster.thumb);
        //        bmpPoster.EndInit();

        //        imgPoster.Source = bmpPoster;
        //    }
        //    else if (serie.Images.poster.medium != null)
        //    {
        //        BitmapImage bmpPoster = new BitmapImage();
        //        bmpPoster.BeginInit();
        //        bmpPoster.UriSource = new Uri(serie.Images.poster.medium);
        //        bmpPoster.EndInit();

        //        imgPoster.Source = bmpPoster;
        //    }
        //    else if (serie.Images.poster.full != null)
        //    {
        //        BitmapImage bmpPoster = new BitmapImage();
        //        bmpPoster.BeginInit();
        //        bmpPoster.UriSource = new Uri(serie.Images.poster.full);
        //        bmpPoster.EndInit();

        //        imgPoster.Source = bmpPoster;
        //    }
        //    else
        //    {
        //        BitmapImage bmpPoster = new BitmapImage();
        //        bmpPoster.BeginInit();
        //        bmpPoster.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png");
        //        bmpPoster.EndInit();

        //        imgPoster.Source = bmpPoster;
        //    }

        //    if (serie.Images.banner.full != null)
        //    {
        //        BitmapImage bmpBanner = new BitmapImage();
        //        bmpBanner.BeginInit();
        //        bmpBanner.UriSource = new Uri(serie.Images.banner.full);
        //        bmpBanner.EndInit();

        //        imgBanner.Source = bmpBanner;
        //    }
        //    else
        //    {
        //        BitmapImage bmpBanner = new BitmapImage();
        //        bmpBanner.BeginInit();
        //        bmpBanner.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_BannerDefault.png");
        //        bmpBanner.EndInit();

        //        imgBanner.Source = bmpBanner;
        //    }

        //    if (serie.AvailableTranslations != null && serie.AvailableTranslations.Contains(Settings.pref_IdiomaPesquisa))
        //    {
        //        SerieTraduzida = await Helper.API_GetSerieSinopseAsync(serie.Ids.slug);
        //        if (SerieTraduzida.Overview != null && SerieTraduzida.Overview != "")
        //        {
        //            tbxSinopse.Text = SerieTraduzida.Overview;
        //        }
        //    }
        //    else if (SerieTraduzida.Overview == "" || serie.Overview != null)
        //    {
        //        tbxSinopse.Text = serie.Overview;
        //    }
        //    else
        //    {
        //        tbxSinopse.Text = "Sinopse não encontrada.";
        //    };
        //}

        //private async void AtualizarInformacoesAsync(Filme filme)
        //{
        //    tbxPasta.Text = filme.FolderPath;

        //    if (filme.Images.poster.thumb != null)
        //    {
        //        BitmapImage bmpPoster = new BitmapImage();
        //        bmpPoster.BeginInit();
        //        bmpPoster.UriSource = new Uri(filme.Images.poster.thumb);
        //        bmpPoster.EndInit();

        //        imgPoster.Source = bmpPoster;
        //    }
        //    else if (filme.Images.poster.medium != null)
        //    {
        //        BitmapImage bmpPoster = new BitmapImage();
        //        bmpPoster.BeginInit();
        //        bmpPoster.UriSource = new Uri(filme.Images.poster.medium);
        //        bmpPoster.EndInit();

        //        imgPoster.Source = bmpPoster;
        //    }
        //    else if (filme.Images.poster.full != null)
        //    {
        //        BitmapImage bmpPoster = new BitmapImage();
        //        bmpPoster.BeginInit();
        //        bmpPoster.UriSource = new Uri(filme.Images.poster.full);
        //        bmpPoster.EndInit();

        //        imgPoster.Source = bmpPoster;
        //    }
        //    else
        //    {
        //        BitmapImage bmpPoster = new BitmapImage();
        //        bmpPoster.BeginInit();
        //        bmpPoster.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png");
        //        bmpPoster.EndInit();

        //        imgPoster.Source = bmpPoster;
        //    }

        //    if (filme.Images.banner.full != null)
        //    {
        //        BitmapImage bmpBanner = new BitmapImage();
        //        bmpBanner.BeginInit();
        //        bmpBanner.UriSource = new Uri(filme.Images.banner.full);
        //        bmpBanner.EndInit();

        //        imgBanner.Source = bmpBanner;
        //    }
        //    else
        //    {
        //        BitmapImage bmpBanner = new BitmapImage();
        //        bmpBanner.BeginInit();
        //        bmpBanner.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_BannerDefault.png");
        //        bmpBanner.EndInit();

        //        imgBanner.Source = bmpBanner;
        //    }

        //    if (filme.AvailableTranslations != null && filme.AvailableTranslations.Contains(Settings.pref_IdiomaPesquisa))
        //    {
        //        FilmeTraduzido = await Helper.API_GetFilmeSinopseAsync(filme.Ids.slug);
        //        if (FilmeTraduzido.Overview != null)
        //        {
        //            tbxSinopse.Text = FilmeTraduzido.Overview;
        //        }
        //    }
        //    else if (filme.Overview != null)
        //    {
        //        tbxSinopse.Text = filme.Overview;
        //    }
        //    else
        //    {
        //        tbxSinopse.Text = "Sinopse não encontrada.";
        //    }
        //}

        //private void cboListaConteudo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (TipoConteudo == Helper.Enums.TipoConteudo.show || TipoConteudo == Helper.Enums.TipoConteudo.anime)
        //    {
        //        if ((sender as ComboBox).SelectedItem.ToString() == "Busca personalizada...")
        //        {
        //            frmPopupPesquisa frmPopupPesquisa = new frmPopupPesquisa(TipoConteudo, true);
        //            frmPopupPesquisa.ShowDialog();
        //            if (frmPopupPesquisa.DialogResult == true)
        //            {
        //                cboListaConteudo.SelectionChanged -= cboListaConteudo_SelectionChanged;
        //                cboListaConteudo.SelectedIndex = -1;
        //                cboListaConteudo.Items.Clear();
        //                cboListaConteudo.Items.Add("Selecione");
        //                ResultPesquisa = frmPopupPesquisa.ResultPesquisa;
        //                foreach (var item in ResultPesquisa)
        //                {
        //                    cboListaConteudo.Items.Add(item.show.title);
        //                }
        //                cboListaConteudo.Items.Add("Busca personalizada...");

        //                cboListaConteudo.SelectedIndex = 1;
        //                cboListaConteudo.SelectionChanged += cboListaConteudo_SelectionChanged;
        //                AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.slug);
        //            }
        //        }
        //        else if ((sender as ComboBox).SelectedIndex != 0)
        //        {
        //            AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.slug);
        //        }
        //    }
        //    else if (TipoConteudo == Helper.Enums.TipoConteudo.movie)
        //    {
        //        if ((sender as ComboBox).SelectedItem.ToString() == "Busca personalizada...")
        //        {
        //            frmPopupPesquisa frmPopupPesquisa = new frmPopupPesquisa(TipoConteudo, true);
        //            frmPopupPesquisa.ShowDialog();
        //            if (frmPopupPesquisa.DialogResult == true)
        //            {
        //                cboListaConteudo.SelectionChanged -= cboListaConteudo_SelectionChanged;
        //                cboListaConteudo.SelectedIndex = -1;
        //                cboListaConteudo.Items.Clear();
        //                cboListaConteudo.Items.Add("Selecione");
        //                ResultPesquisa = frmPopupPesquisa.ResultPesquisa;
        //                foreach (var item in ResultPesquisa)
        //                {
        //                    cboListaConteudo.Items.Add(item.movie.title);
        //                }
        //                cboListaConteudo.Items.Add("Busca personalizada...");

        //                cboListaConteudo.SelectedIndex = 1;
        //                cboListaConteudo.SelectionChanged += cboListaConteudo_SelectionChanged;
        //                AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].movie.ids.slug);
        //            }
        //        }
        //        if ((sender as ComboBox).SelectedIndex != 0)
        //        {
        //            AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].movie.ids.slug);
        //        }
        //    }
        ////}

        //private async void PreencherCombo()
        //{
        //    switch (TipoConteudo)
        //    {
        //        case Helper.Enums.TipoConteudo.movie:
        //            ResultPesquisa = await Helper.API_PesquisarConteudoAsync(Path.GetFileName(Filme.FolderPath), TipoConteudo.ToString());
        //            if (ResultPesquisa.Count > 0)
        //            {
        //                foreach (var item in ResultPesquisa)
        //                {
        //                    cboListaConteudo.Items.Add(item.movie.title);
        //                }

        //                cboListaConteudo.Items.Add("Busca personalizada...");

        //                cboListaConteudo.SelectedItem = Filme.Title;
        //                if (Conteudo == null)
        //                    AtualizarInformacoesAsync(Filme);
        //                else
        //                {
        //                    AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].movie.ids.slug);
        //                    tbxPasta.Text = Filme.FolderPath;
        //                }
        //            }
        //            else
        //            {
        //                cboListaConteudo.Items.Add(Filme.Title);
        //                cboListaConteudo.SelectedIndex = 1;
        //                cboListaConteudo.Items.Add("Busca personalizada...");
        //            }
        //            break;

        //        case Helper.Enums.TipoConteudo.show:
        //            ResultPesquisa = await Helper.API_PesquisarConteudoAsync(Path.GetFileName(Serie.FolderPath), TipoConteudo.ToString());
        //            if (ResultPesquisa.Count > 0)
        //            {
        //                foreach (var item in ResultPesquisa)
        //                {
        //                    cboListaConteudo.Items.Add(item.show.title);
        //                }

        //                cboListaConteudo.Items.Add("Busca personalizada...");

        //                cboListaConteudo.SelectedItem = Serie.Title;
        //                if (Conteudo == null)
        //                    AtualizarInformacoesAsync(Serie);
        //                else
        //                {
        //                    AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.slug);
        //                    tbxPasta.Text = Serie.FolderPath;
        //                }
        //            }
        //            else
        //            {
        //                cboListaConteudo.Items.Add(Serie.Title);
        //                cboListaConteudo.SelectedIndex = 1;
        //                cboListaConteudo.Items.Add("Busca personalizada...");
        //            }
        //            break;

        //        case Helper.Enums.TipoConteudo.anime:
        //            ResultPesquisa = await Helper.API_PesquisarConteudoAsync(Path.GetFileName(Serie.FolderPath), TipoConteudo.ToString());
        //            if (ResultPesquisa.Count > 0)
        //            {
        //                foreach (var item in ResultPesquisa)
        //                {
        //                    cboListaConteudo.Items.Add(item.show.title);
        //                }

        //                cboListaConteudo.Items.Add("Busca personalizada...");

        //                cboListaConteudo.SelectedItem = Serie.Title;
        //                if (Conteudo == null)
        //                    AtualizarInformacoesAsync(Serie);
        //                else
        //                {
        //                    AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.slug);
        //                    tbxPasta.Text = Serie.FolderPath;
        //                }
        //            }
        //            else
        //            {
        //                cboListaConteudo.Items.Add(Serie.Title);
        //                cboListaConteudo.SelectedIndex = 1;
        //                cboListaConteudo.Items.Add("Busca personalizada...");
        //            }
        //            break;

        //        default:
        //            break;
        //    }

        //    cboListaConteudo.SelectionChanged += cboListaConteudo_SelectionChanged;
        //}
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

        #endregion [ Métodos antigos ]
    }
}