using Microsoft.Xrm.Sdk;
using System;

namespace ScheduelAPISamples
{
    public static class CUDScenario
    {
        public static void Run(ScheduleAPIHelpers scheduleAPI)
        {
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
            Console.WriteLine($"Task Create resquest'{task1["msdyn_subject"]}'");
            var task2Response = scheduleAPI.CallPssCreateAction(task2, operationSetId);
            Console.WriteLine($"Task Create resquest'{task2["msdyn_subject"]}'");
            var task3Response = scheduleAPI.CallPssCreateAction(task3, operationSetId);
            Console.WriteLine($"Task Create resquest'{task3["msdyn_subject"]}'");
            var task4Response = scheduleAPI.CallPssCreateAction(task4, operationSetId);
            Console.WriteLine($"Task Create resquest'{task4["msdyn_subject"]}'");

            var assignment1Response = scheduleAPI.CallPssCreateAction(assignment1, operationSetId);
            Console.WriteLine($"Assignment Create resquest'{assignment1["msdyn_name"]}'");
            var assignment2Response = scheduleAPI.CallPssCreateAction(assignment2, operationSetId);
            Console.WriteLine($"Assignment Create resquest'{assignment2["msdyn_name"]}'");

            task2["msdyn_subject"] = "Update Task resquest";
            var task2UpdateResponse = scheduleAPI.CallPssUpdateAction(task2, operationSetId);

            project["msdyn_subject"] = $"Proj update {DateTime.Now.ToShortTimeString()}";
            var projectUpdateResponse = scheduleAPI.CallPssUpdateAction(project, operationSetId);
            Console.WriteLine($"Task Update resquest'{task2["msdyn_subject"]}'");

            var task4DeleteResponse = scheduleAPI.CallPssDeleteAction(task4.Id.ToString(), task4.LogicalName, operationSetId);
            Console.WriteLine($"Task delete resquest'{task4["msdyn_subject"]}'");

            var assignment2DeleteResponse = scheduleAPI.CallPssDeleteAction(assignment2.Id.ToString(), assignment2.LogicalName, operationSetId);
            Console.WriteLine($"Assignment delete resquest'{assignment2["msdyn_name"]}'");

            var dependency1 = scheduleAPI.GetTaskDependency(project, task2, task3);
            var dependency1Response = scheduleAPI.CallPssCreateAction(dependency1, operationSetId);

            Console.WriteLine("Caling ExecuteOperationSet...");
            scheduleAPI.CallExecuteOperationSetAction(operationSetId);

            Console.WriteLine("Operation Set Executed");
        }
    }
}
