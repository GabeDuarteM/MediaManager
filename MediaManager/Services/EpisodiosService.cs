// Developed by: Gabriel Duarte
// 
// Created at: 15/12/2015 19:12

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using MediaManager.Helpers;
using MediaManager.Localizacao;
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
                        new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_adicionar_o_episódio_com_o_ID_0_, episodio.nCdEpisodioAPI));
                        return false;
                    }
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(Mensagens.Ocorreu_um_erro_ao_adicionar_episódios);
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
                new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_retornar_o_episódio_com_o_código_0_, id));
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
            try
            {
                foreach (Episodio episodio in obj)
                {
                    try
                    {
                        Episodio original = _context.Episodio.Find(episodio.nCdEpisodio);

                        if (original == null)
                        {
                            return false;
                        }

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
                    catch (Exception e)
                    {
                        new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_atualizar_o_episódio_de_ID_0_, episodio.nCdEpisodio));
                        return false;
                    }
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                string frase = Mensagens.Ocorreu_um_erro_ao_atualizar_os_episódios;

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
                    new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_remover_o_episódio_0_, episodio.sDsFilepath));
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
                        });
                        Adicionar(serie.lstEpisodios.ToArray());
                    }
                    catch (Exception e)
                    {
                        new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_salvar_os_episódios_de_0_, serie.sDsTitulo));
                        return false;
                    }
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(Mensagens.Ocorreu_um_erro_ao_adicionar_episódios);
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
                new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_retornar_o_episódio_0_da_temporada_1_do_vídeo_de_ID_2_, nNrEpisodio, nNrTemporada, nCdVideo));
                return null;
            }
        }

        public Episodio Get(int nCdVideo, int nNrAbsoluto)
        {
            Episodio episodio = _context.Episodio
                                        .FirstOrDefault(x => x.nCdVideo == nCdVideo && x.nNrAbsoluto == nNrAbsoluto);
            return episodio;
        }

        public Episodio GetPorCodigoAPI(int nCdEpisodioApi)
        {
            Episodio episodio = _context.Episodio
                                        .FirstOrDefault(x => x.nCdEpisodioAPI == nCdEpisodioApi);
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
                Episodio original = _context.Episodio.Find(atualizado.nCdEpisodio);

                if (original == null)
                {
                    return false;
                }

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
                    Episodio episodioDb = _context.Episodio.Find(episodio.nCdEpisodio);
                    episodioDb.sDsFilepathOriginal = atualizado.sDsFilepathOriginal;
                    episodioDb.sDsFilepath = atualizado.sDsFilepath;
                    episodioDb.bFlRenomeado = atualizado.bFlRenomeado;
                    episodioDb.nIdEstadoEpisodio = atualizado.nIdEstadoEpisodio;
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_atualizar_o_episódio_de_ID_0_, atualizado.nCdEpisodioAPI));
                return false;
            }
        }

        public bool VerificarSeEpisodioJaFoiRenomeado(Episodio episodio)
        {
            try
            {
                IQueryable<Episodio> episodios = from episodiosDb in _context.Episodio
                                                 where
                                                     episodiosDb.nCdEpisodio == episodio.nCdEpisodio &&
                                                     episodiosDb.bFlRenomeado
                                                 select episodiosDb;
                return episodios.Any();
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_verificar_se_o_episódio_ja_foi_renomeado_0_, episodio.sDsFilepath));
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
                        if (!extensoesPermitidas.Contains(item.Extension))
                        {
                            continue;
                        }

                        var episodio = new Episodio
                        {
                            sDsFilepath = item.FullName,
                            oSerie = (Serie) serie
                        };

                        if (!episodio.IdentificarEpisodio())
                        {
                            continue;
                        }

                        episodio.sDsFilepath = item.FullName;
                        episodio.bFlRenomeado = episodio.sDsFilepath ==
                                                Path.Combine(serie.sDsPasta,
                                                             Helper.RenomearConformePreferencias(episodio)) +
                                                item.Extension;

                        Episodio episodeDb = Get(serie.nCdVideo, episodio.nNrTemporada, episodio.nNrEpisodio);
                        episodeDb = _context.Episodio.Find(episodeDb.nCdEpisodio);
                        episodeDb.sDsFilepath = episodio.sDsFilepath;
                        episodeDb.bFlRenomeado = episodio.bFlRenomeado;
                        episodeDb.nIdEstadoEpisodio = Enums.EstadoEpisodio.Baixado;

                        foreach (int nNrEpisodio in episodio.lstIntEpisodios)
                        {
                            if (nNrEpisodio == episodeDb.nNrEpisodio)
                            {
                                continue;
                            }

                            Episodio episodioAgregado = Get(episodeDb.nCdVideo, episodeDb.nNrTemporada,
                                                            nNrEpisodio);
                            Episodio episodioAgregadoDb = _context.Episodio.Find(episodioAgregado.nCdEpisodio);

                            episodioAgregadoDb.sDsFilepath = episodeDb.sDsFilepath;
                            episodioAgregadoDb.bFlRenomeado = episodeDb.bFlRenomeado;
                            episodioAgregadoDb.nIdEstadoEpisodio = episodeDb.nIdEstadoEpisodio;
                        }

                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_verificar_os_episódios_no_diretório_0__Tipo_1_, serie.sDsTitulo, serie.nIdTipoConteudo.GetDescricao().ToLower()));
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
