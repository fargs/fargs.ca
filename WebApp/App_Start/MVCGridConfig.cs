[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebApp.MVCGridConfig), "RegisterGrids")]

namespace WebApp
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;

    using MVCGrid.Models;
    using MVCGrid.Web;
    using ViewModels;
    using Library;
    using Orvosi.Data.Filters;
    using Library.Extensions;
    using Models;
    using LinqKit;
    using ViewDataModels;

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
                Filtering = true
            };

            MVCGridDefinitionTable.Add("TaskGrid", new MVCGridBuilder<TaskGridRow>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .WithRenderingMode(RenderingMode.Controller)
                .WithViewPath("~/Views/MVCGrid/_TaskGrid.cshtml")
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
                    cols.Add().WithColumnName("AssignedTo")
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
                        .WithValueExpression(i => i.DueDate.ToOrvosiDateShortFormat());
                    cols.Add().WithColumnName("Task")
                        .WithHeaderText("Task")
                        .WithSorting(true)
                        .WithFiltering(true)
                        .WithValueExpression(i => i.TaskShortName);
                    cols.Add().WithColumnName("ClaimantName")
                        .WithHeaderText("Claimant")
                        .WithHtmlEncoding(false)
                        .WithSorting(true)
                        .WithFiltering(true)
                        .WithValueExpression(i => i.ToJson());
                    cols.Add().WithColumnName("Company")
                        .WithHeaderText("Company")
                        .WithSorting(true)
                        .WithFiltering(true)
                        .WithValueExpression(i => i.Company);
                    cols.Add().WithColumnName("Service")
                        .WithHeaderText("Service")
                        .WithSorting(true)
                        .WithFiltering(true)
                        .WithValueExpression(i => i.Service);
                    cols.Add().WithColumnName("AppointmentDateAndStartTime")
                        .WithHeaderText("Exam Date")
                        .WithSorting(true)
                        .WithFiltering(true)
                        .WithValueExpression(i => i.AppointmentDateAndStartTime.HasValue ? i.AppointmentDateAndStartTime.Value.ToOrvosiDateShortFormat() : "" );
                    cols.Add().WithColumnName("City")
                        .WithHeaderText("City")
                        .WithSorting(true)
                        .WithFiltering(true)
                        .WithValueExpression(i => i.City);
                    cols.Add().WithColumnName("Physician")
                        .WithHeaderText("Physician")
                        .WithSorting(true)
                        .WithFiltering(true)
                        .WithHtmlEncoding(false)
                        .WithValueExpression(i => i.ToJson());
                })
                .WithSorting(true, "DueDate", SortDirection.Asc)
                .WithRetrieveDataMethod((context) =>
                {
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>
                    var db = ContextPerRequest.db;
                    var identity = context.CurrentHttpContext.User.Identity;
                    var physicianContext = context.CurrentHttpContext.User.Identity.GetPhysicianContext();
                    var userId = identity.GetGuidUserId();

                    var entityQuery = db.ServiceRequestTasks
                        .AreAssignedToUser(userId)
                        .AreActive()
                        .Where(srt => srt.DueDate.HasValue);

                    if (physicianContext != null)
                    {
                        entityQuery = entityQuery
                            .Where(srt => srt.ServiceRequest.PhysicianId == physicianContext.Id);
                    }

                    var dto = entityQuery
                        .OrderByDescending(srt => srt.DueDate).ThenBy(srt => srt.ServiceRequestId).ThenBy(srt => srt.Sequence)
                        .Select(TaskDto.FromServiceRequestTaskAndServiceRequestEntity.Expand())
                        .ToList();

                    var query = dto.AsEnumerable();

                    var options = context.QueryOptions;
                    var result = new QueryResult<TaskGridRow>();

                    // apply filters
                    foreach (var filter in context.QueryOptions.Filters)
                    {
                        switch (filter.Key.ToLower())
                        {
                            case "duedate":
                                var range = new DateFilterArgs() { StartDate = DateTime.Parse(filter.Value), FilterType = ViewDataModels.DateFilterType.On };
                                query = query.AreDueBetween(range);
                                break;
                            case "claimantname":
                                query = query.Where(sr => sr.ServiceRequest.ClaimantName.Contains(filter.Value));
                                break;
                            case "task":
                                query = query.Where(sr => sr.TaskId == short.Parse(filter.Value));
                                break;
                            case "city":
                                query = query.Where(sr => sr.ServiceRequest.Address == null ? true : sr.ServiceRequest.Address.CityId == short.Parse(filter.Value));
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
                                query = options.SortDirection == SortDirection.Asc ?
                                    query.OrderBy(i => i.DueDate) :
                                    query.OrderByDescending(i => i.DueDate);
                                break;
                            case "task":
                                query = options.SortDirection == SortDirection.Asc ?
                                    query.OrderBy(i => i.ShortName) :
                                    query.OrderByDescending(i => i.ShortName);
                                break;
                            case "company":
                                query = options.SortDirection == SortDirection.Asc ?
                                    query.OrderBy(i => i.ServiceRequest.Company.Name) :
                                    query.OrderByDescending(i => i.ServiceRequest.Company.Name);
                                break;
                            case "service":
                                query = options.SortDirection == SortDirection.Asc ?
                                    query.OrderBy(i => i.ServiceRequest.Service.Name) :
                                    query.OrderByDescending(i => i.ServiceRequest.Service.Name);
                                break;
                            case "city":
                                query = options.SortDirection == SortDirection.Asc ?
                                    query.OrderBy(i => i.ServiceRequest.Address == null ? "" : i.ServiceRequest.Address.CityCode) :
                                    query.OrderByDescending(i => i.ServiceRequest.Address == null ? "" : i.ServiceRequest.Address.CityCode);
                                break;
                            case "claimantname":
                                query = options.SortDirection == SortDirection.Asc ?
                                    query.OrderBy(i => i.ServiceRequest.ClaimantName) :
                                    query.OrderByDescending(i => i.ServiceRequest.ClaimantName);
                                break;
                            case "appointmentdateandstarttime":
                                query = options.SortDirection == SortDirection.Asc ?
                                    query.OrderBy(i => i.ServiceRequest.AppointmentDateAndStartTime) :
                                    query.OrderByDescending(i => i.ServiceRequest.AppointmentDateAndStartTime);
                                break;
                            case "physician":
                                query = options.SortDirection == SortDirection.Asc ?
                                    query.OrderBy(i => i.ServiceRequest.Physician.DisplayName) :
                                    query.OrderByDescending(i => i.ServiceRequest.Physician.DisplayName);
                                break;
                            default:
                                break;
                        }
                    }

                    result.TotalRecords = query.Count();

                    if (options.GetLimitOffset().HasValue)
                    {
                        query = query
                            .Skip(options.GetLimitOffset().Value)
                            .Take(options.GetLimitRowcount().Value);
                    }

                    result.Items = query
                        .AsQueryable()
                        .Select(TaskGridRow.FromTaskDto.Expand());

                    return result;
                })
            );
            
        }
    }
}