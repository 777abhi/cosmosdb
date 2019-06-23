using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;


namespace testconsoleappcosmosdb
{

    public class Program
    {

        static string sourceFolder = @"D:\myFiles\c\csvfiles";
        static string destinationFile = @"D:\myFiles\c\csvfiles\CSV_Files_Combined.csv";



        public static void Main(string[] args) {


       
            CSVHelpers.CombineCsvFiles(sourceFolder, destinationFile);


        }


        public void Test()
        {



            // Specify wildcard search to match CSV files that will be combined
            string[] filePaths = Directory.GetFiles(sourceFolder, "CSV_File_Number?.csv");
            StreamWriter fileDest = new StreamWriter(destinationFile, true);

            int i;
            for (i = 0; i < filePaths.Length; i++)
            {
                string file = filePaths[i];

                string[] lines = File.ReadAllLines(file);

                if (i > 0)
                {
                    lines = lines.Skip(1).ToArray(); // Skip header row for all but first file
                }

                foreach (string line in lines)
                {
                    fileDest.WriteLine(line);
                }
            }

            fileDest.Close();

        }

    }
        
}