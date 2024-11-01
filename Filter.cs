using System;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics;
using System.Numerics;

public class Filter
{
    private readonly double[] butterworthGain;
    private readonly int N;
    private readonly Complex[] fftSignal;

    public Filter(double lowCut = 0.5, double highCut = 50.0, double fs = 250.0, int n = 1024)
    {
        N = n;
        butterworthGain = new double[N / 2];
        fftSignal = new Complex[N];
        InitializeButterworthGain(lowCut, highCut, fs);
    }

    private void InitializeButterworthGain(double lowCut, double highCut, double fs)
    {
        for (int i = 0; i < N / 2; i++)
        {
            double frequency = i * fs / N;
            if (frequency < lowCut || frequency > highCut)
                butterworthGain[i] = 0;
            else
                butterworthGain[i] = 1;
        }
    }

    public double ApplyFilter(double data)
    {
        for (int i = 0; i < N - 1; i++)
        {
            fftSignal[i] = fftSignal[i + 1];
        }
        fftSignal[N - 1] = new Complex(data, 0);
        Fourier.Forward(fftSignal, FourierOptions.Default);

        for (int w = 0; w < N / 2; w++)
        {
            fftSignal[w] *= butterworthGain[w];
        }

        Fourier.Inverse(fftSignal, FourierOptions.Default);
        return fftSignal[0].Real;
    }
}

//using System;
//using System.Collections.Generic;
//using MathNet.Filtering;
//using MathNet.Filtering.IIR;

//public class Filter
//{
//    private readonly IIRFilter filter;

//    public Filter(double lowCut = 0.5, double highCut = 50.0, double fs = 250.0)
//    {
//        filter = new IirButterworthBandpass(4, fs, lowCut, highCut);
//    }

//    public double ApplyFilter(double data)
//    {
//        return filter.Filter(data);
//    }
//}