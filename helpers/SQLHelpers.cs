using System;
using System.Data;
using System.Data.SqlClient;
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
