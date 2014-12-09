using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OutliersRemoval.DataSourceAdapter
{
    /// <summary>
    /// We can add more functions to this interface to perform more operations
    /// </summary>
    public interface IDataSourceAdapter
    {
        IDictionary<string, double> ReadCsvFile(string filePath);

        bool WriteToCsvFile(string filePath, IDictionary<string, double> dataSet);
    }

    /// <summary>
    /// We can add more implementations classes if later we have more different type of data source such as SQL DB or other DB
    /// </summary>
    public class CsvAdapter : IDataSourceAdapter
    {
        //Open a file and read through the file to separate each row into a date,price pair
        public IDictionary<string, double> ReadCsvFile(string filePath)
        {
            var dataset = new Dictionary<string, double>();
            var reader = new StreamReader(File.OpenRead(filePath));
            do
            {
                var tmp = reader.ReadLine().Split(new char[] { ',' });
                if (tmp.Length == 2)
                {
                    var date = tmp[0];
                    double price;
                    if (double.TryParse(tmp[1], out price))
                    {
                        dataset.Add(date, price);
                    }                       
                }
            } while (!reader.EndOfStream);
            reader.Close();
            return dataset;
        }    

        //write the dataset to a csv file
        public bool WriteToCsvFile(string filePath, IDictionary<string, double> dataSet)
        {
            using (StreamWriter writer = File.CreateText(filePath))
            {
                try
                {
                    writer.WriteLine("Date,Price");
                    foreach (var key in dataSet.Keys)
                    {
                        writer.WriteLine(String.Format("{0},{1}", key, dataSet[key]));
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
