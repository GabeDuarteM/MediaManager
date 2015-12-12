using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Model
{
    [DebuggerDisplay("{sDsAlias} {nNrTemporada}x{nNrEpisodio}")]
    public class SerieAlias : ModelBase
    {
        public string sDsAlias { get; set; }

        public int nNrEpisodio { get; set; }

        [Key]
        public int nCdAlias { get; set; }

        [Required]
        public int nCdVideo { get; set; }

        [Column(Order = 2), ForeignKey("nCdVideo")]
        public Serie oSerie { get; set; }

        public int nNrTemporada { get; set; }

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
    }
}