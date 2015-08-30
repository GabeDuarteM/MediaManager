using System;

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

        public static implicit operator ConteudoGrid(Serie v)
        {
            ConteudoGrid conteudoGrid = new ConteudoGrid();

            conteudoGrid.FolderPath = v.FolderPath;
            conteudoGrid.IDBanco = v.IDBanco;
            conteudoGrid.IDApi = v.IDApi;
            conteudoGrid.ImgFanart = v.ImgFanart;
            conteudoGrid.ImgPoster = v.ImgPoster;
            conteudoGrid.Language = v.Language;
            conteudoGrid.LastUpdated = v.LastUpdated;
            conteudoGrid.Overview = v.Overview;
            conteudoGrid.Title = v.Title;
            conteudoGrid.ContentType = v.ContentType;
            conteudoGrid.AliasNames = v.AliasNames;

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
            AliasNames = conteudo.AliasNames;
        }
    }
}