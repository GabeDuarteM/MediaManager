using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MediaManager.Helpers;
using MediaManager.Properties;

namespace MediaManager.Model
{
    public class MediaManagerException
    {
        public List<string> lstDetalhes { get; set; }
        public Exception Exception { get; }

        public MediaManagerException(Exception e)
        {
            lstDetalhes = new List<string>();
            Exception = e;
        }

        public void TratarException(string sErro = "Ocorreu um erro na aplicação.", bool bIsSilencioso = true)
        {
            var sDetalhes = "";
            var sMensagem = "";

            if (sErro.Last() != '.')
            {
                sErro += ".";
            }

            sDetalhes += $"Detalhes: " + Exception.ToString();

            foreach (var item in lstDetalhes)
            {
                sDetalhes += Environment.NewLine + item;
            }

            sMensagem = sErro + Environment.NewLine + sDetalhes;

            //if (Exception.StackTrace != null)
            //{
            //    mensagem += "\r\nStackTrace: " + Exception.StackTrace;
            //}

            if (bIsSilencioso)
            {
                Helper.LogMessage(sMensagem);
            }
            else
            {
                Helper.LogMessage(sMensagem);
                Helper.MostrarMensagem(sErro + Environment.NewLine + Environment.NewLine + "Verifique mais detalhes no Log.", Enums.eTipoMensagem.Erro);
            }
        }
    }
}