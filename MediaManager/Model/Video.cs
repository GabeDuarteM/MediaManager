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
        string Title { get; set; }

        string FolderPath { get; set; }

        Images Images { get; set; }

        string Overview { get; set; }
    }
}