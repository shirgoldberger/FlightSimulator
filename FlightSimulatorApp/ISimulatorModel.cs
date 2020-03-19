using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp
{
    interface ISimulatorModel : INotifyPropertyChanged
    {
        // connection to the simulator
        void connect(string ip, int port);
        void disconnect();
        void start();

        // Dashboard
        double IndicatedHeadingDeg { get; set; }
        double GpsIndicatedVerticalSpeed { get; set; }
        double GpsIndicatedGroundSpeedKt { get; set; }
        double AirspeedIndicatorIndicatedSpeedKt { get; set; }
        double GpsIndicatedAltitudeFt { get; set; }
        double AttitudeIndicatorInternalRollDeg { get; set; }
        double AttitudeIndicatorInternalPitchDeg { get; set; }
        double AltimeterIndicatedAltitudeFt { get; set; }

        double Rudder { get; set; }
        double Elevator { get; set; }
        double Aileron { get; set; }
        double Throttle { get; set; }

    }
}
