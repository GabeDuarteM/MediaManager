using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class RenomearViewModel : INotifyPropertyChanged
    {
        public bool bFlSilencioso { get; set; }

        private bool? _bFlSelecionarTodos;

        public bool? bFlSelecionarTodos { get { return _bFlSelecionarTodos; } set { _bFlSelecionarTodos = value; OnPropertyChanged(); } }

        public Action ActionFechar { get; set; } // Para poder fechar depois no RenomearCommand

        private ObservableCollection<Episodio> _ListaEpisodios;

        public ObservableCollection<Episodio> ListaEpisodios { get { return _ListaEpisodios; } set { _ListaEpisodios = value; OnPropertyChanged(); } }

        public ICommand CommandRenomear { get; set; }

        public ICommand CommandSelecionar { get; set; }

        public ICommand CommandSelecionarTodos { get; set; }

        public RenomearViewModel(IEnumerable<FileInfo> arquivos = null)
        {
            CommandRenomear = new RenomearCommands.CommandRenomear();
            CommandSelecionar = new RenomearCommands.CommandSelecionar();
            CommandSelecionarTodos = new RenomearCommands.CommandSelecionarTodos();

            if (arquivos == null)
                arquivos = new DirectoryInfo(Properties.Settings.Default.pref_PastaDownloads).EnumerateFiles("*.*", SearchOption.AllDirectories);
            Carregar(arquivos);
        }

        private void Carregar(IEnumerable<FileInfo> arquivos)
        {
            ListaEpisodios = new ObservableCollection<Episodio>();
            ListaEpisodios.Add(new Episodio() { oSerie = new Serie() { sDsTitulo = "Carregando..." }, sDsFilepathOriginal = "Carregando...", bFlSelecionado = false });

            string[] extensoesPermitidas = Properties.Settings.Default.ExtensoesRenomeioPermitidas.Split('|');
            List<Episodio> listaEpisodios = new List<Episodio>();
            List<Episodio> listaEpisodiosNaoEncontrados = new List<Episodio>();

            foreach (var item in arquivos)
            {
                if (extensoesPermitidas.Contains(item.Extension))
                {
                    Episodio episodio = new Episodio();
                    episodio.sDsFilepath = item.FullName;

                    asdfasdfasdfasdfasdfasdfasdf;
                    if (episodio.GetEpisode())
                    {
                        episodio.sDsFilepathOriginal = item.FullName;
                        episodio.sDsFilepath = Path.Combine(episodio.oSerie.sDsPasta, Helper.RenomearConformePreferencias(episodio) + item.Extension);
                        episodio.bFlRenomeado = true;
                        listaEpisodios.Add(episodio);
                    }
                    else
                    {
                        listaEpisodiosNaoEncontrados.Add(episodio);
                    }
                }
            }
            //foreach (var item in listaEpisodiosNotFound)
            //{
            //    TextWriter tw = new StreamWriter(@"C:/Episodios não encontrados.txt", true);
            //    tw.WriteLine(item.Filename);
            //    tw.Close();
            //}
            ListaEpisodios = new ObservableCollection<Episodio>(listaEpisodios);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
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