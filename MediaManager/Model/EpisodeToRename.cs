using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MediaManager.Helpers;

namespace MediaManager.Model
{
    [System.Diagnostics.DebuggerDisplay("{FilenameRenamed}"), NotMapped]
    public class EpisodeToRenames : Episodio, INotifyPropertyChanged
    {
        private string _Filename;

        private string _FilenameRenamed;

        //public string Filename { get { return _Filename; } set { _Filename = value; OnPropertyChanged(); } }

        //public string FilenameRenamed { get { return _FilenameRenamed; } set { _FilenameRenamed = value; OnPropertyChanged(); } }

        //public string ParentTitle { get; set; }

        public EpisodeToRenames(Episodio episode)
        {
            //if (episode == null)
            //    return;
            //AbsoluteNumber = episode.AbsoluteNumber;
            //AirsAfterSeason = episode.AirsAfterSeason;
            //AirsBeforeEpisode = episode.AirsBeforeEpisode;
            //AirsBeforeSeason = episode.AirsBeforeSeason;
            //Artwork = episode.Artwork;
            //EpisodeName = episode.EpisodeName;
            //EpisodeNumber = episode.EpisodeNumber;
            //FirstAired = episode.FirstAired;
            //FolderPath = episode.FolderPath;
            //IDBanco = episode.IDBanco;
            //IDSeasonTvdb = episode.IDSeasonTvdb;
            //IDSerie = episode.IDSerie;
            //IDSeriesAPI = episode.IDSeriesAPI;
            //IDApi = episode.IDApi;
            //IsRenamed = episode.IsRenamed;
            //Language = episode.Language;
            //LastUpdated = episode.LastUpdated;
            //OriginalFilePath = episode.OriginalFilePath;
            //Overview = episode.Overview;
            //Rating = episode.Rating;
            //RatingCount = episode.RatingCount;
            //SeasonNumber = episode.SeasonNumber;
            //Serie = episode.Serie != null ? episode.Serie : null;
            //ThumbAddedDate = episode.ThumbAddedDate;
        }

        public EpisodeToRenames()
        {
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
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