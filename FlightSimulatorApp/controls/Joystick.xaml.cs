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

namespace FlightSimulatorApp.controls
{
    /// <summary>
    /// Interaction logic for Joystick.xaml
    /// </summary>
    public partial class Joystick : UserControl, Notify
    {
        private double rudder, elevator;
        bool mousePressed;

        public int i;
        private Point currentPlace = new Point();

        public event PropertyChangedEventHandler PropertyChanged;

        public Joystick()
        {
            InitializeComponent();
            mousePressed = false;
        }
        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!mousePressed)
            {
                currentPlace = e.GetPosition(this);
                mousePressed = true;
            }
        }
        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousePressed)
            {
                double x = e.GetPosition(this).X - currentPlace.X;
                double y = e.GetPosition(this).Y - currentPlace.Y;
                double disance = Math.Sqrt(x * x + y * y);
                if (disance < (Base.Width - KnobBase.Width) / 2)
                {
                    knobPosition.X = x;
                    knobPosition.Y = y;
                }
                Rudder = x / (Base.Width - KnobBase.Width) * 2;
                Elevetor = y / (Base.Width - KnobBase.Width) * 2;
            }
        }
        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mousePressed = false;
            knobPosition.X = 0;
            knobPosition.Y = 0;
        }
        public void NotifyPropertyChanged(string propertyName, object newValue)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedExtendedEventArgs(propertyName, newValue));
        }

        public double Elevetor
        {
            get { return this.elevator; }
            set
            {
                this.elevator = value;
                NotifyPropertyChanged("Elevator", value);
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mousePressed = false;
            knobPosition.X = 0;
            knobPosition.Y = 0;
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousePressed)
            {
                Knob_MouseMove(sender, e);
            }
        }

        public double Rudder
        {
            get { return this.rudder; }
            set
            {
                this.rudder = value;
                NotifyPropertyChanged("Rudder", value);
            }
        }
    }
}
