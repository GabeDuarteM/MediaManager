using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MediaManager.Model
{
    [XmlRoot("Data", Namespace = "", IsNullable = false)]
    public class SeriesData
    {
        [XmlElement("Episode")]
        public Episodio[] Episodios { get; set; }

        [XmlElement("Series")]
        public Serie[] Series { get; set; }
    }
}