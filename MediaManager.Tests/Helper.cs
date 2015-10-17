using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Tests
{
    public class Helper
    {
        public static void AdicionarNovosArquivosBaixadosParaPastaDeTestes()
        {
            var arquivosPath = "D:\\Videos\\Downloads\\Completos";
            var arquivos = new DirectoryInfo(arquivosPath).EnumerateFiles("*.*", SearchOption.AllDirectories);
            foreach (var item in arquivos)
            {
                var pathDownloadsFake = "D:\\Videos Testes Fake\\[[ Downloads ]]";
                var filename = Path.Combine(pathDownloadsFake, item.Directory.FullName.Replace(arquivosPath + "\\", "").Replace(arquivosPath, ""), item.Name);
                var filepath = Path.Combine(pathDownloadsFake, item.Directory.FullName.Replace(arquivosPath + "\\", "").Replace(arquivosPath, ""));
                if (!File.Exists(filename))
                {
                    if (!File.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    using (File.Create(filename)) { }
                }
            }
        }
    }
}