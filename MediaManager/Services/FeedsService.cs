using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.Services
{
    public class FeedsService : IService<Feed>
    {
        private IContext _context;

        public FeedsService(IContext context)
        {
            _context = context;
        }

        public bool Adicionar(params Feed[] obj)
        {
            foreach (var feed in obj)
            {
                try
                {
                    var lstFeeds = _context.Feed.Where(x => x.nIdTipoConteudo == feed.nIdTipoConteudo && x.bIsFeedPesquisa == feed.bIsFeedPesquisa).ToList();

                    // Para não interferir no CommandSalvar da tela de adicionar feed quando o feed vai ser adicionado a mais de um tipo de conteúdo
                    // (caso contrário nunca vai cair no if acima, pois a prioridade será != 0)
                    Feed feedClone = new Feed(feed);

                    if (feedClone.nNrPrioridade == 0)
                    {
                        feedClone.nNrPrioridade = (lstFeeds.Count > 0) ? lstFeeds.Last().nNrPrioridade + 1 : 1;
                    }

                    _context.Feed.Add(feedClone);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    Helper.TratarException(e, "Ocorreu um erro ao adicionar o feed " + feed.sDsFeed, true);
                    return false;
                }
            }

            return true;
        }

        public Feed Get(int ID)
        {
            try
            {
                Feed oFeed = _context.Feed.Where(x => x.nCdFeed == ID).FirstOrDefault();
                return oFeed;
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao pesquisar o feed de código " + ID); return null; }
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
                Helper.TratarException(e, "Ocorreu um erro ao retornar a lista de feeds."); return null;
            }
        }

        public bool Remover(params Feed[] obj)
        {
            foreach (var feed in obj)
            {
                try
                {
                    feed.Clone(new Feed() { nCdFeed = feed.nCdFeed });
                    _context.Feed.Attach(feed);
                    _context.Feed.Remove(feed);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    Helper.TratarException(e, "Ocorreu um erro ao remover o feed " + feed.sDsFeed, true);
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
                foreach (var item in obj)
                {
                    feed = item;
                    _context.Feed.Attach(item);
                    var entry = _context.Entry(item);
                    entry.State = System.Data.Entity.EntityState.Modified;
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Helper.TratarException(e, "Ocorreu um erro ao atualizar o feed " + feed.sDsFeed, true);
                return false;
            }
        }
    }
}