// Developed by: Gabriel Duarte
// 
// Created at: 14/11/2015 00:35
// Last update: 19/04/2016 02:57

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
