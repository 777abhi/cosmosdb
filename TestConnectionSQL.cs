using System;
using System.Data.SqlClient;
using System.Text;

namespace testconsoleappcosmosdb
{
    class TestConnectionSQL
    {
        // static void Main(string[] args)
        public void test(string expected)
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
                                Console.WriteLine("Actual -->"+reader.GetString(0));
                                Console.WriteLine("Expected -->" +expected);
                                Console.WriteLine("--------------------------------------------");

                                if (reader.GetString(0) == expected) {
                                    Console.WriteLine("Test Passed");
                                }
                                else{
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
