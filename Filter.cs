using System;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics;
using System.Numerics;
using System.Collections.Generic;
using MathNet.Filtering;
using MathNet.Filtering.IIR;

// фильтр Баттерворта
public class Filter
{
    private readonly double[] butterworthGain;
    private readonly int N;
    private readonly Complex[] fftSignal;

    public Filter(double lowCut = 45.0, double highCut = 55.0, double fs = 250.0, int n = 1024)
    {
        N = n;
        butterworthGain = new double[N / 2];
        fftSignal = new Complex[N];
        InitializeButterworthGain(lowCut, highCut, fs);
    }

    private void InitializeButterworthGain(double lowCut, double highCut, double fs)
    {
        // Вычисляем усиление для каждой частоты в зависимости от ее близости к границам диапазона
        for (int i = 0; i < N / 2; i++)
        {
            double frequency = i * fs / N;
            if (frequency < lowCut || frequency > highCut)
                butterworthGain[i] = 0;
            else
                // Применяем формулу Баттерворта
                butterworthGain[i] = 1 / Math.Sqrt(1 + Math.Pow((frequency - (highCut + lowCut) / 2) / ((highCut - lowCut) / 2), 4));
        }
    }

    public double ApplyFilter(double data)
    {
        // Перемещаем старые значения для создания кольцевого буфера
        for (int i = 0; i < N - 1; i++)
        {
            fftSignal[i] = fftSignal[i + 1];
        }
        fftSignal[N - 1] = new Complex(data, 0);

        // Прямое FFT-преобразование
        Fourier.Forward(fftSignal, FourierOptions.Default);

        // Применяем коэффициенты усиления Баттерворта к каждой частоте
        for (int w = 0; w < N / 2; w++)
        {
            fftSignal[w] *= butterworthGain[w];
            fftSignal[N - w - 1] *= butterworthGain[w]; // Симметрия для зеркальной части спектра
        }

        // Обратное FFT-преобразование
        Fourier.Inverse(fftSignal, FourierOptions.Default);

        // Возвращаем отфильтрованное значение
        return fftSignal[0].Real;
    }
}


//public class Filter2
//{
//    private readonly IirButterworthBandpass filter;

//    public Filter2(double lowCut = 45.0, double highCut = 55.0, double fs = 250.0)
//    {
//        filter = new IirButterworthBandpass(4, fs, lowCut, highCut);
//    }

//    public double ApplyFilter(double data)
//    {
//        return filter.Filter(data);
//    }
//}