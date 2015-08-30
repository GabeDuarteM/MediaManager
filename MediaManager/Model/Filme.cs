using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediaManager.Helpers;
using Newtonsoft.Json;

namespace MediaManager.Model
{
    [Table("Filmes")]
    public class Filme
    {
        private IList<string> _availableTranslations;
        private IList<string> _genresList;
        private int _id;
        private Ids _ids;

        [JsonProperty("available_translations", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> AvailableTranslations { get { return _availableTranslations; } set { _availableTranslations = value; Traducoes = Helper.ListToString(value); } }

        [JsonProperty("certification")]
        public string Certification { get; set; }

        public string FolderMetadata { get; set; }
        public string FolderPath { get; set; }

        public string Generos { get; private set; }

        [JsonProperty("genres", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> GenresList { get { return _genresList; } set { _genresList = value; Generos = Helper.ListToString(value); } }

        [JsonProperty("homepage", NullValueHandling = NullValueHandling.Ignore)]
        public object Homepage { get; set; }

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

        [Key, Column(Order = 0)]
        public int IDBanco { get { return _id; } set { _id = value; } }

        [JsonProperty("ids"), NotMapped]
        public virtual Ids Ids { get { return _ids; } set { _ids = value; } }

        [JsonProperty("images"), NotMapped]
        public virtual Images Images { get; set; }

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

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? LastUpdated { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
        public double Rating { get; set; }

        [JsonProperty("released")]
        public string Released { get; set; }

        [JsonProperty("runtime", NullValueHandling = NullValueHandling.Ignore)]
        public int Runtime { get; set; }

        [JsonProperty("tagline")]
        public string Tagline { get; set; }

        [NotMapped]
        public string TipoString { get { return Helper.Enums.ToString(Type); } }

        [JsonProperty("title")]
        public string Title { get; set; }

        public string Traducoes { get; private set; }

        [JsonProperty("trailer", NullValueHandling = NullValueHandling.Ignore)]
        public object Trailer { get; set; }

        [NotMapped]
        public Helper.Enums.ContentType Type { get { return Helper.Enums.ContentType.movie; } set { throw new NotSupportedException(); } }

        public string TypeString
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        [JsonProperty("votes", NullValueHandling = NullValueHandling.Ignore)]
        public int Votes { get; set; }

        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public int Year { get; set; }

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