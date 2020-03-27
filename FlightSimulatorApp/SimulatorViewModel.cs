using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp
{
    class SimulatorViewModel : INotifyPropertyChanged
    {
        private MySimulatorModel model;
        public SimulatorViewModel(MySimulatorModel m)
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

        // 8 properties that get from the simulator
        public string VM_IndicatedHeadingDeg
        {
            get { return String.Format("{0:0.000}", model.IndicatedHeadingDeg); }
        }
        public string VM_GpsIndicatedVerticalSpeed
        {
            get { return String.Format("{0:0.000}", model.GpsIndicatedVerticalSpeed); }
        }
        public string VM_GpsIndicatedGroundSpeedKt
        {
            get { return String.Format("{0:0.000}", model.GpsIndicatedGroundSpeedKt); }
        }
        public string VM_AirspeedIndicatorIndicatedSpeedKt
        {
            get { return String.Format("{0:0.000}", model.AirspeedIndicatorIndicatedSpeedKt); }
        }
        public string VM_GpsIndicatedAltitudeFt 
        { 
            get { return String.Format("{0:0.000}", model.GpsIndicatedAltitudeFt); } 
        }
        public string VM_AttitudeIndicatorInternalRollDeg 
        {
            get { return String.Format("{0:0.000}", model.AttitudeIndicatorInternalRollDeg); } 
        }
        public string VM_AttitudeIndicatorInternalPitchDeg 
        {
            get { return String.Format("{0:0.000}", model.AttitudeIndicatorInternalPitchDeg); } 
        }
        public string VM_AltimeterIndicatedAltitudeFt 
        {
            get { return String.Format("{0:0.000}", model.AltimeterIndicatedAltitudeFt); } 
        }

        // 4 properties that set to the simulator
        public double VM_Rudder
        {
            get
            {
                return this.model.Rudder;
            }
            set
            {
                this.model.Rudder = value;
            }
        }
        public double VM_Elevator
        {
            get
            {
                return this.model.Elevator;
            }
            set
            {
                this.model.Elevator = value;
            }
        }
        public double VM_Aileron
        {
            get
            {
                return this.model.Aileron;
            }
            set
            {
                this.model.Aileron = value;
            }
        }
        public double VM_Throttle
        {
            get
            {
                return this.model.Throttle;
            }
            set
            {
                this.model.Throttle = value;
            }
        }

        public void disconnect()
        {
            this.model.disconnect();
        }

        // the location of the plane on the map
        public Location VM_Location
        {
            get {
                return model.Location;
            }
        }


        public string VM_LongitudeT
        {
            get { return "Longitude: " + String.Format("{0:0.000}", model.Longitude); }
        }
        public string VM_LatitudeT
        {
            get { return "Latitude: " + String.Format("{0:0.000}", model.Latitude); }
        }
        public bool VM_ServerError
        {
            get
            {
                return this.model.ServerError;
            }
            set
            {
                this.model.LongError = value;
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
            set {
                this.model.LongError = value;
            }
        }
    }
}