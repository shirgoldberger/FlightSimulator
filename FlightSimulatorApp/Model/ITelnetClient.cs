namespace FlightSimulatorApp.Model
{
    public interface ITelnetClient
    {
        void Connect();
        void Write(string command);
        // Blocking call.
        string Read(); 
        void Disconnect();
        void SetTimeOutRead(int time);
    }
}