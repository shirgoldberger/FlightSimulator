﻿using System;
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
        private string ip = "";
        private string port = "";
        private string message = "";
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
            if (ip.Equals("") || port.Equals(""))
            {
                Message = "You need to insert IP and Port";
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
            ServerIP.Text = "127.0.0.1";
            ServerPort.Text = "5402";
            Port = "5402";
            IP = "127.0.0.1";
            Message = "";
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ServerIP.Text = "";
            ServerPort.Text = "";
            Port = "";
            IP = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBoxResult exit = MessageBox.Show("Are you sure you want to leave?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //switch (exit)
            //{
            //    case MessageBoxResult.Yes:
            //        System.Environment.Exit(0);
            //        break;
            //}
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