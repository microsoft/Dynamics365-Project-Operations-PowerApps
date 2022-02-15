using Microsoft.Xrm.Sdk;
using System;

namespace ScheduelAPISamples
{
    public static class ChangeTaskHierarchy
    {
        public static void Run(ScheduleAPIHelpers scheduleAPI)
        {
            string projectId = scheduleAPI.ReadStringFromConsole("Enter a project Id");
            string taskId = scheduleAPI.ReadStringFromConsole("Enter a task Id");
            var task5 = new Entity("msdyn_projecttask", new Guid(taskId));
            var op = scheduleAPI.CallCreateOperationSetAction(new Guid(projectId), "New op");
            task5["msdyn_outlinelevel"] = 1;
            scheduleAPI.CallPssUpdateAction(task5, op);
            Console.WriteLine("Caling ExecuteOperationSet...");
            scheduleAPI.CallExecuteOperationSetAction(op);

            Console.WriteLine("Operation Set Executed");
        }
    }
}
