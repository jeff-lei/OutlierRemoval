using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutliersRemoval.Analyzer;
using OutliersRemoval.DataSourceAdapter;

namespace OutliersRemoval
{
    /// <summary>
    /// This test solution is written by Jeff Lei
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //get the location of the current running executable and find the csvfiles folder
            string filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\CsvFiles\Outliers.csv";
            
            //the output csv file will get written into the CsvFiles folder where the running assembly is 
            //if this code is started as Debug, the output file will be inside /bin/Debug/CsvFiles/
            string newFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\CsvFiles\cleanedData.csv";

            var csvAdapter = new CsvAdapter();
            var dataSet = csvAdapter.ReadCsvFile(filePath);
            var outlierFound = false;
            var cleanedSet = new OutliersDetector().RemoveOutliers(dataSet, out outlierFound);

            if (outlierFound)
            {
                Console.WriteLine("Outlier found in the dataset");
                //write the cleaned dataset to a csv file
                var resultWriting = csvAdapter.WriteToCsvFile(newFile, cleanedSet);
                if (resultWriting)
                    Console.WriteLine(String.Format("New dataset is saved at ", newFile));
                else
                    Console.WriteLine("Error writing cleaned dataset to csv file");
            }
            else
                Console.WriteLine("No outlier was found");
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
