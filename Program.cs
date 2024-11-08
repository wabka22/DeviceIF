using MathNet.Filtering.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceIF
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


        //static void Main(string[] args)
        //{
        //    double frequency = 50.0;
        //    double discretization = 500.0;
        //    int count = 100;

        //    double[] signal = SignalGenerator.GenerateSinSignal(frequency, discretization, count);

        //    NotchFilter notchFilter = new NotchFilter(frequency);
        //    double[] filteredSignal = new double[count];

        //    for (int i = 0; i < count; i++)
        //    {
        //        filteredSignal[i] = notchFilter.Apply(signal[i]);
        //        Console.WriteLine($"Original: {signal[i]}, Filtered: {filteredSignal[i]}");
        //    }
        //}
    }
}
