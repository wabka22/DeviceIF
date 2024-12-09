using System;
using Neurosky;
using ThinkGearNET;
//https://www.codeproject.com/Articles/567963/ThinkUino-Project

class Program
{

    private bool comando = false;

    private static string ARDUPORT = "COM5"; //declare the arduino serial port
    private static string MINDPORT = "COM5"; //declare the Mindwave serial port

    static void Main(string[] args)
    {
        string error = "";
        ThinkGearWrapper thinkGearWrapper = new ThinkGearWrapper();
        if (thinkGearWrapper.Connect(MINDPORT, 57600, true)) //check if the Mindwave is connected
        {
            thinkGearWrapper.EnableBlinkDetection(true); //enable the eye blink on the eSense protocol
            thinkGearWrapper.ThinkGearChanged += new EventHandler<ThinkGearChangedEventArgs>(
              thinkGearWrapper_ThinkGearChanged); //capture the event when a new data is incoming
        }
        else
            error = "Could not connect to headset";





    }

    private static void thinkGearWrapper_ThinkGearChanged(object sender, ThinkGearChangedEventArgs e)
    {
        throw new NotImplementedException();
    }
}