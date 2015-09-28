using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
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

        public Enums.ContentType ContentType { get; set; }

        public string ContentTypeString { get { return Enums.ToString(ContentType); } }

        public List<string> EpisodeArray { get; set; } = new List<string>(); // Para quando tiver mais de um episódio (nome.do.episodio.S00E00E01E02E03E04)

        public List<string> AbsoluteArray { get; set; } = new List<string>();// Para quando tiver mais de um episódio absoluto

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
            AbsoluteArray = episode.AbsoluteArray;
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
            Helper.RegexEpisodio regexes = new Helper.RegexEpisodio();

            var FilenameTratado = System.IO.Path.GetFileNameWithoutExtension(Filename).Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();

            try
            {
                if (regexes.regex_S00E00.IsMatch(FilenameTratado))
                {
                    var match = regexes.regex_S00E00.Match(FilenameTratado);
                    ParentTitle = match.Groups["name"].Value.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim();
                    SeriesData data = new SeriesData();
                    List<SerieAlias> listaAlias = DBHelper.GetAllAliases();
                    SerieAlias alias = null;
                    foreach (var item in listaAlias)
                    {
                        if (item.AliasName.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim() == ParentTitle)
                        {
                            if (Serie == null)
                                data.Series = new Serie[1] { DBHelper.GetSeriePorID(item.IDSerie) };
                            alias = item;
                            break;
                        }
                    }
                    if (Serie != null)
                        data = new SeriesData() { Series = new Serie[1] { Serie }, Episodes = Serie.Episodes.ToArray() };
                    else
                    {
                        List<Serie> listaSeriesBanco = DBHelper.GetSerieOuAnimePorTitulo(ParentTitle, true);
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
                        if (data.Series == null)
                            data = await APIRequests.GetSeriesAsync(ParentTitle, false);

                        if (data.Series == null)
                            data = await APIRequests.GetSeriesAsync(match.Groups["name"].Value, false);
                    }

                    if (data.Series != null && DBHelper.VerificaSeExiste(data.Series[0].IDApi))
                    {
                        char separador = string.IsNullOrWhiteSpace(match.Groups["separador"].Value) // Para quando se tem multi-episódios
                            ? default(char)
                            : match.Groups["separador"].Value.ToCharArray()[0];
                        EpisodeToRename episodeToClone = null;
                        if (Serie == null)
                            Serie = data.Series[0];

                        SeasonNumber = int.Parse(match.Groups["season"].Value);
                        EpisodeArray = new List<string>(match.Groups["episodes"].Value.Split(separador));
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
                                    episodeToClone.EpisodeArray = EpisodeArray;
                                    episodeToClone.AbsoluteArray = AbsoluteArray;
                                    episodeToClone.Serie = Serie;
                                    episodeToClone.ContentType = episodeToClone.Serie.ContentType;
                                    episodeToClone.Filename = Filename;
                                    episodeToClone.FolderPath = FolderPath;
                                    episodeToClone.ParentTitle = episodeToClone.Serie.Title;
                                    for (int i = 0; i < episodeToClone.EpisodeArray.Count; i++)
                                    {
                                        episodeToClone.EpisodeArray[i] = alias.Episodio + EpisodeNumber - 1 + "";

                                        if (i == 0)
                                            continue;

                                        var episodeTemp = new EpisodeToRename(episodios.Find(x => x.SeasonNumber == alias.Temporada + SeasonNumber - 1 && x.EpisodeNumber == alias.Episodio + EpisodeNumber - 1));
                                        episodeToClone.EpisodeName += " & " + episodeTemp.EpisodeName;
                                    }
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
                            foreach (var item in EpisodeArray)
                            {
                                if (item == EpisodeArray[0])
                                    continue;

                                var episodeTemp = new EpisodeToRename(DBHelper.GetEpisode(Serie.IDBanco, SeasonNumber, int.Parse(item)));
                                episodeToClone.EpisodeName += " & " + episodeTemp.EpisodeName;
                            }
                            Clone(episodeToClone);
                        }
                        return true;
                    }
                }
                else if (regexes.regex_0x00.IsMatch(FilenameTratado)) // TODO Fazer funcionar com alias
                {
                    var match = regexes.regex_0x00.Match(FilenameTratado);
                    SeriesData data = null;
                    if (Serie == null)
                        data = await APIRequests.GetSeriesAsync(match.Groups["name"].Value, false);
                    else
                        data = new SeriesData() { Series = new Serie[1] { Serie }, Episodes = Serie.Episodes.ToArray() };

                    foreach (var item in data.Series)
                    {
                        if (data.Series != null && DBHelper.VerificaSeExiste(item.IDApi))
                        {
                            char separador = string.IsNullOrWhiteSpace(match.Groups["separador"].Value) // Para quando se tem multi-episódios
                            ? default(char)
                            : match.Groups["separador"].Value.ToCharArray()[0];
                            Serie = DBHelper.GetSerieOuAnimePorIDApi(item.IDApi);
                            SeasonNumber = int.Parse(match.Groups["season"].Value);
                            EpisodeArray = new List<string>(match.Groups["episodes"].Value.Split(separador));
                            EpisodeNumber = int.Parse(EpisodeArray[0]);
                            EpisodeToRename episodeToClone = new EpisodeToRename(DBHelper.GetEpisode(Serie.IDBanco, SeasonNumber, EpisodeNumber));
                            if (episodeToClone.IDBanco == 0) // Caso IDBanco seja 0 é porque não foi encontrado o episódio no banco.
                                return false;
                            //episodeToClone.EpisodeArray = EpisodeArray;
                            episodeToClone.Serie = Serie;
                            episodeToClone.ContentType = episodeToClone.Serie.ContentType;
                            episodeToClone.Filename = Filename;
                            episodeToClone.FolderPath = FolderPath;
                            episodeToClone.ParentTitle = episodeToClone.Serie.Title;
                            foreach (var itemArray in EpisodeArray)
                            {
                                if (itemArray == EpisodeArray[0])
                                    continue;

                                var episodeTemp = new EpisodeToRename(DBHelper.GetEpisode(Serie.IDBanco, SeasonNumber, int.Parse(itemArray)));
                                episodeToClone.EpisodeName += " & " + episodeTemp.EpisodeName;
                            }
                            Clone(episodeToClone);
                            //EpisodeArray = match.Groups["episodes"].Value.Split(separador);
                            break;
                        }
                    }
                    return true;
                }
                else if (regexes.regex_Fansub0000.IsMatch(FilenameTratado))
                {
                    var match = regexes.regex_Fansub0000.Match(FilenameTratado);
                    ParentTitle = match.Groups["name"].Value.Trim();
                    SeriesData data = new SeriesData();
                    SerieAlias alias = null;
                    List<SerieAlias> listaAlias = DBHelper.GetAllAliases();
                    foreach (var item in listaAlias)
                    {
                        if (item.AliasName.Replace(".", " ").Replace("_", " ").Replace("'", "").Trim() == ParentTitle)
                        {
                            if (Serie == null)
                                data.Series = new Serie[1] { DBHelper.GetSeriePorID(item.IDSerie) };
                            alias = item;
                            break;
                        }
                    }
                    if (Serie != null)
                        data = new SeriesData { Series = new Serie[1] { Serie }, Episodes = Serie.Episodes.ToArray() };
                    else
                    {
                        List<Serie> listaSeriesBanco = DBHelper.GetSerieOuAnimePorTitulo(ParentTitle, true);
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
                        if (data.Series == null)
                            data = await APIRequests.GetSeriesAsync(ParentTitle, false);

                        if (data.Series == null)
                            data = await APIRequests.GetSeriesAsync(match.Groups["name"].Value, false);
                    }

                    if (data.Series != null && DBHelper.VerificaSeExiste(data.Series[0].IDApi))
                    {
                        string separador = string.IsNullOrEmpty(match.Groups["separador"].Value) // Para quando se tem multi-episódios
                            ? default(string)
                            : match.Groups["separador"].Value;
                        if (Serie == null)
                            Serie = DBHelper.GetSerieOuAnimePorIDApi(data.Series[0].IDApi);
                        EpisodeArray = new List<string>(match.Groups["episodes"].Value.Split(new[] { separador }, StringSplitOptions.None));
                        AbsoluteNumber = int.Parse(EpisodeArray[0]);
                        if (alias != null) // Entra se estiver utilizando alias para identificar a serie
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
                                    episodeToClone.EpisodeArray = EpisodeArray;
                                    //episodeToClone.AbsoluteNumber = AbsoluteNumber;
                                    episodeToClone.Serie = Serie;
                                    episodeToClone.ContentType = episodeToClone.Serie.ContentType;
                                    episodeToClone.Filename = Filename;
                                    episodeToClone.FolderPath = FolderPath;
                                    episodeToClone.ParentTitle = episodeToClone.Serie.Title;
                                    for (int i = 0; i < episodeToClone.EpisodeArray.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            //episodeToClone.EpisodeArray[i] = item.AbsoluteNumber + "";
                                            episodeToClone.AbsoluteArray.Add(item.AbsoluteNumber+"");
                                            continue;
                                        }
                                        episodeToClone.EpisodeArray[i] = (int)primeiroEpisodioAlias.AbsoluteNumber + int.Parse(episodeToClone.EpisodeArray[i]) - 1 + "";
                                        var episodeTemp = new EpisodeToRename(DBHelper.GetEpisode(episodios.Find(x => x.AbsoluteNumber == int.Parse(episodeToClone.EpisodeArray[i])).IDTvdb));
                                        episodeToClone.EpisodeName += " & " + episodeTemp.EpisodeName;
                                    }
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
                            episodeToClone.AbsoluteArray = new List<string>();
                            episodeToClone.AbsoluteNumber = AbsoluteNumber;
                            episodeToClone.Serie = Serie;
                            episodeToClone.ContentType = episodeToClone.Serie.ContentType;
                            episodeToClone.Filename = Filename;
                            episodeToClone.FolderPath = FolderPath;
                            episodeToClone.ParentTitle = episodeToClone.Serie.Title;
                            foreach (var itemArray in EpisodeArray)
                            {
                                episodeToClone.AbsoluteArray.Add(itemArray);
                                if (itemArray == EpisodeArray[0])
                                    continue;

                                var episodeTemp = new EpisodeToRename(DBHelper.GetEpisode(Serie.IDBanco, episodeToClone.SeasonNumber, int.Parse(itemArray)));
                                episodeToClone.EpisodeName += " & " + episodeTemp.EpisodeName;
                            }
                            Clone(episodeToClone);
                            return true;
                        }
                    }
                }
            }
            catch (Exception e) { Helper.TratarException(e, "Ocorreu um erro ao reconhecer o episódio " + Filename, true); }
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