// Developed by: Gabriel Duarte
// 
// Created at: 21/04/2016 16:28

using System.Collections.Generic;
using System.Diagnostics;
using Argotic.Syndication;
using MediaManager.Model;

namespace MediaManager.ViewModel
{
    [DebuggerDisplay("{ObjEpisodio.nNrTemporada}x{ObjEpisodio.nNrEpisodio} ({ObjEpisodio.nNrAbsoluto}) - {ObjEpisodio.sDsEpisodio}")]
    public class ItemDownload
    {
        public Dictionary<RssItem, QualidadeDownload> LstObjRssItem { get; set; }

        public Episodio ObjEpisodio { get; set; }
    }
}
