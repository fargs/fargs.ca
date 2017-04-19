using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Shared.Enums.Features
{
    public class SecurityAdmin
    {
        public const short Section = 1;
        public const short UserManagement = 5;
        public const short RoleManagement = 10;
        public const short FeatureManagement = 15;
    }

    public class SysAdmin
    {
        public const short Section = 755;
        public const short ManageAddresses = 757;
        public const short ViewAddresses = 759;
        public const short ManageProcessTemplates = 760;
        public const short ViewProcessTemplates = 761;
        public const short ManageCompanies = 763;
        public const short ViewCompanies = 764;
        public const short ManageServices = 766;
        public const short ViewServices = 767;
        public const short ManageTasks = 769;
        public const short ViewTasks = 770;
    }

    public class Admin
    {
        public const short Section = 55;
        public const short ManageAddresses = 57;
        public const short ViewAddresses = 59;
        public const short ManageProcessTemplates = 60;
        public const short ViewProcessTemplates = 61;
        public const short ManageCompanies = 63;
        public const short ViewCompanies = 64;
        public const short ManageServices = 66;
        public const short ViewServices = 67;
        public const short ManageTasks = 69;
        public const short ViewTasks = 70;
    }

    public class AppContext
    {
        public const short ChangeUserContext = 20;
        public const short ChangeRole = 50;
    }

    public class ServiceRequest
    {
        public const short ViewServices = 222;
        public const short SubmitRequest = 224;
        public const short Search = 226;
        public const short Edit = 230;
        public const short View = 232;
        public const short Cancel = 233;
        public const short ToggleNoShow = 234;
        public const short ChangeCompanyOrService = 235;
        public const short AssignResources = 236;
        public const short ChangeProcessTemplate = 237;
        public const short Reschedule = 238;
        public const short Delete = 239;
        public const short ToggleOnHold = 240;
        public const short ManageInvoiceNote = 280;
        public const short ViewInvoiceNote = 282;
        public const short LiveChat = 284;
        public const short PickupTask = 300;
        public const short AssignTask = 302;
        public const short UpdateTaskStatus = 304;
        public const short AddTask = 306;
        public const short DeleteTask = 308;
        public const short ViewTaskList = 310;
        public const short ManageTasks = 299;
    }

    public class ServiceRequest_Box
    {
        public const short CreateFolder = 240;
        public const short AddCollaborator = 242;
        public const short RemoveCollaborator = 244;
        public const short SyncUnsyncCollaborator = 246;
        public const short UpdateFolder = 248;
        public const short MoveFolder = 250;
        public const short ManageBoxFolder = 252;
        public const short ViewBoxFolder = 254;
        public const short Reconcile = 255;
    }

    public class ServiceRequest_Google
    {
        public const short CreateEvent = 260;
        public const short CancelEvent = 262;
        public const short AddAttendee = 264;
        public const short RemoveAttendee = 266;
        public const short OpenInGoogle = 268;
    }

    public class ServiceRequestTask
    {
    }

    public class Accounting
    {
        public const short AccountingSection = 99;
        public const short CreateInvoice = 100;
        public const short ViewInvoice = 101;
        public const short EditInvoice = 103;
        public const short DeleteInvoice = 104;
        public const short ViewUnsentInvoices = 106;
        public const short ViewUnpaidInvoices = 107;
        public const short SearchInvoices = 108;
        public const short Analytics = 109;
        public const short SendInvoice = 110;
        public const short DownloadInvoice = 111;
        public const short ManageReceipts = 113;
    }

    public class Services
    {
        public const short ServicesSection = 400;
        public const short Search = 401;
        public const short Manage = 402;
    }

    public class ServiceCatalogue
    {
        public const short View = 430;
        public const short Manage = 431;
    }

    public class Work
    {
        public const short WorkSection = 320;
        public const short Agenda = 322;
        public const short DueDates = 324;
        public const short Schedule = 326;
        public const short Additionals = 328;
    }

    public class Availability
    {
        public const short AvailabilitySection = 200;
        public const short Manage = 202;
        public const short RemoveDays = 205;
        public const short Publish = 208;
        public const short Unpublish = 210;
        public const short ViewUnpublished = 212;
        public const short ViewPublished = 214;
        public const short BookAssessment = 216;
        public const short Reschedule = 218;
        public const short Cancel = 219;
    }

    public class Collaborator
    {
        public const short CollaboratorSection = 450;
        public const short Create = 460;
        public const short ViewList = 462;
        public const short Search = 464;
    }

    public class Dashboard
    {
        public const short DashboardSection = 450;
    }

    public class PhysicianCompany
    {
        public const short Section = 500;
        public const short Create = 502;
        public const short Search = 504;

    }
}
