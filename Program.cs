﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace testconsoleappcosmosdb
{

    public class Program
    {
        private const string EndpointUrl = "https://testazuredbab.documents.azure.com:443/";
        private const string PrimaryKey = "aPEGGeYykTdoJBJK4PTlrPPvZeFbyIsYPJabiX7eo0Ex6I3IRC7hOHUGthIpds3wStwjl2w6uel1s4gbUGoz3w==";
        private DocumentClient client;
        private async Task GetStartedDemo()
        {
            client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            await client.CreateDatabaseIfNotExistsAsync(new Database { Id = "FamilyDB" });
            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("FamilyDB"), new DocumentCollection { Id = "FamilyCollection" });
            Family andersenFamily = new Family
            {
                Id = "AndersenFamily",
                LastName = "Andersen",
                Parents = new Parent[]
     {
         new Parent { FirstName = "Thomas" },
         new Parent { FirstName = "Mary Kay" }
     },
                Children = new Child[]
     {
         new Child
         {
             FirstName = "Henriette Thaulow",
             Gender = "female",
             Grade = 5,
             Pets = new Pet[]
             {
                 new Pet { GivenName = "Fluffy" }
             }
         }
     },
                Address = new Address { State = "WA", County = "King", City = "Seattle" },
                IsRegistered = true
            };

            await CreateFamilyDocumentIfNotExists("FamilyDB", "FamilyCollection", andersenFamily);

            Family wakefieldFamily = new Family
            {
                Id = "WakefieldFamily",
                LastName = "Wakefield",
                Parents = new Parent[]
                {
         new Parent { FamilyName = "Wakefield", FirstName = "Robin" },
         new Parent { FamilyName = "Miller", FirstName = "Ben" }
                },
                Children = new Child[]
                {
         new Child
         {
             FamilyName = "Merriam",
             FirstName = "Jesse",
             Gender = "female",
             Grade = 8,
             Pets = new Pet[]
             {
                 new Pet { GivenName = "Goofy" },
                 new Pet { GivenName = "Shadow" }
             }
         },
         new Child
         {
             FamilyName = "Miller",
             FirstName = "Lisa",
             Gender = "female",
             Grade = 1
         }
                },
                Address = new Address { State = "NY", County = "Manhattan", City = "NY" },
                IsRegistered = false
            };

            await CreateFamilyDocumentIfNotExists("FamilyDB", "FamilyCollection", wakefieldFamily);

            ExecuteSimpleQuery("FamilyDB", "FamilyCollection");

            await DeleteFamilyDocument("FamilyDB", "FamilyCollection", "AndersenFamily");

            // Clean up - delete the database
            await client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri("FamilyDB"));

            // Update the Grade of the Andersen Family child
            andersenFamily.Children[0].Grade = 6;
            await ReplaceFamilyDocument("FamilyDB", "FamilyCollection", "AndersenFamily", andersenFamily);
            ExecuteSimpleQuery("FamilyDB", "FamilyCollection");
        }

        private async Task ReplaceFamilyDocument(string databaseName, string collectionName, string familyName, Family updatedFamily)
        {
            await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, familyName), updatedFamily);
            WriteToConsoleAndPromptToContinue($"Replaced Family {familyName}");
        }

        private async Task DeleteFamilyDocument(string databaseName, string collectionName, string documentName)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, documentName));
            Console.WriteLine($"Deleted Family {documentName}");
        }

        private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public class Family
        {
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }
            public string LastName { get; set; }
            public Parent[] Parents { get; set; }
            public Child[] Children { get; set; }
            public Address Address { get; set; }
            public bool IsRegistered { get; set; }
            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        public class Parent
        {
            public string FamilyName { get; set; }
            public string FirstName { get; set; }
        }

        public class Child
        {
            public string FamilyName { get; set; }
            public string FirstName { get; set; }
            public string Gender { get; set; }
            public int Grade { get; set; }
            public Pet[] Pets { get; set; }
        }

        public class Pet
        {
            public string GivenName { get; set; }
        }

        public class Address
        {
            public string State { get; set; }
            public string County { get; set; }
            public string City { get; set; }
        }

        private async Task CreateFamilyDocumentIfNotExists(string databaseName, string collectionName, Family family)
        {
            try
            {
                await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, family.Id));
                WriteToConsoleAndPromptToContinue($"Found {family.Id}");
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), family);
                    WriteToConsoleAndPromptToContinue($"Created Family {family.Id}");
                }
                else
                {
                    throw;
                }
            }
        }

        private void ExecuteSimpleQuery(string databaseName, string collectionName)
        {
            // Set some common query options.
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Find the Andersen family by its LastName.
            IQueryable<Family> familyQuery = client.CreateDocumentQuery<Family>(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions)
                .Where(f => f.LastName == "Andersen");

            // Execute the query synchronously. 
            // You could also execute it asynchronously using the IDocumentQuery<T> interface.
            Console.WriteLine("Running LINQ query...");
            foreach (Family family in familyQuery)
            {
                Console.WriteLine($"\tRead {family}");
            }

            // Now execute the same query using direct SQL.
            IQueryable<Family> familyQueryInSql = client.CreateDocumentQuery<Family>(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                "SELECT * FROM Family WHERE Family.LastName = 'Andersen'",
                queryOptions);

            Console.WriteLine("Running direct SQL query...");
            foreach (Family family in familyQueryInSql)
            {
                Console.WriteLine($"\tRead {family}");
            }

            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }




        static void Main(string[] args)
        {
            try
            {
                Program p = new Program();
                p.GetStartedDemo().Wait();
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine($"{de.StatusCode} error occurred: {de.Message}, Message: {baseException.Message}");
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine($"Error: {e.Message}, Message: {baseException.Message}");
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}