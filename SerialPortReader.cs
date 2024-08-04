using System;
using System.IO.Ports;

namespace NMEADecoder
{
    public class SerialPortReader
    {
        private SerialPort _serialPort;

        public SerialPortReader(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.DataReceived += SerialPortDataReceived;
            _serialPort.Open();
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = _serialPort.ReadLine();
            NmeaData nmeaData = NmeaParser.Parse(data);
            Console.WriteLine(nmeaData);
        }

        public void Close()
        {
            _serialPort.Close();
        }
    }
}
