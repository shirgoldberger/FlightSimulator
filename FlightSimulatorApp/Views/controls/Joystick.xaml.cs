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
using System.Windows.Media.Animation;

namespace FlightSimulatorApp.Views.controls
{
    /// <summary>
    /// Interaction logic for Joystick.xaml
    /// </summary>
    public partial class Joystick : UserControl, Notify
    {
        double k = 30;
        Rect rec;
        Point ofset;
        private double rudder, elevator;
        public int i;
        private bool firstTime;
        Rect KnobRec;
        private readonly Storyboard centerKnob;


        public event PropertyChangedEventHandler PropertyChanged;

        public Joystick()
        {
            InitializeComponent();
            centerKnob = Knob.Resources["CenterKnob"] as Storyboard;
            firstTime = true;
            this.rec = new Rect();
            KnobRec.X = 0 - black_Circle.Width / 2 + k;
            KnobRec.Y = 0 - black_Circle.Height / 2 + k;
            KnobRec.Width = black_Circle.Width - 2*k;
            KnobRec.Height = black_Circle.Height - 2*k;
            firstTime = true;
        }
        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            centerKnob.Stop();
            if (firstTime)
            {
                ofset = black_Circle.PointToScreen(new Point(0, 0));
                this.rec = new Rect();
                rec.X = ofset.X + k;
                rec.Y = ofset.Y + k;
                rec.Width = black_Circle.Width - 2*k;
                rec.Height = black_Circle.Height - 2* k;
                firstTime = false;
            }
            (Knob).CaptureMouse();
        }
        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (Knob.IsMouseCaptured)
            {
                //real place of x and y 
                double x = e.GetPosition(black_Circle).X + rec.Left - k;
                double y = e.GetPosition(black_Circle).Y + rec.Top - k;

                if (x < rec.Left)
                {
                    //y is in range
                    if (y > rec.Top && y < rec.Bottom)
                    {
                        knobPosition.X = KnobRec.Left;
                        knobPosition.Y = y - rec.Top - rec.Height / 2;
                    }
                }
                else if (x > rec.Right)
                {
                    if (y > rec.Top && y < rec.Bottom)
                    {
                        knobPosition.X = KnobRec.Right;
                        knobPosition.Y = y - rec.Top - rec.Height / 2;

                    }
                }
                else if (y < rec.Top)
                {
                    if (x < rec.Right && x > rec.Left)
                    {
                        knobPosition.X = x - rec.Left - rec.Width / 2;
                        knobPosition.Y = KnobRec.Top;
                    }

                }
                else if (y > rec.Bottom)
                {
                    if (x < rec.Right && x > rec.Left)
                    {
                        knobPosition.X = x - rec.Left - rec.Width / 2;
                        knobPosition.Y = KnobRec.Bottom;
                    }
                }
                else
                {
                    knobPosition.X = x - rec.Left - rec.Width / 2;
                    knobPosition.Y = y - rec.Top - rec.Height / 2;
                }

                updateParams(knobPosition.X, knobPosition.Y);
            }
        }
        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // return the joystick to the center
            centerKnob.Begin();
            (Knob).ReleaseMouseCapture();
            Rudder = 0;
            Elevetor = 0;
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
        public double Rudder
        {
            get { return this.rudder; }
            set
            {
                this.rudder = value;
                NotifyPropertyChanged("Rudder", value);
            }
        }

        public void updateParams(double x, double y)
        {
            // set rudder and elevator
            Rudder = x / ((rec.Width) / 2);
            Elevetor = -(y / ((rec.Width) / 2));
        }
    }
}
