using System;
using System.IO.Ports;
using System.Linq;

namespace DeviceIF
{
    public class Neurosky : Device
    {
        public override void Connect(string portName, int baudRate)
        {
            base.Connect(portName, baudRate);

            if (Connected)
            {
                byte[] request = { 0xAA };
                try
                {
                    _serialPort.Write(request, 0, request.Length);
                    Console.WriteLine("Neurosky data request sent.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to send Neurosky request: " + ex.Message);
                }
            }
        }

        protected override void HandleDataReceived(object sender, SerialDataReceivedEventArgs eventArgs)
        {
            var sp = (SerialPort)sender;
            byte[] buffer = new byte[sp.BytesToRead];
            sp.Read(buffer, 0, buffer.Length);

            for (int i = 0; i < buffer.Length; i++)
            {
                byte code = buffer[i];

                switch (code)
                {
                    case 0x02: // Poor signal (0-255)
                        if (i + 1 < buffer.Length)
                        {
                            int poorSignal = buffer[i + 1];
                            Console.WriteLine($"[SIGNAL] POOR_SIGNAL: {poorSignal}");
                            i++;
                        }
                        break;

                    case 0x04: // Attention eSense (0-100)
                        if (i + 1 < buffer.Length)
                        {
                            int attention = buffer[i + 1];
                            Console.WriteLine($"[eSense] ATTENTION: {attention}");
                            //OnDataParsed?.Invoke(attention); // Отправляем данные через событие
                            i++;
                        }
                        break;

                    case 0x05: // Meditation eSense (0-100)
                        if (i + 1 < buffer.Length)
                        {
                            int meditation = buffer[i + 1];
                            Console.WriteLine($"[eSense] MEDITATION: {meditation}");
                            i++;
                        }
                        break;

                    case 0x03: // Heart rate (0-255)
                        if (i + 1 < buffer.Length)
                        {
                            int heartRate = buffer[i + 1];
                            Console.WriteLine($"[HR] HEART_RATE: {heartRate}");
                            i++;
                        }
                        break;

                    case 0x81: // EEG Power (8x4 bytes)
                        if (i + 32 < buffer.Length)
                        {
                            Console.WriteLine("[EEG] EEG_POWER:");
                            for (int j = 0; j < 8; j++)
                            {
                                uint val = (uint)(
                                    (buffer[i + 1 + j * 4] << 24) |
                                    (buffer[i + 2 + j * 4] << 16) |
                                    (buffer[i + 3 + j * 4] << 8) |
                                    buffer[i + 4 + j * 4]);
                                Console.WriteLine($"  Band {j + 1}: {val}");
                            }
                            i += 32;
                        }
                        break;

                    case 0x83: // ASIC EEG Power (8x3 bytes)
                        if (i + 24 < buffer.Length)
                        {
                            Console.WriteLine("[EEG] ASIC_EEG_POWER:");
                            for (int j = 0; j < 8; j++)
                            {
                                int offset = i + 1 + j * 3;
                                uint value = (uint)((buffer[offset] << 16) | (buffer[offset + 1] << 8) | buffer[offset + 2]);
                                Console.WriteLine($"  Band {j + 1}: {value}");
                            }
                            i += 24;
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
}