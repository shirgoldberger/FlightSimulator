
namespace FlightSimulatorApp.Model
{
    public interface ITelnetClient
    {
        void connect();
        void write(string command);
        // Blocking call.
        string read(); 
        void disconnect();
        void setTimeOutRead(int time);
    }
}