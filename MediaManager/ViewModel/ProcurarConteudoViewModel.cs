using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using MediaManager.Helpers;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    public class ProcurarConteudoViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ConteudoGrid> _Conteudos = new ObservableCollection<ConteudoGrid>();
        public ObservableCollection<ConteudoGrid> Conteudos { get { return _Conteudos; } set { _Conteudos = value; OnPropertyChanged("Conteudos"); } }

        public ProcurarConteudoViewModel(Enums.ContentType contentType)
        {
            Conteudos.Add(new ConteudoGrid { Title = "Carregando...", FolderPath = "Carregando...", IsSelected = false });

            LoadConteudos(contentType);
        }

        public async void LoadConteudos(Enums.ContentType contentType)
        {
            ObservableCollection<ConteudoGrid> conteudos = new ObservableCollection<ConteudoGrid>();

            switch (contentType)
            {
                case Enums.ContentType.Filme:
                    break;

                case Enums.ContentType.Série:
                    break;

                case Enums.ContentType.Anime:
                    break;

                case Enums.ContentType.AnimeFilmeSérie:
                    DirectoryInfo[] dirSeries = Helper.retornarDiretoriosSeries();
                    DirectoryInfo[] dirAnimes = Helper.retornarDiretoriosAnimes();
                    DirectoryInfo[] dirFilmes = Helper.retornarDiretoriosFilmes();

                    if (dirSeries != null)
                    {
                        foreach (var dir in dirSeries)
                        {
                            if (!DBHelper.VerificaSeSerieOuAnimeExiste(dir.FullName))
                            {
                                SeriesData data = await APIRequests.GetSeriesAsync(dir.Name, false);
                                if (data.Series.Length == 0)
                                {
                                    ConteudoGrid conteudo = new ConteudoGrid();
                                    conteudo.ContentType = Enums.ContentType.Série;
                                    conteudo.FolderPath = dir.FullName;
                                    conteudo.IsNotFound = true;
                                    conteudos.Add(conteudo);
                                }
                                else if (data.Series.Length > 0 && !DBHelper.VerificaSeSerieOuAnimeExiste(data.Series[0].IDApi))
                                {
                                    ConteudoGrid conteudo = data.Series[0];
                                    conteudo.ContentType = Enums.ContentType.Série;
                                    conteudo.FolderPath = dir.FullName;
                                    conteudo.IsSelected = true;
                                    conteudo.Video = data.Series[0];
                                    conteudo.Video.ContentType = Enums.ContentType.Série;
                                    conteudo.Video.FolderPath = dir.FullName;

                                    if (!string.IsNullOrWhiteSpace(conteudo.SerieAliasStr))
                                    {
                                        foreach (var item in conteudo.SerieAliasStr.Split('|'))
                                        {
                                            SerieAlias alias = new SerieAlias(item);
                                            if (conteudo.SerieAlias == null)
                                            {
                                                conteudo.SerieAlias = new ObservableCollection<SerieAlias>();
                                                conteudo.Video.SerieAlias = new ObservableCollection<SerieAlias>();
                                            }
                                            conteudo.SerieAlias.Add(alias);
                                            conteudo.Video.SerieAlias.Add(alias);
                                        }
                                    }

                                    conteudos.Add(conteudo);
                                }
                            }
                        }
                    }

                    if (dirAnimes != null)
                    {
                        foreach (var dir in dirAnimes)
                        {
                            if (!DBHelper.VerificaSeSerieOuAnimeExiste(dir.FullName))
                            {
                                SeriesData data = await APIRequests.GetSeriesAsync(dir.Name, false);
                                if (data.Series == null || data.Series.Length == 0)
                                {
                                    ConteudoGrid conteudo = new ConteudoGrid();
                                    conteudo.ContentType = Enums.ContentType.Anime;
                                    conteudo.FolderPath = dir.FullName;
                                    conteudo.IsNotFound = true;
                                    conteudos.Add(conteudo);
                                }
                                else if (data.Series.Length > 0 && !DBHelper.VerificaSeSerieOuAnimeExiste(data.Series[0].IDApi))
                                {
                                    ConteudoGrid conteudo = data.Series[0];
                                    conteudo.ContentType = Enums.ContentType.Anime;
                                    conteudo.FolderPath = dir.FullName;
                                    conteudo.IsSelected = true;
                                    conteudo.Video = data.Series[0];
                                    conteudo.Video.ContentType = Enums.ContentType.Anime;
                                    conteudo.Video.FolderPath = dir.FullName;
                                    (conteudo.Video as Serie).IsAnime = true;

                                    if (!string.IsNullOrWhiteSpace(conteudo.SerieAliasStr))
                                    {
                                        foreach (var item in conteudo.SerieAliasStr.Split('|'))
                                        {
                                            SerieAlias alias = new SerieAlias(item);
                                            if (conteudo.SerieAlias == null)
                                            {
                                                conteudo.SerieAlias = new ObservableCollection<SerieAlias>();
                                                conteudo.Video.SerieAlias = new ObservableCollection<SerieAlias>();
                                            }
                                            conteudo.SerieAlias.Add(alias);
                                            conteudo.Video.SerieAlias.Add(alias);
                                        }
                                    }

                                    conteudos.Add(conteudo);
                                }
                            }
                        }
                    }

                    //if (dirFilmes != null) {
                    //    foreach (var dir in dirFilmes) // TODO Fazer funfar
                    //{
                    //        if (!DatabaseHelper.VerificaSeExiste(dir.FullName))
                    //        {
                    //            filmes = await Helper.API_PesquisarConteudoAsync(dir.Name, Enums.TipoConteudo.movie.ToString(), false);
                    //            if (filmes.Count != 0 && !DatabaseHelper.VerificaSeExiste(filmes[0].Video.ids.trakt))
                    //                conteudos.Add(new ConteudoGrid { Nome = filmes[0].Video.title, Pasta = dir.FullName, TipoConteudo = Enums.TipoConteudo.movie, TraktSlug = filmes[0].Video.ids.slug, IsSelected = true });
                    //        }
                    //    }
                    //}
                    break;

                default:
                    break;
            }

            Conteudos = conteudos;

            //Conteudos.Clear();

            //foreach (var item in conteudos)
            //{
            //    Conteudos.Add(item);
            //}
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