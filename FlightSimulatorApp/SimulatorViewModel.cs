using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlightSimulatorApp
{
    class SimulatorViewModel : INotifyPropertyChanged
    {
        private MySimulatorModel model;
        public SimulatorViewModel(MySimulatorModel m)
        {
            this.model = m;
            m.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
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


        public double MV_IndicatedHeadingDeg
        {
            get { return model.IndicatedHeadingDeg; }
        }
        public double MV_GpsIndicatedVerticalSpeed
        {
            get { return model.GpsIndicatedVerticalSpeed; }
        }
        public double MV_GpsIndicatedGroundSpeedKt
        {
            get
            {
                return model.GpsIndicatedGroundSpeedKt;
            }
        }
        public double MV_AirspeedIndicatorIndicatedSpeedKt
        {
            get { return model.AirspeedIndicatorIndicatedSpeedKt; }
        }

        public double MV_GpsIndicatedAltitudeFt { get { return model.GpsIndicatedAltitudeFt; } }
        public double MV_AttitudeIndicatorInternalRollDeg { get { return model.AttitudeIndicatorInternalRollDeg; } }
        public double MV_AttitudeIndicatorInternalPitchDeg { get { return model.AttitudeIndicatorInternalPitchDeg; } }
        public double MV_AltimeterIndicatedAltitudeFt { get { return model.AltimeterIndicatedAltitudeFt; } }



        public double MV_Rudder
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
        public double MV_Elevator
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
        public double MV_Aileron
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
        public double MV_Throttle
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

    }
}