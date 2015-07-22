using System;
using System.ComponentModel;

namespace MediaManager.Model
{
    public class ConteudoGrid : INotifyPropertyChanged
    {
        private bool _isAlterado;
        private bool _isSelected;
        private string _nome;
        private string _pasta;
        private Helpers.Helper.Enums.TipoConteudo _tipoConteudo;
        private string _tipoConteudoString;
        private string _traktSlug;

        public bool IsAlterado
        {
            get { return _isAlterado; }
            set { _isAlterado = value; OnPropertyChanged("IsAlterado"); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged("IsSelected"); }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; OnPropertyChanged("Nome"); }
        }

        public string Pasta
        {
            get { return _pasta; }
            set { _pasta = value; OnPropertyChanged("Pasta"); }
        }

        public Helpers.Helper.Enums.TipoConteudo TipoConteudo { get { return _tipoConteudo; } set { _tipoConteudo = value; OnPropertyChanged("TipoConteudo"); DefinirTipoConteudoString(); } }

        public string TipoConteudoString { get { return _tipoConteudoString; } }

        public string TraktSlug { get { return _traktSlug; } set { _traktSlug = value; OnPropertyChanged("TraktSlug"); } }

        /// <summary>
        /// Realiza a conversão do objeto para o tipo Video. O objeto retornado irá conter somente a pasta e o título informado no ConteudoGrid.
        /// </summary>
        /// <param name="conteudoGrid">Objeto a ser convertido.</param>
        /// <returns>Retorna um objeto Video, contendo SOMENTE a pasta e o título do ConteudoGrid.</returns>
        public Video ToVideo()
        {
            Video video = new Serie();
            video.FolderPath = Pasta;
            video.Title = Nome;
            return video;
        }

        private void DefinirTipoConteudoString()
        {
            switch (TipoConteudo)
            {
                case Helpers.Helper.Enums.TipoConteudo.unknown:
                    _tipoConteudoString = "Desconhecido";
                    break;

                case Helpers.Helper.Enums.TipoConteudo.movie:
                    _tipoConteudoString = "Filme";
                    break;

                case Helpers.Helper.Enums.TipoConteudo.show:
                    _tipoConteudoString = "Série";
                    break;

                case Helpers.Helper.Enums.TipoConteudo.anime:
                    _tipoConteudoString = "Anime";
                    break;

                case Helpers.Helper.Enums.TipoConteudo.season:
                    _tipoConteudoString = "Temporada";
                    break;

                case Helpers.Helper.Enums.TipoConteudo.episode:
                    _tipoConteudoString = "Episódio";
                    break;

                case Helpers.Helper.Enums.TipoConteudo.person:
                    _tipoConteudoString = "Pessoa";
                    break;

                case Helpers.Helper.Enums.TipoConteudo.movieShowAnime:
                    _tipoConteudoString = "Filme, Serie e Anime";
                    break;

                default:
                    _tipoConteudoString = null;
                    break;
            }
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