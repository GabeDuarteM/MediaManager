using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MediaManager.Model
{
    public interface Video : INotifyPropertyChanged
    {
        string FolderPath { get; set; }
        Ids Ids { get; set; }
        Images Images { get; set; }
        string MetadataFolder { get; set; }
        string Overview { get; set; }
        string Title { get; set; }
    }
}