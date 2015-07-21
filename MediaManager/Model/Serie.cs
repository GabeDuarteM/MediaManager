using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MediaManager.Model
{
    [Table("Series")]
    public class Serie : Video
    {
        private string _title;
        private string _folderPath;
        private string _overview;
        private Images _images;

        [Key]
        public int IDSerie { get; set; }

        [JsonProperty("title")]
        public string Title { get { return _title; } set { _title = value; OnPropertyChanged("Title"); } }

        public string FolderPath { get { return _folderPath; } set { _folderPath = value; OnPropertyChanged("FolderPath"); } }

        [JsonProperty("images")]
        public virtual Images Images { get { return _images; } set { _images = value; OnPropertyChanged("Images"); } }

        [JsonProperty("overview")]
        public string Overview { get { return _overview; } set { _overview = value; OnPropertyChanged("Overview"); } }

        public string MetadataFolder { get; set; }

        public bool IsAnime { get; set; }

        public string Traducoes { get; set; }

        public string Generos { get; set; }

        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public int Year { get; set; }

        [JsonProperty("ids")]
        public virtual Ids Ids { get; set; }

        [JsonProperty("first_aired", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? FirstAired { get; set; }

        [JsonProperty("runtime", NullValueHandling = NullValueHandling.Ignore)]
        public int Runtime { get; set; }

        [JsonProperty("certification")]
        public string Certification { get; set; }

        [JsonProperty("network")]
        public string Network { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("trailer")]
        public object Trailer { get; set; }

        [JsonProperty("homepage")]
        public object Homepage { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
        public double Rating { get; set; }

        [JsonProperty("votes", NullValueHandling = NullValueHandling.Ignore)]
        public int Votes { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("available_translations")]
        public IList<string> AvailableTranslations { get; set; }

        [JsonProperty("genres")]
        public IList<string> Genres { get; set; }

        [JsonProperty("aired_episodes", NullValueHandling = NullValueHandling.Ignore)]
        public int AiredEpisodes { get; set; }

        [JsonProperty("day")]
        public string AirDay { get; set; }

        [JsonProperty("time")]
        public string AirTime { get; set; }

        [JsonProperty("timezone")]
        public string AirTimezone { get; set; }

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