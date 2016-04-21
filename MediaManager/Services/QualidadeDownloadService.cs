// Developed by: Gabriel Duarte
// 
// Created at: 10/04/2016 20:04

using MediaManager.Localizacao;
using MediaManager.Model;

namespace MediaManager.Services
{
    public class QualidadeDownloadService : RepositorioBase<QualidadeDownload>
    {
        public QualidadeDownloadService(IContext context) :
            base(context,
                 new[] {nameof(QualidadeDownload.sQualidade), nameof(QualidadeDownload.nCdQualidadeDownload)},
                 Mensagens.Ocorreu_um_erro_ao_adicionar_a_qualidade_0_,
                 Mensagens.Ocorreu_um_erro_ao_pesquisar_a_qualidade_de_download_de_código_0_,
                 Mensagens.Ocorreu_um_erro_ao_retornar_a_lista_de_qualidade_de_download,
                 Mensagens.Ocorreu_um_erro_ao_remover_a_qualidade_0_,
                 Mensagens.Ocorreu_um_erro_ao_atualizar_a_qualidade_0_)
        {
        }
    }
}
