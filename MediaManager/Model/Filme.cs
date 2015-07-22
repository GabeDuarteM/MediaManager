using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MediaManager.Model
{
    [Table("Filmes")]
    public class Filme : Video
    {
        private IList<string> _availableTranslations;
        private IList<string> _genres;
        private int _idFilme;
        private Ids _ids = new Ids();

        [JsonProperty("available_translations", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> AvailableTranslations { get { return _availableTranslations; } set { _availableTranslations = value; Helpers.Helper.ListToString(value); } }

        [JsonProperty("certification")]
        public string Certification { get; set; }

        public string FolderPath { get; set; }

        public string Generos { get; private set; }

        [JsonProperty("genres", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Genres { get { return _genres; } set { _genres = value; Generos = Helpers.Helper.ListToString(value); } }

        [JsonProperty("homepage", NullValueHandling = NullValueHandling.Ignore)]
        public object Homepage { get; set; }

        [Key]
        public int IDFilme { get { return _idFilme; } set { _idFilme = value; _ids.IdBanco = value; } }

        [JsonProperty("ids")]
        public virtual Ids Ids { get { return _ids; } set { _ids = value; } }

        [JsonProperty("images")]
        public virtual Images Images { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        public string MetadataFolder { get; set; }

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

        [JsonProperty("title")]
        public string Title { get; set; }

        public string Traducoes { get; private set; }

        [JsonProperty("trailer", NullValueHandling = NullValueHandling.Ignore)]
        public object Trailer { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime UpdatedAt { get; set; }

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