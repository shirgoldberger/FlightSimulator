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

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for SimulatorView.xaml
    /// </summary>
    public partial class SimulatorView : Page
    {
        SimulatorViewModel VM;
        double elevator, rudder;
        public SimulatorView(string ip, int port)
        {
            InitializeComponent();
            this.VM = new SimulatorViewModel(new MySimulatorModel(new Telnet(ip, port)));
            joystick1.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                var args = e as PropertyChangedExtendedEventArgs;
                if (args != null) {
                    string property = args.PropertyName as string;
                    if (property.Equals("Elevator"))
                    {
                        V_Elevator = (double)args.NewValue;
                        
                    }
                    else {
                        if (property.Equals("Rudder"))
                        {
                            V_Rudder = (double)args.NewValue;
                            VM.MV_Rudder = (double)args.NewValue;
                        }
                    }
                }

          };
            DataContext = VM;
        }

        public double V_Elevator{
        get{
                return this.elevator;
        }
            set{
                this.elevator = value;
                this.VM.MV_Elevator = this.elevator;
         }
        }

        public double V_Rudder{
          get{
                return this.rudder;
             }
          set{
                this.rudder = value;
                this.VM.MV_Rudder = this.rudder;
             }
        }
   }
}

