using Microsoft.Xrm.Sdk;
using System;

namespace ScheduelAPISamples
{
    public static class UpdateScenario
    {
        public static void Run(ScheduleAPIHelpers scheduleAPI)
        {
            Entity project = scheduleAPI.CreateProject();
            project.Id = scheduleAPI.CallCreateProjectAction(project);
            Console.WriteLine($"Project Created '{project["msdyn_subject"]}'");
            var projectReference = project.ToEntityReference();

            var teamMember = scheduleAPI.GetTeamMember(projectReference);
            var createTeamMemberResponse = scheduleAPI.CallCreateTeamMemberAction(teamMember);
            var description = $"My demo {DateTime.Now.ToShortTimeString()}";
            var operationSetId = scheduleAPI.CallCreateOperationSetAction(project.Id, description);

            var bucket = scheduleAPI.GetBucket(projectReference, "API bucket");
            var bucketResponse = scheduleAPI.CallPssCreateAction(bucket, operationSetId);
            var bucketReference = bucket.ToEntityReference();
            var task1 = scheduleAPI.GetTask("1WW", projectReference, null, bucketReference);
            var task2 = scheduleAPI.GetTask("2XX", projectReference, task1.ToEntityReference(), bucketReference);
            var task3 = scheduleAPI.GetTask("3YY", projectReference);

            var assignment1 = scheduleAPI.GetResourceAssignment("R1", teamMember, task2, project);
            var assignment2 = scheduleAPI.GetResourceAssignment("R2", teamMember, task3, project);
            var task1Response = scheduleAPI.CallPssCreateAction(task1, operationSetId);
            var task2Response = scheduleAPI.CallPssCreateAction(task2, operationSetId);
            var task3Response = scheduleAPI.CallPssCreateAction(task3, operationSetId);

            var assignment1Response = scheduleAPI.CallPssCreateAction(assignment1, operationSetId);
            var assignment2Response = scheduleAPI.CallPssCreateAction(assignment2, operationSetId);
            var task2Update = new Entity(task2.LogicalName, task2.Id);

            task2Update["msdyn_subject"] = "Updated Task";
            task2Update["msdyn_scheduledstart"] = DateTime.Today.AddDays(1);
            task2Update["msdyn_scheduledend"] = DateTime.Today.AddDays(3);

            var task2UpdateResponse = scheduleAPI.CallPssUpdateAction(task2Update, operationSetId);
            var dependency1 = scheduleAPI.GetTaskDependency(project, task2, task3);
            var dependency1Response = scheduleAPI.CallPssCreateAction(dependency1, operationSetId);
            var deleteDependencyResponse = scheduleAPI.CallPssDeleteAction(dependency1.Id.ToString(), dependency1.LogicalName, operationSetId);
            Console.WriteLine("Caling ExecuteOperationSet...");
            scheduleAPI.CallExecuteOperationSetAction(operationSetId);

            Console.WriteLine("Operation Set Executed");
        }
    }
}
