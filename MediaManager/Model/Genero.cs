using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MediaManager.Model
{
    [Table("Generos")]
    public class Genero
    {
        [Key]
        public int IdGenero { get; set; }

        public string NomeGenero { get; set; }

        public int? IdSerie { get; set; }

        public Serie Serie { get; set; }

        public int? IdFilme { get; set; }

        public Filme Filme { get; set; }
    }
}