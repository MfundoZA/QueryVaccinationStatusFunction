using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace QueryVaccinationStatusFunction
{
    public static class QueryVaccinationStatus
    {
        // Hardcoded database that holds registered users
        static List<User> users = new List<User> 
        { 
            new User("9607175800088", false),
            new User("8304064800087", true),
            new User("0202185708080", true),
            new User("9903024800084", false)
        };

        [FunctionName("QueryVaccinationStatus")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string id = req.Query["id"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            id = id ?? data?.id;

            string responseMessage = string.IsNullOrEmpty(id)
                ? "This HTTP triggered function executed successfully. Pass an ID in the query string (?id=XXXXXXXXXXXXX)" +
                " or in the request body to recieve your vaccination status."
                : checkVaccinationStatus(id);

            return new OkObjectResult(responseMessage);
        }

        /*
         * This method uses the id number provided by the user to search the database
         * for a record that has a matching id. If a user is found their id and vaccination status is
         * displayed to them otherwise an error is shown.
         */
        private static string checkVaccinationStatus(string id)
        {
            User user = searchForUser(id);

            if (user == null)
            {
                return "Id could not be found in the database! Please try again.";
            }
            else
            {
                return $"User: {id} \n" +
                    $"Status: {(user.IsVaccinated ? "Vaccinated" : "Not vaccinated")}";
            }
        }

        // Iterates through database to verify if a user is registered.
        private static User searchForUser(string id)
        {
            User tempUser = null;

            foreach (User user in users)
            {
                if (user.Id == id)
                {
                    tempUser = user;
                    break;
                }
            }

            return tempUser;
        }
    }
}
