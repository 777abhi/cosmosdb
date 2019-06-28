using System;
using System.Data;
using System.Data.SqlClient;

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
                Console.ReadKey();
                    reader.Close();
                }
            }

            public static void ReadSingleRow(IDataRecord record)
            {
                Console.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", record[0], record[1], record[2], record[3], record[4], record[5], record[6], record[7]));
            }


        }
    }
