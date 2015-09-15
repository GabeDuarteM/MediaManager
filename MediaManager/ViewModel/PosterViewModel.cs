using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class PosterViewModel : INotifyPropertyChanged
    {
        private PosterGrid _poster;

        public ICommand AbrirEdicaoCommand { get; private set; }

        public PosterGrid Poster { get { return _poster; } set { _poster = value; OnPropertyChanged("Poster"); } }

        public PosterViewModel()
        {
            AbrirEdicaoCommand = new EdicaoPosterCommand(this);
        }

        public void Editar()
        {
            switch (Poster.ContentType)
            {
                case Enums.ContentType.movie:
                    {
                        //Video filme = new Filme();
                        //filme = DatabaseHelper.GetFilmePorId(Poster.ID);

                        //frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Poster.Type, filme);
                        //frmAdicionarConteudo.IsEdicao = true;
                        //frmAdicionarConteudo.ShowDialog();

                        //if (frmAdicionarConteudo.DialogResult == true)
                        //{
                        //    filme = frmAdicionarConteudo.AdicionarConteudoViewModel.Video;

                        //    Poster.ID = filme.ID;
                        //    Poster.ImgPoster = Path.Combine(filme.FolderMetadata, "poster.jpg");
                        //    Poster.Type = Enums.TipoConteudo.movie;
                        //}
                        break;
                    }
                case Enums.ContentType.show:
                    {
                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Poster.ContentType, Poster);
                        frmAdicionarConteudo.IsEdicao = true;
                        frmAdicionarConteudo.ShowDialog();

                        if (frmAdicionarConteudo.DialogResult == true)
                        {
                            if (frmAdicionarConteudo.AdicionarConteudoViewModel.SelectedVideo is PosterGrid)
                                Poster = (PosterGrid)frmAdicionarConteudo.AdicionarConteudoViewModel.SelectedVideo;
                            else if (frmAdicionarConteudo.AdicionarConteudoViewModel.SelectedVideo is Serie)
                                Poster = (Serie)frmAdicionarConteudo.AdicionarConteudoViewModel.SelectedVideo;

                            //Poster.IDBanco = serie.IDBanco;
                            //Poster.ImgPoster = Path.Combine(serie.FolderMetadata, "poster.jpg");
                            //Poster.ContentType = Enums.ContentType.show;
                        }
                        break;
                    }
                case Enums.ContentType.anime:
                    {
                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Poster.ContentType, Poster);
                        frmAdicionarConteudo.IsEdicao = true;
                        frmAdicionarConteudo.ShowDialog();

                        if (frmAdicionarConteudo.DialogResult == true)
                        {
                            if (frmAdicionarConteudo.AdicionarConteudoViewModel.SelectedVideo is PosterGrid)
                                Poster = (PosterGrid)frmAdicionarConteudo.AdicionarConteudoViewModel.SelectedVideo;
                            else if (frmAdicionarConteudo.AdicionarConteudoViewModel.SelectedVideo is Serie)
                                Poster = (Serie)frmAdicionarConteudo.AdicionarConteudoViewModel.SelectedVideo;

                            //Poster.IDBanco = anime.IDBanco;
                            //Poster.ImgPoster = Path.Combine(anime.FolderMetadata, "poster.jpg");
                            //Poster.ContentType = Enums.ContentType.anime;
                        }
                        break;
                    }
                default:
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