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
    public class SerieAlias
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

        public void Clone(object objOrigem)
        {
            PropertyInfo[] variaveisObjOrigem = objOrigem.GetType().GetProperties();
            PropertyInfo[] variaveisObjAtual = GetType().GetProperties();

            foreach (PropertyInfo item in variaveisObjOrigem)
            {
                PropertyInfo variavelIgual = variaveisObjAtual.FirstOrDefault(x => x.Name == item.Name && x.PropertyType == item.PropertyType);

                if (variavelIgual != null && variavelIgual.CanWrite)
                {
                    variavelIgual.SetValue(this, item.GetValue(objOrigem, null));
                }
            }

            return;
        }
    }
}