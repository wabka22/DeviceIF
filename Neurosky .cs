using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurosky;

internal class Neurosky
{
    private ThinkGearClient _client;

    public void Connect(string port)
    {
        _client = new ThinkGearClient();
        _client.Connect(port);
        _client.OnDataReceived += Client_OnDataReceived;
    }

    private void Client_OnDataReceived(object sender, ThinkGearDataEventArgs e)
    {
        Console.WriteLine($"Attention: {e.Attention}, Meditation: {e.Meditation}");
    }

    public void Disconnect()
    {
        if (_client != null)
        {
            _client.Disconnect();
            _client = null;
        }
    }
}