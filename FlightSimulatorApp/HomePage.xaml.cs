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
using System.Resources;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for openPage.xaml
    /// </summary>
    public partial class HomePage : INotifyPropertyChanged
    {
        private string ip = "Enter IP";
        private string port = "Enter Port";
        private bool check = false;
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
            int p = 0;
            // if the user didn't insert port and ip
            if (ip.Equals("") || port.Equals("")|| ip.Equals("Enter IP") || port.Equals("Enter Port"))
            {
                string message = string.Format("You need to insert IP and Port");
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!port.Equals(""))
            {
                p = int.Parse(this.port);
            }
            SimulatorView simulatorView;
            try
            {
                // show the view
                simulatorView = new SimulatorView(this, this.ip, p);
                this.NavigationService.Navigate(simulatorView);
                check_box.IsChecked = false;
            }
            catch (Exception e1)
            {
                // if the server is not connect
                if (e1.Message == "not connected")
                {
                    string message = string.Format("not connected to the simulator, try again");
                    MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


        }
        public string IP
        {
            get 
            {
                return this.ip; 
            }
            set 
            { 
                this.ip = value;
                NotifyPropertyChanged("Port");

            }
        }
        public string Port
        {
            get
            {
                return this.port;
            }
            set 
            {
                this.port = value;
                NotifyPropertyChanged("Port");

            }
        }
        private void ServerPort_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Port = "";
            NotifyPropertyChanged("Port");
        }
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            // the user choose to use default port and ip
            if (!check)
            {
                ServerIP.Text = "127.0.0.1";
                ServerPort.Text = "5402";
                Port = "5402";
                IP = "127.0.0.1";
            }
            else
            {
                ServerIP.Text = "";
                ServerPort.Text = "";
                Port = "";
                IP = "";
            }
            check = !check;
        }

        private void ServerIP_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ServerIP.Text.Equals("Enter IP")) {
                ServerIP.Text = "";
            }
        }

        private void ServerIP_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ServerIP.Text.Equals(""))
            {
                ServerIP.Text = "Enter IP";
            }
        }

        private void ServerPort_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ServerPort.Text.Equals("Enter Port"))
            {
                ServerPort.Text = "";
            }
        }

        private void ServerPort_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ServerPort.Text.Equals(""))
            {
                ServerPort.Text = "Enter Port";
            }
        }
    }
}