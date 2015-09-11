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

        public Action CloseAction { get; set; } // Para poder fechar depois no RenomearCommand

        public ObservableCollection<EpisodeToRename> Episodes { get { return _Episodes; } set { _Episodes = value; OnPropertyChanged("Episodes"); } }

        public ICommand RenomearCommand { get; set; } = new RenomearCommand();

        public RenomearViewModel(IEnumerable<FileInfo> arquivos = null)
        {
            if (arquivos == null)
                arquivos = new DirectoryInfo(Properties.Settings.Default.pref_PastaDownloads).EnumerateFiles("*.*", SearchOption.AllDirectories);
            Carregar(arquivos);
        }

        private async void Carregar(IEnumerable<FileInfo> arquivos)
        {
            string[] extensoesPermitidas = Properties.Settings.Default.ExtensoesRenomeioPermitidas.Split('|');
            List<EpisodeToRename> listaEpisodios = new List<EpisodeToRename>();
            List<EpisodeToRename> listaEpisodiosNotFound = new List<EpisodeToRename>();

            foreach (var item in arquivos)
            {
                if (extensoesPermitidas.Contains(item.Extension))
                {
                    EpisodeToRename episode = new EpisodeToRename();
                    episode.Filename = item.Name;
                    episode.FolderPath = item.DirectoryName;
                    if (DBHelper.VerificarSeEpisodioJaFoiRenomeado(item.FullName))
                        continue;
                    if (await episode.GetEpisodeAsync())
                    {
                        episode.OriginalFilePath = item.FullName;
                        episode.FilenameRenamed = episode.Serie.IsAnime
                            ? episode.ParentTitle + " - " + string.Format("{0:00}", episode.AbsoluteNumber) + " - " + episode.EpisodeName + item.Extension
                            : episode.ParentTitle + " - " + episode.SeasonNumber + "x" + string.Format("{0:00}", episode.EpisodeNumber) + " - " + episode.EpisodeName + item.Extension;
                        episode.FilenameRenamed = Helper.RetirarCaracteresInvalidos(episode.FilenameRenamed);
                        episode.IsRenamed = true;
                        listaEpisodios.Add(episode);
                    }
                    else
                    {
                        episode.FilenameRenamed = episode.Filename;
                        listaEpisodiosNotFound.Add(episode);
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