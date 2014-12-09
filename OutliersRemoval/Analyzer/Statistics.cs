using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutliersRemoval.Analyzer
{
    public interface IStatistics
    {
        double LowerQuartile(IOrderedEnumerable<double> sample);

        double UpperQuartile(IOrderedEnumerable<double> sample);

        double InterQuartileRange(IOrderedEnumerable<double> sample);

        double GetQuartile(IOrderedEnumerable<double> sample, double quartile);

        double Interpolate(double a, double b, double remainder);
    }

    public class Statistics : IStatistics
    {
        //Return the lower quartile of the sample data set
        public double LowerQuartile(IOrderedEnumerable<double> sample)
        {
            return GetQuartile(sample, 0.25);
        }

        //Return the upper quartile of the sample data set
        public double UpperQuartile(IOrderedEnumerable<double> sample)
        {
            return GetQuartile(sample, 0.75);
        }

        //Return the difference between Q3(upper quartile) and Q1(lower quartile)
        public double InterQuartileRange(IOrderedEnumerable<double> sample)
        {
            return UpperQuartile(sample) - LowerQuartile(sample);
        }

        /// <summary>
        /// Calcualte the quartile by giving the data sampele and the quartile looking for
        /// 0.25 = Q1 (Lower Quartile)
        /// 0.5 = Median
        /// 0.75 = Q3 (Upper Quartile)
        /// </summary>
        /// <param name="sample"> the sorted data sample list</param>
        /// <param name="quartile"> the quartile decimal </param>
        /// <returns></returns>
        public double GetQuartile(IOrderedEnumerable<double> sample, double quartile)
        {
            double result;

            // Get roughly the index
            double index = quartile * (sample.Count() + 1);

            // Get the remainder of that index value if exists
            double remainder = index % 1;

            // Get the integer value of that index
            index = Math.Floor(index) - 1;

            if (remainder.Equals(0))
            {
                // we have an integer value, no interpolation needed
                result = sample.ElementAt((int)index);
            }
            else
            {
                // we need to interpolate, e.g. giving a remainder 0.5, we need to add back 0.5*difference between the two samples in front and after the index
                double value = sample.ElementAt((int)index);
                double interpolationValue = Interpolate(value, sample.ElementAt((int)(index + 1)), remainder);

                result = value + interpolationValue;
            }

            return result;
        }

        public double Interpolate(double a, double b, double remainder)
        {
            return (b - a) * remainder;
        }
    }
}
