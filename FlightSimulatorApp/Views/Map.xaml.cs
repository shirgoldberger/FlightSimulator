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
        LocationRect bounds;
        double preX, preY;
        private bool firstTime = true;
        private int zoom = 5;
        public Map()
        {
            InitializeComponent();
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
                    myMap.SetView(new Location(latitude, longtitude), zoom);
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
                            myMap.SetView(new Location(2 * latitude - centerLat - 2.5, 2 * longtitude - centerLon), zoom);
                        }
                        else
                        {
                            myMap.SetView(new Location(2 * latitude - centerLat - 2.5, centerLon), zoom);
                        }
                    }

                    else if (m < 0 && latitude - 2.5 <= bounds.South)
                    {
                        if (longtitude >= centerLon)
                        {
                            myMap.SetView(new Location(2 * latitude - centerLat + 2.5, 2 * longtitude - centerLon), zoom);
                        }
                        else
                        {
                            myMap.SetView(new Location(2 * latitude - centerLat + 2.5, centerLon), zoom);
                        }
                    }

                    else if (m > 0 && longtitude + 0.5 >= bounds.East)
                    {
                        if (latitude >= centerLat)
                        {
                            myMap.SetView(new Location(2 * latitude - centerLat + (bounds.North - centerLat), 2 * longtitude - centerLon - 2.5), zoom);
                        }
                        else
                        {
                            myMap.SetView(new Location(centerLat, 2 * longtitude - centerLon - 2.5), zoom);
                        }
                    }

                    else if (m < 0 && longtitude + 0.5 >= bounds.East)
                    {
                        if (latitude <= centerLat)
                        {
                            myMap.SetView(new Location(2 * latitude - centerLat + (bounds.North - centerLat), 2 * longtitude - centerLon - 2.5), zoom);
                        }
                        else
                        {
                            myMap.SetView(new Location(centerLat, 2 * longtitude - centerLon - 2.5), zoom);
                        }
                    }

                    else if (m < 0 && longtitude - 2.5 <= bounds.West)
                    {
                        if (latitude >= centerLat)
                        {
                            myMap.SetView(new Location(2 * latitude - centerLat + (bounds.North - centerLat), 2 * longtitude - centerLon + 2.5), zoom);
                        }
                        else
                        {
                            myMap.SetView(new Location(centerLat, 2 * longtitude - centerLon + 2.5), zoom);
                        }
                    }

                    else if (m > 0 && longtitude - 2.5 <= bounds.West)
                    {
                        if (latitude <= centerLat)
                        {
                            myMap.SetView(new Location(2 * latitude - centerLat + (bounds.North - centerLat), 2 * longtitude - centerLon + 2.5), zoom);
                        }
                        else
                        {
                            myMap.SetView(new Location(centerLat, 2 * longtitude - centerLon + 2.5), zoom);
                        }
                    }
                }
                preX = latitude;
                preY = longtitude;
            }
        }

    }
}
