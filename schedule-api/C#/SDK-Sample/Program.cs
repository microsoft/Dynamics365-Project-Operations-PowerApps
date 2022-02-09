using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;

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
            string userName = Console.ReadLine(); ;
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

                Entity project = scheduleAPI.CreateProject();
                project.Id = scheduleAPI.CallCreateProjectAction(project);
                Console.WriteLine($"Project Created '{project["msdyn_subject"]}'");
                var projectReference = project.ToEntityReference();

                var teamMember = new Entity("msdyn_projectteam", Guid.NewGuid());
                teamMember["msdyn_name"] = $"TM {DateTime.Now.ToShortTimeString()}";
                teamMember["msdyn_project"] = projectReference;
                var createTeamMemberResponse = scheduleAPI.CallCreateTeamMemberAction(teamMember);
                Console.WriteLine($"Team Member Created '{teamMember["msdyn_name"]}'");


                var description = $"My demo {DateTime.Now.ToShortTimeString()}";
                var operationSetId = scheduleAPI.CallCreateOperationSetAction(project.Id, description);
                Console.WriteLine($"Operation Set Created '{description}'");

                var task1 = scheduleAPI.GetTask("1WW", projectReference);
                var task2 = scheduleAPI.GetTask("2XX", projectReference, task1.ToEntityReference());
                var task3 = scheduleAPI.GetTask("3YY", projectReference);
                var task4 = scheduleAPI.GetTask("4ZZ", projectReference);

                var assignment1 = scheduleAPI.GetResourceAssignment("R1", teamMember, task2, project);
                var assignment2 = scheduleAPI.GetResourceAssignment("R2", teamMember, task3, project);

                var task1Response = scheduleAPI.CallPssCreateAction(task1, operationSetId);
                Console.WriteLine($"Task Created '{task1["msdyn_subject"]}'");
                var task2Response = scheduleAPI.CallPssCreateAction(task2, operationSetId);
                Console.WriteLine($"Task Created '{task2["msdyn_subject"]}'");
                var task3Response = scheduleAPI.CallPssCreateAction(task3, operationSetId);
                Console.WriteLine($"Task Created '{task3["msdyn_subject"]}'");
                var task4Response = scheduleAPI.CallPssCreateAction(task4, operationSetId);
                Console.WriteLine($"Task Created '{task4["msdyn_subject"]}'");

                var assignment1Response = scheduleAPI.CallPssCreateAction(assignment1, operationSetId);
                Console.WriteLine($"Assignment Created '{assignment1["msdyn_name"]}'");
                var assignment2Response = scheduleAPI.CallPssCreateAction(assignment2, operationSetId);
                Console.WriteLine($"Assignment Created '{assignment2["msdyn_name"]}'");

                task2["msdyn_subject"] = "Updated Task";
                var task2UpdateResponse = scheduleAPI.CallPssUpdateAction(task2, operationSetId);

                project["msdyn_subject"] = $"Proj update {DateTime.Now.ToShortTimeString()}";
                var projectUpdateResponse = scheduleAPI.CallPssUpdateAction(project, operationSetId);
                Console.WriteLine($"Task Updated '{task2["msdyn_subject"]}'");

                var task4DeleteResponse = scheduleAPI.CallPssDeleteAction(task4.Id.ToString(), task4.LogicalName, operationSetId);
                Console.WriteLine($"Task deleted '{task4["msdyn_subject"]}'");

                var assignment2DeleteResponse = scheduleAPI.CallPssDeleteAction(assignment2.Id.ToString(), assignment2.LogicalName, operationSetId);
                Console.WriteLine($"Assignment deleted '{assignment2["msdyn_name"]}'");

                var dependency1 = scheduleAPI.GetTaskDependency(project, task2, task3);
                var dependency1Response = scheduleAPI.CallPssCreateAction(dependency1, operationSetId);

                scheduleAPI.CallExecuteOperationSetAction(operationSetId);

                Console.WriteLine($"Operation Set Executed");

                Console.WriteLine("Done....");
                Console.Read();
            }
        }
    }
}
