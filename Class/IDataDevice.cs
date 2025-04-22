using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceIF
{
    public interface IDataDevice
    {
        event Action<int> OnDataParsed;
        event Action<string[]> PortsChanged;

        string[] Ports { get; set; }
        bool Connected { get; }

        void Connect(string portName, int baudRate);
        void Disconnect();
        void CheckPorts();
    }

}
