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
        public event Action<int> DataReceived;

        public bool IsOpen => _serialPort?.IsOpen ?? false;

        public void Open(string portName, int baudRate)
        {
            if (_serialPort != null && _serialPort.IsOpen)
                _serialPort.Close();

            _serialPort = new SerialPort(portName)
            {
                BaudRate = baudRate
            };

            _serialPort.DataReceived += OnDataReceived;
            _serialPort.Open();
        }

        public void Close()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.DataReceived -= OnDataReceived;
                _serialPort.Close();
            }
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string dataRead = _serialPort.ReadLine();
                if (int.TryParse(dataRead, out int value))
                {
                    DataReceived?.Invoke(value); 
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while receiving data: {ex.Message}");
            }
        }
    }
}