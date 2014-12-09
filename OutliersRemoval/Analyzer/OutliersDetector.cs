using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutliersRemoval.Analyzer;

namespace OutliersRemoval.Analyzer
{
    /// <summary>
    /// using an interface, the implementation of this interface can then be made into a WCF service easily, if required later.
    /// </summary>
    public interface IOutliersDetector
    {
        IDictionary<string, double> RemoveOutliers(IDictionary<string, double> data, out bool outlierFound);
    }

    public class OutliersDetector : IOutliersDetector
    {
        private Statistics stats;
        const double innerFenceMultiply = 1.5d; //looking for mild outlier , user 3d for more extreme outliers

        public OutliersDetector()
        {
            stats = new Statistics();
        }

        public IDictionary<string, double> RemoveOutliers(IDictionary<string, double> data, out bool outlierFound)
        {
            outlierFound = false;

            //order the value points
            var sample = data.Values.ToList<double>().OrderBy(d => d);

            //calculate the required values for calculating outlier range
            double lowerQuartile = stats.LowerQuartile(sample);
            double upperQuartile = stats.UpperQuartile(sample);
            double innerFenceRange = stats.InterQuartileRange(sample) * innerFenceMultiply;
            double innerFenceLower = lowerQuartile - ((innerFenceRange) * innerFenceMultiply);
            double innerFenceOuter = upperQuartile + ((innerFenceRange) * innerFenceMultiply);

            //only pickup the data point which the values are within the lowerfence and upperfence, the  points didn't get selected are considered as Outliers points
            var cleanedData = data.Where(point => point.Value >= innerFenceLower && point.Value <= innerFenceOuter).ToDictionary( p => p.Key, p => p.Value);
            outlierFound = cleanedData.Count() != data.Count(); //incidates if any outliers are found in the dataset 
            
            return cleanedData;
        }
    }
}
