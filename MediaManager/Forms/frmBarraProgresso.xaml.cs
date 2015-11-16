using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MediaManager.Forms
{
    /// <summary>
    /// Interaction logic for frmBarraProgresso.xaml
    /// </summary>
    public partial class frmBarraProgresso : Window
    {
        public BarraProgressoViewModel BarraProgressoViewModel { get; set; }

        public frmBarraProgresso()
        {
            InitializeComponent();

            BarraProgressoViewModel = new BarraProgressoViewModel();

            BarraProgressoViewModel.ActionFechar = new Action(() => Close());

            DataContext = BarraProgressoViewModel;
        }

        public void ShowDialog(Window owner)
        {
            Owner = owner;
            ShowDialog();
        }
    }

    public class BarraProgressoViewModel : INotifyPropertyChanged
    {
        private string _sDsTarefa;

        public string sDsTarefa { get { return _sDsTarefa; } set { _sDsTarefa = value; OnPropertyChanged(); } }

        private string _sDsTexto;

        public string sDsTexto { get { return _sDsTexto; } set { _sDsTexto = value; OnPropertyChanged(); } }

        public int nPcProgressoAtual { get { return (int)((dNrProgressoAtual / dNrProgressoMaximo) * 100); } }

        private double _dNrProgressoAtual;

        public double dNrProgressoAtual { get { return _dNrProgressoAtual; } set { _dNrProgressoAtual = value; OnPropertyChanged(); OnPropertyChanged("nPcProgressoAtual"); } }

        private double _dNrProgressoMaximo;

        public double dNrProgressoMaximo { get { return _dNrProgressoMaximo; } set { _dNrProgressoMaximo = value; OnPropertyChanged(); OnPropertyChanged("nPcProgressoAtual"); } }

        public BackgroundWorker Worker { get; set; }

        public Action ActionFechar { get; set; }

        public BarraProgressoViewModel()
        {
            Worker = new BackgroundWorker();
            Worker.RunWorkerCompleted += (s, e) =>
            {
                ActionFechar();
            };
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