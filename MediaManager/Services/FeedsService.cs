// Developed by: Gabriel Duarte
// 
// Created at: 16/12/2015 00:39

using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using MediaManager.Localizacao;
using MediaManager.Model;

namespace MediaManager.Services
{
    public class FeedsService : IRepositorio<Feed>
    {
        private readonly IContext _context;

        public FeedsService(IContext context)
        {
            _context = context;
        }

        public bool Adicionar(params Feed[] obj)
        {
            foreach (Feed feed in obj)
            {
                try
                {
                    List<Feed> lstFeeds =
                        _context.Feed.Where(
                                            x =>
                                            x.nIdTipoConteudo == feed.nIdTipoConteudo &&
                                            x.bIsFeedPesquisa == feed.bIsFeedPesquisa)
                                .ToList();

                    // Para não interferir no CommandSalvar da tela de adicionar feed quando o feed vai ser adicionado a mais de um tipo de conteúdo
                    // (caso contrário nunca vai cair no if acima, pois a prioridade será != 0)
                    var feedClone = new Feed(feed);

                    if (feedClone.nNrPrioridade == 0)
                    {
                        feedClone.nNrPrioridade = lstFeeds.Count > 0
                                                      ? lstFeeds.Last().nNrPrioridade + 1
                                                      : 1;
                    }

                    _context.Feed.Add(feedClone);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_adicionar_o_feed_0_, feed.sDsFeed));
                    return false;
                }
            }

            return true;
        }

        public Feed Get(int id)
        {
            try
            {
                Feed oFeed = _context.Feed.FirstOrDefault(x => x.nCdFeed == id);
                return oFeed;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_pesquisar_o_feed_de_código_0_, id));
                return null;
            }
        }

        public List<Feed> GetLista()
        {
            try
            {
                List<Feed> lstFeeds = _context.Feed.ToList();

                return lstFeeds;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(Mensagens.Ocorreu_um_erro_ao_retornar_a_lista_de_feeds);
                return null;
            }
        }

        public bool Remover(params Feed[] obj)
        {
            foreach (Feed feed in obj)
            {
                try
                {
                    feed.Clone(new Feed {nCdFeed = feed.nCdFeed});
                    _context.Feed.Attach(feed);
                    _context.Feed.Remove(feed);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_remover_o_feed_0_, feed.sDsFeed));
                    return false;
                }
            }

            return true;
        }

        public bool Update(params Feed[] obj)
        {
            Feed feed = null; // Para mostrar no catch.

            try
            {
                foreach (Feed item in obj)
                {
                    feed = item;
                    _context.Feed.Attach(item);
                    DbEntityEntry<Feed> entry = _context.Entry(item);
                    entry.State = System.Data.Entity.EntityState.Modified;
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                new MediaManagerException(e).TratarException(string.Format(Mensagens.Ocorreu_um_erro_ao_atualizar_o_feed_0_, feed?.sDsFeed));
                return false;
            }
        }
    }
}
