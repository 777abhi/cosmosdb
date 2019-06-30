using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace testconsoleappcosmosdb.helpers
{
    class JsonHelpers
    {

        //class Person
        //{
        //    public int id
        //    {
        //        get;
        //        set;
        //    }
        //    public string Name
        //    {
        //        get;
        //        set;
        //    }
        //    public bool Gender
        //    {
        //        get;
        //        set;
        //    }
        //    public DateTime DateOfBirth
        //    {
        //        get;
        //        set;
        //    }
        //}




        public static void testJSON()
        {


            //// Create the person  
            //Person myself = new Person
            //{
            //    id = 123,
            //    Name = "Afzaal Ahmad Zeeshan",
            //    Gender = true,
            //    DateOfBirth = new DateTime(1995, 08, 29)
            //};
            //// Serialize it.  
            //string serializedJson = JsonConvert.SerializeObject(myself);

            //// Print on the screen.  
            //Console.WriteLine(serializedJson);



            //string data = serializedJson.ToString();

            //string data = "{\"ID\": 123,\"Name\": \"Afzaal Ahmad Zeeshan\",\"Gender\":true, \"DateOfBirth\": \"1995-08-29T00:00:00\"}";
            string data = @"{'ID': 123,'Name':'Afzaal Ahmad Zeeshan','Gender':true,'DateOfBirth':'1995-08-29T00:00:00'}";

            // Serialize it.  
            dynamic obj = JsonConvert.DeserializeObject(data);

            // Print on the screen.  
            Console.WriteLine(obj.ID);
            Console.WriteLine(obj.Name);
            Console.WriteLine(obj.Gender);
            Console.WriteLine(obj.DateOfBirth);
            Console.WriteLine("Press any key to close-->"); Console.ReadKey();



        }








    }
}
