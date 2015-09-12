using System;
using System.Collections.ObjectModel;
using MediaManager.Helpers;

namespace MediaManager.Model
{
    public class ConteudoGrid : Video
    {
        private bool _IsEdited;
        private bool _IsNotFound;
        private bool _IsSelected;
        public bool IsEdited { get { return _IsEdited; } set { _IsEdited = value; OnPropertyChanged("IsEdited"); } }
        public bool IsNotFound { get { return _IsNotFound; } set { _IsNotFound = value; OnPropertyChanged("IsNotFound"); if (value == true) { Title = "Sem resultados..."; Overview = "Sem resultados..."; } } }
        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; OnPropertyChanged("IsSelected"); } }
        public Video Video { get; set; }

        public ConteudoGrid()
        {
        }

        public static implicit operator ConteudoGrid(Serie serie)
        {
            ConteudoGrid conteudoGrid = new ConteudoGrid();

            conteudoGrid.FolderPath = serie.FolderPath;
            conteudoGrid.IDBanco = serie.IDBanco;
            conteudoGrid.IDApi = serie.IDApi;
            conteudoGrid.ImgFanart = serie.ImgFanart;
            conteudoGrid.ImgPoster = serie.ImgPoster;
            conteudoGrid.Language = serie.Language;
            conteudoGrid.LastUpdated = serie.LastUpdated;
            conteudoGrid.Overview = serie.Overview;
            conteudoGrid.Title = serie.Title;
            conteudoGrid.ContentType = serie.ContentType;
            conteudoGrid.SerieAliasStr = serie.SerieAliasStr;
            conteudoGrid.SerieAlias = serie.SerieAlias;

            return conteudoGrid;
        }

        public override void Clone(object objectToClone)
        {
            ConteudoGrid conteudo = objectToClone as ConteudoGrid;

            ContentType = conteudo.ContentType;
            FolderPath = conteudo.FolderPath;
            IDApi = conteudo.IDApi;
            IDBanco = conteudo.IDBanco;
            ImgFanart = conteudo.ImgFanart;
            ImgPoster = conteudo.ImgPoster;
            IsEdited = conteudo.IsEdited;
            IsNotFound = conteudo.IsNotFound;
            IsSelected = conteudo.IsSelected;
            Language = conteudo.Language;
            LastUpdated = conteudo.LastUpdated;
            Overview = conteudo.Overview;
            Title = conteudo.Title;
            SerieAlias = conteudo.SerieAlias;
            SerieAliasStr = conteudo.SerieAliasStr;
        }
    }
}