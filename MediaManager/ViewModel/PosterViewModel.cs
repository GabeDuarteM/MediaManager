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

        public PosterViewModel()
        {
            AbrirEdicaoCommand = new EdicaoPosterCommand(this);
        }

        public ICommand AbrirEdicaoCommand { get; private set; }

        public PosterGrid Poster { get { return _poster; } set { _poster = value; } }

        internal void Editar()
        {
            switch (Poster.TipoConteudo)
            {
                case MediaManager.Helpers.Helper.TipoConteudo.movie:
                    {
                        Filme filme = new Filme();
                        filme = DatabaseHelper.GetFilmePorId(Poster.IdBanco);

                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Poster.TipoConteudo, filme);
                        frmAdicionarConteudo.ShowDialog();

                        if (frmAdicionarConteudo.DialogResult == true)
                        {
                            filme = frmAdicionarConteudo.Filme;

                            Poster.IdBanco = filme.IDFilme;
                            Poster.PosterPath = (File.Exists(Path.Combine(filme.MetadataFolder, "poster.jpg"))) ?
                                Path.Combine(filme.MetadataFolder, "poster.jpg") : "";
                            Poster.TipoConteudo = Helper.TipoConteudo.movie;
                        }
                        break;
                    }
                case MediaManager.Helpers.Helper.TipoConteudo.show:
                    {
                        Serie serie = new Serie();
                        serie = DatabaseHelper.GetSeriePorId(Poster.IdBanco);

                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Poster.TipoConteudo, serie);
                        frmAdicionarConteudo.ShowDialog();

                        if (frmAdicionarConteudo.DialogResult == true)
                        {
                            serie = frmAdicionarConteudo.Serie;

                            Poster.IdBanco = serie.IDSerie;
                            Poster.PosterPath = (File.Exists(Path.Combine(serie.MetadataFolder, "poster.jpg"))) ?
                                Path.Combine(serie.MetadataFolder, "poster.jpg") : "";
                            Poster.TipoConteudo = Helper.TipoConteudo.show;
                        }
                        break;
                    }
                case MediaManager.Helpers.Helper.TipoConteudo.anime:
                    {
                        Serie anime = new Serie();
                        anime = DatabaseHelper.GetAnimePorId(Poster.IdBanco);

                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Poster.TipoConteudo, anime);
                        frmAdicionarConteudo.ShowDialog();

                        if (frmAdicionarConteudo.DialogResult == true)
                        {
                            anime = frmAdicionarConteudo.Serie;

                            Poster.IdBanco = anime.IDSerie;
                            Poster.PosterPath = (File.Exists(Path.Combine(anime.MetadataFolder, "poster.jpg"))) ?
                                Path.Combine(anime.MetadataFolder, "poster.jpg") : "";
                            Poster.TipoConteudo = Helper.TipoConteudo.anime;
                        }
                        break;
                    }
                default:
                    break;
            }
        }
    }
}