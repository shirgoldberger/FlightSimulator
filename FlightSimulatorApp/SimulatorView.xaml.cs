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
        SimulatorViewModel VM;
        double elevator, rudder;
        LocationRect bounds;
        bool flag = false;
        public SimulatorView(string ip, int port)
        {
            InitializeComponent();
            this.VM = new SimulatorViewModel(new MySimulatorModel(new Telnet(ip, port)));
            
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
            DataContext = VM;

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
                Elevator_Slider.Value = value;
                this.VM.VM_Elevator = this.elevator;
            }
        }



        private void pin_LayoutUpdated(object sender, EventArgs e)
        {
            if (pin.Location != null)
            {
                this.bounds = myMap.BoundingRectangle;
                //Update the current latitude and longitude
                double latitude = pin.Location.Latitude;
                double longtitude = pin.Location.Longitude;
                if (latitude + 1 >= bounds.North || latitude - 1 <= bounds.South || longtitude + 1 >= bounds.East || longtitude - 1 <= bounds.West)
                {
                    myMap.SetView(new Location(latitude, longtitude), 5);
                    PlainPosition.X = 0;
                    PlainPosition.Y = 0;
                }
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
                Rudder_Slider.Value = value;
                this.VM.VM_Rudder = this.rudder;
            }
        }
    }
}