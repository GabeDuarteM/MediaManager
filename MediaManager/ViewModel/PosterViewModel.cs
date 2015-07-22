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
                        Video video = new Filme();
                        video = DatabaseHelper.GetFilmePorId(Poster.IdBanco);

                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Poster.TipoConteudo, video);
                        frmAdicionarConteudo.IsEdicao = true;
                        frmAdicionarConteudo.ShowDialog();

                        if (frmAdicionarConteudo.DialogResult == true)
                        {
                            video = frmAdicionarConteudo.AdicionarConteudoViewModel.Video;

                            Poster.IdBanco = video.Ids.IdBanco;
                            Poster.PosterPath = (File.Exists(Path.Combine(video.MetadataFolder, "poster.jpg"))) ?
                                Path.Combine(video.MetadataFolder, "poster.jpg") : null;
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

                            Poster.IdBanco = serie.Ids.IdBanco;
                            Poster.PosterPath = (File.Exists(Path.Combine(serie.MetadataFolder, "poster.jpg"))) ?
                                Path.Combine(serie.MetadataFolder, "poster.jpg") : null;
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

                            Poster.IdBanco = anime.Ids.IdBanco;
                            Poster.PosterPath = (File.Exists(Path.Combine(anime.MetadataFolder, "poster.jpg"))) ?
                                Path.Combine(anime.MetadataFolder, "poster.jpg") : null;
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