using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Threading;
using FlightSimulatorApp.ViewModels;

namespace FlightSimulatorApp.Views
{
    /// <summary>
    /// Interaction logic for SimulatorView.xaml
    /// </summary>
    public partial class SimulatorView : Page, INotifyPropertyChanged
    {
        // view models
        Get_VM vm1;
        Set_VM vm2;
        Errors_VM vm3;
        private string message;
        bool connectError = false;
        public SimulatorView()
        {
            InitializeComponent();
            this.vm1 = (Application.Current as App).Get_VM;
            this.vm2 = (Application.Current as App).Set_VM;
            this.vm3 = (Application.Current as App).Errors_VM;

            dash.DataContext = vm1;
            myMap.DataContext = vm1;
            myMessage.DataContext = this;
            connect.DataContext = vm3;

            // When there is an error.
            vm3.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                // Error of connecting to the simulator (the server unexpectedly disconnected).
                if (e.PropertyName.Equals("VM_ServerError") && vm3.VM_ServerError)
                {
                    Message = "We lost contact with the simulator\n" +
                    "you can stay at this page, press back to go back to the log in page or exit to exit";
                    this.vm3.Disconnect();
                }

                // Error of reading data.
                if (e.PropertyName.Equals("VM_ReadError") && vm3.VM_ReadError)
                {
                    Message = "we didn't get response from the simulator for 10 sec...\n" +
                    "you can wait, go back to the home page or exit";
                    Thread.Sleep(10000);
                    Message = "";
                }

                // Invalid value from the simulator.
                if (e.PropertyName.Equals("VM_InValidError") && vm3.VM_InValidError)
                {
                    Message = "we get an invalid value from the simulator\n";
                    Thread.Sleep(10000);
                    this.vm3.VM_InValidError = false;
                    Message = "";
                }

                // Invalid value from the simulator.
                if (e.PropertyName.Equals("VM_InValidError") && vm3.VM_InValidError)
                {
                    Message = "we get an invalid value from the simulator\n";
                    Thread.Sleep(10000);
                    this.vm3.VM_InValidError = false;
                    Message = "";
                }

                // Error of connecting to the simulator (cannot connect).
                if (e.PropertyName.Equals("VM_ConnectError") && vm3.VM_ConnectError)
                {
                    V_ConnectError = true;
                }

                // Error in plane location. 
                if (e.PropertyName.Equals("VM_LatError") && vm3.VM_LatError)
                {
                    Message = "we have recieve an invalid latitude value therefor the latititude hasn't been update\n" +
                    "you can click back to go back to the log in page and try again";
                    Thread.Sleep(10000);
                    this.vm3.VM_LatError = false;
                    Message = "";
                }

                if (e.PropertyName.Equals("VM_LongError") && vm3.VM_LongError)
                {

                    Message = "we have recieve an invalid longtitude value therefor the latititude hasn't been update\n" +
                    "you can click back to go back to the log in page and try again";
                    Thread.Sleep(10000);
                    this.vm3.VM_LongError = false;
                    Message = "";
                }

            };
            // When the values of the navigators change
            sliders.PropertyChangedNotify += delegate (Object sender, PropertyChangedEventArgs e)
            {
                var args = e as PropertyChangedExtendedEventArgs;
                if (args != null)
                {
                    string property = args.PropertyName as string;
                    if (property.Equals("Elevator"))
                    {
                        vm2.VM_Elevator = (double)args.NewValue;
                    }
                    else if (property.Equals("Rudder"))
                    {
                        vm2.VM_Rudder = (double)args.NewValue;
                    }
                    else if (property.Equals("Throttle"))
                    {
                        vm2.VM_Throttle = (double)args.NewValue;
                    }
                    else if (property.Equals("Aileron"))
                    {
                        vm2.VM_Aileron = (double)args.NewValue;
                    }
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            // Disconnect and go back to the home page. 
            this.vm3.Disconnect();
            this.NavigationService.GoBack();
            Message = "";
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            // Disconnect and close the program.
            this.vm3.Disconnect();
            System.Environment.Exit(0);
            Message = "";
        }
        public bool V_ConnectError
        {
            get { return this.connectError; }
            set
            {
                this.connectError = value;
                NotifyPropertyChanged("V_ConnectError");
            }
        }

        // Text box that contains messages of errors.
        public string Message
        {
            get { return this.message; }
            set
            {
                this.message = value;
                NotifyPropertyChanged("Message");
            }
        }

    }
}