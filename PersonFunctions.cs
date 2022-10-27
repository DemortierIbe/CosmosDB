





namespace MCT.Functions
{
    public class PersonFunctions
    {
        [FunctionName("GetPersons")]
        public static async Task<IActionResult> GetPersons(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/addperson/")] HttpRequest req,
            ILogger log)
        {

            try
            {

                var connectionString = Environment.GetEnvironmentVariable("CosmosDb");

                CosmosClientOptions options = new CosmosClientOptions()
                {
                    ConnectionMode = ConnectionMode.Gateway
                };
                CosmosClient client = new CosmosClient(connectionString, options);
                var container = client.GetContainer(General.COSMOS_DB, General.container);

                string sql = "SELECT * FROM c";
                var iterator = container.GetItemQueryIterator<Person>(sql);
                var results = new List<Person>();
                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    results.AddRange(response.ToList());
                }

                return new OkObjectResult(results);

            }
            catch (System.Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex);
            }



        }

        [FunctionName("AddPersons")]
        public static async Task<IActionResult> AddPersons(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/addperson/")] HttpRequest req,
            ILogger log)
        {

            try
            {

                var json = await new StreamReader(req.Body).ReadToEndAsync();
                var person = JsonConvert.DeserializeObject<Person>(json);

                var connectionString = Environment.GetEnvironmentVariable("CosmosDb");

                CosmosClientOptions options = new CosmosClientOptions()
                {
                    ConnectionMode = ConnectionMode.Gateway
                };
                CosmosClient client = new CosmosClient(connectionString, options);
                var container = client.GetContainer("firstdatabase", "persons");

                person.Id = Guid.NewGuid().ToString();
                await container.CreateItemAsync(person, new PartitionKey(person.Id));



                return new OkObjectResult(person);

            }
            catch (System.Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex);
            }



        }


        [FunctionName("DeletePersons")]
        public static async Task<IActionResult> DeletePersons(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "person/{id}")] HttpRequest req, string id,
            ILogger log)
        {

            try
            {

                var connectionString = Environment.GetEnvironmentVariable("CosmosDb");

                CosmosClientOptions options = new CosmosClientOptions()
                {
                    ConnectionMode = ConnectionMode.Gateway
                };
                CosmosClient client = new CosmosClient(connectionString, options);
                var container = client.GetContainer("firstdatabase", "persons");


                await container.DeleteItemAsync<Person>(id, new PartitionKey(id));



                return new OkObjectResult("deleted person");

            }
            catch (System.Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex);
            }



        }

        [FunctionName("UpdatePersons")]
        public static async Task<IActionResult> UpdatePersons(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "person")] HttpRequest req,
            ILogger log)
        {

            try
            {

                var json = await new StreamReader(req.Body).ReadToEndAsync();
                var person = JsonConvert.DeserializeObject<Person>(json);

                var connectionString = Environment.GetEnvironmentVariable("CosmosDb");

                CosmosClientOptions options = new CosmosClientOptions()
                {
                    ConnectionMode = ConnectionMode.Gateway
                };
                CosmosClient client = new CosmosClient(connectionString, options);
                var container = client.GetContainer("firstdatabase", "persons");

                await container.ReplaceItemAsync<Person>(person, person.Id);



                return new OkObjectResult(person);

            }
            catch (System.Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex);
            }


        }



    }
}