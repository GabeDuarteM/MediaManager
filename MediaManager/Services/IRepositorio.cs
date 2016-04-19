// Developed by: Gabriel Duarte
// 
// Created at: 15/12/2015 21:14
// Last update: 19/04/2016 02:57

using System.Collections.Generic;

namespace MediaManager.Services
{
    public interface IRepositorio<T> where T : class
    {
        bool Adicionar(params T[] obj);

        bool Remover(params T[] obj);

        bool Update(params T[] obj);

        List<T> GetLista();

        T Get(int id);
    }
}
