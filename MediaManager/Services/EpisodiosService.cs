// Developed by: Gabriel Duarte
// 
// Created at: 15/12/2015 19:12
// Last update: 19/04/2016 02:57

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.Services
{
    public class EpisodiosService : IRepositorio<Episodio>
    {
        private readonly IContext _context;

        public EpisodiosService(IContext context)
        {
            _context = context;
        }

        public bool Adicionar(params Episodio[] obj)
        {
            try
            {
                foreach (Episodio episodio in obj)
                {
                    try
                    {
                        if (episodio.nCdEpisodio > 0)
                        {
                            _context.Episodio.Add(episodio);
                        }
                        else
                        {
                            Serie serie = App.Container.Resolve<SeriesService>().GetPorCodigoApi(episodio.nCdVideoAPI);
                            episodio.nCdVideo = serie.nCdVideo;
                            _context.Episodio.Add(episodio);
                        }
                    }
                    catch (Exception e)
                    {
                        new MediaManagerException(e).TratarException(
                                                                     "Ocorreu um erro ao adicionar o episódio com o ID " +
                                                                     episodio.nCdEpisodioAPI + " ao banco.");
                        return false;
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException("Ocorreu um erro ao adicionar episódios.");
                return false;
            }
        }

        public Episodio Get(int id)
        {
            try
            {
                Episodio episodio = _context.Episodio.FirstOrDefault(x => x.nCdEpisodio == id);
                return episodio;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException("Ocorreu um erro ao retornar o episódio com o código " + id);
                return null;
            }
        }

        public List<Episodio> GetLista()
        {
            List<Episodio> lstEpisodios = _context.Episodio.ToList();
            return lstEpisodios;
        }

        public bool Update(params Episodio[] obj)
        {
            Episodio original = null;

            try
            {
                foreach (Episodio episodio in obj)
                {
                    try
                    {
                        original = _context.Episodio.Find(episodio.nCdEpisodio);

                        if (original != null)
                        {
                            original.nNrAbsoluto = episodio.nNrAbsoluto;
                            original.sLkArtwork = episodio.sLkArtwork;
                            original.sDsEpisodio = episodio.sDsEpisodio;
                            original.nNrEpisodio = episodio.nNrEpisodio;
                            original.tDtEstreia = episodio.tDtEstreia;
                            original.nCdTemporadaAPI = episodio.nCdTemporadaAPI;
                            original.nCdVideoAPI = episodio.nCdVideoAPI;
                            original.nCdEpisodioAPI = episodio.nCdEpisodioAPI;
                            original.sDsIdioma = episodio.sDsIdioma;
                            original.sNrUltimaAtualizacao = episodio.sNrUltimaAtualizacao;
                            original.sDsSinopse = episodio.sDsSinopse;
                            original.dNrAvaliacao = episodio.dNrAvaliacao;
                            original.nNrTemporada = episodio.nNrTemporada;
                        }
                        else
                            return false;
                    }
                    catch (Exception e)
                    {
                        new MediaManagerException(e).TratarException("Ocorreu um erro ao atualizar o episódio de ID " +
                                                                     episodio.nCdEpisodio);
                        return false;
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                var frase = "Ocorreu um erro ao atualizar os episódios ";

                foreach (Episodio item in obj)
                {
                    frase += "\"" + item.sDsFilepath + "\", ";
                }

                new MediaManagerException(e).TratarException(frase.Remove(frase.Length - 3));
                return false;
            }
        }

        public bool Remover(params Episodio[] obj)
        {
            foreach (Episodio episodio in obj)
            {
                try
                {
                    _context.Episodio.Remove(episodio);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException("Ocorreu um erro ao remover o episódio \"" +
                                                                 episodio.sDsFilepath + "\"");
                    return false;
                }
            }
            return true;
        }

        public bool Adicionar(params Serie[] obj)
        {
            try
            {
                foreach (Serie serie in obj)
                {
                    try
                    {
                        serie.lstEpisodios.ForEach(episodio =>
                        {
                            episodio.nCdVideo = serie.nCdVideo;
                            Adicionar(episodio);
                        });
                    }
                    catch (Exception e)
                    {
                        new MediaManagerException(e).TratarException("Ocorreu um erro ao salvar os episódios de " +
                                                                     serie.sDsTitulo);
                        return false;
                    }
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException("Ocorreu um erro ao adicionar episódios.");
                return false;
            }
        }

        public Episodio Get(int nCdVideo, int nNrTemporada, int nNrEpisodio)
        {
            try
            {
                Episodio episodio = _context.Episodio
                                            .FirstOrDefault(
                                                            x =>
                                                            x.nCdVideo == nCdVideo && x.nNrTemporada == nNrTemporada &&
                                                            x.nNrEpisodio == nNrEpisodio);
                return episodio;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException("Ocorreu um erro ao retornar o episódio " + nNrEpisodio +
                                                             " da temporada " + nNrTemporada + " do vídeo de ID " +
                                                             nCdVideo);
                return null;
            }
        }

        public Episodio Get(int nCdVideo, int nNrAbsoluto)
        {
            Episodio episodio = _context.Episodio
                                        .Where(x => x.nCdVideo == nCdVideo && x.nNrAbsoluto == nNrAbsoluto)
                                        .FirstOrDefault();
            return episodio;
        }

        public Episodio GetPorCodigoAPI(int nCdEpisodioAPI)
        {
            Episodio episodio = _context.Episodio
                                        .FirstOrDefault(x => x.nCdEpisodioAPI == nCdEpisodioAPI);
            return episodio;
        }

        public List<Episodio> GetLista(Video serie)
        {
            List<Episodio> lstEpisodios = _context.Episodio.Where(x => x.nCdVideo == serie.nCdVideo).ToList();
            return lstEpisodios;
        }

        public bool UpdateEpisodioRenomeado(Episodio atualizado)
        {
            try
            {
                Episodio original;
                original = _context.Episodio.Find(atualizado.nCdEpisodio);
                if (original != null)
                {
                    original.sDsFilepathOriginal = atualizado.sDsFilepathOriginal;
                    original.sDsFilepath = atualizado.sDsFilepath;
                    original.bFlRenomeado = atualizado.bFlRenomeado;
                    original.nIdEstadoEpisodio = atualizado.nIdEstadoEpisodio;

                    foreach (int nNrEpisodio in atualizado.lstIntEpisodios)
                    {
                        if (nNrEpisodio == atualizado.nNrEpisodio)
                        {
                            continue;
                        }
                        Episodio episodio = Get(atualizado.nCdVideo, atualizado.nNrTemporada, nNrEpisodio);
                        Episodio episodioDB = _context.Episodio.Find(episodio.nCdEpisodio);
                        episodioDB.sDsFilepathOriginal = atualizado.sDsFilepathOriginal;
                        episodioDB.sDsFilepath = atualizado.sDsFilepath;
                        episodioDB.bFlRenomeado = atualizado.bFlRenomeado;
                        episodioDB.nIdEstadoEpisodio = atualizado.nIdEstadoEpisodio;
                    }

                    _context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(
                                                             "Ocorreu um erro ao atualizar o episódio de ID " +
                                                             atualizado.nCdEpisodioAPI + " no banco.", true);
                return false;
            }
        }

        public bool VerificarSeEpisodioJaFoiRenomeado(Episodio episodio)
        {
            try
            {
                IQueryable<Episodio> episodios = from episodiosDB in _context.Episodio
                                                 where
                                                     episodiosDB.nCdEpisodio == episodio.nCdEpisodio &&
                                                     episodiosDB.bFlRenomeado
                                                 select episodiosDB;
                return episodios.Count() > 0
                           ? true
                           : false;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException("Ocorreu um erro ao verificar se o episódio \"" +
                                                             episodio.sDsFilepath + "\" ja foi renomeado.");
                return false;
            }
        }

        public bool VerificaEpisodiosNoDiretorio(Video serie)
        {
            try
            {
                if (Directory.Exists(serie.sDsPasta))
                {
                    IEnumerable<FileInfo> arquivos = new DirectoryInfo(serie.sDsPasta).EnumerateFiles("*.*",
                                                                                                      SearchOption
                                                                                                          .AllDirectories);
                    string[] extensoesPermitidas = Properties.Settings.Default.ExtensoesRenomeioPermitidas.Split('|');

                    foreach (FileInfo item in arquivos)
                    {
                        if (extensoesPermitidas.Contains(item.Extension))
                        {
                            var episodio = new Episodio();
                            episodio.sDsFilepath = item.FullName;
                            episodio.oSerie = (Serie) serie;

                            if (episodio.IdentificarEpisodio())
                            {
                                episodio.sDsFilepath = item.FullName;
                                episodio.bFlRenomeado = episodio.sDsFilepath ==
                                                        Path.Combine(serie.sDsPasta,
                                                                     Helper.RenomearConformePreferencias(episodio)) +
                                                        item.Extension;

                                Episodio episodeDB = Get(serie.nCdVideo, episodio.nNrTemporada, episodio.nNrEpisodio);
                                episodeDB = _context.Episodio.Find(episodeDB.nCdEpisodio);
                                episodeDB.sDsFilepath = episodio.sDsFilepath;
                                episodeDB.bFlRenomeado = episodio.bFlRenomeado;
                                episodeDB.nIdEstadoEpisodio = Enums.EstadoEpisodio.Baixado;

                                foreach (int nNrEpisodio in episodio.lstIntEpisodios)
                                {
                                    if (nNrEpisodio == episodeDB.nNrEpisodio)
                                    {
                                        continue;
                                    }
                                    Episodio episodioAgregado = Get(episodeDB.nCdVideo, episodeDB.nNrTemporada,
                                                                    nNrEpisodio);
                                    Episodio episodioAgregadoDB = _context.Episodio.Find(episodioAgregado.nCdEpisodio);

                                    episodioAgregadoDB.sDsFilepath = episodeDB.sDsFilepath;
                                    episodioAgregadoDB.bFlRenomeado = episodeDB.bFlRenomeado;
                                    episodioAgregadoDB.nIdEstadoEpisodio = episodeDB.nIdEstadoEpisodio;
                                }

                                _context.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(
                                                             "Ocorreu um erro ao verificar os episódios no diretório da série \"" +
                                                             serie.sDsTitulo + "\".", true);
                return false;
            }
            return true;
        }

        public void UpdateEstadoEpisodio(params Episodio[] atualizados)
        {
            foreach (Episodio item in atualizados)
            {
                Episodio original = _context.Episodio.Find(item.nCdEpisodio);
                original.nIdEstadoEpisodio = item.nIdEstadoEpisodio;
            }
            _context.SaveChanges();
        }
    }
}
