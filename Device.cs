using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceIF
{
    public class Device
    {
        private SerialPort _serialPort;
        public event Action<int> OnDataParsed;

        private void HandleDataReceived(object sender, SerialDataReceivedEventArgs eventArgs)
        {
            string receivedData = _serialPort.ReadLine();
            Console.WriteLine($"Received Data: {receivedData}");// For debugging

            if (int.TryParse(receivedData, out int parsedInteger))
            {
                OnDataParsed.Invoke(parsedInteger);
            }
        }


        public void Connect(string portName, int baudRate) 
        {
            if (_serialPort != null && _serialPort.IsOpen) _serialPort.Close();

            _serialPort = new SerialPort(portName)  {BaudRate = baudRate};

            _serialPort.DataReceived += HandleDataReceived; 
            _serialPort.Open();
        }

        public void Disconnect() 
        {
            if (_serialPort != null && _serialPort.IsOpen) 
            {
                _serialPort.DataReceived -= HandleDataReceived; 
                _serialPort.Close();
            }
        }

        public bool IsConnected()
        {
            return (_serialPort != null) ? _serialPort.IsOpen : false;
        }


    }
}
