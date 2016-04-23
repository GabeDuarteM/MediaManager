// Developed by: Gabriel Duarte
// 
// Created at: 20/07/2015 21:10

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
                    throw new NotImplementedException(); // TODO Filmes
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
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
