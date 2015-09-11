using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Model
{
    [DebuggerDisplay("{AliasName} {Temporada}x{Episodio}")]
    public class SerieAlias
    {
        public string AliasName { get; set; }

        public int Episodio { get; set; }

        public int ID { get; set; }

        [Required]
        public int IDSerie { get; set; }

        [Column(Order = 2), ForeignKey("IDSerie")]
        public Serie Serie { get; set; }

        public int Temporada { get; set; }

        public SerieAlias()
        {
        }

        public SerieAlias(string aliasName)
        {
            AliasName = aliasName;
            Temporada = 1;
            Episodio = 1;
        }
    }
}