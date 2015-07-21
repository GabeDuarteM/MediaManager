using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
        public Filme Filme = new Filme();
        public Serie Serie = new Serie();
        public Helper.TipoConteudo TipoConteudo = new Helper.TipoConteudo();
        private ConteudoGrid Conteudo;
        private Filme FilmeTraduzido = new Filme();
        private List<Search> ResultPesquisa = new List<Search>();
        private Serie SerieTraduzida = new Serie();
        private Properties.Settings Settings = Properties.Settings.Default;

        public frmAdicionarConteudo(Helper.TipoConteudo tipoConteudo)
        {
            InitializeComponent();

            InputMessageBox inputMessageBox = new InputMessageBox(inputType.AdicionarConteudo);
            inputMessageBox.ShowDialog();

            if (inputMessageBox.DialogResult == true)
            {
                AdicionarConteudoViewModel = new ViewModel.AdicionarConteudoViewModel(inputMessageBox.InputVM.Properties.InputText, tipoConteudo);
            }

            DataContext = AdicionarConteudoViewModel;
        }

        public frmAdicionarConteudo(Helper.TipoConteudo tipoConteudo, List<Search> resultPesquisa)
        {
            InitializeComponent();

            //AdicionarConteudoViewModel = new AdicionarConteudoViewModel(resultPesquisa[0].ToVideo(), tipoConteudo);

            //ResultPesquisa = resultPesquisa;
            //TipoConteudo = tipoConteudo;
            if (tipoConteudo == Helper.TipoConteudo.show || tipoConteudo == Helper.TipoConteudo.anime)
            {
                foreach (var item in resultPesquisa)
                {
                    cboListaConteudo.Items.Add(item.show.title);
                }

                if (ResultPesquisa.Count > 0)
                    cboListaConteudo.SelectedIndex = 1;

                cboListaConteudo.Items.Add("Busca personalizada...");

                cboListaConteudo.SelectionChanged += cboListaConteudo_SelectionChanged;
                AtualizarInformacoesAsync(resultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.slug);
                tbxPasta.Text = Serie.FolderPath;
            }
            else if (tipoConteudo == Helper.TipoConteudo.movie)
            {
                foreach (var item in resultPesquisa)
                {
                    cboListaConteudo.Items.Add(item.movie.title);
                }

                if (ResultPesquisa.Count > 0)
                    cboListaConteudo.SelectedIndex = 1;

                cboListaConteudo.Items.Add("Busca personalizada...");

                cboListaConteudo.SelectionChanged += cboListaConteudo_SelectionChanged;
                AtualizarInformacoesAsync(resultPesquisa[cboListaConteudo.SelectedIndex - 1].movie.ids.slug);
                tbxPasta.Text = Filme.FolderPath;
            }
        }

        public frmAdicionarConteudo(Helper.TipoConteudo tipoConteudo, int IdBanco)
        {
            InitializeComponent();

            AdicionarConteudoViewModel adicionarConteudoViewModel = new AdicionarConteudoViewModel(IdBanco, tipoConteudo);

            DataContext = adicionarConteudoViewModel;
        }

        public frmAdicionarConteudo(ConteudoGrid conteudo)
        {
            InitializeComponent();

            Conteudo = conteudo;

            btnSalvar.Content = "Alterar";

            switch (conteudo.Tipo)
            {
                case "Show":
                    TipoConteudo = Helper.TipoConteudo.show;
                    Serie.Title = conteudo.Nome;
                    Serie.FolderPath = conteudo.Pasta;
                    PreencherCombo();
                    break;

                case "Anime":
                    TipoConteudo = Helper.TipoConteudo.anime;
                    Serie.Title = conteudo.Nome;
                    Serie.FolderPath = conteudo.Pasta;
                    PreencherCombo();
                    break;

                case "Filme":
                    TipoConteudo = Helper.TipoConteudo.movie;
                    Filme.Title = conteudo.Nome;
                    Filme.FolderPath = conteudo.Pasta;
                    PreencherCombo();
                    break;

                default:
                    break;
            }
        }

        public AdicionarConteudoViewModel AdicionarConteudoViewModel { get; set; }

        private async void AtualizarInformacoesAsync(string slugTrakt)
        {
            if (TipoConteudo == Helper.TipoConteudo.show || TipoConteudo == Helper.TipoConteudo.anime)
            {
                int idSerieTemp = Serie.IDSerie;
                Serie = await Helper.API_GetSerieInfoAsync(slugTrakt, TipoConteudo);
                Serie.IDSerie = idSerieTemp;

                //tbxPasta.Text = Serie.folderPath;

                if (Serie.Images.poster.thumb != null)
                {
                    BitmapImage bmpPoster = new BitmapImage();
                    bmpPoster.BeginInit();
                    bmpPoster.UriSource = new Uri(Serie.Images.poster.thumb);
                    bmpPoster.EndInit();

                    imgPoster.Source = bmpPoster;
                }
                else if (Serie.Images.poster.medium != null)
                {
                    BitmapImage bmpPoster = new BitmapImage();
                    bmpPoster.BeginInit();
                    bmpPoster.UriSource = new Uri(Serie.Images.poster.medium);
                    bmpPoster.EndInit();

                    imgPoster.Source = bmpPoster;
                }
                else if (Serie.Images.poster.full != null)
                {
                    BitmapImage bmpPoster = new BitmapImage();
                    bmpPoster.BeginInit();
                    bmpPoster.UriSource = new Uri(Serie.Images.poster.full);
                    bmpPoster.EndInit();

                    imgPoster.Source = bmpPoster;
                }
                else
                {
                    BitmapImage bmpPoster = new BitmapImage();
                    bmpPoster.BeginInit();
                    bmpPoster.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png");
                    bmpPoster.EndInit();

                    imgPoster.Source = bmpPoster;
                }

                if (Serie.Images.banner.full != null)
                {
                    BitmapImage bmpBanner = new BitmapImage();
                    bmpBanner.BeginInit();
                    bmpBanner.UriSource = new Uri(Serie.Images.banner.full);
                    bmpBanner.EndInit();

                    imgBanner.Source = bmpBanner;
                }
                else
                {
                    BitmapImage bmpBanner = new BitmapImage();
                    bmpBanner.BeginInit();
                    bmpBanner.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_BannerDefault.png");
                    bmpBanner.EndInit();

                    imgBanner.Source = bmpBanner;
                }

                if (Serie.AvailableTranslations != null && Serie.AvailableTranslations.Contains(Settings.pref_IdiomaPesquisa))
                {
                    SerieTraduzida = await Helper.API_GetSerieSinopseAsync(slugTrakt);
                    if (SerieTraduzida.Overview != null)
                    {
                        tbxSinopse.Text = SerieTraduzida.Overview;
                    }
                }
                else if (Serie.Overview != null)
                {
                    tbxSinopse.Text = Serie.Overview;
                }
                else
                {
                    tbxSinopse.Text = "Sinopse não encontrada.";
                }
            }
            else if (TipoConteudo == Helper.TipoConteudo.movie)
            {
                int idFilmeTemp = Filme.IDFilme;
                Filme = await Helper.API_GetFilmeInfoAsync(slugTrakt);
                Filme.IDFilme = idFilmeTemp;

                if (Filme.Images.poster.thumb != null)
                {
                    BitmapImage bmpPoster = new BitmapImage();
                    bmpPoster.BeginInit();
                    bmpPoster.UriSource = new Uri(Filme.Images.poster.thumb);
                    bmpPoster.EndInit();

                    imgPoster.Source = bmpPoster;
                }
                else if (Filme.Images.poster.medium != null)
                {
                    BitmapImage bmpPoster = new BitmapImage();
                    bmpPoster.BeginInit();
                    bmpPoster.UriSource = new Uri(Filme.Images.poster.medium);
                    bmpPoster.EndInit();

                    imgPoster.Source = bmpPoster;
                }
                else if (Filme.Images.poster.full != null)
                {
                    BitmapImage bmpPoster = new BitmapImage();
                    bmpPoster.BeginInit();
                    bmpPoster.UriSource = new Uri(Filme.Images.poster.full);
                    bmpPoster.EndInit();

                    imgPoster.Source = bmpPoster;
                }
                else
                {
                    BitmapImage bmpPoster = new BitmapImage();
                    bmpPoster.BeginInit();
                    bmpPoster.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png");
                    bmpPoster.EndInit();

                    imgPoster.Source = bmpPoster;
                }

                if (Filme.Images.banner.full != null)
                {
                    BitmapImage bmpBanner = new BitmapImage();
                    bmpBanner.BeginInit();
                    bmpBanner.UriSource = new Uri(Filme.Images.banner.full);
                    bmpBanner.EndInit();

                    imgBanner.Source = bmpBanner;
                }
                else
                {
                    BitmapImage bmpBanner = new BitmapImage();
                    bmpBanner.BeginInit();
                    bmpBanner.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_BannerDefault.png");
                    bmpBanner.EndInit();

                    imgBanner.Source = bmpBanner;
                }

                if (Filme.AvailableTranslations != null && Filme.AvailableTranslations.Contains(Settings.pref_IdiomaPesquisa))
                {
                    FilmeTraduzido = await Helper.API_GetFilmeSinopseAsync(slugTrakt);
                    if (FilmeTraduzido.Overview != null)
                    {
                        tbxSinopse.Text = FilmeTraduzido.Overview;
                    }
                }
                else if (Filme.Overview != null)
                {
                    tbxSinopse.Text = Filme.Overview;
                }
                else
                {
                    tbxSinopse.Text = "Sinopse não encontrada.";
                };

                if (Settings.pref_PastaFilmes != "")
                {
                    //tbxPasta.Text = Filme.folderPath;
                }
            }
        }

        private async void AtualizarInformacoesAsync(Serie serie)
        {
            tbxPasta.Text = serie.FolderPath;

            if (serie.Images.poster.thumb != null)
            {
                BitmapImage bmpPoster = new BitmapImage();
                bmpPoster.BeginInit();
                bmpPoster.UriSource = new Uri(serie.Images.poster.thumb);
                bmpPoster.EndInit();

                imgPoster.Source = bmpPoster;
            }
            else if (serie.Images.poster.medium != null)
            {
                BitmapImage bmpPoster = new BitmapImage();
                bmpPoster.BeginInit();
                bmpPoster.UriSource = new Uri(serie.Images.poster.medium);
                bmpPoster.EndInit();

                imgPoster.Source = bmpPoster;
            }
            else if (serie.Images.poster.full != null)
            {
                BitmapImage bmpPoster = new BitmapImage();
                bmpPoster.BeginInit();
                bmpPoster.UriSource = new Uri(serie.Images.poster.full);
                bmpPoster.EndInit();

                imgPoster.Source = bmpPoster;
            }
            else
            {
                BitmapImage bmpPoster = new BitmapImage();
                bmpPoster.BeginInit();
                bmpPoster.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png");
                bmpPoster.EndInit();

                imgPoster.Source = bmpPoster;
            }

            if (serie.Images.banner.full != null)
            {
                BitmapImage bmpBanner = new BitmapImage();
                bmpBanner.BeginInit();
                bmpBanner.UriSource = new Uri(serie.Images.banner.full);
                bmpBanner.EndInit();

                imgBanner.Source = bmpBanner;
            }
            else
            {
                BitmapImage bmpBanner = new BitmapImage();
                bmpBanner.BeginInit();
                bmpBanner.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_BannerDefault.png");
                bmpBanner.EndInit();

                imgBanner.Source = bmpBanner;
            }

            if (serie.AvailableTranslations != null && serie.AvailableTranslations.Contains(Settings.pref_IdiomaPesquisa))
            {
                SerieTraduzida = await Helper.API_GetSerieSinopseAsync(serie.Ids.slug);
                if (SerieTraduzida.Overview != null && SerieTraduzida.Overview != "")
                {
                    tbxSinopse.Text = SerieTraduzida.Overview;
                }
            }
            else if (SerieTraduzida.Overview == "" || serie.Overview != null)
            {
                tbxSinopse.Text = serie.Overview;
            }
            else
            {
                tbxSinopse.Text = "Sinopse não encontrada.";
            };
        }

        private async void AtualizarInformacoesAsync(Filme filme)
        {
            tbxPasta.Text = filme.FolderPath;

            if (filme.Images.poster.thumb != null)
            {
                BitmapImage bmpPoster = new BitmapImage();
                bmpPoster.BeginInit();
                bmpPoster.UriSource = new Uri(filme.Images.poster.thumb);
                bmpPoster.EndInit();

                imgPoster.Source = bmpPoster;
            }
            else if (filme.Images.poster.medium != null)
            {
                BitmapImage bmpPoster = new BitmapImage();
                bmpPoster.BeginInit();
                bmpPoster.UriSource = new Uri(filme.Images.poster.medium);
                bmpPoster.EndInit();

                imgPoster.Source = bmpPoster;
            }
            else if (filme.Images.poster.full != null)
            {
                BitmapImage bmpPoster = new BitmapImage();
                bmpPoster.BeginInit();
                bmpPoster.UriSource = new Uri(filme.Images.poster.full);
                bmpPoster.EndInit();

                imgPoster.Source = bmpPoster;
            }
            else
            {
                BitmapImage bmpPoster = new BitmapImage();
                bmpPoster.BeginInit();
                bmpPoster.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_PosterDefault.png");
                bmpPoster.EndInit();

                imgPoster.Source = bmpPoster;
            }

            if (filme.Images.banner.full != null)
            {
                BitmapImage bmpBanner = new BitmapImage();
                bmpBanner.BeginInit();
                bmpBanner.UriSource = new Uri(filme.Images.banner.full);
                bmpBanner.EndInit();

                imgBanner.Source = bmpBanner;
            }
            else
            {
                BitmapImage bmpBanner = new BitmapImage();
                bmpBanner.BeginInit();
                bmpBanner.UriSource = new Uri("pack://application:,,,/MediaManager;component/Resources/IMG_BannerDefault.png");
                bmpBanner.EndInit();

                imgBanner.Source = bmpBanner;
            }

            if (filme.AvailableTranslations != null && filme.AvailableTranslations.Contains(Settings.pref_IdiomaPesquisa))
            {
                FilmeTraduzido = await Helper.API_GetFilmeSinopseAsync(filme.Ids.slug);
                if (FilmeTraduzido.Overview != null)
                {
                    tbxSinopse.Text = FilmeTraduzido.Overview;
                }
            }
            else if (filme.Overview != null)
            {
                tbxSinopse.Text = filme.Overview;
            }
            else
            {
                tbxSinopse.Text = "Sinopse não encontrada.";
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
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tbxPasta.Text = dialog.SelectedPath;
            }
        }

        private async void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (tbxPasta.Text == "" || cboListaConteudo.SelectedIndex == 0)
            {
                MessageBox.Show("Favor preencher todos os campos antes de salvar.");
            }
            else if (TipoConteudo == Helper.TipoConteudo.show)
            {
                Serie.FolderPath = tbxPasta.Text;
                if (Conteudo != null)
                {
                    DialogResult = true;
                    Close();
                }
                else if (Serie.IDSerie == 0)
                {
                    if (await DatabaseHelper.AddSerieAsync(Serie))
                    {
                        DialogResult = true;
                        Close();
                    }
                }
                else
                {
                    if (await DatabaseHelper.UpdateSerieAsync(Serie))
                    {
                        DialogResult = true;
                        Close();
                    }
                }
            }
            else if (TipoConteudo == Helper.TipoConteudo.movie)
            {
                Filme.FolderPath = tbxPasta.Text;
                if (Conteudo != null)
                {
                    DialogResult = true;
                    Close();
                }
                else if (Filme.IDFilme == 0)
                {
                    if (await DatabaseHelper.AddFilmeAsync(Filme))
                    {
                        DialogResult = true;
                        Close();
                    }
                }
                else
                {
                    if (await DatabaseHelper.UpdateFilmeAsync(Filme))
                    {
                        DialogResult = true;
                        Close();
                    }
                }
            }
            else if (TipoConteudo == Helper.TipoConteudo.anime)
            {
                Serie.FolderPath = tbxPasta.Text;
                if (Conteudo != null)
                {
                    DialogResult = true;
                    Close();
                }
                else if (Serie.IDSerie == 0)
                {
                    if (await DatabaseHelper.AddAnimeAsync(Serie))
                    {
                        DialogResult = true;
                        Close();
                    }
                }
                else
                {
                    if (await DatabaseHelper.UpdateAnimeAsync(Serie))
                    {
                        DialogResult = true;
                        Close();
                    }
                }
            }
        }

        private void cboListaConteudo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TipoConteudo == Helper.TipoConteudo.show || TipoConteudo == Helper.TipoConteudo.anime)
            {
                if ((sender as ComboBox).SelectedItem.ToString() == "Busca personalizada...")
                {
                    frmPopupPesquisa frmPopupPesquisa = new frmPopupPesquisa(TipoConteudo, true);
                    frmPopupPesquisa.ShowDialog();
                    if (frmPopupPesquisa.DialogResult == true)
                    {
                        cboListaConteudo.SelectionChanged -= cboListaConteudo_SelectionChanged;
                        cboListaConteudo.SelectedIndex = -1;
                        cboListaConteudo.Items.Clear();
                        cboListaConteudo.Items.Add("Selecione");
                        ResultPesquisa = frmPopupPesquisa.ResultPesquisa;
                        foreach (var item in ResultPesquisa)
                        {
                            cboListaConteudo.Items.Add(item.show.title);
                        }
                        cboListaConteudo.Items.Add("Busca personalizada...");

                        cboListaConteudo.SelectedIndex = 1;
                        cboListaConteudo.SelectionChanged += cboListaConteudo_SelectionChanged;
                        AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.slug);
                    }
                }
                else if ((sender as ComboBox).SelectedIndex != 0)
                {
                    AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.slug);
                }
            }
            else if (TipoConteudo == Helper.TipoConteudo.movie)
            {
                if ((sender as ComboBox).SelectedItem.ToString() == "Busca personalizada...")
                {
                    frmPopupPesquisa frmPopupPesquisa = new frmPopupPesquisa(TipoConteudo, true);
                    frmPopupPesquisa.ShowDialog();
                    if (frmPopupPesquisa.DialogResult == true)
                    {
                        cboListaConteudo.SelectionChanged -= cboListaConteudo_SelectionChanged;
                        cboListaConteudo.SelectedIndex = -1;
                        cboListaConteudo.Items.Clear();
                        cboListaConteudo.Items.Add("Selecione");
                        ResultPesquisa = frmPopupPesquisa.ResultPesquisa;
                        foreach (var item in ResultPesquisa)
                        {
                            cboListaConteudo.Items.Add(item.movie.title);
                        }
                        cboListaConteudo.Items.Add("Busca personalizada...");

                        cboListaConteudo.SelectedIndex = 1;
                        cboListaConteudo.SelectionChanged += cboListaConteudo_SelectionChanged;
                        AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].movie.ids.slug);
                    }
                }
                if ((sender as ComboBox).SelectedIndex != 0)
                {
                    AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].movie.ids.slug);
                }
            }
        }

        private async void PreencherCombo()
        {
            switch (TipoConteudo)
            {
                case Helper.TipoConteudo.movie:
                    ResultPesquisa = await Helper.API_PesquisarConteudoAsync(Path.GetFileName(Filme.FolderPath), TipoConteudo.ToString());
                    if (ResultPesquisa.Count > 0)
                    {
                        foreach (var item in ResultPesquisa)
                        {
                            cboListaConteudo.Items.Add(item.movie.title);
                        }

                        cboListaConteudo.Items.Add("Busca personalizada...");

                        cboListaConteudo.SelectedItem = Filme.Title;
                        if (Conteudo == null)
                            AtualizarInformacoesAsync(Filme);
                        else
                        {
                            AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].movie.ids.slug);
                            tbxPasta.Text = Filme.FolderPath;
                        }
                    }
                    else
                    {
                        cboListaConteudo.Items.Add(Filme.Title);
                        cboListaConteudo.SelectedIndex = 1;
                        cboListaConteudo.Items.Add("Busca personalizada...");
                    }
                    break;

                case Helper.TipoConteudo.show:
                    ResultPesquisa = await Helper.API_PesquisarConteudoAsync(Path.GetFileName(Serie.FolderPath), TipoConteudo.ToString());
                    if (ResultPesquisa.Count > 0)
                    {
                        foreach (var item in ResultPesquisa)
                        {
                            cboListaConteudo.Items.Add(item.show.title);
                        }

                        cboListaConteudo.Items.Add("Busca personalizada...");

                        cboListaConteudo.SelectedItem = Serie.Title;
                        if (Conteudo == null)
                            AtualizarInformacoesAsync(Serie);
                        else
                        {
                            AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.slug);
                            tbxPasta.Text = Serie.FolderPath;
                        }
                    }
                    else
                    {
                        cboListaConteudo.Items.Add(Serie.Title);
                        cboListaConteudo.SelectedIndex = 1;
                        cboListaConteudo.Items.Add("Busca personalizada...");
                    }
                    break;

                case Helper.TipoConteudo.anime:
                    ResultPesquisa = await Helper.API_PesquisarConteudoAsync(Path.GetFileName(Serie.FolderPath), TipoConteudo.ToString());
                    if (ResultPesquisa.Count > 0)
                    {
                        foreach (var item in ResultPesquisa)
                        {
                            cboListaConteudo.Items.Add(item.show.title);
                        }

                        cboListaConteudo.Items.Add("Busca personalizada...");

                        cboListaConteudo.SelectedItem = Serie.Title;
                        if (Conteudo == null)
                            AtualizarInformacoesAsync(Serie);
                        else
                        {
                            AtualizarInformacoesAsync(ResultPesquisa[cboListaConteudo.SelectedIndex - 1].show.ids.slug);
                            tbxPasta.Text = Serie.FolderPath;
                        }
                    }
                    else
                    {
                        cboListaConteudo.Items.Add(Serie.Title);
                        cboListaConteudo.SelectedIndex = 1;
                        cboListaConteudo.Items.Add("Busca personalizada...");
                    }
                    break;

                default:
                    break;
            }

            cboListaConteudo.SelectionChanged += cboListaConteudo_SelectionChanged;
        }

        #region [ OLD AtualizarInformações ]

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

        #endregion [ OLD AtualizarInformações ]
    }
}