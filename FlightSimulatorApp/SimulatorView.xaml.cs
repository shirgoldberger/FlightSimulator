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

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for SimulatorView.xaml
    /// </summary>
    public partial class SimulatorView : Page
    {
        SimulatorViewModel vm;
        double elevator, rudder;
        LocationRect bounds;
        double preX, preY;
        private bool firstTime = true;

        public SimulatorView(HomePage homePage,string ip, int port)
        {
            InitializeComponent();
            this.vm = new SimulatorViewModel(new MySimulatorModel(new Telnet(ip, port)));
            vm.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName.Equals("VM_ServerError") && vm.VM_ServerError)
                {

                    MessageBox.Show("We lost contact with the simulator," +
                        " you are redirected to the log in page", "Server Problem", MessageBoxButton.OK, MessageBoxImage.Error);


                    Dispatcher.Invoke(new Action(() =>
                    {

                        if (NavigationService.CanGoBack)
                        {
                            vm.VM_ServerError = false;
                            this.NavigationService.GoBack();
                        }
                    }));
                }

                if (e.PropertyName.Equals("VM_ReadError") && vm.VM_ReadError)
                {

                    MessageBoxResult result =  MessageBox.Show("we didnt get response from the simulator server, therefor the program has stoped.\n" +
                        "press OK to be redirected to the log in page or Cancel to stay", "Read data Problem", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            Dispatcher.Invoke(new Action(() =>
                            {

                                if (NavigationService.CanGoBack)
                                {
                                    this.vm.disconnect();
                                    vm.VM_ReadError = false;
                                    this.NavigationService.GoBack();

                                }
                            }));

                            break;
                        case MessageBoxResult.Cancel:
                            break;
                    }

                }

                if (e.PropertyName.Equals("VM_LongError") && vm.VM_LongError)
                {

                    MessageBoxResult result = MessageBox.Show("we recieve an invalid Longtitude value, therefor we didnt change the current value\n" +
                        "press OK to be redirected to the log in page or Cancel to stay", "Invalid value detected", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            Dispatcher.Invoke(new Action(() =>
                            {

                                if (NavigationService.CanGoBack)
                                {
                                    this.vm.disconnect();
                                    vm.VM_LongError = false;
                                    this.NavigationService.GoBack();
                                }
                            }));

                            break;
                        case MessageBoxResult.Cancel:
                            break;
                    }

                }

                if (e.PropertyName.Equals("VM_LatError") && vm.VM_LatError)
                {

                    MessageBoxResult result = MessageBox.Show("we recieve an invalid latitude value, therefor we didnt change the current value\n" +
                        "press OK to be redirected to the log in page or Cancel to stay", "Invalid value detected", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            Dispatcher.Invoke(new Action(() =>
                            {

                                if (NavigationService.CanGoBack)
                                {
                                    this.vm.disconnect();
                                    vm.VM_LatError = false;
                                    this.NavigationService.GoBack();
                                }
                            }));

                            break;
                        case MessageBoxResult.Cancel:
                            break;
                    }

                }

            };
            joystick1.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                var args = e as PropertyChangedExtendedEventArgs;
                if (args != null)
                {
                    string property = args.PropertyName as string;
                    if (property.Equals("Elevator"))
                    {
                        V_Elevator = (double)args.NewValue;
                    }
                    else
                    {
                        if (property.Equals("Rudder"))
                        {
                            V_Rudder = (double)args.NewValue;
                        }
                    }
                }


            };
            DataContext = vm;

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
                this.vm.VM_Elevator = this.elevator;
            }
        }
        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            this.vm.disconnect();
            HomePage hp = new HomePage();
            this.NavigationService.Navigate(hp);
        }
        private void pin_LayoutUpdated(object sender, EventArgs e)
        {
            if (pin.Location != null)
            {
                this.bounds = myMap.BoundingRectangle;
                double centerLat = bounds.Center.Latitude;
                double centerLon = bounds.Center.Longitude;
                Console.WriteLine("x: " + PlainPosition.X);
                Console.WriteLine("y: " + PlainPosition.Y);
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
                this.vm.VM_Rudder = this.rudder;
            }
        }
    }
}