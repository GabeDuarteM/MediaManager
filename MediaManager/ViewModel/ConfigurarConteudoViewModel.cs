using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MediaManager.Commands;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class ConfigurarConteudoViewModel : INotifyPropertyChanged
    {
        private string _AliasName;
        private int _Temporada;
        private int _Episodio;
        private SerieAlias _SelectedAlias;

        //private ObservableCollection<SerieAlias> _SerieAliases;
        private Video _Video;

        public string AliasName { get { return _AliasName; } set { _AliasName = value; OnPropertyChanged("AliasName"); } }
        public string TemporadaStr { get { return Temporada > 0 ? "S" + _Temporada.ToString("00") : "S"; } set { _Temporada = ValidarPossivelTexto(value, _Temporada); OnPropertyChanged("Temporada"); } }
        public int Temporada { get { return _Temporada; } set { _Temporada = value; OnPropertyChanged("Temporada"); } }
        public string EpisodioStr { get { return Episodio > 0 ? "E" + _Episodio.ToString("00") : "E"; } set { _Episodio = ValidarPossivelTexto(value, _Episodio); OnPropertyChanged("Episodio"); } }
        public int Episodio { get { return _Episodio; } set { _Episodio = value; OnPropertyChanged("Episodio"); } }
        public SerieAlias SelectedAlias { get { return _SelectedAlias; } set { _SelectedAlias = value; OnPropertyChanged("SelectedAlias"); } }

        //public ObservableCollection<SerieAlias> SerieAliases { get { return _SerieAliases; } set { _SerieAliases = value; OnPropertyChanged("SerieAliases"); } }
        public Video Video { get { return _Video; } set { _Video = value; OnPropertyChanged("Video"); } }

        public Action ActionDialogResult { get; set; }

        public Action ActionClose { get; set; }

        public ICommand DoubleClickCommand { get; set; }
        public ICommand AddAlias { get; set; }
        public ICommand RemoveAlias { get; set; }
        public ICommand CommandSalvar { get; set; }

        public ConfigurarConteudoViewModel(Video video)
        {
            Video = video;
            _Temporada = 1;
            _Episodio = 1;
            if (video.IDBanco == 0 && (video.AliasNames == null || video.AliasNames.Count == 0))
            {
                Video.AliasNames = new ObservableCollection<SerieAlias>();
                if (!string.IsNullOrWhiteSpace(video.AliasNamesStr))
                {
                    foreach (var item in video.AliasNamesStr.Split('|'))
                    {
                        SerieAlias alias = new SerieAlias(item);
                        Video.AliasNames.Add(alias);
                    }
                }
            }

            DoubleClickCommand = new ConfigurarConteudoCommands.DoubleClickNoGridAliasCommand();
            AddAlias = new ConfigurarConteudoCommands.AddAlias();
            RemoveAlias = new ConfigurarConteudoCommands.RemoveAlias();
            CommandSalvar = new ConfigurarConteudoCommands.CommandSalvar();
        }

        private int ValidarPossivelTexto(string valor, int valorAntigo)
        {
            if (Regex.IsMatch(valor, @"^S\d{1,3}$") || Regex.IsMatch(valor, @"^E\d{1,3}$"))
            {
                return Convert.ToInt32(Regex.Match(valor, @"\d+").Value);
            }
            return valorAntigo;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
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