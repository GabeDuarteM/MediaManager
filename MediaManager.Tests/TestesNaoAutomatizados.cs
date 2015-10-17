using System;
using System.IO;
using MediaManager.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaManager.Tests
{
    [TestClass]
    public class TestesNaoAutomatizados
    {
        [TestMethod]
        public void TestesNoDiretorioDeDownloads()
        {
            Helper.AdicionarNovosArquivosBaixadosParaPastaDeTestes();
            var arquivos = new DirectoryInfo("D:\\Videos Testes Fake\\[[ Downloads ]]").EnumerateFiles("*.*", SearchOption.AllDirectories);

            foreach (var item in arquivos)
            {
                EpisodeToRename episode = new EpisodeToRename() { Filename = item.Name, FolderPath = item.Directory.FullName };
                episode.GetEpisode();
            }
        }
    }
}