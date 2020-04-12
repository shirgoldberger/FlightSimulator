using FlightSimulatorApp.ViewModels;
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
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp.Views
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : UserControl
    {
        private bool firstTime = true;
        private int zoom = 5;
        public Map()
        {
            InitializeComponent();
        }

        private void focus_Click(object sender, RoutedEventArgs e)
        {
            double latitude = pin.Location.Latitude;
            double longtitude = pin.Location.Longitude;
            myMap.SetView(new Location(latitude, longtitude), zoom);
            PlainPosition.X = 0;
            PlainPosition.Y = 0;
        }

        private void pin_LayoutUpdated(object sender, EventArgs e)
        {
            if (pin.Location != null)
            {
                double latitude = pin.Location.Latitude;
                double longtitude = pin.Location.Longitude;
                if (firstTime)
                {
                    myMap.SetView(new Location(latitude, longtitude), zoom);
                    PlainPosition.X = 0;
                    PlainPosition.Y = 0;
                    firstTime = false;
                }

            }
        }

    }
}
