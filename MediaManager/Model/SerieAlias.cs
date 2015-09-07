using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Model
{
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
    }
}