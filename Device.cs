using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceIF
{
    public class Device
    {
        private SerialPort _serialPort;
        public event Action<int> OnDataParsed;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isMonitoringEnabled = false;

        private void HandleDataReceived(object sender, SerialDataReceivedEventArgs eventArgs)
        {
            string receivedData = _serialPort.ReadLine();
            Console.WriteLine($"Received Data: {receivedData}");

            if (int.TryParse(receivedData, out int parsedInteger))
            {
                OnDataParsed?.Invoke(parsedInteger);
            }
        }

        public void Connect(string portName, int baudRate)
        {
            if (_serialPort != null && _serialPort.IsOpen) _serialPort.Close();

            _serialPort = new SerialPort(portName) { BaudRate = baudRate };
            _serialPort.DataReceived += HandleDataReceived;
            _serialPort.Open();

            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => MonitorPortsAndConnection(_cancellationTokenSource.Token));
        }

        public void Disconnect()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.DataReceived -= HandleDataReceived;
                _serialPort.Close();
            }

            _cancellationTokenSource?.Cancel();
        }

        public bool Connected
        {
            get { return _serialPort != null && _serialPort.IsOpen; }
        }

        public bool IsMonitoringEnabled
        {
            get => _isMonitoringEnabled;
            set => _isMonitoringEnabled = value;
        }

        public async Task MonitorPortsAndConnection(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (_isMonitoringEnabled) 
                {
                    if (!Connected)
                    {
                        try
                        {
                            if (_serialPort != null && _serialPort.IsOpen)
                            {
                                Disconnect();
                            }

                            Connect(_serialPort?.PortName, _serialPort?.BaudRate ?? 115200);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Reconnection failed: {ex.Message}");
                        }
                    }
                }

                await Task.Delay(1000, token);
            }
        }





    }
}
