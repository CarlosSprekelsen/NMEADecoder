using System;
using System.IO.Ports;

namespace NMEADecoder
{
    public class SerialPortReader : IReader
    {
        private SerialPort _serialPort;
        public event Action<NmeaData> DataReceived;

        public SerialPortReader(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.DataReceived += SerialPortDataReceived;
        }

        public void StartReading()
        {
            _serialPort.Open();
        }

        public void StopReading()
        {
            _serialPort.Close();
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = _serialPort.ReadLine();
            NmeaData nmeaData = NmeaParser.Parse(data);
            DataReceived?.Invoke(nmeaData);
        }

    }
}
