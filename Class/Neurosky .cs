using System;
using System.IO.Ports;
using System.Linq;

namespace DeviceIF
{
    public class Neurosky : Device
    {
        private SerialPort _serialPort;
        public event Action<int> OnDataParsed;
        public event Action<string[]> PortsChanged;

        private string[] _ports = SerialPort.GetPortNames();
        public string[] Ports
        {
            get => _ports;
            set
            {
                if (!_ports.SequenceEqual(value))
                {
                    _ports = value;
                    PortsChanged?.Invoke(_ports);
                }
            }
        }

        public bool Connected { get; private set; }

        public void CheckPorts()
        {
            Ports = SerialPort.GetPortNames();
        }

        public void Connect(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, 57600);
            _serialPort.DataReceived += SerialPort_DataReceived;

            try
            {
                _serialPort.Open();
                Connected = true;
                RequestData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
                Connected = false;
            }
        }

        public void Disconnect()
        {
            if (_serialPort?.IsOpen ?? false)
            {
                _serialPort.DataReceived -= SerialPort_DataReceived;
                _serialPort.Close();
                Connected = false;
            }
        }

        private void RequestData()
        {
            byte[] request = new byte[] { 0xAA };
            try
            {
                _serialPort.Write(request, 0, request.Length);
                Console.WriteLine("Data request sent.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send request: " + ex.Message);
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var sp = (SerialPort)sender;
            byte[] buffer = new byte[sp.BytesToRead];
            sp.Read(buffer, 0, buffer.Length);
            ParseThinkGearStream(buffer);
        }

        private void ParseThinkGearStream(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                byte code = data[i];

                switch (code)
                {
                    case 0x02: // Poor signal
                        if (i + 1 < data.Length)
                        {
                            Console.WriteLine($"[SIGNAL] POOR_SIGNAL: {data[i + 1]}");
                            i++;
                        }
                        break;

                    case 0x04: // Attention
                        if (i + 1 < data.Length)
                        {
                            int attention = data[i + 1];
                            Console.WriteLine($"[eSense] ATTENTION: {attention}");
                            OnDataParsed?.Invoke(attention);
                            i++;
                        }
                        break;

                    case 0x05: // Meditation
                        if (i + 1 < data.Length)
                        {
                            Console.WriteLine($"[eSense] MEDITATION: {data[i + 1]}");
                            i++;
                        }
                        break;

                    case 0x03: // Heart rate
                        if (i + 1 < data.Length)
                        {
                            Console.WriteLine($"[HR] HEART_RATE: {data[i + 1]}");
                            i++;
                        }
                        break;

                    case 0x81: // EEG Power (long format)
                        if (i + 32 < data.Length)
                        {
                            Console.WriteLine($"[EEG] EEG_POWER:");
                            for (int j = 0; j < 8; j++)
                            {
                                uint val = (uint)(
                                    (data[i + 1 + j * 4] << 24) |
                                    (data[i + 2 + j * 4] << 16) |
                                    (data[i + 3 + j * 4] << 8) |
                                    data[i + 4 + j * 4]);
                                Console.WriteLine($"  Band {j + 1}: {val}");
                            }
                            i += 32;
                        }
                        break;

                    case 0x83: // EEG Power (short format)
                        if (i + 24 < data.Length)
                        {
                            Console.WriteLine($"[EEG] ASIC_EEG_POWER:");
                            for (int j = 0; j < 8; j++)
                            {
                                int offset = i + 1 + j * 3;
                                uint value = (uint)((data[offset] << 16) | (data[offset + 1] << 8) | data[offset + 2]);
                                Console.WriteLine($"  Band {j + 1}: {value}");
                            }
                            i += 24;
                        }
                        break;
                }
            }
        }
    }
}
