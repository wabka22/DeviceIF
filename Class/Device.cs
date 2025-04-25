using System;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceIF
{
    public class Device
    {
        public SerialPort _serialPort;
        public event Action<int> OnDataParsed;
        public event Action<string[]> PortsChanged;
        public string[] _ports = SerialPort.GetPortNames();

        public string[] Ports
        {
            get { return _ports; }
            set
            {
                if (!_ports.SequenceEqual(value))
                {
                    _ports = value;
                    PortsChanged?.Invoke(_ports);
                }
            }
        }
        public void CheckPorts()
        {
            string[] currentPorts = SerialPort.GetPortNames();
            Ports = currentPorts;
        }

        public bool Connected { get; protected set; }

        protected virtual void HandleDataReceived(object sender, SerialDataReceivedEventArgs eventArgs)
        {
            string receivedData = _serialPort.ReadLine();
            Console.WriteLine($"Received Data: {receivedData}");

            if (int.TryParse(receivedData, out int parsedInteger))
            {
                OnDataParsed?.Invoke(parsedInteger);
            }
        }

        public virtual void Connect(string portName, int baudRate)
        {
            if (_serialPort != null && _serialPort.IsOpen) _serialPort.Close();

            _serialPort = new SerialPort(portName) { BaudRate = baudRate };
            _serialPort.DataReceived += HandleDataReceived;
            try
            {
                _serialPort.Open();
                Connected = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                Connected = false;
            }
        }

        public virtual void Disconnect()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.DataReceived -= HandleDataReceived;
                _serialPort.Close();
                Connected = false;
            }
        }

    }
}
