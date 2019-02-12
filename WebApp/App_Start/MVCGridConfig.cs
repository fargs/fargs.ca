[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebApp.MVCGridConfig), "RegisterGrids")]

namespace WebApp
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Library.Extensions;
    using LinqKit;
    using Models;
    using MVCGrid.Models;
    using MVCGrid.Web;
    using Orvosi.Data;
    using Orvosi.Data.Filters;
    using WebApp.Areas.Work.Views.Tasks;

    public static class MVCGridConfig 
    {
        public static void RegisterGrids()
        {
            GridDefaults defaults = new GridDefaults()
            {
                Paging = true,
                ItemsPerPage = 20,
                Sorting = true,
                DefaultSortColumn = "DueDate",
                DefaultSortDirection = SortDirection.Asc,
                NoResultsMessage = "Sorry, no results were found",
                Filtering = true,
            };

            MVCGridDefinitionTable.Add("TaskGrid", new MVCGridBuilder<TaskGridRow>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .WithRenderingMode(RenderingMode.Controller)
                .WithViewPath("~/Areas/Work/Views/Tasks/TaskGrid.cshtml")
                .WithRowCssClassExpression(c =>
                {
                    if (c.IsOverdue.HasValue && c.IsDueToday.HasValue)
                    {
                        return c.IsOverdue.Value ? "bg-danger" : c.IsDueToday.Value ? "bg-success" : "";
                    }
                    return "";
                })
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Id")
                        .WithHeaderText("")
                        .WithHtmlEncoding(false)
                        .WithValueExpression(i => i.ToJson());
                    cols.Add().WithColumnName("Physician")
                        .WithHeaderText("")
                        .WithHtmlEncoding(false)
                        .WithValueExpression(i => i.ToJson());
                    cols.Add().WithColumnName("IsDone")
                        .WithHeaderText("")
                        .WithHtmlEncoding(false)
                        .WithValueExpression(i => i.ToJson());
                    cols.Add().WithColumnName("TaskStatusId")
                        .WithHeaderText("")
                        .WithFiltering(true)
                        .WithHtmlEncoding(false)
                        .WithValueExpression(i => i.ToJson());
                    cols.Add().WithColumnName("DueDate")
                        .WithHeaderText("Due")
                        .WithSorting(true)
                        .WithFiltering(true)
                        .WithValueExpression(i => i.DueDate.HasValue ? i.DueDate.Value.ToOrvosiDateShortFormat() : "ASAP");
                    cols.Add().WithColumnName("TaskId")
                        .WithHeaderText("Task")
                        .WithSorting(true)
                        .WithFiltering(true)
                        .WithValueExpression(i => i.TaskName);
                    cols.Add().WithColumnName("ClaimantName")
                        .WithHeaderText("Claimant")
                        .WithHtmlEncoding(false)
                        .WithSorting(true)
                        .WithFiltering(true)
                        .WithValueExpression(i => i.ToJson());
                })
                .WithSorting(true, "DueDate", SortDirection.Asc)
                .WithRetrieveDataMethod((context) =>
                {
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>
                    var db = DependencyResolver.Current.GetService<OrvosiDbContext>();
                    var identity = context.CurrentHttpContext.User.Identity;
                    var physicianId = context.CurrentHttpContext.User.Identity.GetPhysicianId();
                    var userId = identity.GetGuidUserId();
                    var now = DependencyResolver.Current.GetService<DateTime>();

                    var entityQuery = db.ServiceRequestTasks
                        .AreAssignedToUser(userId)
                        .AreActiveOrDone();
                        //.Where(srt => srt.DueDate.HasValue);

                    if (physicianId.HasValue)
                    {
                        entityQuery = entityQuery
                            .Where(srt => srt.ServiceRequest.PhysicianId == physicianId);
                    }

                    
                    var options = context.QueryOptions;
                    var result = new QueryResult<TaskGridRow>();

                    // apply filters
                    foreach (var filter in context.QueryOptions.Filters)
                    {
                        switch (filter.Key.ToLower())
                        {
                            case "taskid":
                                var selectedTaskIds = filter.Value.Split(',').SelectTry<string, string, short>(c => c, short.TryParse).ToList();
                                entityQuery = entityQuery.Where(sr => selectedTaskIds.Any() ? selectedTaskIds.Contains(sr.TaskId.Value) : true);
                                break;
                            case "cityid":
                                short cityId;
                                if (short.TryParse(filter.Value, out cityId))
                                { 
                                    entityQuery = entityQuery.Where(sr => sr.ServiceRequest.Address == null ? true : sr.ServiceRequest.Address.CityId == cityId);
                                }
                                break;
                            case "taskstatusid":
                                var selectedTaskStatusIds = filter.Value.Split(',').SelectTry<string, string, short>(c => c, short.TryParse).ToList(); // filters out any items that are not valid shorts.
                                entityQuery = entityQuery.Where(sr => selectedTaskStatusIds.Any() ? selectedTaskStatusIds.Contains(sr.TaskStatusId) : true);
                                break;
                            default:
                                break;
                        }
                    }

                    // apply sorting
                    if (!String.IsNullOrWhiteSpace(options.SortColumnName))
                    {
                        switch (options.SortColumnName.ToLower())
                        {
                            case "duedate":
                                entityQuery = options.SortDirection == SortDirection.Asc ?
                                    entityQuery.OrderBy(srt => srt.DueDate).ThenBy(srt => srt.ServiceRequestId).ThenBy(srt => srt.Sequence) :
                                    entityQuery.OrderByDescending(srt => srt.DueDate).ThenBy(srt => srt.ServiceRequestId).ThenBy(srt => srt.Sequence); ;
                                break;
                            case "task":
                                entityQuery = options.SortDirection == SortDirection.Asc ?
                                    entityQuery.OrderBy(i => i.ShortName) :
                                    entityQuery.OrderByDescending(i => i.ShortName);
                                break;
                            case "claimantname":
                                entityQuery = options.SortDirection == SortDirection.Asc ?
                                    entityQuery.OrderBy(i => i.ServiceRequest.ClaimantName) :
                                    entityQuery.OrderByDescending(i => i.ServiceRequest.ClaimantName);
                                break;
                            default:
                                entityQuery = entityQuery.OrderByDescending(srt => srt.DueDate).ThenBy(srt => srt.ServiceRequestId).ThenBy(srt => srt.Sequence);
                                break;
                        }
                    }

                    result.TotalRecords = entityQuery.Count();

                    if (options.GetLimitOffset().HasValue)
                    {
                        entityQuery = entityQuery
                            .Skip(options.GetLimitOffset().Value)
                            .Take(options.GetLimitRowcount().Value);
                    }

                    var dto = entityQuery
                        .Select(TaskDto.FromServiceRequestTaskForTasks.Expand())
                        .ToList();

                    result.Items = dto
                        .Select(t => new TaskGridRow(t, identity, now));

                    return result;
                })
            );
            
        }
    }
}