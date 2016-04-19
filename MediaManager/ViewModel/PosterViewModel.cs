// Developed by: Gabriel Duarte
// 
// Created at: 20/07/2015 21:10
// Last update: 19/04/2016 02:57

using System;
using System.Windows;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Forms;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class PosterViewModel : ViewModelBase
    {
        private Video _oPoster;

        public PosterViewModel()
        {
            AbrirEdicaoCommand = new EdicaoPosterCommand(this);
        }

        public ICommand AbrirEdicaoCommand { get; private set; }

        public Video oPoster
        {
            get { return _oPoster; }
            set
            {
                _oPoster = value;
                OnPropertyChanged();
            }
        }

        public Window Owner { get; set; }

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
                    var frmAdicionarConteudo = new frmAdicionarConteudo(oPoster.nIdTipoConteudo,
                                                                        oPoster);
                    frmAdicionarConteudo.IsEdicao = true;
                    frmAdicionarConteudo.Owner = Owner;
                    frmAdicionarConteudo.ShowDialog();

                    frmMain.MainVM.AtualizarPosters(oPoster.nIdTipoConteudo);
                    break;
                }
                default:
                    break;
            }
        }
    }
}
