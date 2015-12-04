using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MediaManager.Forms;
using MediaManager.Model;
using MediaManager.ViewModel;

namespace MediaManager.Helpers
{
    public class DBHelper
    {
        private Context db;

        public DBHelper(Context db = null)
        {
            if (db == null)
            {
                db = new Context();
            }

            this.db = db;
        }

        public bool AddEpisodio(Episodio episode)
        {
            try
            {
                if (episode.nCdEpisodio > 0)
                    db.Episodio.Add(episode);
                else
                {
                    Serie serie = GetSerieOuAnimePorIDApi(episode.nCdVideoAPI);
                    episode.nCdVideo = serie.nCdVideo;
                    db.Episodio.Add(episode);
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao adicionar o episódio com o ID " + episode.nCdEpisodioAPI + " ao banco.", true); return false; }
        }

        public bool AddSerieAlias(SerieAlias alias)
        {
            try
            {
                db.SerieAlias.Add(alias);
                db.SaveChanges();
                return true;
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao adicionar o alias \"" + alias.sDsAlias + "\" ao banco.", true); return false; }
        }

        public bool AddSerieAlias(Video video)
        {
            try
            {
                foreach (var item in video.sAliases.Split('|'))
                {
                    SerieAlias alias = new SerieAlias(item);
                    alias.nNrEpisodio = 1;
                    alias.nNrTemporada = 1;
                    alias.nCdVideo = video.nCdVideo;
                    db.SerieAlias.Add(alias);
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao adicionar o alias padrão do video \"" + video.sDsTitulo + "\" ao banco.", true); return false; }
        }

        public async Task<bool> AddSerieAsync(Serie serie)
        {
            try
            {
                if (!Directory.Exists(serie.sDsMetadata))
                    Directory.CreateDirectory(serie.sDsMetadata);

                if (!await Helper.DownloadImagesAsync(serie))
                { MessageBox.Show("Erro ao baixar as imagens."); return false; }

                db.Serie.Add(serie);
                db.SaveChanges();

                VerificaEpisodiosNoDiretorio(serie);
                return true;
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao adicionar a série \"" + serie.sDsTitulo + "\" ao banco.", true); return false; }
        }

        public bool AddSerie(Serie serie)
        {
            try
            {
                if (!Directory.Exists(serie.sDsMetadata))
                    Directory.CreateDirectory(serie.sDsMetadata);

                if (!Helper.DownloadImages(serie))
                { MessageBox.Show("Erro ao baixar as imagens."); return false; }

                db.Serie.Add(serie);
                db.SaveChanges();

                VerificaEpisodiosNoDiretorio(serie);
                return true;
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao adicionar a série \"" + serie.sDsTitulo + "\" ao banco.", true); return false; }
        }

        public List<SerieAlias> GetAllAliases()
        {
            var aliases = from aliasDB in db.SerieAlias
                          select aliasDB;
            return aliases != null ? aliases.ToList() : null;
        }

        public Serie GetAnimePorIDApi(int IDApi)
        {
            var animesDB = (from animeDB in db.Serie
                            where animeDB.bFlAnime && animeDB.nCdApi == IDApi
                            select animeDB);
            Serie anime = animesDB.First();
            return anime;
        }

        /// <summary>
        /// Consulta todos os animes contidos no banco.
        /// </summary>
        /// <returns>Retorna um List<Serie> contendo todos os animes que tenham serie.isAnime == true ordenados pelo título.</returns>
        public List<Serie> GetAnimes()
        {
            var animesDB = (from animeDB in db.Serie
                            where animeDB.bFlAnime
                            orderby animeDB.sDsTitulo
                            select animeDB);
            List<Serie> animes = animesDB.ToList();
            foreach (var item in animes)
            {
                item.nIdTipoConteudo = item.bFlAnime == true ? Enums.TipoConteudo.Anime
                    : Enums.TipoConteudo.Série;
                item.nIdEstado = Enums.Estado.CompletoSemForeignKeys;
            }
            return animes;
        }

        public List<Serie> GetAnimesComForeignKeys()
        {
            var animesDB = (from animeDB in db.Serie.Include(x => x.lstEpisodios).Include(x => x.lstSerieAlias)
                            where animeDB.bFlAnime
                            orderby animeDB.sDsTitulo
                            select animeDB);
            List<Serie> animes = animesDB.ToList();
            foreach (var item in animes)
            {
                item.nIdTipoConteudo = item.bFlAnime == true ? Enums.TipoConteudo.Anime
                    : Enums.TipoConteudo.Série;
                item.nIdEstado = Enums.Estado.Completo;
            }
            return animes;
        }

        public Episodio GetEpisode(int IDSerie, int seasonNumber, int episodeNumber)
        {
            var todosEpisodiosDB = from episodioDB in db.Episodio
                                   where episodioDB.nCdVideo == IDSerie && episodioDB.nNrTemporada == seasonNumber && episodioDB.nNrEpisodio == episodeNumber
                                   select episodioDB;
            return todosEpisodiosDB.FirstOrDefault();
        }

        public Episodio GetEpisode(int IDSerie, int absoluteNumber)
        {
            var todosEpisodiosDB = from episodioDB in db.Episodio
                                   where episodioDB.nCdVideo == IDSerie && episodioDB.nNrAbsoluto == absoluteNumber
                                   select episodioDB;
            return todosEpisodiosDB.FirstOrDefault();
        }

        public Episodio GetEpisode(int IDApi)
        {
            var todosEpisodiosDB = from episodioDB in db.Episodio
                                   where episodioDB.nCdEpisodioAPI == IDApi
                                   select episodioDB;
            return todosEpisodiosDB.Count() != 0 ? todosEpisodiosDB.First() : null;
        }

        public List<Episodio> GetEpisodes()
        {
            var todosEpisodiosDB = from episodioDB in db.Episodio
                                   select episodioDB;
            var lstEpisodios = todosEpisodiosDB.ToList();
            return lstEpisodios;
        }

        public List<Episodio> GetEpisodes(Video serie)
        {
            var episodios = from episodiosDB in db.Episodio
                            where episodiosDB.nCdVideo == serie.nCdVideo
                            select episodiosDB;
            var lstEpisodios = episodios.ToList();
            return lstEpisodios;
        }

        public void AlterarPastaPadraoVideos(Enums.TipoConteudo nIdTipoConteudo, string sPasta)
        {
            switch (nIdTipoConteudo)
            {
                case Enums.TipoConteudo.Filme:
                    {
                        break;
                    }
                case Enums.TipoConteudo.Série:
                    {
                        var series = from seriesDB in db.Serie
                                     where !seriesDB.bFlAnime
                                     select seriesDB;
                        foreach (var item in series)
                        {
                            var sPastaItem = Path.GetDirectoryName(item.sDsPasta);
                            item.sDsPasta = item.sDsPasta.Replace(sPastaItem, sPasta);
                        }
                        db.SaveChanges();
                        break;
                    }
                case Enums.TipoConteudo.Anime:
                    {
                        var series = from seriesDB in db.Serie
                                     where seriesDB.bFlAnime
                                     select seriesDB;
                        foreach (var item in series)
                        {
                            var sPastaItem = Path.GetDirectoryName(item.sDsPasta);
                            item.sDsPasta = item.sDsPasta.Replace(sPastaItem, sPasta);
                        }
                        db.SaveChanges();
                        break;
                    }

                default:
                    break;
            }
        }

        public List<Episodio> GetEpisodesToRename()
        {
            var allEpisodesDB = from episodeDB in db.Episodio
                                where !episodeDB.bFlRenomeado
                                select episodeDB;

            var allEpisodes = allEpisodesDB.ToList();
            foreach (var item in allEpisodes)
            {
                item.oSerie = db.Serie.Find(item.nCdVideo);
            }
            return allEpisodes;
        }

        public List<Feed> GetFeeds()
        {
            var feeds = from feedsDB in db.Feed
                        select feedsDB;
            return feeds.ToList();
        }

        public List<SerieAlias> GetSerieAliases(Video video)
        {
            var aliases = from aliasDB in db.SerieAlias
                          where aliasDB.nCdVideo == video.nCdVideo
                          select aliasDB;
            return aliases != null ? aliases.ToList() : null;
        }

        public Serie GetSerieOuAnimePorIDApi(int IDApi)
        {
            var seriesDB = (from serieDB in db.Serie
                            where serieDB.nCdApi == IDApi
                            select serieDB);
            Serie serie = seriesDB.First();
            return serie;
        }

        public Serie GetSerieOuAnimePorLevenshtein(string titulo)
        {
            Serie melhorCorrespondencia = null;
            int levenshtein = int.MaxValue;
            try
            {
                // Verifica se existe série com nome igual, se tiver seta como melhor correspondencia e a retorna direto.
                var series = from serieDB in db.Serie
                             where serieDB.sDsTitulo == titulo
                             select serieDB;
                if (series.Count() > 0)
                {
                    levenshtein = 0;
                    melhorCorrespondencia = series.First();
                    return melhorCorrespondencia;
                }

                // Verifica se existem séries que contenham o título citado e calcula o levenshtein.
                series = from serieDB in db.Serie
                         where serieDB.sDsTitulo.Contains(titulo)
                         select serieDB;

                foreach (var serie in series)
                {
                    int levensTemp = Helper.CalcularAlgoritimoLevenshtein(titulo, serie.sDsTitulo);
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
                    foreach (var item in titulo.Replace(".", " ").Replace("_", " ").Split(' '))
                    {
                        if (item.Length <= 3)
                        {
                            continue;
                        }

                        series = from serieDB in db.Serie where serieDB.sDsTitulo.Contains(item) select serieDB;
                        foreach (var serie in series)
                        {
                            int levensTemp = Helper.CalcularAlgoritimoLevenshtein(titulo, serie.sDsTitulo);
                            if (levensTemp < levenshtein)
                            {
                                levenshtein = levensTemp;
                                melhorCorrespondencia = serie;
                            }
                        }

                        var aliases = from aliasDB in db.SerieAlias where aliasDB.sDsAlias.Contains(item) select aliasDB;
                        foreach (var alias in aliases)
                        {
                            int levensTemp = Helper.CalcularAlgoritimoLevenshtein(titulo, alias.sDsAlias);
                            if (levensTemp < levenshtein)
                            {
                                levenshtein = levensTemp;
                                melhorCorrespondencia = GetSeriePorID(alias.nCdVideo);
                            }
                        }
                    }
                }
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao pesquisar a correspondencia do arquivo \"" + titulo + "\" no banco.", true); return null; }
            return melhorCorrespondencia;
        }

        /// <summary>
        /// Pesquisa por séries ou animes que contenham a string informada
        /// </summary>
        /// <param name="titulo">Título inteiro ou parte dele</param>
        /// <returns>Lista de séries que contenham a string informada</returns>
        public List<Serie> GetSerieOuAnimePorTitulo(string titulo, bool removerCaracteresEspeciais)
        {
            if (!removerCaracteresEspeciais)
            {
                var seriesDB = (from serieDB in db.Serie
                                where serieDB.sDsTitulo.Contains(titulo)
                                select serieDB);
                List<Serie> series = seriesDB.ToList();
                return series;
            }
            else
            {
                var seriesDB = (from serieDB in db.Serie
                                where serieDB.sDsTitulo.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim().Contains(titulo)
                                select serieDB);
                List<Serie> series = seriesDB.ToList();
                return series;
            }
        }

        /// <summary>
        /// Consulta todos as séries contidas no banco que tenham a id especificada.
        /// </summary>
        /// <param name="IDBanco">ID da série.</param>
        /// <returns>Retorna a série caso esta exista no banco, ou null caso não exista.</returns>
        public Serie GetSeriePorID(int IDBanco)
        {
            var seriesDB = (from serieDB in db.Serie
                            where serieDB.nCdVideo == IDBanco
                            select serieDB);
            Serie serie = seriesDB.First();
            serie.nIdEstado = Enums.Estado.CompletoSemForeignKeys;
            return serie;
        }

        /// <summary>
        /// Consulta todas as séries contidas no banco.
        /// </summary>
        /// <returns>Retorna um List<Serie> contendo todas as séries que tenham serie.isAnime == false ordenados pelo título.</returns>
        public List<Serie> GetSeries()
        {
            var seriesDB = (from serieDB in db.Serie
                            where !serieDB.bFlAnime
                            orderby serieDB.sDsTitulo
                            select serieDB);
            List<Serie> series = seriesDB.ToList();
            foreach (var item in series)
            {
                item.nIdTipoConteudo = item.bFlAnime == true ? Enums.TipoConteudo.Anime
                    : Enums.TipoConteudo.Série;
                item.nIdEstado = Enums.Estado.CompletoSemForeignKeys;
            }
            return series;
        }

        public List<Serie> GetSeriesComForeignKeys()
        {
            var seriesDB = (from serieDB in db.Serie.Include(x => x.lstEpisodios).Include(x => x.lstSerieAlias)
                            where !serieDB.bFlAnime
                            orderby serieDB.sDsTitulo
                            select serieDB);
            List<Serie> series = seriesDB.ToList();
            series.ForEach(x =>
            {
                x.nIdTipoConteudo = x.bFlAnime == true ? Enums.TipoConteudo.Anime : Enums.TipoConteudo.Série;
                x.nIdEstado = Enums.Estado.Completo;
            });
            return series;
        }

        public List<Serie> GetSeriesEAnimes()
        {
            var seriesDB = (from serieDB in db.Serie
                            orderby serieDB.sDsTitulo
                            select serieDB);
            List<Serie> series = seriesDB.ToList();
            foreach (var item in series)
            {
                item.nIdTipoConteudo = item.bFlAnime == true ? Enums.TipoConteudo.Anime
                    : Enums.TipoConteudo.Série;
                item.nIdEstado = Enums.Estado.CompletoSemForeignKeys;
            }
            return series;
        }

        public List<Serie> GetSeriesEAnimesComForeignKeys()
        {
            var seriesDB = (from serieDB in db.Serie.Include(x => x.lstEpisodios).Include(x => x.lstSerieAlias)
                            orderby serieDB.sDsTitulo
                            select serieDB);
            List<Serie> series = seriesDB.ToList();
            foreach (var item in series)
            {
                item.nIdTipoConteudo = item.bFlAnime == true ? Enums.TipoConteudo.Anime
                    : Enums.TipoConteudo.Série;
                item.nIdEstado = Enums.Estado.Completo;
            }
            return series;
        }

        // Efeito cascata se encarrega de remover tudo relacionado a ela nas outras tabelas.
        public bool RemoverSerieOuAnime(Serie serie)
        {
            try
            {
                db.Serie.Remove(serie);
                db.SaveChanges();
                return true;
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao deletar a série ou anime \"" + serie.sDsTitulo + "\"."); return false; }
        }

        // Efeito cascata se encarrega de remover tudo relacionado a ela nas outras tabelas.
        public bool RemoverSerieOuAnimePorID(int ID)
        {
            string sNmSerie = null;
            try
            {
                var serie = db.Serie.Find(ID);
                sNmSerie = serie.sDsTitulo;
                db.Serie.Remove(serie);

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

                db.SaveChanges();
                return true;
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao deletar a série ou anime " + sNmSerie); return false; }
        }

        public bool RemoveSerieAlias(SerieAlias alias)
        {
            try
            {
                db.SerieAlias.Attach(alias);
                db.SerieAlias.Remove(alias);
                db.SaveChanges();
                return true;
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao remover o alias \"" + alias.sDsAlias + "\" do banco.", true); return false; }
        }

        public bool UpdateEpisodio(Episodio atualizado)
        {
            Episodio original = null;
            try
            {
                original = db.Episodio.Find(atualizado.nCdEpisodio);

                if (original != null)
                {
                    original.nNrAbsoluto = atualizado.nNrAbsoluto;
                    //original.AirsAfterSeason = atualizado.AirsAfterSeason;
                    //original.AirsBeforeEpisode = atualizado.AirsBeforeEpisode;
                    //original.AirsBeforeSeason = atualizado.AirsBeforeSeason;
                    original.sLkArtwork = atualizado.sLkArtwork;
                    original.sDsEpisodio = atualizado.sDsEpisodio;
                    original.nNrEpisodio = atualizado.nNrEpisodio;
                    original.tDtEstreia = atualizado.tDtEstreia;
                    original.nCdTemporadaAPI = atualizado.nCdTemporadaAPI;
                    original.nCdVideoAPI = atualizado.nCdVideoAPI;
                    original.nCdEpisodioAPI = atualizado.nCdEpisodioAPI;
                    original.sDsIdioma = atualizado.sDsIdioma;
                    original.sNrUltimaAtualizacao = atualizado.sNrUltimaAtualizacao;
                    original.sDsSinopse = atualizado.sDsSinopse;
                    original.dNrAvaliacao = atualizado.dNrAvaliacao;
                    //original.RatingCount = atualizado.RatingCount;
                    original.nNrTemporada = atualizado.nNrTemporada;

                    db.SaveChanges();
                    return true;
                }
                else return false;
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao atualizar o episódio de ID " + atualizado.nCdEpisodioAPI + " no banco.", true); return false; }
        }

        public bool UpdateEpisodioRenomeado(Episodio atualizado)
        {
            try
            {
                Episodio original;
                original = db.Episodio.Find(atualizado.nCdEpisodio);
                if (original != null)
                {
                    original.sDsFilepathOriginal = atualizado.sDsFilepathOriginal;
                    original.sDsFilepath = atualizado.sDsFilepath;
                    original.bFlRenomeado = atualizado.bFlRenomeado;
                    original.nIdEstadoEpisodio = atualizado.nIdEstadoEpisodio;
                    db.SaveChanges();
                    return true;
                }
                else return false;
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao atualizar o episódio de ID " + atualizado.nCdEpisodioAPI + " no banco.", true); return false; }
        }

        public void UpdateListaEpisodios(List<Episodio> lstEpisodiosModificados)
        {
            foreach (var item in lstEpisodiosModificados)
            {
                db.Episodio.Attach(item);
                var entry = db.Entry(item);
                entry.State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
        }

        public async Task<bool> UpdateSerieAsync(Serie atualizado)
        {
            bool isDiferente = false;
            Serie original = null;
            Serie serieOld = new Serie();
            try
            {
                original = db.Serie.Find(atualizado.nCdVideo);
                serieOld.Clone(original);
                if (original.nCdApi != atualizado.nCdApi)
                    isDiferente = true;

                if (original != null)
                {
                    db.Entry(original).CurrentValues.SetValues(atualizado);
                    if (isDiferente)
                    {
                        db.Episodio.RemoveRange(db.Episodio.Where(x => x.nCdVideoAPI == serieOld.nCdApi));
                        db.SerieAlias.RemoveRange(db.SerieAlias.Where(x => x.nCdVideo == atualizado.nCdVideo));
                        AddEpisodios(atualizado);
                        AddSerieAlias(atualizado);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao atualizar a série \"" + atualizado.sDsTitulo + "\" no banco.", true); return false; }

            if (isDiferente || serieOld.sDsMetadata != atualizado.sDsMetadata) // Pode acontecer da serie ser a mesma mas o nome ter alterado, alterando tb o folderMetadata.
            {
                if (Directory.Exists(serieOld.sDsMetadata))
                {
                    DirectoryInfo metaDir = new DirectoryInfo(serieOld.sDsMetadata);

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

                if (!Directory.Exists(atualizado.sDsMetadata))
                    Directory.CreateDirectory(atualizado.sDsMetadata);

                if (!await Helper.DownloadImagesAsync(atualizado))
                { MessageBox.Show("Erro ao baixar as imagens."); return false; }
            }
            return true;
        }

        public bool VerificarSeEpisodioJaFoiRenomeado(Episodio episodio)
        {
            var episodios = from episodiosDB in db.Episodio
                            where episodiosDB.nCdEpisodio == episodio.nCdEpisodio && episodiosDB.bFlRenomeado
                            select episodiosDB;
            return episodios.Count() > 0 ? true : false;
        }

        public bool VerificaSeSerieOuAnimeExiste(int IDApi)
        {
            var series = from seriesDB in db.Serie where seriesDB.nCdApi == IDApi select seriesDB;
            return series.Count() > 0 ? true : false;
        }

        public bool VerificaSeSerieOuAnimeExiste(string folderPath)
        {
            var series = from serie in db.Serie
                         where serie.sDsPasta == folderPath
                         select serie;
            //var filmes = from filme in db.Filmes
            //             where filme.FolderPath == folderPath
            //             select filme;

            return (series.Count() > 0 /*|| filmes.Count() > 0*/) ? true : false;
        }

        private bool AddEpisodios(Serie atualizado)
        {
            foreach (var item in atualizado.lstEpisodios)
            {
                item.nCdVideo = atualizado.nCdVideo;
                db.Episodio.Add(item);
            }
            db.SaveChanges();
            return true;
        }

        public bool AddFeed(Feed feed)
        {
            try
            {
                db.Feed.Add(feed);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Helper.TratarException(e, "Ocorreu um erro ao adicionar o feed " + feed.sDsFeed, true);
                return false;
            }
        }

        public bool RemoveFeed(Feed feed)
        {
            try
            {
                db.Feed.Remove(feed);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Helper.TratarException(e, "Ocorreu um erro ao remover o feed " + feed.sDsFeed, true);
                return false;
            }
        }

        public bool UpdateFeed(params Feed[] atualizado)
        {
            Feed feed = null; // Para mostrar no catch.

            try
            {
                foreach (var item in atualizado)
                {
                    feed = item;
                    db.Feed.Attach(item);
                    var entry = db.Entry(item);
                    entry.State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Helper.TratarException(e, "Ocorreu um erro ao atualizar o feed " + feed.sDsFeed, true);
                return false;
            }
        }

        private bool VerificaEpisodiosNoDiretorio(Serie serie)
        {
            try
            {
                if (Directory.Exists(serie.sDsPasta))
                {
                    var arquivos = new DirectoryInfo(serie.sDsPasta).EnumerateFiles("*.*", SearchOption.AllDirectories);
                    string[] extensoesPermitidas = Properties.Settings.Default.ExtensoesRenomeioPermitidas.Split('|');

                    foreach (var item in arquivos)
                    {
                        if (extensoesPermitidas.Contains(item.Extension))
                        {
                            Episodio episodio = new Episodio();
                            episodio.sDsFilepath = item.FullName;
                            episodio.oSerie = serie;

                            if (episodio.IdentificarEpisodio())
                            {
                                episodio.sDsFilepath = item.FullName;
                                episodio.bFlRenomeado = (episodio.sDsFilepath == Path.Combine(serie.sDsPasta, Helper.RenomearConformePreferencias(episodio)) + item.Extension);

                                Episodio episodeDB = GetEpisode(serie.nCdVideo, episodio.nNrTemporada, episodio.nNrEpisodio);
                                episodeDB = db.Episodio.Find(episodeDB.nCdEpisodio);
                                episodeDB.sDsFilepath = episodio.sDsFilepath;
                                episodeDB.bFlRenomeado = episodio.bFlRenomeado;
                                episodeDB.nIdEstadoEpisodio = Enums.EstadoEpisodio.Baixado;
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao verificar os episódios no diretório da série \"" + serie.sDsTitulo + "\".", true); return false; }
            return true;
        }
    }
}