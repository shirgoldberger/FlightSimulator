using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FlightSimulatorApp.Model;
using FlightSimulatorApp.ViewModels;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MySimulatorModel m;
        Errors_VM errors_VM;
        Get_VM get_VM;
        Set_VM set_VM;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Model = new MySimulatorModel();
            Errors_VM = new Errors_VM(m);
            Get_VM = new Get_VM(m);
            Set_VM = new Set_VM(m);
        }
        public Errors_VM Errors_VM
        {
            get
            {
                return this.errors_VM;
            }
            internal set
            {
                this.errors_VM = value;
            }
        }
        public Set_VM Set_VM
        {
            get
            {
                return this.set_VM;
            }
            internal set
            {
                this.set_VM = value;
            }
        }
        public Get_VM Get_VM
        {
            get
            {
                return this.get_VM;
            }
            internal set
            {
                this.get_VM = value;
            }
        }
        public MySimulatorModel Model
        {
            get
            {
                return this.m;
            }
            internal set
            {
                this.m = value;
            }
        }
    }
    
}
