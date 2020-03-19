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

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for SimulatorView.xaml
    /// </summary>
    public partial class SimulatorView : Page
    {
        SimulatorViewModel VM;
        public SimulatorView(string ip, int port)
        {
            InitializeComponent();
            this.VM = new SimulatorViewModel(new MySimulatorModel(new Telnet(ip, port)));
            joystick1 = new controls.Joystick();

            DataContext = VM;
        }
    }
}

