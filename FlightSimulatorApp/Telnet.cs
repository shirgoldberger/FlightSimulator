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
            this.tcpclnt = new TcpClient();
            try
            {
                // Console.WriteLine("Connecting.....");
                tcpclnt.Connect(ip, port);
                stm = tcpclnt.GetStream();
                // use the ipaddress as in the server program
                // Console.WriteLine("Connected");
            }
            catch (Exception e)
            {
                Exception e1 = new Exception("not connected");
                throw e1;
                //Console.WriteLine("not connected");
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
            stm.Write(ba, 0, ba.Length);
        }
    }
}
