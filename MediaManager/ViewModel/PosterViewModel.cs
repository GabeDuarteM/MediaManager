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
            switch (Poster.TipoConteudo)
            {
                case Helper.Enums.TipoConteudo.movie:
                    {
                        Video filme = new Filme();
                        filme = DatabaseHelper.GetFilmePorId(Poster.IdBanco);

                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Poster.TipoConteudo, filme);
                        frmAdicionarConteudo.IsEdicao = true;
                        frmAdicionarConteudo.ShowDialog();

                        if (frmAdicionarConteudo.DialogResult == true)
                        {
                            filme = frmAdicionarConteudo.AdicionarConteudoViewModel.Video;

                            Poster.IdBanco = filme.ID;
                            Poster.PosterPath = Path.Combine(filme.MetadataFolder, "poster.jpg");
                            Poster.TipoConteudo = Helper.Enums.TipoConteudo.movie;
                        }
                        break;
                    }
                case Helper.Enums.TipoConteudo.show:
                    {
                        Video serie = new Serie();
                        serie = DatabaseHelper.GetSeriePorId(Poster.IdBanco);

                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Poster.TipoConteudo, serie);
                        frmAdicionarConteudo.IsEdicao = true;
                        frmAdicionarConteudo.ShowDialog();

                        if (frmAdicionarConteudo.DialogResult == true)
                        {
                            serie = frmAdicionarConteudo.AdicionarConteudoViewModel.Video;

                            Poster.IdBanco = serie.ID;
                            Poster.PosterPath = Path.Combine(serie.MetadataFolder, "poster.jpg");
                            Poster.TipoConteudo = Helper.Enums.TipoConteudo.show;
                        }
                        break;
                    }
                case Helper.Enums.TipoConteudo.anime:
                    {
                        Video anime = new Serie();
                        anime = DatabaseHelper.GetAnimePorId(Poster.IdBanco);

                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Poster.TipoConteudo, anime);
                        frmAdicionarConteudo.IsEdicao = true;
                        frmAdicionarConteudo.ShowDialog();

                        if (frmAdicionarConteudo.DialogResult == true)
                        {
                            anime = frmAdicionarConteudo.AdicionarConteudoViewModel.Video;

                            Poster.IdBanco = anime.ID;
                            Poster.PosterPath = Path.Combine(anime.MetadataFolder, "poster.jpg");
                            Poster.TipoConteudo = Helper.Enums.TipoConteudo.anime;
                        }
                        break;
                    }
                default:
                    break;
            }
        }
    }
}