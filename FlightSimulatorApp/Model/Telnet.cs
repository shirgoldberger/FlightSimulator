﻿using System;
using System.Net.Sockets;
using System.Text;

namespace FlightSimulatorApp.Model
{
    public class Telnet : ITelnetClient
    {
        // for connecting to the simulator
        TcpClient tcpclnt;
        // for reading and writing data
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
                // try connect the server
                tcpclnt.Connect(ip, port);
                stm = tcpclnt.GetStream();
            }
            catch (Exception e)
            {
                // can't connect
                if (e.Message.Contains("No connection"))
                {
                    Exception e1 = new Exception("not connected");
                    throw e1;
                }
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
            // convert from bytes to string
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
        public void setTimeOutRead(int time)
        {
            this.tcpclnt.ReceiveTimeout = time;
        }
    }
}
