using System;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics;
using System.Numerics;
using System.Collections.Generic;
using MathNet.Filtering;
using MathNet.Filtering.IIR;


public class Filter
{

    private readonly MathNet.Filtering.IIR.OnlineIirFilter filter;

    public Filter(double fs = 250.0)
    {
        var lowCut = 45.0;
        var highCut = 55.0;
        var passBandFreq = highCut / (fs / 2.0);
        var stopBandFreq = lowCut / (fs / 2.0);
        var passBandRipple = 1.0;
        var stopBandAttenuation = 20.0;
        var coefficients = MathNet.Filtering.Butterworth.IirCoefficients.HighPass(stopBandFreq, passBandFreq, passBandRipple, stopBandAttenuation);

        var coeffs = new List<double>();
        foreach (var numerator in coefficients.numerator)
        {
            coeffs.Add(numerator);
        }
        foreach (var denominator in coefficients.denominator)
        {
            coeffs.Add(denominator);
        }
        filter = new MathNet.Filtering.IIR.OnlineIirFilter(coeffs.ToArray());

    }

    public double ApplyFilter(double data)
    {
        return filter.ProcessSample(data);
    }
}