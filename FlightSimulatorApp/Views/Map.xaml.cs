using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private double zoom;
        private bool freeMove = false;
        private double diffrance = 0;
        public Map()
        {
            InitializeComponent();
            zoom = myMap.ZoomLevel;
        }

        private void Focus_Click(object sender, RoutedEventArgs e)
        {
            double latitude = pin.Location.Latitude;
            double longtitude = pin.Location.Longitude;
            myMap.SetView(new Location(latitude, longtitude), zoom);
            PlainPosition.X = 0;
            PlainPosition.Y = 0;
            freeMove = false;
        }

        private void MyMap_MouseDown(object sender, MouseButtonEventArgs e)
        {
            freeMove = true;
        }

        private void MyMap_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (freeMove)
            {
                double latitude = pin.Location.Latitude;
                double longtitude = pin.Location.Longitude;
                this.bounds = myMap.BoundingRectangle;
                if (longtitude > bounds.West && longtitude < bounds.East && latitude < bounds.North && latitude > bounds.South)
                {
                    freeMove = false;
                }

            }
        }

        private void MyMap_LayoutUpdated(object sender, EventArgs e)
        {
            if (!firstTime)
            {
                double latitude = pin.Location.Latitude;
                double longtitude = pin.Location.Longitude;
                this.bounds = myMap.BoundingRectangle;
                if (longtitude > bounds.West && longtitude < bounds.East && latitude < bounds.North && latitude > bounds.South)
                {
                    freeMove = false;
                }
                else
                {
                    freeMove = true;
                }
                zoom = myMap.ZoomLevel;
            }
        }

        private void Pin_LayoutUpdated(object sender, EventArgs e)
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
                if (!freeMove){

                    //we are moving left or right.
                    if ((latitude - preY) == 0)
                    {
                        //we are moving right
                        if (preX < longtitude && longtitude + diffrance >= bounds.East)
                        {
                            myMap.SetView(new Location(centerLat, 2 * longtitude - centerLon - diffrance), zoom);
                        }
                        else if (preX > longtitude && longtitude - diffrance <= bounds.West)
                        {
                            myMap.SetView(new Location(centerLat, 2 * longtitude - centerLon + diffrance), zoom);
                        }

       
                    }
                    else
                    {
                        //found the Slope between the two points
                        double m = (longtitude - preX) / (latitude - preY);

                        //we are moving up with positive M and pass the limit of the current map page.
                        if (m>0 && preY < latitude && latitude + diffrance >= bounds.North)
                        {
                            //we are exceed from the right side above.
                            if (longtitude >= centerLon)
                            {
                                myMap.SetView(new Location(2 * latitude - centerLat - diffrance, 2 * longtitude - centerLon), zoom);
                            }
                            //we are exceed from the left side above.
                            else
                            {
                                myMap.SetView(new Location(2 * latitude - centerLat - diffrance, centerLon), zoom);
                            }
                        }
                        //we are moving up moving with nagative M and pass the limit of the current map page.
                        else if (m < 0 && preY < latitude && latitude + diffrance >= bounds.North)
                        {
                            //we are exceed from the right side above.
                            if (longtitude < centerLon)
                            {
                                myMap.SetView(new Location(2 * latitude - centerLat - diffrance, 2 * longtitude - centerLon), zoom);
                            }
                            //we are exceed from the left side above.
                            else
                            {
                                myMap.SetView(new Location(2 * latitude - centerLat - diffrance, centerLon), zoom);
                            }
                        }
                        //we are moving down with positive M and pass the limit of the current map page.
                        else if (m >0 && preY > latitude && latitude - diffrance <= bounds.South)
                        {
                            //we are exceed from the right side down.
                            if (longtitude < centerLon)
                            {
                                myMap.SetView(new Location(2 * latitude - centerLat + diffrance, 2 * longtitude - centerLon), zoom);
                            }
                            //we are exceed from the left side down.
                            else
                            {
                                myMap.SetView(new Location(2 * latitude - centerLat + diffrance, centerLon), zoom);
                            }
                        }
                        //we are moving down with nagative M and pass the limit of the current map page.
                        else if (m < 0 && preY > latitude && latitude - diffrance <= bounds.South)
                        {
                            //we are exceed from the right side down.
                            if (longtitude >= centerLon)
                            {
                                myMap.SetView(new Location(2 * latitude - centerLat + diffrance, 2 * longtitude - centerLon), zoom);
                            }
                            //we are exceed from the left side down.
                            else
                            {
                                myMap.SetView(new Location(2 * latitude - centerLat + diffrance, centerLon), zoom);
                            }
                        }

                        //we are moving with positive M and going to exceed the map from the right side of the map
                        else if (m > 0 && longtitude + diffrance >= bounds.East)
                        {
                            //we are at the top part of the right side of the map.
                            if (latitude >= centerLat)
                            {
                                myMap.SetView(new Location(2 * latitude - centerLat, 2 * longtitude - centerLon - diffrance), zoom);
                            }
                            //we are at the down part of the right side of the map.
                            else
                            {
                                myMap.SetView(new Location(centerLat, 2 * longtitude - centerLon - diffrance), zoom);
                            }
                        }
                        //we are moving with negative M and going to exceed the map from the right side of the map
                        else if (m < 0 && longtitude + diffrance >= bounds.East)
                        {
                            //we are at the down part of the right side of the map.
                            if (latitude <= centerLat)
                            {
                                myMap.SetView(new Location(2 * latitude - centerLat, 2 * longtitude - centerLon - diffrance), zoom);
                            }
                            //we are at the top part of the right side of the map.
                            else
                            {
                                myMap.SetView(new Location(centerLat, 2 * longtitude - centerLon - diffrance), zoom);
                            }
                        }
                        //we are moving down and going to exceed the map from the left side of the map
                        else if (m < 0 && longtitude - diffrance <= bounds.West)
                        {
                            //we are at the Top part of the left side of the map.
                            if (latitude >= centerLat)
                            {
                                myMap.SetView(new Location(2 * latitude - centerLat, 2 * longtitude - centerLon + diffrance), zoom);
                            }
                            //we are at the down part of the left side of the map.
                            else
                            {
                                myMap.SetView(new Location(centerLat, 2 * longtitude - centerLon + diffrance), zoom);
                            }
                        }
                        //we are moving up and going to exceed the map from the left side of the map
                        else if (m > 0 && longtitude - diffrance <= bounds.West)
                        {
                            //we are at the down part of the left side of the map.
                            if (latitude <= centerLat)
                            {
                                myMap.SetView(new Location(2 * latitude - centerLat, 2 * longtitude - centerLon + diffrance), zoom);
                            }
                            //we are at the Top part of the left side of the map.
                            else
                            {
                                myMap.SetView(new Location(centerLat, 2 * longtitude - centerLon + diffrance), zoom);
                            }
                        }
                    }
                    preX = longtitude;
                    preY = latitude;
                }
            }
        }

    }
}

