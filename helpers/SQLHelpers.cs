using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace testconsoleappcosmosdb
{
    internal class SQLHelpers
    {


             public static void ReadOrderData(string connectionString)
            {
                string queryString =
                    "SELECT * FROM [dbo].[DimAccount];";

                using (SqlConnection connection =
                           new SqlConnection(connectionString))
                {
                    SqlCommand command =
                        new SqlCommand(queryString, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    // Call Read before accessing data.
                    while (reader.Read())
                    {
                        ReadSingleRow((IDataRecord)reader);
                    }

                // Call Close when done reading.
                Console.WriteLine("Please press any key to continue!! Thanks!"); Console.ReadKey();
                 reader.Close();
                }
            }

        internal static void CreateTableInSQLDatabaseUsingAnyCSV()
        {
            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string LogFolder = Path.GetTempPath();
            try
            {

                //Declare Variables and provide values
                string SourceFolderPath = Path.GetTempPath();
                string FileExtension = ConfigurationManager.AppSettings.Get("FileExtension");
                string FileDelimiter = ConfigurationManager.AppSettings.Get("FileDelimiter");
                string ArchiveFolder = Path.GetTempPath();
                string ColumnsDataType = "NVARCHAR(4000)";
                string SchemaName = "dbo";


                //Get files from folder
                string[] fileEntries = Directory.GetFiles(SourceFolderPath, "Expected" + FileExtension);
                foreach (string fileName in fileEntries)
                {

                    //Create Connection to SQL Server in which you would like to create tables and load data
                    SqlConnection SQLConnection = new SqlConnection();
                    SQLConnection.ConnectionString = ConfigurationManager.ConnectionStrings["Local_SQL_Connection"].ConnectionString;
                    FileInfo file = new FileInfo(fileName);
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
                            ColumnList = "[" + line.Replace(FileDelimiter, "],[") + "] ,[filename]";
                            TableName = (((fileName.Replace(SourceFolderPath, "")).Replace(FileExtension, "")).Replace("\\", ""));
                            string CreateTableStatement = "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[" + SchemaName + "].";
                            CreateTableStatement += "[" + TableName + "]')";
                            CreateTableStatement += " AND type in (N'U'))DROP TABLE [" + SchemaName + "].";
                            CreateTableStatement += "[" + TableName + "]  Create Table " + SchemaName + ".[" + TableName + "]";
                            CreateTableStatement += "([" + line.Replace(FileDelimiter, "] " + ColumnsDataType + ",[") + "] " + ColumnsDataType + ",[filename] NVARCHAR(4000))";
                            SqlCommand CreateTableCmd = new SqlCommand(CreateTableStatement, SQLConnection);
                            CreateTableCmd.ExecuteNonQuery();

                        }
                        else
                        {

                            //Prepare Insert Statement and execute to insert data
                            string query = "Insert into " + SchemaName + ".[" + TableName + "] (" + ColumnList + ") ";
                            query += "VALUES('" + line.Replace(FileDelimiter, "','") + "','"+file.Name+"')";

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

        public static void ReadSingleRow(IDataRecord record)
            {
                Console.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", record[0], record[1], record[2], record[3], record[4], record[5], record[6], record[7]));
            }

        public static void ConnectToSQLANDQuery(string expected)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "";
                builder.UserID = "";
                builder.Password = "";
                builder.InitialCatalog = "";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");
                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT [DESCRIPTION],[DESCRIPTION] FROM [SalesLT].[ProductDescription] WHERE PRODUCTDESCRIPTIONID = 3 ");
                    //sb.Append("FROM [SalesLT].[ProductCategory] pc ");
                    //sb.Append("JOIN [SalesLT].[Product] p ");
                    //sb.Append("ON pc.productcategoryid = p.productcategoryid;");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                                Console.WriteLine("Actual -->" + reader.GetString(0));
                                Console.WriteLine("Expected -->" + expected);
                                Console.WriteLine("--------------------------------------------");

                                if (reader.GetString(0) == expected)
                                {
                                    Console.WriteLine("Test Passed");
                                }
                                else
                                {
                                    Console.WriteLine("Test Failed");
                                }

                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }


    }
    }
