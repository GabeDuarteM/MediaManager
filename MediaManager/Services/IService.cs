using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.Services
{
    public interface IService<T> where T : class
    {
        bool Adicionar(params T[] obj);

        bool Remover(params T[] obj);

        bool Update(params T[] obj);

        List<T> GetLista();

        T Get(int ID);
    }
}