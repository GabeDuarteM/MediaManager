// Developed by: Gabriel Duarte
// 
// Created at: 10/04/2016 20:04
// Last update: 19/04/2016 02:47

using MediaManager.Model;

namespace MediaManager.Services
{
    public class QualidadeDownloadService : RepositorioBase<QualidadeDownload>
    {
        public QualidadeDownloadService(IContext context) :
            base(context,
                 new[] {nameof(QualidadeDownload.sQualidade), nameof(QualidadeDownload.nCdQualidadeDownload)},
                 "Ocorreu um erro ao adicionar a qualidade {0}",
                 "Ocorreu um erro ao pesquisar a qualidade de download de código {1}",
                 "Ocorreu um erro ao retornar a lista de qualidade de downloads.",
                 "Ocorreu um erro ao remover a qualidade {0}",
                 "Ocorreu um erro ao atualizar a qualidade {0}")
        {
        }
    }
}
