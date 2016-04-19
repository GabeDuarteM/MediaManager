// Developed by: Gabriel Duarte
// 
// Created at: 30/08/2015 18:52
// Last update: 19/04/2016 02:47

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace MediaManager.Model
{
    [DebuggerDisplay("{sDsAlias} {nNrTemporada}x{nNrEpisodio}")]
    public class SerieAlias : ModelBase
    {
        public SerieAlias()
        {
        }

        public SerieAlias(string nomeAlias)
        {
            sDsAlias = nomeAlias;
            nNrTemporada = 1;
            nNrEpisodio = 1;
        }

        public SerieAlias(SerieAlias serieAlias)
        {
            Clone(serieAlias);
        }

        public string sDsAlias { get; set; }

        public int nNrEpisodio { get; set; }

        [Key]
        public int nCdAlias { get; set; }

        [Required]
        public int nCdVideo { get; set; }

        [Column(Order = 2), ForeignKey("nCdVideo")]
        public Serie oSerie { get; set; }

        public int nNrTemporada { get; set; }
    }
}
