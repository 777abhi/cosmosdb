using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace testconsoleappcosmosdb
{

    public static class CSVHelpers
    {

        public static void CombineMultipleSameCSVintoOne()
        {



            // Specify wildcard search to match CSV files that will be combined
            string[] filePaths = Directory.GetFiles(ConfigurationManager.AppSettings.Get("SourceFolder"), "CSV_File_Number?.csv");
            StreamWriter fileDest = new StreamWriter(ConfigurationManager.AppSettings.Get("destinationFile"), true);

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

        public static void ForEach<T>(
            this IEnumerable<T> source,
            Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }
        public static void CombineCsvFiles(string sourceFolder, string destinationFile, string searchPattern = "Expected_*.csv", bool isMismatched = true)
        {
            // Specify wildcard search to match CSV files that will be combined
            string[] filePaths = Directory.GetFiles(sourceFolder, searchPattern);
            if (isMismatched)
                CombineMisMatchedCsvFiles(filePaths, destinationFile);
            else
                CombineCsvFiles(filePaths, destinationFile);
        }

        internal static void DeleteFiles()
        {



            string File_Path = Path.GetTempPath() + @"test.csv";


            using (var stream = File.Open(File_Path, FileMode.OpenOrCreate))
            {
                File.Create("Test");
            }

            using (var stream = File.Open(File_Path, FileMode.Open))
            {
                //File.Delete(File_Path);
            }
            File.Delete(File_Path);



        }

        public static void CombineCsvFiles(string[] filePaths, string destinationFile)
        {
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

        public static void CombineMisMatchedCsvFiles(string[] filePaths, string destinationFile, char splitter = ',')
        {

            HashSet<string> combinedheaders = new HashSet<string>();
            int i;
            // aggregate headers
            for (i = 0; i < filePaths.Length; i++)
            {
                string file = filePaths[i];
                FileInfo fileInformation = new FileInfo(file);
                combinedheaders.UnionWith(File.ReadLines(file).First().Split(splitter));
                combinedheaders.Add(fileInformation.Name.Replace(".csv",""));


            }
            var hdict = combinedheaders.ToDictionary(y => y, y => new List<object>());

            string[] combinedHeadersArray = combinedheaders.ToArray();
            for (i = 0; i < filePaths.Length; i++)
            {
                string file = filePaths[i];
                FileInfo fileInformation = new FileInfo(file);

                var fileheaders = File.ReadLines(filePaths[i]).First().Split(splitter);
                var notfileheaders = combinedheaders.Except(fileheaders);

                File.ReadLines(filePaths[i]).Skip(1).Select(line => line.Split(splitter)).ForEach(spline =>
                {
                    for (int j = 0; j < fileheaders.Length; j++)
                    {
                        hdict[fileheaders[j]].Add(spline[j]);
                    }
                    foreach (string header in notfileheaders)
                    {
                        hdict[header].Add(fileInformation.Name);
                    }

                });
            }

            DataTable dt = hdict.ToDataTable();

            dt.ToCSV(destinationFile);
        }

        //You are working as C# developer, you need to write a program that can read file's information such as

        //  FolderPath
        //  FileName
        //  LastWriteTime
        //  CreateTime
        //  FileSizeinKB
        //from a table and write into SQL Server table.Also as part of file information, you would like to insert folder from which we are reading the file properties.

        public static void GetFileProperties()
        {
            string[] files = Directory.GetFiles(Path.GetTempPath(), "*"+ConfigurationManager.AppSettings.Get("FileExtension"));

            foreach (string filename in files)
            {

                FileInfo file = new FileInfo(filename);
                Console.WriteLine(file.Attributes);
                Console.WriteLine(file.CreationTime);
                Console.WriteLine(file.CreationTimeUtc);
                Console.WriteLine(file.Directory);
                Console.WriteLine(file.DirectoryName);
                Console.WriteLine(file.Exists);
                Console.WriteLine(file.Extension);
                Console.WriteLine(file.FullName);
                Console.WriteLine(file.IsReadOnly);
                Console.WriteLine(file.LastAccessTime);
                Console.WriteLine(file.LastAccessTimeUtc);
                Console.WriteLine(file.LastWriteTime);
                Console.WriteLine(file.LastWriteTimeUtc);
                Console.WriteLine(file.Length);

                Console.WriteLine(file.Name);
                Console.WriteLine(file.ToString());


            }
        }
    }


    public static class DataTableHelper
    {
        public static DataTable ToDataTable(this Dictionary<string, List<object>> dict)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.AddRange(dict.Keys.Select(c => new DataColumn(c)).ToArray());

            for (int i = 0; i < dict.Values.Max(item => item.Count()); i++)
            {
                DataRow dataRow = dataTable.NewRow();

                foreach (var key in dict.Keys)
                {
                    if (dict[key].Count > i)
                        dataRow[key] = dict[key][i];
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        public static void ToCSV(this DataTable dt, string destinationfile)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(destinationfile+"Expected.csv", sb.ToString());
        }







    }




}
