
namespace FlightSimulatorApp.Model
{
    public interface ITelnetClient
    {
        void connect();
        void write(string command);
        string read(); // blocking call 
        void disconnect();
        void setTimeOutRead(int time);
    }
}