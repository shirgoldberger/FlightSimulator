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
    /// Interaction logic for openPage.xaml
    /// </summary>
    public partial class HomePage : INotifyPropertyChanged
    {
        private string ip = "";
        private string port = "";
        public HomePage()
        {
            InitializeComponent();
            DataContext = this;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        private void Button_Click_Fly(object sender, RoutedEventArgs e)
        {
            int p;
            if (ip.Equals(""))
            {
                ip = "127.0.0.1";
            }
            if (port.Equals(""))
            {
                p = 5402;
            }
            else
            {
                p = int.Parse(this.port);
            }
            SimulatorView simulatorView = new SimulatorView(this.ip, p);
            this.NavigationService.Navigate(simulatorView);
       
        }

        public string IP
        {
            get { return this.ip; }
            set { this.ip = value; }
        }

        public string Port
        {
            get { return this.port; }
            set { this.port = value; }
        }

        private void ServerPort_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Port = "";
            NotifyPropertyChanged("Port");
        }

        private void ServerPort_MouseEnter(object sender, MouseEventArgs e)
        {
            //Port = "";
            //NotifyPropertyChanged("Port");
        }
        bool hasBeenClicked = false;
        private void ServerIP_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!hasBeenClicked)
            {
                TextBox box = sender as TextBox;
                box.Text = "ip";
                hasBeenClicked = true;
            }
        }

    }
}