// Developed by: Gabriel Duarte
// 
// Created at: 15/12/2015 18:11

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using MediaManager.Helpers;
using MediaManager.Localizacao;
using MediaManager.Model;

namespace MediaManager.Services
{
    public class SeriesService : IRepositorio<Serie>
    {
        private readonly IContext _context;

        public SeriesService(IContext context)
        {
            _context = context;
        }

        public bool Adicionar(params Serie[] obj)
        {
            foreach (Serie serie in obj)
            {
                try
                {
                    if (!Directory.Exists(serie.sDsMetadata))
                    {
                        Directory.CreateDirectory(serie.sDsMetadata);
                    }

                    Helper.DownloadImages(serie);

                    _context.Serie.Add(serie);
                    _context.SaveChanges();

                    App.Container.Resolve<EpisodiosService>().VerificaEpisodiosNoDiretorio(serie);
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_adicionar_1_0_, serie.sDsTitulo, serie.nIdTipoConteudo.GetDescricao().ToLower()));
                    return false;
                }
            }

            return true;
        }

        public bool Remover(params Serie[] series)
        {
            foreach (Serie serie in series)
            {
                try
                {
                    _context.Serie.Remove(serie);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_remover_1_0_, serie.sDsTitulo, serie.nIdTipoConteudo.GetDescricao().ToLower()));
                    return false;
                }
            }

            return true;
        }

        public bool Update(params Serie[] obj)
        {
            var retorno = true;
            foreach (Serie serie in obj)
            {
                if (!UpdateAsync(serie).Result && retorno)
                {
                    retorno = false;
                }
            }

            return retorno;
        }

        public Serie Get(int id)
        {
            Serie serie = _context.Serie.FirstOrDefault(x => x.nCdVideo == id);
            serie.nIdEstado = Enums.Estado.CompletoSemForeignKeys;
            serie.nIdTipoConteudo = serie.bFlAnime
                                        ? Enums.TipoConteudo.Anime
                                        : Enums.TipoConteudo.Série;

            return serie;
        }

        public List<Serie> GetLista()
        {
            List<Serie> lstSeries = _context.Serie.ToList();

            lstSeries.ForEach(serie =>
            {
                serie.nIdTipoConteudo = serie.bFlAnime
                                            ? Enums.TipoConteudo.Anime
                                            : Enums.TipoConteudo.Série;
                serie.nIdEstado = Enums.Estado.CompletoSemForeignKeys;
            });

            return lstSeries;
        }

        public async Task<bool> AdicionarAsync(params Serie[] series)
        {
            foreach (Serie serie in series)
            {
                try
                {
                    if (!Directory.Exists(serie.sDsMetadata))
                    {
                        Directory.CreateDirectory(serie.sDsMetadata);
                    }

                    await Helper.DownloadImagesAsync(serie);

                    _context.Serie.Add(serie);
                    _context.SaveChanges();

                    App.Container.Resolve<EpisodiosService>().VerificaEpisodiosNoDiretorio(serie);
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_adicionar_1_0_, serie.sDsTitulo, serie.nIdTipoConteudo.GetDescricao().ToLower()));
                    return false;
                }
            }

            return true;
        }

        public bool Remover(params int[] nCdVideos)
        {
            foreach (int nCdVideo in nCdVideos)
            {
                string sNmSerie = null;

                string tipoConteudo = null;

                try
                {
                    Serie serie = _context.Serie.Find(nCdVideo);
                    sNmSerie = serie.sDsTitulo;
                    tipoConteudo = serie.nIdTipoConteudo.GetDescricao().ToLower();
                    _context.Serie.Remove(serie);

                    if (Directory.Exists(serie.sDsMetadata))
                    {
                        var metaDir = new DirectoryInfo(serie.sDsMetadata);

                        foreach (FileInfo file in metaDir.GetFiles())
                        {
                            file.Delete();
                        }

                        foreach (DirectoryInfo dir in metaDir.GetDirectories())
                        {
                            dir.Delete(true);
                        }

                        Directory.Delete(metaDir.FullName);
                    }

                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_remover_1_0_, sNmSerie, tipoConteudo));
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> UpdateAsync(params Serie[] obj)
        {
            foreach (Serie serie in obj)
            {
                var isDiferente = false;
                var serieOld = new Serie();

                try
                {
                    Serie original = _context.Serie.Find(serie.nCdVideo);
                    serieOld.Clone(original);

                    if (original.nCdApi != serie.nCdApi)
                    {
                        isDiferente = true;
                    }

                    if (original != null)
                    {
                        _context.Entry(original).CurrentValues.SetValues(serie);
                        if (isDiferente)
                        {
                            _context.Episodio.RemoveRange(_context.Episodio.Where(x => x.nCdVideoAPI == serieOld.nCdApi));
                            _context.SerieAlias.RemoveRange(_context.SerieAlias.Where(x => x.nCdVideo == serie.nCdVideo));
                            App.Container.Resolve<EpisodiosService>().Adicionar(serie);
                            App.Container.Resolve<SerieAliasService>().Adicionar(serie);
                        }
                        _context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_atualizar_1_0_, serie.sDsTitulo, serie.nIdTipoConteudo.GetDescricao().ToLower()));
                    return false;
                }

                // Pode acontecer da serie ser a mesma mas o nome ter alterado, alterando tb o folderMetadata, por isso o if.
                if (isDiferente || serieOld.sDsMetadata != serie.sDsMetadata)
                {
                    if (Directory.Exists(serieOld.sDsMetadata))
                    {
                        var metaDir = new DirectoryInfo(serieOld.sDsMetadata);

                        foreach (FileInfo file in metaDir.GetFiles())
                        {
                            file.Delete();
                        }

                        foreach (DirectoryInfo dir in metaDir.GetDirectories())
                        {
                            dir.Delete(true);
                        }

                        Directory.Delete(metaDir.FullName);
                    }

                    if (!Directory.Exists(serie.sDsMetadata))
                    {
                        Directory.CreateDirectory(serie.sDsMetadata);
                    }

                    if (!await Helper.DownloadImagesAsync(serie))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public List<Serie> GetListaSeries()
        {
            List<Serie> lstSeries = _context.Serie.Where(x => !x.bFlAnime).OrderBy(x => x.sDsTitulo).ToList();
            lstSeries.ForEach(serie =>
            {
                serie.nIdTipoConteudo = serie.bFlAnime
                                            ? Enums.TipoConteudo.Anime
                                            : Enums.TipoConteudo.Série;
                serie.nIdEstado = Enums.Estado.CompletoSemForeignKeys;
            });

            return lstSeries;
        }

        public Serie GetPorCodigoApi(int nCdApi)
        {
            Serie serie = _context.Serie.First(x => x.nCdApi == nCdApi);
            return serie;
        }

        public List<Serie> GetListaSeriesComForeignKeys()
        {
            List<Serie> lstSeries = _context.Serie
                                            .Include(x => x.lstEpisodios)
                                            .Include(x => x.lstSerieAlias)
                                            .Where(x => !x.bFlAnime)
                                            .OrderBy(x => x.sDsTitulo)
                                            .ToList();

            lstSeries.ForEach(x =>
            {
                x.nIdTipoConteudo = x.bFlAnime
                                        ? Enums.TipoConteudo.Anime
                                        : Enums.TipoConteudo.Série;
                x.nIdEstado = Enums.Estado.Completo;
            });

            return lstSeries;
        }

        public List<Serie> GetListaAnimes()
        {
            List<Serie> animes = _context.Serie.Where(x => x.bFlAnime).OrderBy(x => x.sDsTitulo).ToList();

            animes.ForEach(anime =>
            {
                anime.nIdTipoConteudo = anime.bFlAnime
                                            ? Enums.TipoConteudo.Anime
                                            : Enums.TipoConteudo.Série;
                anime.nIdEstado = Enums.Estado.CompletoSemForeignKeys;
            });

            return animes;
        }

        public List<Serie> GetListaAnimesComForeignKeys()
        {
            List<Serie> lstAnimes = _context.Serie
                                            .Include(x => x.lstEpisodios)
                                            .Include(x => x.lstSerieAlias)
                                            .Where(x => x.bFlAnime)
                                            .OrderBy(x => x.sDsTitulo)
                                            .ToList();

            lstAnimes.ForEach(anime =>
            {
                anime.nIdTipoConteudo = anime.bFlAnime == true
                                            ? Enums.TipoConteudo.Anime
                                            : Enums.TipoConteudo.Série;
                anime.nIdEstado = Enums.Estado.Completo;
            });

            return lstAnimes;
        }

        public Serie GetSerieOuAnimePorLevenshtein(string titulo)
        {
            Serie melhorCorrespondencia = null;
            var levenshtein = 10;
            try
            {
                // Verifica se existe série com nome igual, se tiver seta como melhor correspondencia e a retorna direto.
                List<Serie> series = _context.Serie.Where(x => x.sDsTitulo.ToLower() == titulo.ToLower()).ToList();
                if (series.Any())
                {
                    melhorCorrespondencia = series.First();
                    return melhorCorrespondencia;
                }

                // Verifica se existem séries que contenham o título citado e calcula o levenshtein.
                series = _context.Serie.Where(x => x.sDsTitulo.ToLower().Contains(titulo.ToLower())).ToList();

                foreach (Serie serie in series)
                {
                    int levensTemp = Helper.CalcularAlgoritimoLevenshtein(titulo.ToLower(), serie.sDsTitulo.ToLower());
                    if (levensTemp < levenshtein)
                    {
                        levenshtein = levensTemp;
                        melhorCorrespondencia = serie;
                    }
                }

                // Caso a série possua mais de uma palavra, realiza uma pesquisa no banco por cada palavra que tenha mais de 3 letras
                // (para evitar falsos positivos com palavras tipo "The") e calcula o levenshtein
                if (titulo.Replace('.', ' ').Replace('_', ' ').Split(' ').Length > 1)
                {
                    foreach (string item in titulo.Replace('.', ' ').Replace('_', ' ').ToLower().Split(' '))
                    {
                        if (item.Length <= 3)
                        {
                            continue;
                        }

                        series = _context.Serie.Where(x => x.sDsTitulo.ToLower().Contains(item)).ToList();
                        foreach (Serie serie in series)
                        {
                            int levensTemp = Helper.CalcularAlgoritimoLevenshtein(titulo.ToLower(),
                                                                                  serie.sDsTitulo.ToLower());
                            if (levensTemp < levenshtein)
                            {
                                levenshtein = levensTemp;
                                melhorCorrespondencia = serie;
                            }
                        }

                        List<SerieAlias> aliases =
                            _context.SerieAlias.Where(x => x.sDsAlias.ToLower().Contains(item.ToLower())).ToList();
                        foreach (SerieAlias alias in aliases)
                        {
                            int levensTemp = Helper.CalcularAlgoritimoLevenshtein(titulo.ToLower(),
                                                                                  alias.sDsAlias.ToLower());
                            if (levensTemp < levenshtein)
                            {
                                levenshtein = levensTemp;
                                melhorCorrespondencia = Get(alias.nCdVideo);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_aplicar_o_algoritimo_de_correspondencia_no_arquivo_0_, titulo));
                return null;
            }

            return melhorCorrespondencia;
        }

        public List<Serie> GetSerieOuAnimePorTitulo(string titulo, bool removerCaracteresEspeciais)
        {
            List<Serie> lstSeries = _context.Serie
                                            .Where(x => removerCaracteresEspeciais
                                                            ? x.sDsTitulo.Replace('.', ' ').Replace('_', ' ').Replace(
                                                                                                                      "'", "").Trim().Contains(titulo)
                                                            : x.sDsTitulo.Contains(titulo))
                                            .ToList();
            return lstSeries;
        }

        public List<Serie> GetSeriesEAnimes()
        {
            List<Serie> lstSeries = GetLista();
            List<Serie> lstAnimes = GetListaAnimes();
            List<Serie> lstSeriesAnimes = lstSeries.Concat(lstAnimes).ToList();

            return lstSeriesAnimes;
        }

        public List<Serie> GetSeriesEAnimesComForeignKeys()
        {
            List<Serie> lstSeries = GetListaSeriesComForeignKeys();
            List<Serie> lstAnimes = GetListaAnimesComForeignKeys();
            List<Serie> lstSeriesAnimes = lstSeries.Concat(lstAnimes).ToList();

            return lstSeriesAnimes;
        }

        public void AlterarPastaPadraoVideos(Enums.TipoConteudo nIdTipoConteudo, string sPasta)
        {
            switch (nIdTipoConteudo)
            {
                case Enums.TipoConteudo.Série:
                case Enums.TipoConteudo.Anime:
                {
                    IQueryable<Serie> lstSeries = _context.Serie.Where(x => x.nIdTipoConteudo == nIdTipoConteudo);
                    foreach (Serie item in lstSeries)
                    {
                        string sPastaItem = Path.GetDirectoryName(item.sDsPasta);
                        item.sDsPasta = item.sDsPasta.Replace(sPastaItem, sPasta);
                    }

                    _context.SaveChanges();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(nIdTipoConteudo), nIdTipoConteudo, null);
            }
        }

        public bool VerificarSeExiste(int nCdApi)
        {
            List<Serie> lstSeries = _context.Serie.Where(x => x.nCdApi == nCdApi).ToList();

            return lstSeries.Any();
        }

        public bool VerificarSeExiste(string sDsPasta)
        {
            List<Serie> lstSerie = _context.Serie.Where(x => x.sDsPasta == sDsPasta).ToList();

            return lstSerie.Any();
        }
    }
}
