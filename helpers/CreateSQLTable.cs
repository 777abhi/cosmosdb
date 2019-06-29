using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;

namespace testconsoleappcosmosdb.helpers
{
    public static class CreateSQLTable
    {
        //static void Main(string[] args)
        public static void CreateSQLTablesFromCSV()
        {

            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string LogFolder = @"C:\Log\";
            try
            {

                //Declare Variables and provide values
                string SourceFolderPath = @"C:\Source\";
                string FileExtension = ".txt";
                string FileDelimiter = ",";
                string ArchiveFolder = @"C:\Archive\";
                string ColumnsDataType = "NVARCHAR(4000)";
                string SchemaName = "dbo";


                //Get files from folder
                string[] fileEntries = Directory.GetFiles(SourceFolderPath, "*" + FileExtension);
                foreach (string fileName in fileEntries)
                {

                    //Create Connection to SQL Server in which you would like to create tables and load data
                    SqlConnection SQLConnection = new SqlConnection();
                    SQLConnection.ConnectionString = "Data Source = (local); Initial Catalog =TechBrothersIT; "
                       + "Integrated Security=true;";

                    //Writing Data of File Into Table
                    string TableName = "";
                    int counter = 0;
                    string line;
                    string ColumnList = "";

                    System.IO.StreamReader SourceFile =
                    new System.IO.StreamReader(fileName);

                    SQLConnection.Open();
                    while ((line = SourceFile.ReadLine()) != null)
                    {
                        if (counter == 0)
                        {

                            //Read the header and prepare Create Table Statement
                            ColumnList = "[" + line.Replace(FileDelimiter, "],[") + "]";
                            TableName = (((fileName.Replace(SourceFolderPath, "")).Replace(FileExtension, "")).Replace("\\", ""));
                            string CreateTableStatement = "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[" + SchemaName + "].";
                            CreateTableStatement += "[" + TableName + "]')";
                            CreateTableStatement += " AND type in (N'U'))DROP TABLE [" + SchemaName + "].";
                            CreateTableStatement += "[" + TableName + "]  Create Table " + SchemaName + ".[" + TableName + "]";
                            CreateTableStatement += "([" + line.Replace(FileDelimiter, "] " + ColumnsDataType + ",[") + "] " + ColumnsDataType + ")";
                            SqlCommand CreateTableCmd = new SqlCommand(CreateTableStatement, SQLConnection);
                            CreateTableCmd.ExecuteNonQuery();

                        }
                        else
                        {

                            //Prepare Insert Statement and execute to insert data
                            string query = "Insert into " + SchemaName + ".[" + TableName + "] (" + ColumnList + ") ";
                            query += "VALUES('" + line.Replace(FileDelimiter, "','") + "')";

                            SqlCommand SQLCmd = new SqlCommand(query, SQLConnection);
                            SQLCmd.ExecuteNonQuery();
                        }

                        counter++;
                    }

                    SourceFile.Close();
                    SQLConnection.Close();
                    //move the file to archive folder after adding datetime to it
                    File.Move(fileName, ArchiveFolder + "\\" + (fileName.Replace(SourceFolderPath, "")).Replace(FileExtension, "") + "_" + datetime + FileExtension);

                }
            }
            catch (Exception exception)
            {
                // Create Log File for Errors
                using (StreamWriter sw = File.CreateText(LogFolder
                    + "\\" + "ErrorLog_" + datetime + ".log"))
                {
                    sw.WriteLine(exception.ToString());

                }

            }

        }

        public static void CreateSQLTablesFromMultipleCSVWithDifferentDataAndSameColumnsName() {


            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string LogFolder = Path.GetTempPath();
            try
            {
                                                            
                //Declare Variables and provide values
                string SourceFolderPath = Path.GetTempPath();
                string FileExtension = ConfigurationManager.AppSettings.Get("Key0");
                string FileDelimiter = ",";
                string TableName = "dbo.Customer";
                string ArchiveFolder = Path.GetTempPath();


                NameValueCollection sAll;
                sAll = ConfigurationManager.AppSettings;

                foreach (string s in sAll.AllKeys)
                    Console.WriteLine("Key: " + s + " Value: " + sAll.Get(s));
                Console.ReadLine();






                //Get files from folder
                string[] fileEntries = Directory.GetFiles(SourceFolderPath, "*" + FileExtension);
                foreach (string fileName in fileEntries)
                {

                    //Create Connection to SQL Server
                    SqlConnection SQLConnection = new SqlConnection();
                   
                    SQLConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Local_SQL_Connection"].ConnectionString;


                    int counter = 0;
                    string line;
                    string ColumnList = "";

                    System.IO.StreamReader SourceFile =
                    new System.IO.StreamReader(fileName);

                    SQLConnection.Open();
                    while ((line = SourceFile.ReadLine()) != null)
                    {
                        if (counter == 0)
                        {
                            //By using Header Row, Build Column List
                            ColumnList = "[" + line.Replace(FileDelimiter, "],[") + "],[tablename]";

                        }
                        else
                        {

                            //Build and Execute Insert Statement to insert record
                            string query = "Insert into " + TableName + " (" + ColumnList + ") ";
                            query += "VALUES('" + line.Replace(FileDelimiter, "','") + "','"+fileName+"')";

                            SqlCommand SQLCmd = new SqlCommand(query, SQLConnection);
                            SQLCmd.ExecuteNonQuery();
                        }

                        counter++;
                    }

                    SourceFile.Close();
                    SQLConnection.Close();
                    //move the file to archive folder after adding datetime to it
                    File.Move(fileName, ArchiveFolder +
                        (fileName.Replace(SourceFolderPath, "")).Replace(FileExtension, "")
                        + "_" + datetime + FileExtension);

                }
            }
            catch (Exception exception)
            {
                // Create Log File for Errors
                using (StreamWriter sw = File.CreateText(LogFolder
                    + "\\" + "ErrorLog_" + datetime + ".log"))
                {
                    sw.WriteLine(exception.ToString());

                }

            }
        }

    }
}