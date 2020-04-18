using System;
using System.ComponentModel;
using Microsoft.Maps.MapControl.WPF;
using FlightSimulatorApp.Model;
namespace FlightSimulatorApp.ViewModels
{
    public class Get_VM : INotifyPropertyChanged
    {
        private MySimulatorModel model;
        public Get_VM(MySimulatorModel m)
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

        // 8 properties that get from the simulator.
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

        // The location of the plane on the map.
        public Location VM_Location
        {
            get
            {
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
    }
}
