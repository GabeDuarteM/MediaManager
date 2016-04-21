// Developed by: Gabriel Duarte
// 
// Created at: 15/11/2015 00:08

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MediaManager.Forms
{
    /// <summary>
    ///     Interaction logic for frmBarraProgresso.xaml
    /// </summary>
    public partial class frmBarraProgresso : Window
    {
        public frmBarraProgresso()
        {
            InitializeComponent();

            BarraProgressoViewModel = new BarraProgressoViewModel();

            BarraProgressoViewModel.ActionFechar = new Action(() => Close());

            DataContext = BarraProgressoViewModel;
        }

        public BarraProgressoViewModel BarraProgressoViewModel { get; set; }

        public void ShowDialog(Window owner)
        {
            Owner = owner;
            ShowDialog();
        }
    }

    public class BarraProgressoViewModel : INotifyPropertyChanged
    {
        private double _dNrProgressoAtual;

        private double _dNrProgressoMaximo;

        private string _sDsTarefa;

        private string _sDsTexto;

        public BarraProgressoViewModel()
        {
            Worker = new BackgroundWorker();
            Worker.RunWorkerCompleted += (s, e) => { ActionFechar(); };
        }

        public string sDsTarefa
        {
            get { return _sDsTarefa; }
            set
            {
                _sDsTarefa = value;
                OnPropertyChanged();
            }
        }

        public string sDsTexto
        {
            get { return _sDsTexto; }
            set
            {
                _sDsTexto = value;
                OnPropertyChanged();
            }
        }

        public int nPcProgressoAtual
        {
            get { return (int) (dNrProgressoAtual / dNrProgressoMaximo * 100); }
        }

        public double dNrProgressoAtual
        {
            get { return _dNrProgressoAtual; }
            set
            {
                _dNrProgressoAtual = value;
                OnPropertyChanged();
                OnPropertyChanged("nPcProgressoAtual");
            }
        }

        public double dNrProgressoMaximo
        {
            get { return _dNrProgressoMaximo; }
            set
            {
                _dNrProgressoMaximo = value;
                OnPropertyChanged();
                OnPropertyChanged("nPcProgressoAtual");
            }
        }

        public BackgroundWorker Worker { get; set; }

        public Action ActionFechar { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
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
