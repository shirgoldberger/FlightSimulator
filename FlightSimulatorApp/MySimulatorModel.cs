using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using System.Net.Sockets;
using System.Windows;
namespace FlightSimulatorApp
{
    class MySimulatorModel : ISimulatorModel
    {

        double indicatedHeadingDeg, gpsIndicatedVerticalSpeed, gpsIndicatedGroundSpeedKt, airspeedIndicatorIndicatedSpeedKt,
            gpsIndicatedAltitudeFt, attitudeIndicatorInternalRollDeg, attitudeIndicatorInternalPitchDeg, altimeterIndicatedAltitudeFt;

        double rudder = 0, elevator = 0, throttle, aileron;
        double latitude, longitude;
        Queue<string> update = new Queue<string>();
        ITelnetClient telnetClient;
        volatile Boolean stop;
        public MySimulatorModel(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            stop = false;
            this.connect();
        }

        public void connect()
        {
            telnetClient.connect();
            this.start();
        }
        public void disconnect()
        {
            stop = true;
            telnetClient.disconnect();
        }

        public void start()
        {
            new Thread(delegate ()
            {
                String msg;
                while (!stop)
                {
                    // 1
                    telnetClient.write("get /instrumentation/gps/indicated-ground-speed-kt\n");
                    msg = telnetClient.read();
                    if (!msg.Contains("ERR"))
                    {
                        IndicatedHeadingDeg = Double.Parse(msg);
                    }
                    // 2
                    telnetClient.write("get /instrumentation/gps/indicated-vertical-speed\n");
                    msg = telnetClient.read();
                    if (!msg.Contains("ERR"))
                    {
                        GpsIndicatedVerticalSpeed = Double.Parse(msg);
                    }
                    // 3
                    telnetClient.write("get /instrumentation/gps/indicated-ground-speed-kt\n");
                    msg = telnetClient.read();
                    if (!msg.Contains("ERR"))
                    {
                        GpsIndicatedGroundSpeedKt = Double.Parse(msg);
                    }
                    // 4
                    telnetClient.write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
                    msg = telnetClient.read();
                    if (!msg.Contains("ERR"))
                    {
                        AirspeedIndicatorIndicatedSpeedKt = Double.Parse(msg);
                    }
                    // 5
                    telnetClient.write("get /instrumentation/gps/indicated-altitude-ft\n");
                    msg = telnetClient.read();
                    if (!msg.Contains("ERR"))
                    {
                        GpsIndicatedAltitudeFt = Double.Parse(msg);
                    }
                    // 6
                    telnetClient.write("get /instrumentation/attitude-indicator/indicated-roll-deg\n");
                    msg = telnetClient.read();
                    if (!msg.Contains("ERR"))
                    {
                        AttitudeIndicatorInternalRollDeg = Double.Parse(msg);
                    }
                    // 7
                    telnetClient.write("get /instrumentation/attitude-indicator/indicated-pitch-deg\n");
                    msg = telnetClient.read();
                    if (!msg.Contains("ERR"))
                    {
                        AttitudeIndicatorInternalPitchDeg = Double.Parse(msg);
                    }
                    // 8
                    telnetClient.write("get /instrumentation/altimeter/indicated-altitude-ft\n");
                    msg = telnetClient.read();
                    if (!msg.Contains("ERR"))
                    {
                        AltimeterIndicatedAltitudeFt = Double.Parse(msg);
                    }
                    while (this.update.Count != 0)
                    {
                        string s = "set " + update.Dequeue();
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
                    this.NotifyPropertyChanged("IndicatedHeadingDeg");
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
                    this.NotifyPropertyChanged("GpsIndicatedVerticalSpeed");
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
                    this.NotifyPropertyChanged("GpsIndicatedGroundSpeedKt");
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
                    this.NotifyPropertyChanged("AirspeedIndicatorIndicatedSpeedKt");
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
                    this.NotifyPropertyChanged("GpsIndicatedAltitudeFt");
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
                    this.NotifyPropertyChanged("AttitudeIndicatorInternalRollDeg");
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
                    this.NotifyPropertyChanged("AttitudeIndicatorInternalPitchDeg");
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
                    this.NotifyPropertyChanged("AltimeterIndicatedAltitudeFt");
                }
            }
        }

        public double Rudder
        {
            get { return this.rudder; }
            set
            {
                if (value > 1)
                {
                    this.rudder = 1;
                }
                else if (value < -1)
                {
                    this.rudder = -1;
                }
                else
                {
                    this.rudder = value;
                }
                this.update.Enqueue("/controls/flight/rudder " + value);
            }
        }
        public double Elevator
        {
            get { return this.elevator; }
            set
            {
                if (value > 1)
                {
                    this.elevator = 1;
                }
                else if (value < -1)
                {
                    this.elevator = -1;
                }
                else
                {
                    this.elevator = value;
                }
                this.elevator = value;
                this.update.Enqueue("/controls/flight/elevator " + value);
            }
        }
        public double Aileron
        {
            get { return this.aileron; }
            set
            {
                if (value > 1)
                {
                    this.aileron = 1;
                }
                else if (value < -1)
                {
                    this.aileron = -1;
                }
                else
                {
                    this.aileron = value;
                }
                this.aileron = value;
                this.update.Enqueue("/controls/flight/aileron " + value);
            }
        }
        public double Throttle
        {
            get { return this.throttle; }
            set
            {
                if (value > 1)
                {
                    this.throttle = 1;
                }
                else if (value < 0)
                {
                    this.throttle = 0;
                }
                else
                {
                    this.throttle = value;
                }
                this.throttle = value;
                this.update.Enqueue("/controls/engines/engine/throttle " + value);
            }
        }
        public double Longitude
        {
            get { return this.longitude; }
            set
            {
                if (this.longitude != value)
                {
                    this.longitude = value;
                    this.NotifyPropertyChanged("Longitude");
                }
            }
        }
        public double Latitude
        {
            get { return this.latitude; }
            set
            {
                if (this.latitude != value)
                {
                    this.latitude = value;
                    this.NotifyPropertyChanged("Latitude");
                }
            }
        }
    }
}

