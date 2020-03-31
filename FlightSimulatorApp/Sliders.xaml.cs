using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for Sliders.xaml
    /// </summary>
    public partial class Sliders : UserControl, Notify
    {
        private double throttle, elevator, rudder, aileron;
        public Sliders()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName, object newValue)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedExtendedEventArgs(propertyName, newValue));
        }
        public double Rudder
        {
            get { return this.rudder; }
            set
            {
                 this.rudder = value;
                this.NotifyPropertyChanged("Rudder", value);

            }
        }

        public double Elevator
        {
            get { return this.elevator; }
            set
            {
                    this.elevator = value;
                this.NotifyPropertyChanged("Elevator", value);

            }
        }
        public double Aileron
        {
            get { return this.aileron; }
            set
            {

                    this.aileron = value;
                this.NotifyPropertyChanged("Aileron", value);

            }
        }
        public double Throttle
        {
            get { return this.throttle; }
            set
            {
                    this.throttle = value;
                this.NotifyPropertyChanged("Throttle", value);

            }
        }
    }
}