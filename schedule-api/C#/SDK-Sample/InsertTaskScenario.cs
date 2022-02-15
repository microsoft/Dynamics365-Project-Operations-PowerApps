using Microsoft.Xrm.Sdk;
using System;

namespace ScheduelAPISamples
{
    public static class InsertTaskScenario
    {
        public static void Run(ScheduleAPIHelpers scheduleAPI)
        {
            string projectId = scheduleAPI.ReadStringFromConsole("Enter a project Id");
            string taskId = scheduleAPI.ReadStringFromConsole("Enter a task Id");

            Entity project = new Entity("msdyn_project", new Guid(projectId));
            var projectReference = project.ToEntityReference();
            Entity task1 = new Entity("msdyn_projecttask", new Guid(taskId));
            var description = $"My demo {DateTime.Now.ToShortTimeString()}";

            var operationSetId = scheduleAPI.CallCreateOperationSetAction(project.Id, description);
            var task2 = scheduleAPI.GetTask("2XX", projectReference, task1.ToEntityReference());
            var task1Response = scheduleAPI.CallPssCreateAction(task2, operationSetId);

            Console.WriteLine("Caling ExecuteOperationSet...");
            scheduleAPI.CallExecuteOperationSetAction(operationSetId);

            Console.WriteLine("Operation Set Executed");
        }
    }
}
