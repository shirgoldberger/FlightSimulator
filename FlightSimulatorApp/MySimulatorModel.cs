using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace FlightSimulatorApp
{
    class MySimulatorModel : ISimulatorModel
    {

        double indicatedHeadingDeg = 0, gpsIndicatedVerticalSpeed, gpsIndicatedGroundSpeedKt, airspeedIndicatorIndicatedSpeedKt,
            gpsIndicatedAltitudeFt, attitudeIndicatorInternalRollDeg, attitudeIndicatorInternalPitchDeg, altimeterIndicatedAltitudeFt;

        double rudder, elevator, throttle, aileron;
        Queue<string> update = new Queue<string>();
        ITelnetClient telnetClient;
        volatile Boolean stop;
        public MySimulatorModel(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            stop = false;
        }

        public void connect(string ip, int port)
        {
            telnetClient.connect();
        }
        public void disconnect()
        {
            stop = true;
            telnetClient.disconnect();
        }

        public void start()
        {
            new Thread(delegate () {
                while (!stop)
                {
                    telnetClient.write("get indicatedHeadingDeg");
                    indicatedHeadingDeg = Double.Parse(telnetClient.read());
                    while (this.update.Count != 0)
                    {
                        string s = "set" + update.Dequeue();
                        telnetClient.write(s);
                    }

                    // the same for the other sensors properties
                    Thread.Sleep(250);// read the data in 4Hz
                }
            }).Start();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }


        public double IndicatedHeadingDeg
        {
            get { return this.indicatedHeadingDeg; }
            set
            {
                if (this.indicatedHeadingDeg != value)
                {
                    this.indicatedHeadingDeg = value;
                    this.NotifyPropertyChanged("indicatedHeadingDeg");
                }
            }
        }
        public double GpsIndicatedVerticalSpeed
        {
            get { return this.gpsIndicatedVerticalSpeed; }
            set
            {
                if (this.gpsIndicatedVerticalSpeed != value)
                {
                    this.gpsIndicatedVerticalSpeed = value;
                    this.NotifyPropertyChanged("indicatedHeadingDeg");
                }
            }
        }
        public double GpsIndicatedGroundSpeedKt
        {
            get { return this.gpsIndicatedGroundSpeedKt; }
            set
            {
                if (this.gpsIndicatedGroundSpeedKt != value)
                {
                    this.gpsIndicatedGroundSpeedKt = value;
                    this.NotifyPropertyChanged("indicatedHeadingDeg");
                }
            }
        }
        public double AirspeedIndicatorIndicatedSpeedKt
        {
            get { return this.airspeedIndicatorIndicatedSpeedKt; }
            set
            {
                if (this.airspeedIndicatorIndicatedSpeedKt != value)
                {
                    this.airspeedIndicatorIndicatedSpeedKt = value;
                    this.NotifyPropertyChanged("indicatedHeadingDeg");
                }
            }
        }

        public double GpsIndicatedAltitudeFt
        {
            get { return this.gpsIndicatedAltitudeFt; }
            set
            {
                if (this.gpsIndicatedAltitudeFt != value)
                {
                    this.gpsIndicatedAltitudeFt = value;
                    this.NotifyPropertyChanged("gpsIndicatedAltitudeFt");
                }
            }
        }
        public double AttitudeIndicatorInternalRollDeg
        {
            get { return this.attitudeIndicatorInternalRollDeg; }
            set
            {
                if (this.attitudeIndicatorInternalRollDeg != value)
                {
                    this.attitudeIndicatorInternalRollDeg = value;
                    this.NotifyPropertyChanged("attitudeIndicatorInternalRollDeg");
                }
            }
        }
        public double AttitudeIndicatorInternalPitchDeg
        {
            get { return this.attitudeIndicatorInternalPitchDeg; }
            set
            {
                if (this.attitudeIndicatorInternalPitchDeg != value)
                {
                    this.attitudeIndicatorInternalPitchDeg = value;
                    this.NotifyPropertyChanged("attitudeIndicatorInternalPitchDeg");
                }
            }
        }
        public double AltimeterIndicatedAltitudeFt
        {
            get { return this.altimeterIndicatedAltitudeFt; }
            set
            {
                if (this.altimeterIndicatedAltitudeFt != value)
                {
                    this.altimeterIndicatedAltitudeFt = value;
                    this.NotifyPropertyChanged("altimeterIndicatedAltitudeFt");
                }
            }
        }

        public double Rudder
        {
            get { return this.rudder; }
            set
            {
                this.rudder = value;
                this.update.Enqueue("rudder");
            }
        }
        public double Elevator
        {
            get { return this.elevator; }
            set
            {
                this.elevator = value;
                this.update.Enqueue("elevator");
            }
        }
        public double Aileron
        {
            get { return this.aileron; }
            set
            {
                this.aileron = value;
                this.update.Enqueue("aileron");
            }
        }
        public double Throttle
        {
            get { return this.throttle; }
            set
            {
                this.throttle = value;
                this.update.Enqueue("throttle");
            }
        }
    }
}

