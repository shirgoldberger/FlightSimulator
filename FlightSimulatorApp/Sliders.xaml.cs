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
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangedEventHandler PropertyChangedNotify;


        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        public void NotifyPropertyChanged(string propertyName, object newValue)
        {
            if (this.PropertyChangedNotify != null)
                PropertyChangedNotify(this, new PropertyChangedExtendedEventArgs(propertyName, newValue));
        }
        public double Rudder
        {
            get { return this.rudder; }
            set
            {
                 this.rudder = value;
                this.NotifyPropertyChanged("Rudder", value);
                this.NotifyPropertyChanged("Rudder_Text");

            }
        }


        public double Elevator
        {
            get { return this.elevator; }
            set
            {
                    this.elevator = value;
                this.NotifyPropertyChanged("Elevator", value);
                this.NotifyPropertyChanged("Elevator_Text");

            }
        }
        public double Aileron
        {
            get { return this.aileron; }
            set
            {

                    this.aileron = value;
                this.NotifyPropertyChanged("Aileron", value);
                this.NotifyPropertyChanged("Aileron_Text");

            }
        }
        public double Throttle
        {
            get { return this.throttle; }
            set
            {
                 this.throttle = value;
                Console.WriteLine("hello im ttrotle");
                this.NotifyPropertyChanged("Throttle", value);
                this.NotifyPropertyChanged("Throttle_Text");
                this.NotifyPropertyChanged("Throttle");


            }
        }

        public string Rudder_Text {
            get { return String.Format("{0:0.000}", Rudder); }
        }

        public string Throttle_Text
        {
            get {
                Console.WriteLine("propertycsdfdsf");
                return String.Format("{0:0.000}", Throttle); }
        }

        public string Aileron_Text
        {
            get { return String.Format("{0:0.000}", Aileron); }
        }

        public string Elevator_Text
        {
            get { return String.Format("{0:0.000}", Rudder); }
        }
    }
}