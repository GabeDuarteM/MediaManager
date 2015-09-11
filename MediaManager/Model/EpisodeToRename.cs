using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MediaManager.Helpers;

namespace MediaManager.Model
{
    [System.Diagnostics.DebuggerDisplay("{FilenameRenamed}"), NotMapped]
    public class EpisodeToRename : Episode, INotifyPropertyChanged
    {
        private string _Filename;
        private string _FilenameRenamed;
        private bool _IsSelected = true;

        public Helper.Enums.ContentType ContentType { get; set; }
        public string ContentTypeString { get { return Helper.Enums.ToString(ContentType); } }
        public string[] EpisodeArray { get; set; } // Para quando tiver mais de um episódio (nome.do.episodio.S00E00E01E02E03E04)
        public string Filename { get { return _Filename; } set { _Filename = value; OnPropertyChanged("Filename"); } }
        public string FilenameRenamed { get { return _FilenameRenamed; } set { _FilenameRenamed = value; OnPropertyChanged("FilenameRenamed"); } }

        [NotMapped]
        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; OnPropertyChanged("IsSelected"); } }

        public string ParentTitle { get; set; }

        public EpisodeToRename(Episode episode)
        {
            if (episode == null)
                return;
            AbsoluteNumber = episode.AbsoluteNumber;
            AirsAfterSeason = episode.AirsAfterSeason;
            AirsBeforeEpisode = episode.AirsBeforeEpisode;
            AirsBeforeSeason = episode.AirsBeforeSeason;
            Artwork = episode.Artwork;
            EpisodeName = episode.EpisodeName;
            EpisodeNumber = episode.EpisodeNumber;
            FirstAired = episode.FirstAired;
            FolderPath = episode.FolderPath;
            IDBanco = episode.IDBanco;
            IDSeasonTvdb = episode.IDSeasonTvdb;
            IDSerie = episode.IDSerie;
            IDSeriesTvdb = episode.IDSeriesTvdb;
            IDTvdb = episode.IDTvdb;
            IsRenamed = episode.IsRenamed;
            Language = episode.Language;
            LastUpdated = episode.LastUpdated;
            OriginalFilePath = episode.OriginalFilePath;
            Overview = episode.Overview;
            Rating = episode.Rating;
            RatingCount = episode.RatingCount;
            SeasonNumber = episode.SeasonNumber;
            Serie = episode.Serie != null ? episode.Serie : null;
            ThumbAddedDate = episode.ThumbAddedDate;
        }

        public EpisodeToRename()
        {
        }

        public void Clone(EpisodeToRename episode)
        {
            AbsoluteNumber = episode.AbsoluteNumber;
            AirsAfterSeason = episode.AirsAfterSeason;
            AirsBeforeEpisode = episode.AirsBeforeEpisode;
            AirsBeforeSeason = episode.AirsBeforeSeason;
            Artwork = episode.Artwork;
            ContentType = episode.ContentType;
            EpisodeArray = episode.EpisodeArray;
            EpisodeName = episode.EpisodeName;
            EpisodeNumber = episode.EpisodeNumber;
            Filename = episode.Filename;
            FilenameRenamed = episode.FilenameRenamed;
            FirstAired = episode.FirstAired;
            FolderPath = episode.FolderPath;
            IDBanco = episode.IDBanco;
            IDSeasonTvdb = episode.IDSeasonTvdb;
            IDSerie = episode.IDSerie;
            IDSeriesTvdb = episode.IDSeriesTvdb;
            IDTvdb = episode.IDTvdb;
            IsRenamed = episode.IsRenamed;
            Language = episode.Language;
            LastUpdated = episode.LastUpdated;
            OriginalFilePath = episode.OriginalFilePath;
            Overview = episode.Overview;
            ParentTitle = episode.ParentTitle;
            Rating = episode.Rating;
            RatingCount = episode.RatingCount;
            SeasonNumber = episode.SeasonNumber;
            Serie = episode.Serie;
            ThumbAddedDate = episode.ThumbAddedDate;
        }

        public async Task<bool> GetEpisodeAsync()
        {
            // nome.da.serie.S00E00 ou nome.da.serie.S00E00E01E02E03E04 ou nome.da.serie.S00E00-01-02-03-04
            Regex regex_S00E00 = new Regex(@"^(?i)(?<name>.*)[._\s-]S(?<season>\d{1,2})e(?<episodes>\d{1,3}(?:(?<separador>[e-])\d{1,3})*)");

            // nome.da.serie.000
            Regex regex_000 = new Regex(@"^(?i)(?<name>.*)[._\s](?<episodes>\d{3,4}(?:(?<separador>[-])\d{1,3})*)");

            // [Nome do Fansub] Nome da Série - 00 ou [Nome do Fansub] Nome da Série - 0000
            Regex regex_Fansub0000 = new Regex(@"^(?i)(?:\[\S*\])(?<name>.*?)(?:(?:_-_)|(?: - )|(?:_))(?<episodes>\d{1,4}(?:(?<separador>[e_-])\d{1,3})*)");

            // Nome da Série - 0x00 - Nome do episódio
            Regex regex_0x00 = new Regex(@"^(?i)(?<name>.*) - (?<season>\d{1,2})x(?<episodes>\d{1,3}(?:(?<separador>[-])\d{1,3})*)");

            if (regex_S00E00.IsMatch(Filename))
            {
                var match = regex_S00E00.Match(Filename);
                ParentTitle = match.Groups["name"].Value.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
                SeriesData data = new SeriesData();
                List<Serie> listaSeriesBanco = DBHelper.GetSerieOuAnimePorTitulo(ParentTitle, true);
                List<SerieAlias> listaAlias = DBHelper.GetAllAliases();
                SerieAlias alias = null;
                if (listaSeriesBanco.Count > 0)
                {
                    foreach (var item in listaSeriesBanco)
                    {
                        if (item.Title.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim() == ParentTitle)
                        {
                            data.Series = new Serie[1] { item };
                            break;
                        }
                    }
                    if (data.Series == null)
                        data.Series = new Serie[1] { listaSeriesBanco[0] };
                }
                else
                    foreach (var item in listaAlias)
                    {
                        if (item.AliasName.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim() == ParentTitle)
                        {
                            data.Series = new Serie[1] { DBHelper.GetSeriePorID(item.IDSerie) };
                            alias = item;
                            break;
                        }
                    }
                if (data.Series == null)
                    data = await APIRequests.GetSeriesAsync(ParentTitle, false);

                if (data.Series == null)
                    data = await APIRequests.GetSeriesAsync(match.Groups["name"].Value, false);

                if (data.Series != null && DBHelper.VerificaSeExiste(data.Series[0].IDApi))
                {
                    char separador = string.IsNullOrWhiteSpace(match.Groups["separador"].Value) // Para quando se tem multi-episódios
                        ? default(char)
                        : match.Groups["separador"].Value.ToCharArray()[0];
                    EpisodeToRename episodeToClone = null;
                    Serie = DBHelper.GetSerieOuAnimePorIDApi(data.Series[0].IDApi);

                    SeasonNumber = int.Parse(match.Groups["season"].Value);
                    EpisodeArray = match.Groups["episodes"].Value.Split(separador);
                    EpisodeNumber = int.Parse(EpisodeArray[0]);
                    if (alias != null)
                    {
                        var episodios = DBHelper.GetEpisodes(Serie);
                        foreach (var item in episodios)
                        {
                            if (alias.Temporada + SeasonNumber - 1 == item.SeasonNumber && alias.Episodio + EpisodeNumber - 1 == item.EpisodeNumber)
                            {
                                episodeToClone = new EpisodeToRename(DBHelper.GetEpisode(Serie.IDBanco, item.SeasonNumber, item.EpisodeNumber));
                                if (episodeToClone.IDBanco == 0) // Caso IDBanco seja 0 é porque não foi encontrado o episódio no banco.
                                    return false;
                                //episodeToClone.EpisodeArray = EpisodeArray;
                                episodeToClone.Serie = Serie;
                                episodeToClone.ContentType = episodeToClone.Serie.ContentType;
                                episodeToClone.Filename = Filename;
                                episodeToClone.FolderPath = FolderPath;
                                episodeToClone.ParentTitle = episodeToClone.Serie.Title;
                                Clone(episodeToClone);
                            }
                        }
                    }
                    else
                    {
                        episodeToClone = new EpisodeToRename(DBHelper.GetEpisode(Serie.IDBanco, SeasonNumber, EpisodeNumber));
                        if (episodeToClone.IDBanco == 0) // Caso IDBanco seja 0 é porque não foi encontrado o episódio no banco.
                            return false;
                        episodeToClone.EpisodeArray = EpisodeArray;
                        episodeToClone.Serie = Serie;
                        episodeToClone.ContentType = episodeToClone.Serie.ContentType;
                        episodeToClone.Filename = Filename;
                        episodeToClone.FolderPath = FolderPath;
                        episodeToClone.ParentTitle = episodeToClone.Serie.Title;
                        Clone(episodeToClone);
                    }
                    return true;
                }
            }
            else if (regex_0x00.IsMatch(Filename))
            {
                SeriesData data = await APIRequests.GetSeriesAsync(regex_S00E00.Match(Filename).Groups["name"].Value, false);
                foreach (var item in data.Series)
                {
                    if (DBHelper.VerificaSeExiste(item.IDApi))
                    {
                        var match = regex_0x00.Match(Filename);
                        char separador = match.Groups["separador"].Value.ToCharArray()[0];
                        Serie = DBHelper.GetSerieOuAnimePorIDApi(item.IDApi);
                        SeasonNumber = int.Parse(match.Groups["season"].Value);
                        EpisodeArray = match.Groups["episode"].Value.Split(separador);
                        EpisodeNumber = int.Parse(EpisodeArray[0]);
                        var episodeDB = DBHelper.GetEpisode(Serie.IDBanco, SeasonNumber, EpisodeNumber);
                        Clone(new EpisodeToRename(episodeDB));
                        EpisodeArray = match.Groups["episode"].Value.Split(separador);
                        break;
                    }
                }
                return true;
            }
            else if (regex_Fansub0000.IsMatch(Filename))
            {
                var match = regex_Fansub0000.Match(Filename);
                ParentTitle = match.Groups["name"].Value.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
                SeriesData data = new SeriesData();
                List<Serie> listaSeriesBanco = DBHelper.GetSerieOuAnimePorTitulo(ParentTitle, true);
                List<SerieAlias> listaAlias = DBHelper.GetAllAliases();
                SerieAlias alias = null;
                if (listaSeriesBanco.Count > 0)
                {
                    foreach (var item in listaSeriesBanco)
                    {
                        if (item.Title.Replace(".", " ").Replace("_", " ") == ParentTitle)
                        {
                            data.Series = new Serie[1] { item };
                            break;
                        }
                        else if (item.Title.Replace(".", " ").Replace("_", " ").Replace("'", "") == ParentTitle.Replace("'", ""))
                        {
                            data.Series = new Serie[1] { item };
                            break;
                        }
                    }
                    if (data.Series == null)
                        data.Series = new Serie[1] { listaSeriesBanco[0] };
                }
                else
                {
                    foreach (var item in listaAlias)
                    {
                        if (item.AliasName.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim() == ParentTitle)
                        {
                            data.Series = new Serie[1] { DBHelper.GetSeriePorID(item.IDSerie) };
                            alias = item;
                            break;
                        }
                    }
                }
                if (data.Series == null)
                    data = await APIRequests.GetSeriesAsync(ParentTitle, false);

                if (data.Series == null)
                    data = await APIRequests.GetSeriesAsync(match.Groups["name"].Value, false);

                if (data.Series != null && DBHelper.VerificaSeExiste(data.Series[0].IDApi))
                {
                    char separador = string.IsNullOrWhiteSpace(match.Groups["separador"].Value) // Para quando se tem multi-episódios
                        ? default(char)
                        : match.Groups["separador"].Value.ToCharArray()[0];
                    Serie = DBHelper.GetSerieOuAnimePorIDApi(data.Series[0].IDApi);
                    EpisodeArray = match.Groups["episodes"].Value.Split(separador);
                    AbsoluteNumber = int.Parse(EpisodeArray[0]);
                    if (alias != null)
                    {
                        var episodios = DBHelper.GetEpisodes(Serie);
                        var primeiroEpisodioAlias = DBHelper.GetEpisode(Serie.IDBanco, alias.Temporada, alias.Episodio);
                        foreach (var item in episodios)
                        {
                            if (item.AbsoluteNumber == primeiroEpisodioAlias.AbsoluteNumber + AbsoluteNumber - 1)
                            {
                                var episodeToClone = new EpisodeToRename(item);
                                if (episodeToClone.IDBanco == 0) // Caso seja 0 é porque não foi encontrado o episódio no banco.
                                    return false;
                                //episodeToClone.EpisodeArray = EpisodeArray;
                                //episodeToClone.AbsoluteNumber = AbsoluteNumber;
                                episodeToClone.Serie = Serie;
                                episodeToClone.ContentType = episodeToClone.Serie.ContentType;
                                episodeToClone.Filename = Filename;
                                episodeToClone.FolderPath = FolderPath;
                                episodeToClone.ParentTitle = episodeToClone.Serie.Title;

                                Clone(episodeToClone);
                                return true;
                            }
                        }
                    }
                    else
                    {
                        var episodeDB = DBHelper.GetEpisode(Serie.IDBanco, (int)AbsoluteNumber);
                        var episodeToClone = new EpisodeToRename(episodeDB);
                        if (episodeToClone.IDBanco == 0) // Caso seja 0 é porque não foi encontrado o episódio no banco.
                            return false;
                        episodeToClone.EpisodeArray = EpisodeArray;
                        episodeToClone.AbsoluteNumber = AbsoluteNumber;
                        episodeToClone.Serie = Serie;
                        episodeToClone.ContentType = episodeToClone.Serie.ContentType;
                        episodeToClone.Filename = Filename;
                        episodeToClone.FolderPath = FolderPath;
                        episodeToClone.ParentTitle = episodeToClone.Serie.Title;

                        Clone(episodeToClone);
                        return true;
                    }
                }
            }
            return false;
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