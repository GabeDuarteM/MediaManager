﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Model
{
    public class Serie_Alias
    {
        public string AliasName { get; set; }

        public int Episodio { get; set; }

        public int ID { get; set; }

        public int IDSerie { get; set; }

        [Column(Order = 2), ForeignKey("IDSerie"), Required]
        public Serie Serie { get; set; }

        public int Temporada { get; set; }
    }
}