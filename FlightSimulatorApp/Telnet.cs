using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp
{
    class Telnet : ITelnetClient
    {
        TcpClient tcpclnt;
        NetworkStream stm;
        private string ip;
        private int port;

        public Telnet(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }
        public void connect()
        {
            try
            {
                this.tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");

                tcpclnt.Connect(ip, port);
                stm = tcpclnt.GetStream();
                // use the ipaddress as in the server program
                Console.WriteLine("Connected");
                }

            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }

        public void disconnect()
        {
            tcpclnt.Close();
        }

        public string read()
        {
            byte[] bb = new byte[100];

            int k = this.stm.Read(bb, 0, 100);
            Console.WriteLine("read");
            string massage = "";
            for (int i = 0; i < k; i++)
                massage += (Convert.ToChar(bb[i]));
            return massage;
        }

        public void write(string command)
        {
            this.stm = this.tcpclnt.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(command);
            Console.WriteLine("Transmitting.....");

            stm.Write(ba, 0, ba.Length);
        }
    }
}
