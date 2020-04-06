using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Threading;
using FlightSimulatorApp.ViewModels;
using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.Views
{
    /// <summary>
    /// Interaction logic for SimulatorView.xaml
    /// </summary>
    public partial class SimulatorView : Page, INotifyPropertyChanged
    {
        Get_VM vm1;
        Set_VM vm2;
        Errors_VM vm3;
        double elevator, rudder, throttle, aileron;
        private string message;
        public SimulatorView(Get_VM vm_1, Set_VM vm_2, Errors_VM vm_3)
        {
            InitializeComponent();
            this.vm1 = vm_1;
            this.vm2 = vm_2;
            this.vm3 = vm_3;

            dash.DataContext = vm1;
            myMap.DataContext = vm1;
            myMessage.DataContext = this;

            vm3.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName.Equals("VM_ServerError") && vm3.VM_ServerError)
                {

                    Message = "We lost contact with the simulator, you are redirected to the log in page";
                    Thread.Sleep(5000);
                    Dispatcher.Invoke(new Action(() =>
                    {

                        if (NavigationService.CanGoBack)
                        {
                            this.NavigationService.GoBack();
                        }
                    }));
                }

                if (e.PropertyName.Equals("VM_ReadError") && vm3.VM_ReadError)
                {

                    Message = "we didnt get response from the simulator server, therefor the program has stoped.\n" +
                        "you are redirected to the log in page";
                    Thread.Sleep(5000);
                    Dispatcher.Invoke(new Action(() =>
                    {

                        if (NavigationService.CanGoBack)
                        {
                            this.NavigationService.GoBack();
                        }
                    }));
                }

                if (e.PropertyName.Equals("VM_LatError") && vm3.VM_LatError)
                {

                    Message = "we have recieve an invalid latitude value therefor the latititude hasn't been update\n" +
                    "you can click back to go back to the log in page and try again";
                    Thread.Sleep(5000);
                }

                if (e.PropertyName.Equals("VM_LongError") && vm3.VM_LongError)
                {

                    Message = "we have recieve an invalid longtitude value therefor the latititude hasn't been update\n" +
                    "you can click back to go back to the log in page and try again";
                    Thread.Sleep(5000);
                }

            };
            sliders.PropertyChangedNotify += delegate (Object sender, PropertyChangedEventArgs e)
            {
                var args = e as PropertyChangedExtendedEventArgs;
                if (args != null)
                {
                    string property = args.PropertyName as string;
                    if (property.Equals("Elevator"))
                    {
                        V_Elevator = (double)args.NewValue;
                    }
                    else if (property.Equals("Rudder"))
                    {
                        {
                            V_Rudder = (double)args.NewValue;
                        }
                    }
                    else if (property.Equals("Throttle"))
                    {
                        {
                            V_Throttle = (double)args.NewValue;
                        }
                    }
                    else if (property.Equals("Aileron"))
                    {
                        {
                            V_Aileron = (double)args.NewValue;
                        }
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
            this.vm3.disconnect();
            this.NavigationService.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.vm3.disconnect();
            System.Environment.Exit(0);
        }

        public double V_Rudder
        {
            get
            {
                return this.rudder;
            }
            set
            {
                this.rudder = value;
                this.vm2.VM_Rudder = this.rudder;
            }
        }
        public double V_Throttle
        {
            get
            {
                return this.throttle;
            }
            set
            {
                this.throttle = value;
                this.vm2.VM_Throttle = this.throttle;
            }
        }
        public double V_Aileron
        {
            get
            {
                return this.aileron;
            }
            set
            {
                this.aileron = value;
                this.vm2.VM_Aileron = this.aileron;
            }
        }
        public double V_Elevator
        {
            get
            {
                return this.elevator;
            }
            set
            {
                this.elevator = value;
                this.vm2.VM_Elevator = this.elevator;
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
    }
}