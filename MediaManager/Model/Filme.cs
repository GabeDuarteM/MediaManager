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
        [Key]
        public int IDFilme { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        public string FolderPath { get; set; }

        [JsonProperty("images")]
        public virtual Images Images { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        public string MetadataFolder { get; set; }

        public string Generos { get; set; }

        public string Traducoes { get; set; }

        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public int Year { get; set; }

        [JsonProperty("ids")]
        public virtual Ids Ids { get; set; }

        [JsonProperty("tagline")]
        public string Tagline { get; set; }

        [JsonProperty("released")]
        public string Released { get; set; }

        [JsonProperty("runtime", NullValueHandling = NullValueHandling.Ignore)]
        public int Runtime { get; set; }

        [JsonProperty("trailer", NullValueHandling = NullValueHandling.Ignore)]
        public object Trailer { get; set; }

        [JsonProperty("homepage", NullValueHandling = NullValueHandling.Ignore)]
        public object Homepage { get; set; }

        [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
        public double Rating { get; set; }

        [JsonProperty("votes", NullValueHandling = NullValueHandling.Ignore)]
        public int Votes { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("available_translations", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> AvailableTranslations { get; set; }

        [JsonProperty("genres", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Genres { get; set; }

        [JsonProperty("certification")]
        public string Certification { get; set; }

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