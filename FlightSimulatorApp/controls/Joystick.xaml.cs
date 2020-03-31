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
        Rect rec;
        Point ofset;
        private double rudder, elevator;
        bool mousePressed;
        public int i;
        private Point currentPlace = new Point();
        private bool firstTime;
        Rect KnobRec;


        public event PropertyChangedEventHandler PropertyChanged;

        public Joystick()
        {
            InitializeComponent();
            mousePressed = false;
            firstTime = true;
            this.rec = new Rect();
            KnobRec.X = 0-black_Circle.Width/2;
            KnobRec.Y = 0-black_Circle.Height/2;
            KnobRec.Width = black_Circle.Width;
            KnobRec.Height = black_Circle.Height;
            firstTime = true;
        }
        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (firstTime)
            {
                ofset = black_Circle.PointToScreen(new Point(0, 0));
                this.rec = new Rect();
                rec.X = ofset.X;
                rec.Y = ofset.Y;
                rec.Width = black_Circle.Width;
                rec.Height = black_Circle.Height;
                firstTime = false;
            }
            (Knob).CaptureMouse();
        }
        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (Knob.IsMouseCaptured)
            {

                double x = e.GetPosition(black_Circle).X + rec.Left;
                double y = e.GetPosition(black_Circle).Y + rec.Top;

                if (x < rec.Left)
                {
                    if (y > rec.Top && y < rec.Bottom)
                    {
                        knobPosition.X = KnobRec.Left;
                        knobPosition.Y = e.GetPosition(black_Circle).Y - black_Circle.Height / 2;
                    }
                }
                else if (x > rec.Right)
                {
                    if (y > rec.Top && y < rec.Bottom)
                    {
                        knobPosition.X = KnobRec.Right;
                        knobPosition.Y = e.GetPosition(black_Circle).Y - black_Circle.Height / 2;

                    }
                }
                else if (y < rec.Top)
                {
                    if (x < rec.Right && x > rec.Left)
                    {
                        knobPosition.X = e.GetPosition(black_Circle).X- black_Circle.Width / 2;
                        knobPosition.Y = KnobRec.Top;
                    }

                }
                else if (y > rec.Bottom)
                {
                    if (x < rec.Right && x > rec.Left)
                    {
                        knobPosition.X = e.GetPosition(black_Circle).X - black_Circle.Width / 2;
                        knobPosition.Y = KnobRec.Bottom;
                    }
                }
                else
                {
                    knobPosition.X = e.GetPosition(black_Circle).X - black_Circle.Width/2;
                    knobPosition.Y = e.GetPosition(black_Circle).Y- black_Circle.Height/2;
                }

                updateParams(knobPosition.X, knobPosition.Y);
            }
        }
        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // return the joystick to the center
            (Knob).ReleaseMouseCapture();
            knobPosition.X = 0;
            knobPosition.Y = 0;
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

        public void updateParams(double x,double y) {
            //    // set rudder and elevator
            //Rudder = x / (black_Circle.Width - KnobBase.Width) * 2;
            //Elevetor = y / (black_Circle.Width - KnobBase.Width) * 2;
            Rudder = x / ((black_Circle.Width) / 2);
            Elevetor = y / ((black_Circle.Width) / 2);
        }
    }
}
