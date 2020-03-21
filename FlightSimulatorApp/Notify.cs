using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace FlightSimulatorApp
{
    interface Notify
    {
        event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged(string propertyName, object newValue);
    }
}
