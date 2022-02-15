using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using System;

namespace ScheduelAPISamples
{
    public class ScheduleAPIHelpers
    {
        private readonly IOrganizationService organizationService;

        public ScheduleAPIHelpers(IOrganizationService organizationService)
        {
            this.organizationService = organizationService;
        }

        /// <summary>
        /// Calls the action to create an operationSet
        /// </summary>
        /// <param name="projectId">project id for the operations to be included in this operationSet</param>
        /// <param name="description">description of this operationSet</param>
        /// <returns>operationSet id</returns>
        public string CallCreateOperationSetAction(Guid projectId, string description)
        {
            OrganizationRequest operationSetRequest = new OrganizationRequest("msdyn_CreateOperationSetV1");
            operationSetRequest["ProjectId"] = projectId.ToString();
            operationSetRequest["Description"] = description;
            OrganizationResponse response = organizationService.Execute(operationSetRequest);
            return response["OperationSetId"].ToString();
        }

        /// <summary>
        /// Calls the action to create an entity, only Task and Resource Assignment for now
        /// </summary>
        /// <param name="entity">Task or Resource Assignment</param>
        /// <param name="operationSetId">operationSet id</param>
        /// <returns>OperationSetResponse</returns>

        public OperationSetResponse CallPssCreateAction(Entity entity, string operationSetId)
        {
            OrganizationRequest operationSetRequest = new OrganizationRequest("msdyn_PssCreateV1");
            operationSetRequest["Entity"] = entity;
            operationSetRequest["OperationSetId"] = operationSetId;
            return GetOperationSetResponseFromOrgResponse(organizationService.Execute(operationSetRequest));
        }

        /// <summary>
        /// Calls the action to update an entity, only Task for now
        /// </summary>
        /// <param name="entity">Task or Resource Assignment</param>
        /// <param name="operationSetId">operationSet Id</param>
        /// <returns>OperationSetResponse</returns>
        public OperationSetResponse CallPssUpdateAction(Entity entity, string operationSetId)
        {
            OrganizationRequest operationSetRequest = new OrganizationRequest("msdyn_PssUpdateV1");
            operationSetRequest["Entity"] = entity;
            operationSetRequest["OperationSetId"] = operationSetId;
            return GetOperationSetResponseFromOrgResponse(organizationService.Execute(operationSetRequest));
        }

        /// <summary>
        /// Calls the action to update an entity, only Task and Resource Assignment for now
        /// </summary>
        /// <param name="recordId">Id of the record to be deleted</param>
        /// <param name="entityLogicalName">Entity logical name of the record</param>
        /// <param name="operationSetId">OperationSet Id</param>
        /// <returns>OperationSetResponse</returns>
        public OperationSetResponse CallPssDeleteAction(string recordId, string entityLogicalName, string operationSetId)
        {
            OrganizationRequest operationSetRequest = new OrganizationRequest("msdyn_PssDeleteV1");
            operationSetRequest["RecordId"] = recordId;
            operationSetRequest["EntityLogicalName"] = entityLogicalName;
            operationSetRequest["OperationSetId"] = operationSetId;
            return GetOperationSetResponseFromOrgResponse(organizationService.Execute(operationSetRequest));
        }

        /// <summary>
        /// Calls the action to execute requests in an operationSet
        /// </summary>
        /// <param name="operationSetId">operationSet id</param>
        /// <returns>OperationSetResponse</returns>
        public OperationSetResponse CallExecuteOperationSetAction(string operationSetId)
        {
            OrganizationRequest operationSetRequest = new OrganizationRequest("msdyn_ExecuteOperationSetV1");
            operationSetRequest["OperationSetId"] = operationSetId;
            return GetOperationSetResponseFromOrgResponse(organizationService.Execute(operationSetRequest));
        }

        /// <summary>
        /// This can be used to abandon an operationSet that is no longer needed
        /// </summary>
        /// <param name="operationSetId">operationSet id</param>
        /// <returns>OperationSetResponse</returns>
        protected OperationSetResponse CallAbandonOperationSetAction(Guid operationSetId)
        {
            OrganizationRequest operationSetRequest = new OrganizationRequest("msdyn_AbandonOperationSetV1");
            operationSetRequest["OperationSetId"] = operationSetId.ToString();
            return GetOperationSetResponseFromOrgResponse(organizationService.Execute(operationSetRequest));
        }


        /// <summary>
        /// Calls the action to create a new project
        /// </summary>
        /// <param name="project">Project</param>
        /// <returns>project Id</returns>
        public Guid CallCreateProjectAction(Entity project)
        {
            OrganizationRequest createProjectRequest = new OrganizationRequest("msdyn_CreateProjectV1");
            createProjectRequest["Project"] = project;
            OrganizationResponse response = organizationService.Execute(createProjectRequest);
            var projectId = Guid.Parse((string)response["ProjectId"]);
            return projectId;
        }

        /// <summary>
        /// Calls the action to create a new project team member
        /// </summary>
        /// <param name="teamMember">Project team member</param>
        /// <returns>project team member Id</returns>
        public string CallCreateTeamMemberAction(Entity teamMember)
        {
            OrganizationRequest request = new OrganizationRequest("msdyn_CreateTeamMemberV1");
            request["TeamMember"] = teamMember;
            OrganizationResponse response = organizationService.Execute(request);
            return (string)response["TeamMemberId"];
        }

        public OperationSetResponse GetOperationSetResponseFromOrgResponse(OrganizationResponse orgResponse)
        {
            return JsonConvert.DeserializeObject<OperationSetResponse>((string)orgResponse.Results["OperationSetResponse"]);
        }

        public EntityCollection GetDefaultBucket(EntityReference projectReference)
        {
            var columnsToFetch = new ColumnSet("msdyn_project", "msdyn_name");
            var getDefaultBucket = new QueryExpression("msdyn_projectbucket")
            {
                ColumnSet = columnsToFetch,
                Criteria =
        {
            Conditions =
            {
                new ConditionExpression("msdyn_project", ConditionOperator.Equal, projectReference.Id),
                new ConditionExpression("msdyn_name", ConditionOperator.Equal, "Bucket 1")
            }
        }
            };

            return organizationService.RetrieveMultiple(getDefaultBucket);
        }

        public Entity GetBucket(EntityReference projectReference)
        {
            var bucketCollection = GetDefaultBucket(projectReference);
            if (bucketCollection.Entities.Count > 0)
            {
                return bucketCollection[0].ToEntity<Entity>();
            }

            throw new Exception($"Please open project with id {projectReference.Id} in the Dynamics UI and navigate to the Tasks tab");
        }

        public Entity CreateProject()
        {
            var project = new Entity("msdyn_project", Guid.NewGuid());
            project["msdyn_subject"] = $"Proj {DateTime.Now.ToShortTimeString()}";

            return project;
        }

        public Entity GetTask(string name, EntityReference projectReference, EntityReference parentReference = null, EntityReference bucket = null)
        {
            var task = new Entity("msdyn_projecttask", Guid.NewGuid());
            task["msdyn_project"] = projectReference;
            task["msdyn_subject"] = name;
            task["msdyn_effort"] = 4d;
            task["msdyn_scheduledstart"] = DateTime.Today;
            task["msdyn_scheduledend"] = DateTime.Today.AddDays(5);
            task["msdyn_start"] = DateTime.Now.AddDays(1);
            task["msdyn_projectbucket"] = bucket ?? GetBucket(projectReference).ToEntityReference();
            task["msdyn_LinkStatus"] = new OptionSetValue(192350000);

            //Custom field handling
            /*
            task["new_custom1"] = "Just my test";
            task["new_age"] = 98;
            task["new_amount"] = 591.34m;
            task["new_isready"] = new OptionSetValue(100000000);
            */

            if (parentReference == null)
            {
                task["msdyn_outlinelevel"] = 1;
            }
            else
            {
                task["msdyn_parenttask"] = parentReference;
            }

            return task;
        }

        public Entity GetResourceAssignment(string name, Entity teamMember, Entity task, Entity project)
        {
            var assignment = new Entity("msdyn_resourceassignment", Guid.NewGuid());
            assignment["msdyn_projectteamid"] = teamMember.ToEntityReference();
            assignment["msdyn_taskid"] = task.ToEntityReference();
            assignment["msdyn_projectid"] = project.ToEntityReference();
            assignment["msdyn_name"] = name;
            return assignment;
        }

        public Entity GetTaskDependency(Entity project, Entity predecessor, Entity successor)
        {
            var taskDependency = new Entity("msdyn_projecttaskdependency", Guid.NewGuid());
            taskDependency["msdyn_project"] = project.ToEntityReference();
            taskDependency["msdyn_predecessortask"] = predecessor.ToEntityReference();
            taskDependency["msdyn_successortask"] = successor.ToEntityReference();
            taskDependency["msdyn_linktype"] = new OptionSetValue(192350000);

            return taskDependency;
        }

        public Entity GetChecklist(EntityReference taskReference, string name = null, double displayOrder = 1.1, bool completed = false)
        {
            var checklist = new Entity("msdyn_projectchecklist", Guid.NewGuid());
            checklist["msdyn_projecttaskid"] = taskReference;
            checklist["msdyn_name"] = name ?? "API Checklist 1";
            checklist["msdyn_projectchecklistorder"] = displayOrder;
            checklist["msdyn_projectchecklistcompleted"] = completed;

            return checklist;
        }

        public Entity GetTeamMember(EntityReference projectReference)
        {
            var teamMember = new Entity("msdyn_projectteam", Guid.NewGuid());
            teamMember["msdyn_name"] = $"TM {DateTime.Now.ToShortTimeString()}";
            teamMember["msdyn_project"] = projectReference;

            return teamMember;

        }

        public Entity GetBucket(EntityReference projectReference, string name)
        {
            var bucket = new Entity("msdyn_projectbucket", Guid.NewGuid());
            bucket["msdyn_project"] = projectReference;
            bucket["msdyn_name"] = name;

            return bucket;
        }

        public string ReadStringFromConsole(string prompt)
        {
            Console.WriteLine(prompt);

            return Console.ReadLine();
        }
    }
}
