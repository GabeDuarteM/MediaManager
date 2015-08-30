using System.IO;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class PosterViewModel
    {
        private PosterGrid _poster;

        public ICommand AbrirEdicaoCommand { get; private set; }

        public PosterGrid Poster { get { return _poster; } set { _poster = value; } }

        public PosterViewModel()
        {
            AbrirEdicaoCommand = new EdicaoPosterCommand(this);
        }

        internal void Editar()
        {
            switch (Poster.ContentType)
            {
                case Helper.Enums.ContentType.movie:
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
                        //    Poster.Type = Helper.Enums.TipoConteudo.movie;
                        //}
                        break;
                    }
                case Helper.Enums.ContentType.show:
                    {
                        Video serie = new Serie();
                        serie = DatabaseHelper.GetSeriePorID(Poster.IDBanco);

                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Poster.ContentType, serie);
                        frmAdicionarConteudo.IsEdicao = true;
                        frmAdicionarConteudo.ShowDialog();

                        if (frmAdicionarConteudo.DialogResult == true)
                        {
                            serie = frmAdicionarConteudo.AdicionarConteudoViewModel.Video;

                            Poster.IDBanco = serie.IDBanco;
                            Poster.ImgPoster = Path.Combine(serie.FolderMetadata, "poster.jpg");
                            Poster.ContentType = Helper.Enums.ContentType.show;
                        }
                        break;
                    }
                case Helper.Enums.ContentType.anime:
                    {
                        Video anime = new Serie();
                        anime = DatabaseHelper.GetAnimePorID(Poster.IDBanco);

                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Poster.ContentType, anime);
                        frmAdicionarConteudo.IsEdicao = true;
                        frmAdicionarConteudo.ShowDialog();

                        if (frmAdicionarConteudo.DialogResult == true)
                        {
                            anime = frmAdicionarConteudo.AdicionarConteudoViewModel.Video;

                            Poster.IDBanco = anime.IDBanco;
                            Poster.ImgPoster = Path.Combine(anime.FolderMetadata, "poster.jpg");
                            Poster.ContentType = Helper.Enums.ContentType.anime;
                        }
                        break;
                    }
                default:
                    break;
            }
        }
    }
}