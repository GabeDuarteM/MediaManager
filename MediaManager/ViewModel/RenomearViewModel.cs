using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class RenomearViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<EpisodeToRename> _Episodes;

        public bool IsSilencioso { get; set; }

        public Action CloseAction { get; set; } // Para poder fechar depois no RenomearCommand

        public ObservableCollection<EpisodeToRename> Episodes { get { return _Episodes; } set { _Episodes = value; OnPropertyChanged("Episodes"); } }

        public ICommand RenomearCommand { get; set; } = new RenomearCommand();

        public RenomearViewModel(IEnumerable<FileInfo> arquivos = null)
        {
            if (arquivos == null)
                arquivos = new DirectoryInfo(Properties.Settings.Default.pref_PastaDownloads).EnumerateFiles("*.*", SearchOption.AllDirectories);
            Carregar(arquivos);
        }

        private void Carregar(IEnumerable<FileInfo> arquivos)
        {
            Episodes = new ObservableCollection<EpisodeToRename>();
            Episodes.Add(new EpisodeToRename() { ParentTitle = "Carregando...", OriginalFilePath = "Carregando...", FilenameRenamed = "Carregando...", IsSelected = false });

            string[] extensoesPermitidas = Properties.Settings.Default.ExtensoesRenomeioPermitidas.Split('|');
            List<EpisodeToRename> listaEpisodios = new List<EpisodeToRename>();
            List<EpisodeToRename> listaEpisodiosNotFound = new List<EpisodeToRename>();

            foreach (var item in arquivos)
            {
                if (extensoesPermitidas.Contains(item.Extension))
                {
                    EpisodeToRename episodio = new EpisodeToRename();
                    episodio.Filename = item.Name;
                    episodio.FolderPath = item.DirectoryName;
                    if (DBHelper.VerificarSeEpisodioJaFoiRenomeado(item.FullName))
                        continue;
                    if (episodio.GetEpisode())
                    {
                        episodio.OriginalFilePath = item.FullName;
                        //string numeroEpisodio = episodio.Serie.IsAnime ? null : episodio.SeasonNumber + "x";
                        //foreach (var arrayItem in episodio.EpisodeArray)
                        //{
                        //    if (episodio.Serie.IsAnime)
                        //    {
                        //        if (arrayItem != episodio.EpisodeArray[0])
                        //            numeroEpisodio += " & ";
                        //        numeroEpisodio += int.Parse(arrayItem).ToString("00");
                        //    }
                        //    else
                        //    {
                        //        if (arrayItem != episodio.EpisodeArray[0])
                        //            numeroEpisodio += "x";
                        //        numeroEpisodio += int.Parse(arrayItem).ToString("00");
                        //    }
                        //}
                        //episodio.FilenameRenamed = episodio.ParentTitle + " - " + numeroEpisodio + " - " + episodio.EpisodeName + item.Extension;
                        //episodio.FilenameRenamed = Helper.RetirarCaracteresInvalidos(episodio.FilenameRenamed);
                        episodio.FilenameRenamed = Helper.RenomearConformePreferencias(episodio) + item.Extension;
                        episodio.IsRenamed = true;
                        listaEpisodios.Add(episodio);
                    }
                    else
                    {
                        episodio.FilenameRenamed = episodio.Filename;
                        listaEpisodiosNotFound.Add(episodio);
                    }
                }
            }
            //foreach (var item in listaEpisodiosNotFound)
            //{
            //    TextWriter tw = new StreamWriter(@"C:/Episodios não encontrados.txt", true);
            //    tw.WriteLine(item.Filename);
            //    tw.Close();
            //}
            Episodes = new ObservableCollection<EpisodeToRename>(listaEpisodios);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members
    }
}