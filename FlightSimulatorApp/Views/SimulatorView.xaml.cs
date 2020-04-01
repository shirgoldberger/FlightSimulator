using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Microsoft.Maps.MapControl.WPF;
using System.Runtime.InteropServices;
using System.Threading;
using FlightSimulatorApp.ViewModels;
using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.Views
{
    /// <summary>
    /// Interaction logic for SimulatorView.xaml
    /// </summary>
    public partial class SimulatorView : Page, INotifyPropertyChanged
    {
        VM1 vm1;
        VM2 vm2;
        VM3 vm3;
        double elevator, rudder, throttle, aileron;
        LocationRect bounds;
        double preX, preY;
        private bool firstTime = true;
        private string message;
        public SimulatorView(HomePage homePage, string ip, int port)
        {
            InitializeComponent();

            MySimulatorModel m = new MySimulatorModel(new Telnet(ip, port));
            this.vm1 = new VM1(m);
            this.vm2 = new VM2(m);
            this.vm3 = new VM3(m);

            fromSim.DataContext = vm1;
            myMap.DataContext = vm1;
            myMessage.DataContext = this;

            vm3.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName.Equals("VM_ServerError") && vm3.VM_ServerError)
                {

                    Message = "We lost contact with the simulator, you are redirected to the log in page";
                    Thread.Sleep(5000);
                    Dispatcher.Invoke(new Action(() =>
                    {

                        if (NavigationService.CanGoBack)
                        {
                            this.NavigationService.GoBack();
                        }
                    }));
                }

                if (e.PropertyName.Equals("VM_ReadError") && vm3.VM_ReadError)
                {

                    Message = "we didnt get response from the simulator server, therefor the program has stoped.\n" +
                        "you are redirected to the log in page";
                    Thread.Sleep(5000);
                    Dispatcher.Invoke(new Action(() =>
                    {

                        if (NavigationService.CanGoBack)
                        {
                            this.NavigationService.GoBack();
                        }
                    }));
                }

                if (e.PropertyName.Equals("VM_LatError") && vm3.VM_LatError)
                {

                    Message = "we have recieve an invalid latitude value therefor the latititude hasn't been update/n" +
                    "you can click back to go back to the log in page and try again";
                    Thread.Sleep(5000);
                }

                if (e.PropertyName.Equals("VM_LongError") && vm3.VM_LongError)
                {

                    Message = "we have recieve an invalid longtitude value therefor the latititude hasn't been update/n" +
                    "you can click back to go back to the log in page and try again";
                    Thread.Sleep(5000);
                }

            };
            sliders.PropertyChangedNotify += delegate (Object sender, PropertyChangedEventArgs e)
            {
                var args = e as PropertyChangedExtendedEventArgs;
                if (args != null)
                {
                    string property = args.PropertyName as string;
                    if (property.Equals("Elevator"))
                    {
                        V_Elevator = (double)args.NewValue;
                    }
                    else if (property.Equals("Rudder"))
                    {
                        {
                            V_Rudder = (double)args.NewValue;
                        }
                    }
                    else if (property.Equals("Throttle"))
                    {
                        {
                            V_Throttle = (double)args.NewValue;
                        }
                    }
                    else if (property.Equals("Aileron"))
                    {
                        {
                            V_Aileron = (double)args.NewValue;
                        }
                    }
                }
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

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            this.vm3.disconnect();
            this.NavigationService.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.vm3.disconnect();
            System.Environment.Exit(0);
        }

        private void pin_LayoutUpdated(object sender, EventArgs e)
        {
            if (pin.Location != null)
            {
                this.bounds = myMap.BoundingRectangle;
                double centerLat = bounds.Center.Latitude;
                double centerLon = bounds.Center.Longitude;
                //Update the current latitude and longitude
                double latitude = pin.Location.Latitude;
                double longtitude = pin.Location.Longitude;
                if (firstTime)
                {
                    myMap.SetView(new Location(latitude, longtitude), 4);
                    PlainPosition.X = 0;
                    PlainPosition.Y = 0;
                    firstTime = false;
                    preX = latitude;
                    preY = longtitude;
                    return;
                }
                if ((longtitude - preY) == 0)
                {

                }
                else
                {
                    double m = (latitude - preX) / (longtitude - preY);

                    if (m > 0 && latitude + 0.5 >= bounds.North)
                    {
                        if (longtitude >= centerLon)
                        {
                            myMap.SetView(new Location(2 * latitude - centerLat - 2.5, 2 * longtitude - centerLon), 4);
                        }
                        else
                        {
                            myMap.SetView(new Location(2 * latitude - centerLat - 2.5, centerLon), 4);
                        }
                    }

                    else if (m < 0 && latitude - 2.5 <= bounds.South)
                    {
                        if (longtitude >= centerLon)
                        {
                            myMap.SetView(new Location(2 * latitude - centerLat + 2.5, 2 * longtitude - centerLon), 4);
                        }
                        else
                        {
                            myMap.SetView(new Location(2 * latitude - centerLat + 2.5, centerLon), 4);
                        }
                    }

                    else if (m > 0 && longtitude + 0.5 >= bounds.East)
                    {
                        if (latitude >= centerLat)
                        {
                            myMap.SetView(new Location(2 * latitude - centerLat + (bounds.North - centerLat), 2 * longtitude - centerLon - 2.5), 4);
                        }
                        else
                        {
                            myMap.SetView(new Location(centerLat, 2 * longtitude - centerLon - 2.5), 4);
                        }
                    }

                    else if (m < 0 && longtitude + 0.5 >= bounds.East)
                    {
                        if (latitude <= centerLat)
                        {
                            myMap.SetView(new Location(2 * latitude - centerLat + (bounds.North - centerLat), 2 * longtitude - centerLon - 2.5), 4);
                        }
                        else
                        {
                            myMap.SetView(new Location(centerLat, 2 * longtitude - centerLon - 2.5), 4);
                        }
                    }

                    else if (m < 0 && longtitude - 2.5 <= bounds.West)
                    {
                        if (latitude >= centerLat)
                        {
                            myMap.SetView(new Location(2 * latitude - centerLat + (bounds.North - centerLat), 2 * longtitude - centerLon + 2.5), 4);
                        }
                        else
                        {
                            myMap.SetView(new Location(centerLat, 2 * longtitude - centerLon + 2.5), 4);
                        }
                    }

                    else if (m > 0 && longtitude - 2.5 <= bounds.West)
                    {
                        if (latitude <= centerLat)
                        {
                            myMap.SetView(new Location(2 * latitude - centerLat + (bounds.North - centerLat), 2 * longtitude - centerLon + 2.5), 4);
                        }
                        else
                        {
                            myMap.SetView(new Location(centerLat, 2 * longtitude - centerLon + 2.5), 4);
                        }
                    }
                }
                preX = latitude;
                preY = longtitude;
            }
        }

        public double V_Rudder
        {
            get
            {
                return this.rudder;
            }
            set
            {
                this.rudder = value;
                this.vm2.VM_Rudder = this.rudder;
            }
        }
        public double V_Throttle
        {
            get
            {
                return this.throttle;
            }
            set
            {
                this.throttle = value;
                this.vm2.VM_Throttle = this.throttle;
            }
        }
        public double V_Aileron
        {
            get
            {
                return this.aileron;
            }
            set
            {
                this.aileron = value;
                this.vm2.VM_Aileron = this.aileron;
            }
        }
        public double V_Elevator
        {
            get
            {
                return this.elevator;
            }
            set
            {
                this.elevator = value;
                this.vm2.VM_Elevator = this.elevator;
            }
        }
        public string Message
        {
            get { return this.message; }
            set
            {
                this.message = value;
                NotifyPropertyChanged("Message");
            }
        }
    }
}