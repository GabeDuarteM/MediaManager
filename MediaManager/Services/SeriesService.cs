using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using MediaManager.Helpers;
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
            foreach (var serie in obj)
            {
                try
                {
                    if (!Directory.Exists(serie.sDsMetadata))
                        Directory.CreateDirectory(serie.sDsMetadata);

                    Helper.DownloadImages(serie);

                    _context.Serie.Add(serie);
                    _context.SaveChanges();

                    App.Container.Resolve<EpisodiosService>().VerificaEpisodiosNoDiretorio(serie);
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException(
                        "Ocorreu um erro ao adicionar a série \"" + serie.sDsTitulo + "\" ao banco.", true);
                    return false;
                }
            }
            return true;
        }

        public bool Remover(params Serie[] series)
        {
            foreach (var serie in series)
            {
                try
                {
                    _context.Serie.Remove(serie);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException("Ocorreu um erro ao deletar a série ou anime \"" +
                                                                 serie.sDsTitulo + "\".");
                    return false;
                }
            }
            return true;
        }

        public bool Update(params Serie[] obj)
        {
            bool retorno = true;
            foreach (var serie in obj)
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
            Serie serie = _context.Serie.Where(x => x.nCdVideo == id).FirstOrDefault();
            serie.nIdEstado = Enums.Estado.CompletoSemForeignKeys;
            serie.nIdTipoConteudo = serie.bFlAnime ? Enums.TipoConteudo.Anime : Enums.TipoConteudo.Série;

            return serie;
        }

        public List<Serie> GetLista()
        {
            List<Serie> lstSeries = _context.Serie.ToList();

            lstSeries.ForEach(serie =>
            {
                serie.nIdTipoConteudo = serie.bFlAnime ? Enums.TipoConteudo.Anime : Enums.TipoConteudo.Série;
                serie.nIdEstado = Enums.Estado.CompletoSemForeignKeys;
            });

            return lstSeries;
        }

        public async Task<bool> AdicionarAsync(params Serie[] series)
        {
            foreach (var serie in series)
            {
                try
                {
                    if (!Directory.Exists(serie.sDsMetadata))
                        Directory.CreateDirectory(serie.sDsMetadata);

                    await Helper.DownloadImagesAsync(serie);

                    _context.Serie.Add(serie);
                    _context.SaveChanges();

                    App.Container.Resolve<EpisodiosService>().VerificaEpisodiosNoDiretorio(serie);
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException(
                        "Ocorreu um erro ao adicionar a série \"" + serie.sDsTitulo + "\" ao banco.", true);
                    return false;
                }
            }
            return true;
        }

        public bool Remover(params int[] nCdVideos)
        {
            foreach (var nCdVideo in nCdVideos)
            {
                string sNmSerie = null;

                try
                {
                    var serie = _context.Serie.Find(nCdVideo);
                    sNmSerie = serie.sDsTitulo;
                    _context.Serie.Remove(serie);

                    if (Directory.Exists(serie.sDsMetadata))
                    {
                        DirectoryInfo metaDir = new DirectoryInfo(serie.sDsMetadata);

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
                    new MediaManagerException(e).TratarException("Ocorreu um erro ao remover a série ou anime " +
                                                                 sNmSerie);
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> UpdateAsync(params Serie[] obj)
        {
            foreach (var serie in obj)
            {
                bool isDiferente = false;
                Serie original = null;
                Serie serieOld = new Serie();

                try
                {
                    original = _context.Serie.Find(serie.nCdVideo);
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
                    new MediaManagerException(e).TratarException(
                        "Ocorreu um erro ao atualizar a série \"" + serie.sDsTitulo + "\" no banco.", true);
                    return false;
                }

                if (isDiferente || serieOld.sDsMetadata != serie.sDsMetadata)
                    // Pode acontecer da serie ser a mesma mas o nome ter alterado, alterando tb o folderMetadata.
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
                        Directory.CreateDirectory(serie.sDsMetadata);

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
                serie.nIdTipoConteudo = serie.bFlAnime ? Enums.TipoConteudo.Anime : Enums.TipoConteudo.Série;
                serie.nIdEstado = Enums.Estado.CompletoSemForeignKeys;
            });

            return lstSeries;
        }

        public Serie GetPorCodigoApi(int nCdApi)
        {
            Serie serie = _context.Serie.Where(x => x.nCdApi == nCdApi).First();
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
                x.nIdTipoConteudo = x.bFlAnime ? Enums.TipoConteudo.Anime : Enums.TipoConteudo.Série;
                x.nIdEstado = Enums.Estado.Completo;
            });

            return lstSeries;
        }

        public List<Serie> GetListaAnimes()
        {
            List<Serie> animes = _context.Serie.Where(x => x.bFlAnime).OrderBy(x => x.sDsTitulo).ToList();

            animes.ForEach(anime =>
            {
                anime.nIdTipoConteudo = anime.bFlAnime ? Enums.TipoConteudo.Anime : Enums.TipoConteudo.Série;
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
            int levenshtein = 10;
            try
            {
                // Verifica se existe série com nome igual, se tiver seta como melhor correspondencia e a retorna direto.
                var series = _context.Serie.Where(x => x.sDsTitulo.ToLower() == titulo.ToLower()).ToList();
                if (series.Count() > 0)
                {
                    levenshtein = 0;
                    melhorCorrespondencia = series.First();
                    return melhorCorrespondencia;
                }

                // Verifica se existem séries que contenham o título citado e calcula o levenshtein.
                series = _context.Serie.Where(x => x.sDsTitulo.ToLower().Contains(titulo.ToLower())).ToList();

                foreach (var serie in series)
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
                if (titulo.Replace(".", " ").Replace("_", " ").Split(' ').Count() > 1)
                {
                    foreach (var item in titulo.Replace(".", " ").Replace("_", " ").ToLower().Split(' '))
                    {
                        if (item.Length <= 3)
                        {
                            continue;
                        }

                        series = _context.Serie.Where(x => x.sDsTitulo.ToLower().Contains(item)).ToList();
                        foreach (var serie in series)
                        {
                            int levensTemp = Helper.CalcularAlgoritimoLevenshtein(titulo.ToLower(),
                                serie.sDsTitulo.ToLower());
                            if (levensTemp < levenshtein)
                            {
                                levenshtein = levensTemp;
                                melhorCorrespondencia = serie;
                            }
                        }
                        var aliases =
                            _context.SerieAlias.Where(x => x.sDsAlias.ToLower().Contains(item.ToLower())).ToList();
                        foreach (var alias in aliases)
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
                new MediaManagerException(e).TratarException(
                    "Ocorreu um erro ao pesquisar a correspondencia do arquivo \"" + titulo + "\" no banco.", true);
                return null;
            }
            return melhorCorrespondencia;
        }

        public List<Serie> GetSerieOuAnimePorTitulo(string titulo, bool removerCaracteresEspeciais)
        {
            List<Serie> lstSeries = _context.Serie
                .Where(
                    x =>
                        removerCaracteresEspeciais
                            ? x.sDsTitulo.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim().Contains(titulo)
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
                    var lstSeries = _context.Serie.Where(x => x.nIdTipoConteudo == nIdTipoConteudo);
                    foreach (var item in lstSeries)
                    {
                        var sPastaItem = Path.GetDirectoryName(item.sDsPasta);
                        item.sDsPasta = item.sDsPasta.Replace(sPastaItem, sPasta);
                    }
                    _context.SaveChanges();
                    break;
                }

                default:
                    break;
            }
        }

        public bool VerificarSeExiste(int nCdApi)
        {
            List<Serie> lstSeries = _context.Serie.Where(x => x.nCdApi == nCdApi).ToList();
            return lstSeries.Count() > 0 ? true : false;
        }

        public bool VerificarSeExiste(string sDsPasta)
        {
            List<Serie> lstSerie = _context.Serie.Where(x => x.sDsPasta == sDsPasta).ToList();

            return lstSerie.Count() > 0 ? true : false;
        }
    }
}