using System;
using NeuroSky.ThinkGear;

class Program
{
    static void Main(string[] args)
    {
        ThinkGearClient client = new ThinkGearClient();

        try
        {
            // Подключение к устройству
            client.Connect("COM5");
            client.OnDataReceived += (sender, e) =>
            {
                Console.WriteLine($"Attention: {e.Attention}, Meditation: {e.Meditation}");
            };

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            client.Disconnect();
        }
    }
}