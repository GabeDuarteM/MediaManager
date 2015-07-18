using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class PosterViewModel
    {
        public ICommand AbrirEdicaoCommand { get; private set; }

        private PosterGrid _poster;

        public PosterGrid Poster
        {
            get { return _poster; }
            set { _poster = value; }
        }

        public PosterViewModel()
        {
            AbrirEdicaoCommand = new EdicaoPosterCommand(this);
        }

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
                            Poster.IdBanco = filme.IDFilme;
                            Poster.PosterPath = filme.metadataFolder;
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
                            Poster.IdBanco = serie.IDSerie;
                            Poster.PosterPath = serie.metadataFolder;
                            Poster.TipoConteudo = Helper.TipoConteudo.show;
                        }
                        break;
                    }
                case MediaManager.Helpers.Helper.TipoConteudo.anime:
                    {
                        Serie anime = new Serie();
                        anime = DatabaseHelper.GetSeriePorId(Poster.IdBanco);
                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(Poster.TipoConteudo, anime);
                        frmAdicionarConteudo.ShowDialog();
                        if (frmAdicionarConteudo.DialogResult == true)
                        {
                            Poster.IdBanco = anime.IDSerie;
                            Poster.PosterPath = anime.metadataFolder;
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