using System;
using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;

//https://dsp.stackexchange.com/questions/11290/linear-phase-notch-filter-band-reject-filter-implementation-in-c

public class NotchFilter
{
    private double[] a = new double[3];
    private double[] b = new double[3];
    private double[] inputPrev = new double[2];
    private double[] outputPrev = new double[2];

    public NotchFilter(double notchFreq)
    {
        double omega = 2 * Math.PI * notchFreq / 500.0;
        double alpha = Math.Sin(omega) / (2 * 50.0);

        b[0] = 1;
        b[1] = -2 * Math.Cos(omega);
        b[2] = 1;

        a[0] = 1 + alpha;
        a[1] = -2 * Math.Cos(omega);
        a[2] = 1 - alpha;

        for (int i = 0; i < 3; i++)
        {
            b[i] /= a[0];
            a[i] /= a[0];
        }
    }

    public double Apply(double input)
    {
        double output = b[0] * input + b[1] * inputPrev[0] + b[2] * inputPrev[1]
                        - a[1] * outputPrev[0] - a[2] * outputPrev[1];

        inputPrev[1] = inputPrev[0];
        inputPrev[0] = input;
        outputPrev[1] = outputPrev[0];
        outputPrev[0] = output;

        return output;
    }
}

public class SignalGenerator
{
    public static double[] GenerateSinSignal(double frequency, double discretization, int count)
    {
        double[] signal = new double[count];

        for (int i = 0; i < count; i++)
        {
            signal[i] = Math.Sin(2 * Math.PI * frequency * i / discretization);
        }

        return signal;
    }
}