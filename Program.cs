using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using testconsoleappcosmosdb.helpers;
using System.Configuration;

namespace testconsoleappcosmosdb
{

    public class Program
    {



        public static void Main(string[] args) {



            //CSVHelpers.CombineCsvFiles(sourceFolder, destinationFile);


            //CSVHelpers.DeleteFiles();

            //SQLHelpers.ConnectToSQL();


            //SQLHelpers.ReadOrderData(ConfigurationManager.ConnectionStrings["Local_SQL_Connection"].ConnectionString);

            //CreateSQLTable.CreateSQLTablesFromMultipleCSVWithDifferentDataAndSameColumnsName();

            //CSVHelpers.GetFileProperties();
            CSVHelpers.CombineCsvFiles(Path.GetTempPath(), Path.GetTempPath());
            SQLHelpers.CreateTableInSQLDatabaseUsingAnyCSV();


        }


      

    }
        
}