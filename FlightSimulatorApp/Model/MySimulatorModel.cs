using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Microsoft.Maps.MapControl.WPF;
using System.IO;

namespace FlightSimulatorApp.Model
{
    public class MySimulatorModel
    {
        // Dashbord variables.
        double indicatedHeadingDeg, gpsIndicatedVerticalSpeed, gpsIndicatedGroundSpeedKt, airspeedIndicatorIndicatedSpeedKt,
            gpsIndicatedAltitudeFt, attitudeIndicatorInternalRollDeg, attitudeIndicatorInternalPitchDeg, altimeterIndicatedAltitudeFt;
        Thread thread;
        // Navigators variables.
        double rudder, elevator, throttle, aileron;
        // Map variables.
        double latitude, longitude;
        // Queue that contain set commands to send to simulator.
        Queue<string> update = new Queue<string>();
        ITelnetClient telnetClient;
        // Boolean variable for stopping receiving and sending data.
        volatile Boolean stop;
        // For errors.
        private bool readError;
        private bool timeout;
        private bool latError;
        private bool longError;
        private bool serverError;
        private bool connectError;
        private bool inValidError;
        // To know if we are connected.
        private string connecting = "disconnected";

        public MySimulatorModel() { }
        public void Run(string ip, int port)
        {
            // Set ip and port.
            this.telnetClient = new Telnet(ip, port);
            // Connect to the simulator.
            this.Connect();
            // Set time out to 10 seconds.
            this.telnetClient.SetTimeOutRead(10000);
        }
        public void Connect()
        {
            try
            {
                // Try connect.
                telnetClient.Connect();
                Connecting = "connected";
                Initialize();
                this.start();
            } catch (Exception e)
            {
                // We couldn't connect.
                if (e.Message == "not connected")
                {
                    this.ConnectError = true;
                }
            }
        }

        private void Initialize()
        {
            // Initialize all the variables for new running .
            stop = false;
            serverError = false;
            readError = false;
            timeout = false;
            connectError = false;
            latError = false;
            longError = false;
            inValidError = false;
            indicatedHeadingDeg = 0;
            gpsIndicatedVerticalSpeed = 0;
            gpsIndicatedGroundSpeedKt = 0;
            airspeedIndicatorIndicatedSpeedKt = 0;
            gpsIndicatedAltitudeFt = 0;
            attitudeIndicatorInternalRollDeg = 0;
            attitudeIndicatorInternalPitchDeg = 0;
            altimeterIndicatedAltitudeFt = 0;
            rudder = 0;
            elevator = 0;
            throttle = 0;
            aileron = 0;
            latitude = 0;
            longitude = 0;
        }

        public void Disconnect()
        {
            // Stop the thread that receiving and sending data.
            stop = true;
            // Stop the connection with the simulator.
            telnetClient.Disconnect();
            Connecting = "disconnected";
        }
        public void start()
        {
            this.thread = new Thread(delegate ()
            {
                String msg;
                while (!stop)
                {
                    try
                    {
                        if (timeout)
                        {
                            msg = telnetClient.Read();
                            timeout = false;
                        }
                        // 1
                        telnetClient.Write("get /instrumentation/heading-indicator/indicated-heading-deg\n");
                        msg = telnetClient.Read();
                        if (!msg.Contains("ERR"))
                        {
                            IndicatedHeadingDeg = Double.Parse(msg);
                        }
                        // 2
                        telnetClient.Write("get /instrumentation/gps/indicated-vertical-speed\n");
                        msg = telnetClient.Read();
                        if (!msg.Contains("ERR"))
                        {
                            GpsIndicatedVerticalSpeed = Double.Parse(msg);
                        }
                        // 3
                        telnetClient.Write("get /instrumentation/gps/indicated-ground-speed-kt\n");
                        msg = telnetClient.Read();
                        if (!msg.Contains("ERR"))
                        {
                            GpsIndicatedGroundSpeedKt = Double.Parse(msg);
                        }
                        // 4
                        telnetClient.Write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
                        msg = telnetClient.Read();
                        if (!msg.Contains("ERR"))
                        {
                            AirspeedIndicatorIndicatedSpeedKt = Double.Parse(msg);
                        }
                        // 5
                        telnetClient.Write("get /instrumentation/gps/indicated-altitude-ft\n");
                        msg = telnetClient.Read();
                        if (!msg.Contains("ERR"))
                        {
                            GpsIndicatedAltitudeFt = Double.Parse(msg);
                        }
                        // 6
                        telnetClient.Write("get /instrumentation/attitude-indicator/internal-roll-deg\n");
                        msg = telnetClient.Read();
                        if (!msg.Contains("ERR"))
                        {
                            AttitudeIndicatorInternalRollDeg = Double.Parse(msg);
                        }
                        // 7
                        telnetClient.Write("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
                        msg = telnetClient.Read();
                        if (!msg.Contains("ERR"))
                        {
                            AttitudeIndicatorInternalPitchDeg = Double.Parse(msg);
                        }
                        // 8
                        telnetClient.Write("get /instrumentation/altimeter/indicated-altitude-ft\n");
                        msg = telnetClient.Read();
                        if (!msg.Contains("ERR"))
                        {
                            AltimeterIndicatedAltitudeFt = Double.Parse(msg);
                        }
                        // Longitude.
                        telnetClient.Write("get /position/longitude-deg\n");
                        msg = telnetClient.Read();
                        if (!msg.Contains("ERR"))
                        {
                            Longitude = Double.Parse(msg);
                        }
                        // Latitude.
                        telnetClient.Write("get /position/latitude-deg\n");
                        msg = telnetClient.Read();
                        if (!msg.Contains("ERR"))
                        {
                            Latitude = Double.Parse(msg);
                        }
                        // Set the variables in the queue.
                        while (this.update.Count != 0)
                        {
                            string s = "set " + update.Dequeue() + "\n";
                            telnetClient.Write(s);
                            telnetClient.Read();
                        }
                        // The same for the other sensors properties.
                        Thread.Sleep(250);// read the data in 4Hz
                    }
                    catch (IOException e)
                    {
                        if (e.ToString().Contains("A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond."))
                        {
                            // Problem with reading values.
                            ReadError = true;
                            timeout = true;
                        }
                        else
                        {
                            stop = true;
                            Connecting = "disconnected";
                            ServerError = true;
                            update.Clear();
                        }
                    }
                    catch (FormatException) {
                        // Error with values.
                        InValidError = true;
                    }
                    catch (Exception)
                    {
                        // Problem with connecting to the server.
                        update.Clear();
                        stop = true;
                        if (!telnetClient.IsConnect())
                        {
                            Connecting = "disconnected";
                        }
                        ServerError = true;
                    }
                }
            });
            thread.Start();
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

        // 4 properties that set to the simulator.
        public double Rudder
        {
            get { return this.rudder; }
            set
            {
                // Check if in the range.
                if (value > 1 && this.rudder != 1)
                {
                    this.rudder = 1;
                    this.update.Enqueue("/controls/flight/rudder " + value);
                }
                else if (value < -1 && this.rudder != -1)
                {
                    this.rudder = -1;
                    this.update.Enqueue("/controls/flight/rudder " + value);
                }
                else if (Math.Abs(value - this.rudder) > Math.Pow(10, -4))
                {
                    this.rudder = value;
                    this.update.Enqueue("/controls/flight/rudder " + value);
                }
            }
        }
        public double Elevator
        {
            get { return this.elevator; }
            set
            {
                // Check if in the range.
                if (value > 1 && this.elevator != 1)
                {
                    this.elevator = 1;
                    this.update.Enqueue("/controls/flight/elevator " + value);
                }
                else if (value < -1 && this.elevator != -1)
                {
                    this.elevator = -1;
                    this.update.Enqueue("/controls/flight/elevator " + value);
                }
                else if (Math.Abs(value - this.elevator) > Math.Pow(10, -4))
                {
                    this.elevator = value;
                    this.update.Enqueue("/controls/flight/elevator " + value);
                }
            }
        }
        public double Aileron
        {
            get { return this.aileron; }
            set
            {
                // Check if in the range.
                if (value > 1 && this.aileron != 1)
                {
                    this.aileron = 1;
                    this.update.Enqueue("/controls/flight/aileron " + value);
                }
                else if (value < -1 && this.aileron != -1)
                {
                    this.aileron = -1;
                    this.update.Enqueue("/controls/flight/aileron " + value);
                }
                else if (Math.Abs(value - this.aileron) > Math.Pow(10, -4))
                {
                    this.aileron = value;
                    this.update.Enqueue("/controls/flight/aileron " + value);
                }
            }
        }
        public double Throttle
        {
            get { return this.throttle; }
            set
            {
                // Check if in the range.
                if (value > 1 && this.throttle != 1)
                {
                    this.throttle = 1;
                    this.update.Enqueue("/controls/engines/current-engine/throttle " + value);

                }
                else if (value < 0 && this.throttle != 0)
                {
                    this.throttle = 0;
                    this.update.Enqueue("/controls/engines/current-engine/throttle " + value);

                }
                else if (Math.Abs(value - this.throttle) > Math.Pow(10, -4))
                {
                    this.throttle = value;
                    this.update.Enqueue("/controls/engines/current-engine/throttle " + value);

                }
            }
        }

        // The location of the plane on the map.
        public double Longitude
        {
            get { return this.longitude; }
            set
            {
                if (this.longitude != value)
                {
                    if (value >= -180 && value <= 180)
                    {

                        this.longitude = value;
                        this.NotifyPropertyChanged("Location");
                        // For the text.
                        this.NotifyPropertyChanged("LongitudeT");
                    }
                    else
                    {
                        LongError = true;
                    }

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
                    if (value >= -90 && value <= 90)
                    {
                        this.latitude = value;
                        this.NotifyPropertyChanged("Location");
                        // For the text.
                        this.NotifyPropertyChanged("LatitudeT");
                    }
                    else
                    {
                        LatError = true;
                    }

                }
            }
        }
        public Location Location
        {
            get
            {
                return new Location(latitude, longitude);
            }
        }

        // When the simulator is not connected.
        public bool ServerError
        {
            get
            {
                return this.serverError;
            }
            set
            {
                if (!serverError)
                {
                    this.serverError = value;
                    this.NotifyPropertyChanged("ServerError");
                }
            }
        }

        // When we were unable to connect to the simulator.
        public bool ConnectError
        {
            get
            {
                return this.connectError;
            }
            set
            {
                if (!connectError)
                {
                    this.connectError = value;
                    this.NotifyPropertyChanged("ConnectError");
                }
            }
        }
        // When we were unable to connect to the simulator.
        public bool InValidError
        {
            get
            {
                return this.inValidError;
            }
            set
            {
                if (!inValidError)
                {
                    this.inValidError = value;
                    this.NotifyPropertyChanged("InValidError");
                }
                else
                {
                    if (inValidError && !value)
                    {
                        this.inValidError = value;
                    }
                }
            }
        }

        // when there is a problem with reading the variables
        public bool ReadError
        {
            get
            {
                return this.readError;
            }
            set
            {
                if (!readError && value)
                {
                    this.readError = value;
                    this.NotifyPropertyChanged("ReadError");
                }
                else
                {
                    if (readError && !value)
                    {
                        this.readError = value;
                    }
                }
            }
        }

        // When the value of the variables is out of range.
        public bool LatError
        {
            get
            {
                return this.latError;
            }
            set
            {
                if (!latError && value)
                {
                    this.latError = value;
                    this.NotifyPropertyChanged("LatError");
                }
                else
                {
                    if (latError && !value)
                    {
                        this.latError = value;
                    }
                }
            }
        }
        public bool LongError
        {
            get
            {
                return this.longError;
            }
            set
            {
                if (!longError && value)
                {
                    this.longError = value;
                    this.NotifyPropertyChanged("LongError");
                }
                else
                {
                    if (longError && !value)
                    {
                        this.longError = value;
                    }
                }
            }
        }

        // Connect / not connect.
        public string Connecting
        {
            get { return this.connecting; }
            set 
            { 
                this.connecting = value;
                this.NotifyPropertyChanged("Connecting");
            }
        }
    }
}