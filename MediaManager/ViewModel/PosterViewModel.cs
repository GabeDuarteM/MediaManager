using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class PosterViewModel : ModelBase
    {
        private Video _oPoster;

        public ICommand AbrirEdicaoCommand { get; private set; }

        public Video oPoster { get { return _oPoster; } set { _oPoster = value; OnPropertyChanged(); } }

        public Window Owner { get; set; }

        public PosterViewModel()
        {
            AbrirEdicaoCommand = new EdicaoPosterCommand(this);
        }

        public void Editar()
        {
            switch (oPoster.nIdTipoConteudo)
            {
                case Enums.TipoConteudo.Filme:
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
                        throw new NotImplementedException(); // TODO Fazer funfar com filme;
                    }
                case Enums.TipoConteudo.Anime:
                case Enums.TipoConteudo.Série:
                    {
                        frmAdicionarConteudo frmAdicionarConteudo = new frmAdicionarConteudo(oPoster.nIdTipoConteudo, oPoster);
                        frmAdicionarConteudo.IsEdicao = true;
                        frmAdicionarConteudo.Owner = Owner;
                        frmAdicionarConteudo.ShowDialog();

                        frmMain.MainVM.AtualizarConteudo(oPoster.nIdTipoConteudo);
                        break;
                    }
                default:
                    break;
            }
        }
    }
}