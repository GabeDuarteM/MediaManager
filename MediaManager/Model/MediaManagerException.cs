// Developed by: Gabriel Duarte
// 
// Created at: 04/02/2016 20:36

using System;
using System.Collections.Generic;
using System.Linq;
using MediaManager.Helpers;
using MediaManager.Localizacao;

namespace MediaManager.Model
{
    public class MediaManagerException : Exception
    {
        public MediaManagerException(Exception e)
        {
            lstDetalhes = new List<string>();
            Exception = e;
        }

        public MediaManagerException(string e)
        {
            lstDetalhes = new List<string>();
            Exception = new Exception(e);
        }

        private List<string> lstDetalhes { get; }

        private Exception Exception { get; }

        public void TratarException(string sErro = "Ocorreu um erro na aplicação.", bool bIsSilencioso = true)
        {
            var sDetalhes = "";

            if (sErro.Last() != '.')
            {
                sErro += ".";
            }

            sDetalhes += $"Detalhes: {Exception}";

            foreach (string item in lstDetalhes)
            {
                sDetalhes += Environment.NewLine + item;
            }

            string sMensagem = sErro + Environment.NewLine + sDetalhes;

            Helper.LogMessage(sMensagem);

            if (!bIsSilencioso)
            {
                Helper.MostrarMensagem(sErro + Environment.NewLine + Environment.NewLine +
                                       Mensagens.Verifique_mais_detalhes_no_Log_,
                                       Enums.eTipoMensagem.Erro);
            }
        }
    }
}
