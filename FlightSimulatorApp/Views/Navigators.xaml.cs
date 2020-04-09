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

namespace FlightSimulatorApp.Views
{
    /// <summary>
    /// Interaction logic for Navigators.xaml
    /// </summary>
    public partial class Navigators : UserControl, Notify, INotifyPropertyChanged
    {
        private double throttle, elevator, rudder, aileron;
        public Navigators()
        {
            InitializeComponent();
            DataContext = this;
            joystick.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                var args = e as PropertyChangedExtendedEventArgs;
                if (args != null)
                {
                    string property = args.PropertyName as string;
                    if (property.Equals("Elevator"))
                    {
                        Elevator = (double)args.NewValue;
                    }
                    else
                    {
                        if (property.Equals("Rudder"))
                        {
                            Rudder = (double)args.NewValue;
                        }
                    }
                }


            };
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
                this.NotifyPropertyChanged("Throttle", value);
                this.NotifyPropertyChanged("Throttle_Text");
            }
        }

        public string Rudder_Text
        {
            get { return String.Format("{0:0.000}", Rudder); }
        }

        public string Throttle_Text
        {
            get
            {
                return String.Format("{0:0.000}", Throttle);
            }
        }

        public string Aileron_Text
        {
            get { return String.Format("{0:0.000}", Aileron); }
        }

        public string Elevator_Text
        {
            get { return String.Format("{0:0.000}", elevator); }
        }
    }
}
