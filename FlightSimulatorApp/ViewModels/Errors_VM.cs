﻿using System;
using System.ComponentModel;
using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.ViewModels
{
    public class Errors_VM : INotifyPropertyChanged
    {
        private MySimulatorModel model;
        public Errors_VM(MySimulatorModel m)
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

        // properties of errors
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
        public bool VM_ConnectError
        {
            get
            {
                return this.model.ConnectError;
            }
            set
            {
                this.model.ConnectError = value;
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
