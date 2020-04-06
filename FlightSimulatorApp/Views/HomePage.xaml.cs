using System;
using System.Windows;
using System.ComponentModel;
using FlightSimulatorApp.Views;
using System.Configuration;
using FlightSimulatorApp.Model;
using FlightSimulatorApp.ViewModels;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for openPage.xaml
    /// </summary>
    public partial class HomePage : INotifyPropertyChanged
    {
        private string ip = "";
        private string port = "";
        private string message = "";
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
            if (ip.Equals("") || port.Equals(""))
            {
                Message = "You need to insert IP and Port";
                return;
            }
            try
            {
                p = int.Parse(this.port);
            } catch (Exception e1)
            {
                Message = "You need to insert a valid port";
                return;
            }
            SimulatorView simulatorView;
            try
            {
                // show the view
                MySimulatorModel m = new MySimulatorModel(new Telnet(ip, p));
                simulatorView = new SimulatorView(new Get_VM(m), new Set_VM(m), new Errors_VM(m));
                this.NavigationService.Navigate(simulatorView);
                check_box.IsChecked = false;
            }
            catch (Exception e1)
            {
                // if the server is not connect
                if (e1.Message == "not connected")
                {
                    Message = "not connected to the simulator, try again";
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
        public string Message
        {
            get { return this.message; }
            set 
            { 
                this.message = value;
                NotifyPropertyChanged("Message");
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // the user choose to use default port and ip
            ServerIP.Text = ConfigurationManager.AppSettings["ServerIP"];
            ServerPort.Text = ConfigurationManager.AppSettings["ServerPort"];
            Port = ConfigurationManager.AppSettings["ServerPort"];
            IP = ConfigurationManager.AppSettings["ServerIP"];
            Message = "";
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ServerIP.Text = "";
            ServerPort.Text = "";
            Port = "";
            IP = "";
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            // close the program
            System.Environment.Exit(0);
        }

        private void ServerIP_GotFocus(object sender, RoutedEventArgs e)
        {
            Message = "";
        }

        private void ServerPort_GotFocus(object sender, RoutedEventArgs e)
        {
            Message = "";
        }



    }
}