using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Threading;

namespace ScheduelAPISamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // e.g. https://yourorg.crm.dynamics.com
            Console.Write("Enter your environment URL: e.g.  https://myorg.crm.dynamics.com. : ");
            string url = Console.ReadLine();
            // e.g. you@yourorg.onmicrosoft.com
            Console.Write("Enter your username: e.g.  myalias@mytenant.com : ");
            string userName = Console.ReadLine();
            // e.g. y0urp455w0rd 
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            string conn = $@"
                Url = {url};
                AuthType = OAuth;
                UserName = {userName};
                Password = {password};
                AppId = 51f81489-12ee-4a9e-aaae-a2591f45987d;
                RedirectUri = app://58145B91-0C36-4500-8554-080854F2AC97;
                LoginPrompt=Auto;
                RequireNewInstance = True";

            using (var svc = new CrmServiceClient(conn))
            {
                WhoAmIRequest request = new WhoAmIRequest();
                WhoAmIResponse response = (WhoAmIResponse)svc.Execute(request);
                Console.WriteLine("Your UserId is {0}", response.UserId);

                ScheduleAPIHelpers scheduleAPI = new ScheduleAPIHelpers(svc);

                var runAgain = true;
                do
                {
                    Console.WriteLine("Press a number to select a scenario. \n Scenarios: \n 1. Create \n 2. Update \n 3. CUD \n 4. Insert Task \n 5. Change task hierarchy \n Press any other key to exit");
                    var selectedOption = Console.ReadKey();
                    Console.WriteLine(" Thinking ...");
                    switch (selectedOption.Key)
                    {
                        case ConsoleKey.D1:
                            CreateScenario.Run(scheduleAPI);
                            break;
                        case ConsoleKey.D2:
                            UpdateScenario.Run(scheduleAPI);
                            break;
                        case ConsoleKey.D3:
                            CUDScenario.Run(scheduleAPI);
                            break;
                        case ConsoleKey.D4:
                            InsertTaskScenario.Run(scheduleAPI);
                            break;
                        case ConsoleKey.D5:
                            ChangeTaskHierarchy.Run(scheduleAPI);
                            break;
                        default: 
                            Console.WriteLine("Exiting ..."); 
                            runAgain = false; 
                            break;
                    }
                } while (runAgain == true);
                
                Console.WriteLine("Done ....");
                Console.ReadKey();
            }
        }

        //Show how you will poll the operationSet till msdyn_status changes to Completed or Failed
        //You will need to implement the countRecordsFunction
        private static void WaitForOperations(int expectedNumber, Func<int> countRecordsFunction)
        {
            var sleepTime = 5000 + (100 * expectedNumber);
            int numberOfRecords = -1;
            int maxRetry = 10;
            int retryCount = 0;
            while (expectedNumber != numberOfRecords && retryCount++ != maxRetry)
            {
                Thread.Sleep(sleepTime);
                numberOfRecords = countRecordsFunction();
            }

        }

    }
}
