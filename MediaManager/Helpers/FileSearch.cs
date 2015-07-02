using System.Collections;
using System.IO;

namespace MediaManager.Helpers
{
    /// <summary>
    /// Método que pesquisa os arquivos no diretório filtrando por uma ou mais extensões
    /// Exemplo de uso:
    ///
    /// String pastaDeExecucao = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    /// FileSearch searcher = new FileSearch();
    /// searcher.SearchExtensions.AddRange(new string[] { ".mkv", ".avi", ".mp4", ".flv", ".rmvb", ".rm" });
    /// searcher.Recursive = false;
    /// FileInfo[] arquivosNaPasta = searcher.Search(pastaDeExecucao);
    ///
    /// string file = arquivosNaPasta[i].Name.ToString();
    /// string path = arquivosNaPasta[i].Directory.ToString();
    /// </summary>
    ///
    public class FileSearch
    {
        private ArrayList _extensions;
        private bool _recursive;

        public ArrayList SearchExtensions
        {
            get { return _extensions; }
        }

        public bool Recursive
        {
            get { return _recursive; }
            set { _recursive = value; }
        }

        public FileSearch()
        {
            _extensions = ArrayList.Synchronized(new ArrayList());
            _recursive = true;
        }

        public FileInfo[] Search(string path)
        {
            DirectoryInfo root = new DirectoryInfo(path);
            ArrayList subFiles = new ArrayList();
            foreach (FileInfo file in root.GetFiles())
            {
                if (_extensions.Contains(file.Extension))
                {
                    subFiles.Add(file);
                }
            }
            if (_recursive)
            {
                foreach (DirectoryInfo directory in root.GetDirectories())
                {
                    subFiles.AddRange(Search(directory.FullName));
                }
            }
            return (FileInfo[])subFiles.ToArray(typeof(FileInfo));
        }
    }
}