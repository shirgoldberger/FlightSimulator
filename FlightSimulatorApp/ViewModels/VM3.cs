using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.ViewModels
{
    class VM3 : INotifyPropertyChanged
    {
        private MySimulatorModel model;
        public VM3(MySimulatorModel m)
        {
            this.model = m;
            m.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public bool VM_ServerError
        {
            get
            {
                return this.model.ServerError;
            }
            set
            {
                this.model.ServerError = value;
            }
        }

        public bool VM_ReadError
        {
            get
            {
                return this.model.ReadError;
            }
            set
            {
                this.model.ReadError = value;
            }
        }

        public bool VM_LatError
        {
            get
            {
                return this.model.LatError;
            }
            set
            {
                this.model.LatError = value;
            }
        }

        public bool VM_LongError
        {
            get
            {
                return this.model.LongError;
            }
            set
            {
                this.model.LongError = value;
            }
        }

        public void disconnect()
        {
            this.model.disconnect();
        }
    }
}
