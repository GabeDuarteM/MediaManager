using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using Autofac;
using MediaManager.Helpers;
using MediaManager.Model;
using MediaManager.Properties;
using MediaManager.Services;

namespace MediaManager.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<PosterViewModel> _lstAnimes;
        public ObservableCollection<PosterViewModel> lstAnimes { get { return _lstAnimes; } set { _lstAnimes = value; OnPropertyChanged(); } }

        private ObservableCollection<PosterViewModel> _lstFilmes;
        public ObservableCollection<PosterViewModel> lstFilmes { get { return _lstFilmes; } set { _lstFilmes = value; OnPropertyChanged(); } }

        private ObservableCollection<PosterViewModel> _lstSeries;
        public ObservableCollection<PosterViewModel> lstSeries { get { return _lstSeries; } set { _lstSeries = value; OnPropertyChanged(); } }

        public ObservableCollection<PosterViewModel> lstAnimesESeries
        {
            get
            {
                ObservableCollection<PosterViewModel> retorno = new ObservableCollection<PosterViewModel>();

                foreach (var anime in lstAnimes)
                    retorno.Add(anime);

                foreach (var serie in lstSeries)
                    retorno.Add(serie);

                return retorno;
            }
        }

        public static Dictionary<string, string> Argumentos { get; private set; }

        public Window Owner { get; set; }

        public MainViewModel(Window owner = null, ICollection<Serie> animes = null, ICollection<Serie> filmes = null, ICollection<Serie> series = null)
        {
            Owner = owner;
        }

        /// <summary>
        /// Retorna true caso haja arquivos a serem renomeados, para que o resto da aplicação não seja carregada.
        /// </summary>
        /// <returns></returns>
        public bool TratarArgumentos()
        {
            Argumentos = new Dictionary<string, string>();

            // Usa o Skip pois o primeiro sempre vai ser o caminho do executável.
            string[] argsArray = Environment.GetCommandLineArgs().Skip(1).ToArray();
            bool sucesso = false;
            string argsString = null;

            foreach (var item in argsArray)
            {
                if (argsString == null)
                    argsString += "\"" + item + "\"";
                else
                    argsString += ", " + item;
            }
            if (argsString != null)
                Helper.LogMessage("Aplicação iniciada com os seguintes argumentos: " + argsString);

            for (int i = 0; i < argsArray.Length; i++)
            {
                if (argsArray[i].StartsWith("-"))
                {
                    string arg = argsArray[i].Replace("-", "");
                    if (argsArray.Length > i + 1 && !argsArray[i + 1].StartsWith("-"))
                    {
                        try { Argumentos.Add(arg, argsArray[i + 1]); }
                        catch (Exception e)
                        {
                            Helper.TratarException(e, "Os argumentos informados estão incorretos, favor verifica-los.\r\nArgumento: " + arg);
                            return true;
                        }
                        i++; // Soma pois caso o parâmetro possua o identificador, será guardado este identificador e seu valor no dicionário, que será o próximo argumento da lista.
                    }
                    else
                    {
                        try { Argumentos.Add(arg, null); }
                        catch (Exception e)
                        {
                            Helper.TratarException(e, "Os argumentos informados estão incorretos, favor verifica-los.\r\nArgumento: " + arg);
                            return true;
                        }
                    }
                }
                else
                {
                    if (RenomearEpisodiosDosArgumentos(argsArray[i]))
                    {
                        sucesso = true;
                    }
                }
            }
            return sucesso;
        }

        private bool RenomearEpisodiosDosArgumentos(string arg)
        {
            try
            {
                RenomearViewModel renomearVM = null;
                if (Directory.Exists(arg))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(arg);
                    renomearVM = new RenomearViewModel(true, dirInfo.EnumerateFiles("*.*", SearchOption.AllDirectories));
                }
                else if (File.Exists(arg))
                {
                    IEnumerable<FileInfo> arquivo = new FileInfo[1] { new FileInfo(arg) };
                    renomearVM = new RenomearViewModel(true, arquivo);
                }

                renomearVM.bFlSilencioso = true;

                foreach (var item in renomearVM.lstEpisodios)
                {
                    item.bFlSelecionado = true;
                    item.bFlRenomeado = false; // Para quando o episódio ja tiver sido renomeado alguma vez o retorno funcionar corretamente.
                }

                if (renomearVM.CommandRenomear.CanExecute(renomearVM))
                {
                    renomearVM.CommandRenomear.Execute(renomearVM);
                }
                return renomearVM.lstEpisodios.All(x => x.bFlRenomeado == true);
            }
            catch (Exception e)
            {
                Helper.TratarException(e, "Ocorreu um erro ao renomear os episódios dos argumentos na aplicação. Argumento: " + arg);
                return true; // Retorna true para não continuar a executar a aplicação.
            }
        }

        public void AtualizarPosters(Enums.TipoConteudo nIdTipoConteudo)
        {
            switch (nIdTipoConteudo)
            {
                case Enums.TipoConteudo.Filme:
                    {
                        //Filmes = new ObservableCollection<PosterViewModel>();
                        //List<Filme> filmesDB = DatabaseHelper.GetFilmes();

                        //foreach (var item in filmesDB)
                        //{
                        //    var path = Path.Combine(item.FolderMetadata, "poster.jpg");
                        //    PosterGrid pg = new PosterGrid() { IDBanco = item.IDBanco, ImgPoster = File.Exists(path) ? path : null, Type = Enums.TipoConteudo.movie };
                        //    PosterViewModel posterVM = new PosterViewModel();
                        //    posterVM.Poster = pg;
                        //    _filmes.Add(posterVM);
                        //}

                        //Filmes = _filmes;
                        //break;
                        throw new NotImplementedException();
                    }
                case Enums.TipoConteudo.Série:
                    {
                        SeriesService seriesService = App.Container.Resolve<SeriesService>();

                        lstSeries = new ObservableCollection<PosterViewModel>();

                        var lstSeriesDB = seriesService.GetListaSeriesComForeignKeys();

                        foreach (var item in lstSeriesDB)
                        {
                            var posterMetadata = Path.Combine(item.sDsMetadata, "poster.jpg");
                            item.sDsImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.oPoster = item;
                            posterVM.Owner = Owner;
                            _lstSeries.Add(posterVM);
                        }

                        break;
                    }
                case Enums.TipoConteudo.Anime:
                    {
                        SeriesService seriesService = App.Container.Resolve<SeriesService>();
                        lstAnimes = new ObservableCollection<PosterViewModel>();

                        var lstAnimesDB = seriesService.GetListaAnimesComForeignKeys();

                        foreach (var item in lstAnimesDB)
                        {
                            var posterMetadata = Path.Combine(item.sDsMetadata, "poster.jpg");
                            item.sDsImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.oPoster = item;
                            posterVM.Owner = Owner;
                            _lstAnimes.Add(posterVM);
                        }

                        break;
                    }
                case Enums.TipoConteudo.AnimeFilmeSérie:
                    {
                        SeriesService seriesService = App.Container.Resolve<SeriesService>();

                        lstSeries = new ObservableCollection<PosterViewModel>();
                        lstAnimes = new ObservableCollection<PosterViewModel>();
                        //Filmes = new ObservableCollection<PosterViewModel>();

                        var lstSeriesDB = seriesService.GetListaSeriesComForeignKeys();
                        var lstAnimesDB = seriesService.GetListaAnimesComForeignKeys();
                        //List<Filme> filmes = DatabaseHelper.GetFilmes();

                        foreach (var item in lstSeriesDB)
                        {
                            var posterMetadata = Path.Combine(item.sDsMetadata, "poster.jpg");
                            item.sDsImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.oPoster = item;
                            posterVM.Owner = Owner;
                            _lstSeries.Add(posterVM);
                        }

                        foreach (var item in lstAnimesDB)
                        {
                            var posterMetadata = Path.Combine(item.sDsMetadata, "poster.jpg");
                            item.sDsImgPoster = File.Exists(posterMetadata) ? posterMetadata : null;
                            PosterViewModel posterVM = new PosterViewModel();
                            posterVM.oPoster = item;
                            posterVM.Owner = Owner;
                            _lstAnimes.Add(posterVM);
                        }

                        //foreach (var item in filmes)
                        //{
                        //    var path = Path.Combine(item.FolderMetadata, "poster.jpg");
                        //    PosterGrid pg = new PosterGrid() { IDBanco = item.IDBanco, ImgPoster = File.Exists(path) ? path : null, Type = Enums.TipoConteudo.movie };
                        //    PosterViewModel posterVM = new PosterViewModel();
                        //    posterVM.Poster = pg;
                        //    _filmes.Add(posterVM);
                        //}

                        break;
                    }
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public void CriarTimerAtualizacaoConteudo()
        {
            Timer timerAtualizarConteudo = new Timer();

            timerAtualizarConteudo.Tick += (s, e) => { AtualizarConteudo(); };
            timerAtualizarConteudo.Interval = Settings.Default.pref_IntervaloDeProcuraConteudoNovo * 60 * 1000; // em milisegundos
            timerAtualizarConteudo.Start();

            AtualizarConteudo();

            //APIRequests.GetAtualizacoes();
        }

        public void AtualizarConteudo()
        {
            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += async (s, e) =>
            {
                ProcurarNovosEpisodiosBaixados();

                //AlterarStatusEpisodios();

                ProcurarEpisodiosParaBaixar();

                await APIRequests.GetAtualizacoes();
            };

            worker.RunWorkerAsync();
        }

        private void ProcurarEpisodiosParaBaixar()
        {
            try
            {
                FeedsService feedsService = App.Container.Resolve<FeedsService>();

                var lstFeeds = feedsService.GetLista().Where(x => !x.bIsFeedPesquisa && (x.nIdTipoConteudo == Enums.TipoConteudo.Série || x.nIdTipoConteudo == Enums.TipoConteudo.Anime)).OrderBy(x => x.nNrPrioridade);

                foreach (var item in lstFeeds)
                {
                    var rss = Argotic.Syndication.RssFeed.Create(new Uri(item.sLkFeed));

                    foreach (var itemRss in rss.Channel.Items)
                    {
                        Episodio episodio = new Episodio();
                        episodio.sDsFilepath = itemRss.Title;

                        if (episodio.IdentificarEpisodio() && episodio.nIdTipoConteudo == item.nIdTipoConteudo /*&& episodio.nIdEstadoEpisodio == Enums.EstadoEpisodio.Desejado*/)
                        {
                            episodio.BaixarEpisodio(itemRss.Link.ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Helper.TratarException(e, "Ocorreu um erro ao procurar os episódios para baixar.");
            }
        }

        private void AlterarStatusEpisodios()
        {
            throw new NotImplementedException();
        }

        private void ProcurarNovosEpisodiosBaixados()
        {
            var series = lstAnimesESeries.ToList();
            EpisodiosService episodiosService = App.Container.Resolve<EpisodiosService>();

            foreach (var serie in series)
            {
                episodiosService.VerificaEpisodiosNoDiretorio(serie.oPoster);
            }
        }
    }
}