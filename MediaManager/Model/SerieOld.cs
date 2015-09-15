using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using MediaManager.Helpers;
using Newtonsoft.Json;

namespace MediaManager.Model
{
    //[Table("Series")]
    public class SerieOld
    {
        private List<string> _availableTranslations;
        private string _folderPath;
        private List<string> _genresList;
        private int _id;
        private Ids _ids;
        private Images _images;
        private string _overview;
        private string _title;

        [JsonProperty("day")]
        public string AirDay { get; set; }

        [JsonProperty("aired_episodes", NullValueHandling = NullValueHandling.Ignore)]
        public int AiredEpisodes { get; set; }

        [JsonProperty("time")]
        public string AirTime { get; set; }

        [JsonProperty("timezone")]
        public string AirTimezone { get; set; }

        [JsonProperty("available_translations")]
        public List<string> AvailableTranslations { get { return _availableTranslations; } set { _availableTranslations = value; Traducoes = Helper.ListToString(value); } }

        [JsonProperty("certification")]
        public string Certification { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        public string FolderMetadata { get; set; }
        public string FolderPath { get { return _folderPath; } set { _folderPath = value; OnPropertyChanged("FolderPath"); } }

        public string Generos { get; private set; }

        [JsonProperty("genres")]
        public List<string> GenresList { get { return _genresList; } set { _genresList = value; Generos = Helper.ListToString(value); } }

        [JsonProperty("homepage")]
        public object Homepage { get; set; }

        //[Key, Column(Order = 0)]
        public int IDBanco { get { return _id; } set { _id = value; } }

        [JsonProperty("ids"), NotMapped]
        public virtual Ids Ids { get { return _ids; } set { _ids = value; } }

        [JsonProperty("images"), NotMapped]
        public virtual Images Images { get { return _images; } set { _images = value; OnPropertyChanged("Images"); } }

        public bool IsAnime { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? LastUpdated { get; set; }

        [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
        public double Rating { get; set; }

        [JsonProperty("runtime", NullValueHandling = NullValueHandling.Ignore)]
        public int Runtime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [NotMapped]
        public string TipoString { get { return Enums.ToString(Type); } }

        public string Traducoes { get; private set; }

        [JsonProperty("trailer")]
        public object Trailer { get; set; }

        [NotMapped]
        public Enums.ContentType Type { get { return Enums.ContentType.show; } set { throw new NotSupportedException(); } }

        [JsonProperty("votes", NullValueHandling = NullValueHandling.Ignore)]
        public int Votes { get; set; }

        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public int Year { get; set; }

        #region TheTVDB

        [XmlElement("AliasNames")]
        public string AliasNames { get; set; }

        [JsonProperty("first_aired", NullValueHandling = NullValueHandling.Ignore), XmlElement("FirstAired")]
        public DateTime? FirstAired { get; set; }

        public int IDApi
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        [XmlElement("seriesid")]
        public int IDTvdb { get; set; }

        public string ImgFanart
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string ImgPoster
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        [JsonProperty("language"), XmlElement("language")]
        public string Language { get; set; }

        [JsonProperty("network"), XmlElement("Network")]
        public string Network { get; set; }

        [JsonProperty("overview"), XmlElement("Overview")]
        public string Overview { get { return _overview; } set { _overview = value; OnPropertyChanged("Overview"); } }

        [JsonProperty("title"), XmlElement("SeriesName")]
        public string Title { get { return _title; } set { _title = value; OnPropertyChanged("Title"); } }

        public string TypeString
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion TheTVDB

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