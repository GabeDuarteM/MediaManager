// Developed by: Gabriel Duarte
// 
// Created at: 20/07/2015 21:10

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using Argotic.Syndication;
using Autofac;
using MediaManager.Helpers;
using MediaManager.Localizacao;
using MediaManager.Model;
using MediaManager.Properties;
using MediaManager.Services;

namespace MediaManager.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<PosterViewModel> _lstAnimes;

        private ObservableCollection<PosterViewModel> _lstFilmes;

        private ObservableCollection<PosterViewModel> _lstSeries;

        public MainViewModel(Window owner = null, ICollection<Serie> animes = null, ICollection<Serie> filmes = null,
                             ICollection<Serie> series = null)
        {
            Owner = owner;
        }

        public ObservableCollection<PosterViewModel> lstAnimes
        {
            get { return _lstAnimes; }
            set
            {
                _lstAnimes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PosterViewModel> lstFilmes
        {
            get { return _lstFilmes; }
            set
            {
                _lstFilmes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PosterViewModel> lstSeries
        {
            get { return _lstSeries; }
            set
            {
                _lstSeries = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PosterViewModel> lstAnimesESeries
        {
            get
            {
                var retorno = new ObservableCollection<PosterViewModel>();

                foreach (PosterViewModel anime in lstAnimes)
                {
                    retorno.Add(anime);
                }

                foreach (PosterViewModel serie in lstSeries)
                {
                    retorno.Add(serie);
                }

                return retorno;
            }
        }

        public static Dictionary<string, string> Argumentos { get; private set; }

        public Window Owner { get; }

        /// <summary>
        ///     Retorna true caso haja arquivos a serem renomeados, para que o resto da aplicação não seja carregada.
        /// </summary>
        /// <returns></returns>
        public bool TratarArgumentos()
        {
            Argumentos = new Dictionary<string, string>();

            // Usa o Skip pois o primeiro sempre vai ser o caminho do executável.
            string[] argsArray = Environment.GetCommandLineArgs().Skip(1).ToArray();
            var sucesso = false;

            Helper.LogMessage(string.Format(Mensagens.Aplicação_iniciada_com_os_seguintes_argumentos_0_, "".ColocarVirgula(argsArray.Select(x => $"\"{x}\""))));

            for (var i = 0; i < argsArray.Length; i++)
            {
                if (argsArray[i].StartsWith("-"))
                {
                    string arg = argsArray[i].Replace("-", "");

                    if (argsArray.Length > i + 1 && !argsArray[i + 1].StartsWith("-"))
                    {
                        try
                        {
                            Argumentos.Add(arg, argsArray[i + 1]);
                        }
                        catch (Exception e)
                        {
                            new MediaManagerException(e).TratarException(string.Format(Mensagens.Os_argumentos_informados_estão_incorretos_favor_verifica_los_Argumento_0_, arg));
                            return true;
                        }

                        i++;
                        // Soma pois caso o parâmetro possua o identificador, será guardado este identificador e seu valor no dicionário, que será o próximo argumento da lista.
                    }
                    else
                    {
                        try
                        {
                            Argumentos.Add(arg, null);
                        }
                        catch (Exception e)
                        {
                            new MediaManagerException(e).TratarException(string.Format(Mensagens.Os_argumentos_informados_estão_incorretos_favor_verifica_los_Argumento_0_, arg));
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

        private static bool RenomearEpisodiosDosArgumentos(string arg)
        {
            try
            {
                RenomearViewModel renomearVm = null;

                if (Directory.Exists(arg))
                {
                    var dirInfo = new DirectoryInfo(arg);
                    renomearVm = new RenomearViewModel(true, dirInfo.EnumerateFiles("*.*", SearchOption.AllDirectories));
                }
                else if (File.Exists(arg))
                {
                    IEnumerable<FileInfo> arquivo = new[] {new FileInfo(arg)};
                    renomearVm = new RenomearViewModel(true, arquivo);
                }

                if (renomearVm == null)
                {
                    return false;
                }

                renomearVm.bFlSilencioso = true;

                foreach (Episodio item in renomearVm.lstEpisodios)
                {
                    item.bFlSelecionado = true;
                    item.bFlRenomeado = false;
                    // Para quando o episódio ja tiver sido renomeado alguma vez o retorno funcionar corretamente.
                }

                if (renomearVm.CommandRenomear.CanExecute(renomearVm))
                {
                    renomearVm.CommandRenomear.Execute(renomearVm);
                }

                return renomearVm.lstEpisodios.All(x => x.bFlRenomeado);
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_renomear_os_episódios_dos_argumentos_na_aplicação_Argumento_0_, arg));
                return true; // Retorna true para não continuar a executar a aplicação.
            }
        }

        public void AtualizarPosters(Enums.TipoConteudo nIdTipoConteudo)
        {
            switch (nIdTipoConteudo)
            {
                case Enums.TipoConteudo.Filme:
                {
                    AtualizarPosterFilmes();
                    break;
                }
                case Enums.TipoConteudo.Série:
                {
                    AtualizarPosterSeries();
                    break;
                }
                case Enums.TipoConteudo.Anime:
                {
                    AtualizarPosterAnimes();

                    break;
                }
                case Enums.TipoConteudo.AnimeFilmeSérie:
                {
                    AtualizarPosterSeries();
                    AtualizarPosterAnimes();
                    //AtualizarPosterFilmes(); //TODO Filmes
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(nIdTipoConteudo), nIdTipoConteudo, null);
            }
        }

        private static void AtualizarPosterFilmes()
        {
            throw new NotImplementedException(); // TODO Filmes
        }

        private void AtualizarPosterAnimes()
        {
            var seriesService = App.Container.Resolve<SeriesService>();
            lstAnimes = new ObservableCollection<PosterViewModel>();

            List<Serie> lstAnimesDb = seriesService.GetListaAnimesComForeignKeys();

            foreach (Serie item in lstAnimesDb)
            {
                string posterMetadata = Path.Combine(item.sDsMetadata, "poster.jpg");
                item.sDsImgPoster = File.Exists(posterMetadata)
                                        ? posterMetadata
                                        : null;
                var posterVm = new PosterViewModel
                {
                    oPoster = item, Owner = Owner
                };

                _lstAnimes.Add(posterVm);
            }
        }

        private void AtualizarPosterSeries()
        {
            var seriesService = App.Container.Resolve<SeriesService>();

            lstSeries = new ObservableCollection<PosterViewModel>();

            List<Serie> lstSeriesDb = seriesService.GetListaSeriesComForeignKeys();

            foreach (Serie item in lstSeriesDb)
            {
                string posterMetadata = Path.Combine(item.sDsMetadata, "poster.jpg");
                item.sDsImgPoster = File.Exists(posterMetadata)
                                        ? posterMetadata
                                        : null;
                var posterVm = new PosterViewModel
                {
                    oPoster = item, Owner = Owner
                };

                _lstSeries.Add(posterVm);
            }
        }

        public void CriarTimerAtualizacaoConteudo()
        {
            var timerAtualizarConteudo = new Timer();

            timerAtualizarConteudo.Tick += (s, e) => { AtualizarConteudo(); };
            timerAtualizarConteudo.Interval = Settings.Default.pref_IntervaloDeProcuraConteudoNovo * 60 * 1000; // em milisegundos
            timerAtualizarConteudo.Start();

            AtualizarConteudo();

            //APIRequests.GetAtualizacoes();
        }

        // TODO Chamar GetAtualizacoes antes de procurar episodios para baixar
        private void AtualizarConteudo()
        {
            var worker = new BackgroundWorker();

            worker.DoWork += async (s, e) =>
            {
                ProcurarNovosEpisodiosBaixados();

                AlterarStatusEpisodios();

                ProcurarEpisodiosParaBaixar();

                await APIRequests.GetAtualizacoes();
            };

            worker.RunWorkerAsync();
        }

        private static void ProcurarEpisodiosParaBaixar()
        {
            if (string.IsNullOrWhiteSpace(Settings.Default.pref_PastaBlackhole) || !Directory.Exists(Settings.Default.pref_PastaBlackhole))
            {
                new MediaManagerException(Mensagens.Para_que_o_download_possa_ser_realizado_preencha_o_campo_Torrent_blackhole_nas_preferências_do_programa).TratarException(Mensagens.Ocorreu_um_erro_ao_realizar_o_download);
                return;
            }

            try
            {
                var episodiosService = App.Container.Resolve<EpisodiosService>();

                IEnumerable<ItemDownload> lstEpisodiosParaBaixar = ProcurarEpisodiosNosFeeds();

                foreach (ItemDownload objItemDownload in lstEpisodiosParaBaixar)
                {
                    if (!objItemDownload.ObjEpisodio.EncaminharParaDownload(objItemDownload))
                    {
                        continue;
                    }

                    objItemDownload.ObjEpisodio.nIdEstadoEpisodio = Enums.EstadoEpisodio.Baixando;

                    episodiosService.UpdateEstadoEpisodio(objItemDownload.ObjEpisodio);
                }
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(Mensagens.Ocorreu_um_erro_ao_procurar_os_episódios_para_baixar);
            }
        }

        private static IEnumerable<ItemDownload> ProcurarEpisodiosNosFeeds()
        {
            var lstEpisodiosParaBaixar = new List<ItemDownload>();

            var feedsService = App.Container.Resolve<FeedsService>();
            var qualidadeDownloadService = App.Container.Resolve<QualidadeDownloadService>();
            IEnumerable<Feed> lstFeeds = feedsService.GetLista()
                                                     .Where(x => !x.bIsFeedPesquisa &&
                                                                 (x.nIdTipoConteudo == Enums.TipoConteudo.Série || x.nIdTipoConteudo == Enums.TipoConteudo.Anime))
                                                     .OrderBy(x => x.nNrPrioridade);
            IEnumerable<QualidadeDownload> lstQualidadeDownloads = qualidadeDownloadService.GetLista().OrderBy(x => x.nPrioridade);
            var rgxQualidade = new Regex($".*?({string.Join("|", lstQualidadeDownloads.Select(x => x.sIdentificadoresQualidade))})");

            foreach (Feed item in lstFeeds)
            {
                RssFeed rss;

                try
                {
                    rss = RssFeed.Create(new Uri(item.sLkFeed));
                }
                catch (Exception ex)
                {
                    new MediaManagerException(ex).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_abrir_o_feed_0_URL_1_, item.sDsFeed, item.sLkFeed));
                    continue;
                }

                foreach (RssItem objRssItem in rss.Channel.Items)
                {
                    try
                    {
                        var episodio = new Episodio {sDsFilepath = objRssItem.Title};
                        string extensao = Path.GetExtension(Helper.RetirarCaracteresInvalidos(objRssItem.Title));

                        if (!episodio.IdentificarEpisodio() ||
                            episodio.nIdTipoConteudo != item.nIdTipoConteudo ||
                            episodio.nIdEstadoEpisodio != Enums.EstadoEpisodio.Desejado ||
                            episodio.oSerie.bIsParado ||
                            (!string.IsNullOrWhiteSpace(extensao) && !Settings.Default.ExtensoesRenomeioPermitidas.Contains(extensao)))
                        {
                            continue;
                        }

                        string strIdentificador = rgxQualidade.Match(objRssItem.Title).Groups[1].Value;

                        // Verifica se é vazio/null para atribuir a qualidade "Desconhecido".
                        QualidadeDownload objQualidadeDownload = !string.IsNullOrWhiteSpace(strIdentificador)
                                                                     ? lstQualidadeDownloads.FirstOrDefault(x => x.sIdentificadoresQualidade.Split('|').Any(y => y == strIdentificador))
                                                                     : lstQualidadeDownloads.First(x => x.nCdQualidadeDownload == 1);

                        if (lstEpisodiosParaBaixar.Any(x => x.ObjEpisodio.nCdEpisodioAPI == episodio.nCdEpisodioAPI))
                        {
                            lstEpisodiosParaBaixar.First(x => x.ObjEpisodio.nCdEpisodioAPI == episodio.nCdEpisodioAPI).LstObjRssItem.Add(objRssItem, objQualidadeDownload);
                        }
                        else
                        {
                            lstEpisodiosParaBaixar.Add(new ItemDownload {ObjEpisodio = episodio, LstObjRssItem = new Dictionary<RssItem, QualidadeDownload> {{objRssItem, objQualidadeDownload}}});
                        }
                    }
                    catch (Exception e)
                    {
                        new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_procurar_o_item_0_do_feed_RSS_1_, objRssItem.Title, item.sDsFeed));
                    }
                }
            }

            return lstEpisodiosParaBaixar;
        }

        private void AlterarStatusEpisodios()
        {
            var episodiosService = App.Container.Resolve<EpisodiosService>();
            var seriesService = App.Container.Resolve<SeriesService>();

            List<Episodio> lstEpisodios = episodiosService.GetLista();
            List<Episodio> lstEpisodiosDesejar = lstEpisodios.Where(x => x.tDtEstreia > DateTime.Now && x.nIdEstadoEpisodio == Enums.EstadoEpisodio.Novo).ToList();
            List<Episodio> lstEpisodiosBaixados = lstEpisodios.Where(x => x.nIdEstadoEpisodio == Enums.EstadoEpisodio.Baixado).ToList();
            var lstAlterados = new List<Episodio>();

            if (lstEpisodiosBaixados.Count > 0 || lstEpisodiosDesejar.Count > 0)
            {
                foreach (Episodio item in lstEpisodiosDesejar)
                {
                    Serie serie = seriesService.Get(item.nCdVideo);
                    if (!serie.bIsParado)
                    {
                        item.nIdEstadoEpisodio = Enums.EstadoEpisodio.Desejado;
                        lstAlterados.Add(item);
                    }
                }

                foreach (Episodio item in lstEpisodiosBaixados)
                {
                    if (!File.Exists(item.sDsFilepath))
                    {
                        item.nIdEstadoEpisodio = Enums.EstadoEpisodio.Arquivado;
                        lstAlterados.Add(item);
                    }
                }

                episodiosService.UpdateEstadoEpisodio(lstAlterados.ToArray());
            }
        }

        private void ProcurarNovosEpisodiosBaixados()
        {
            List<PosterViewModel> series = lstAnimesESeries.ToList();
            var episodiosService = App.Container.Resolve<EpisodiosService>();

            foreach (PosterViewModel serie in series)
            {
                episodiosService.VerificaEpisodiosNoDiretorio(serie.oPoster);
            }
        }
    }
}
