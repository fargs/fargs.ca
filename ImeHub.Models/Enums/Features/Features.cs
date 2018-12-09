using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImeHub.Models.Enums.Features.UserPortal
{
    public class Physicians
    {
        public const string Manage = "8aadf5d6-61c4-42fb-8c69-f6473ed7aa84";
    }

    public class Users
    {
        public const string Manage = "1bc365ad-7d86-452c-9586-1a63fa66418c";
    }
    public class Roles
    {
        public const string Manage = "ce7c6e27-916a-4218-afd3-e833cd38c27d";
    }
    public class Features
    {
        public const string Manage = "9f26152e-6ba3-4a87-811b-d77265c1248c";
    }
}

namespace ImeHub.Models.Enums.Features.PhysicianPortal
{
    public class Availability
    {
        public const string AvailabilitySection = "449c898b-2a50-4adc-9f6b-475f5b989fc8";
        public const string Manage = "a114ec90-5fd6-4b15-b980-0aa025eb0713";
        public const string RemoveDays = "85285972-cc79-4dfe-a528-8561700d3e64";
        public const string Publish = "fd9df153-cebb-4da3-8be0-30750c0c6bda";
        public const string Unpublish = "30f57e7e-87de-4440-802c-dc8b9ac4d467";
        public const string ViewUnpublished = "0039eac3-8435-4beb-8e7a-16c22f0db68d";
        public const string ViewPublished = "dfc748c8-adf7-4e5a-a700-ff793b8092ae";
        public const string BookAssessment = "868080b2-8dc0-4d5a-ae56-75e794ce74e1";
        public const string Reschedule = "1ff7e1c3-8561-458d-a03b-24cf15f09f35";
        public const string Cancel = "2dc5a5c4-07cf-4eb0-8374-34eb66b1b2fa";
    }

    public class Work
    {
        public const string WorkSection = "78914703-30eb-4215-99e9-7157a9667b40";
        public const string DaySheet = "f3a9caa4-4189-4f85-913d-82a632cbc5f4";
        public const string Tasks = "001da691-9410-4127-b96b-bb54f4ef262f";
        public const string Schedule = "443c3f14-2246-40f0-9967-96c724415cf7";
        public const string Additionals = "31e3217d-6065-45fd-bd54-410f87d7b324";
    }

    public class SysAdmin
    {
        public const string Section = "b472481e-0951-4586-8315-35cf3414e8cf";
        public const string ManageAddresses = "c36fad61-0cf3-4402-ad1a-ec6f9614b0d2";
        public const string ViewAddresses = "4f71d30e-7d10-4381-aab7-91b20915e20e";
        public const string ManageProcessTemplates = "c72e490e-d367-4816-9e82-ac7b97440216";
        public const string ViewProcessTemplates = "4fe47ec1-f4a8-4a57-ad0b-32eb3240c039";
        public const string ManageCompanies = "4bcd7432-6369-4782-a09c-5b0f8c3f29bb";
        public const string ViewCompanies = "49da75ec-fedf-4f23-94e5-b56cc15fca92";
        public const string ManageServices = "f6c5f636-b5eb-4bdb-81f6-82de86497045";
        public const string ViewServices = "cfb0a213-0d68-4526-b959-6a6db87cea9b";
        public const string ManageTasks = "11864032-fe6e-4876-8a24-dd1c41ff95a1";
        public const string ViewTasks = "1ffa1891-cdd1-4c77-8452-8f56a8f4010b";
    }

    public class Admin
    {
        public const string Section = "25d44b77-fcf4-4cf7-b9dd-af83abbd3960";
        public const string ManageAddresses = "d31ddf45-550c-4038-8406-08826c7678b7";
        public const string ViewAddresses = "852acb99-b962-4cd8-b87d-44e7c11deecd";
        public const string ManageProcessTemplates = "a62e4c32-ec43-4ef9-bf5f-a8b4d571bbc0";
        public const string ViewProcessTemplates = "901b841e-890f-49e2-96c8-1cc8e36faa4f";
        public const string ManageCompanies = "07b27024-db3e-4cf5-8ae6-a1a8c2267b6c";
        public const string ViewCompanies = "0eb96302-105c-4e27-a1ab-3ab988823b5d";
        public const string ManageServices = "0a733fd9-7b34-4c16-942f-570aad5d9e80";
        public const string ViewServices = "551c1d8c-7b85-4700-af9a-de6160d0a7ac";
        public const string ManageTasks = "93e282a2-ff91-4eeb-bc0b-fb93d443c311";
        public const string ViewTasks = "46ee158a-cd80-4dc4-b2ff-4275ff73926a";
    }

    public class AppContext
    {
        public const string ChangeUserContext = "f8de0198-73cd-46b8-98d0-0df6b0bd53fc";
        public const string ChangeRole = "0505490f-2d97-4c3e-9f06-412c5d445688";
    }

    public class ServiceRequest
    {
        public const string ViewServices = "f7781913-827c-4a08-8d2b-f267d18bd2c3";
        public const string SubmitRequest = "47cf9991-d665-4c16-b464-1c9f8b093d71";
        public const string Search = "af29e58b-2d81-4527-b81b-b4c123905c6f";
        public const string Edit = "faccd3d8-fe67-4db8-8664-2c99b4f3c692";
        public const string View = "eb2944f8-ab5c-4557-b7ca-a02cb0db32e4";
        public const string Cancel = "7b8c1092-ed56-4919-a385-77aa7665c8b2";
        public const string ToggleNoShow = "d217b02b-dc20-4341-b50a-1304b797622f";
        public const string ChangeCompanyOrService = "bb974712-fd6f-46b4-b69f-175f170e531e";
        public const string AssignResources = "6f22a149-b497-4b2d-b871-bf76a12b452a";
        public const string ChangeProcessTemplate = "049ee851-e8f1-43bd-a6df-62753eae48c2";
        public const string Reschedule = "37bcafcc-9966-41c4-857e-60cf7b7ef525";
        public const string Delete = "c133aa06-4188-4974-aea1-d9347dc5ac56";
        public const string ToggleOnHold = "2b78dbb8-832b-40b6-9330-621f4117ad2d";
        public const string ManageInvoiceNote = "be0a3919-0948-4d81-9b46-8967989c1f04";
        public const string ViewInvoiceNote = "6d070130-7412-4dab-b692-dc71f0fd10be";
        public const string LiveChat = "6370acbf-38ab-4349-9e1d-2896e6f78671";
        public const string ManageTasks = "31e831a7-e677-4616-9fa7-284315057bf7";
        public const string PickupTask = "e3ac82dc-2aef-45ce-9df6-8f2b0a5eea8a";
        public const string AssignTask = "060a6c34-3878-47d4-8e88-67349b85d701";
        public const string UpdateTaskStatus = "7afa5750-c5f5-4545-8fb8-0bf4d4c0373b";
        public const string AddTask = "c40c6718-8b8a-440c-a2ec-35761ab1fc3d";
        public const string EditTask = "b58cb4b7-cb63-497c-bdb2-2235cee2ec03";
        public const string DeleteTask = "e042ebf9-b2d6-4431-9ca2-b699d9b76895";
        public const string ViewTaskList = "7ec79861-9fac-4fd0-810d-d285e4ecadb3";
        public const string BulkUpdateDueDates = "64ad1a0f-0eab-4a20-8ec4-a176dad62d03";
    }

    public class ServiceRequest_Teleconference
    {
        public const string Update = "f8f148f5-36d6-4f2a-8991-baa30df2e79c";
        public const string Read = "60f30e28-099d-434a-998f-ab1af2b7eb9a";
        public const string Delete = "7d47bb87-0534-4e4f-a38e-c2242d31c271";
        public const string Create = "4d95bced-7c8a-431d-8cbc-ea732b5458e2";
    }

    public class ServiceRequest_Box
    {
        public const string CreateFolder = "32aaa81d-192c-4ef5-bdba-3429ed03bcc5";
        public const string AddCollaborator = "774902ff-46a3-43b3-bdf4-a02e6d3d5ef0";
        public const string RemoveCollaborator = "5b9faef9-95bb-41df-b5a4-7db8a785bc03";
        public const string SyncUnsyncCollaborator = "f7f81fae-15ca-47f9-85c3-9fea80fb9dc8";
        public const string UpdateFolder = "131442a9-fcc1-40b7-976b-78450861591f";
        public const string MoveFolder = "610acbc7-0084-449f-8849-e50866c0862c";
        public const string ManageBoxFolder = "e6654c3f-37ac-426f-a597-63a0a80a73b3";
        public const string ViewBoxFolder = "449fd98b-06b3-488e-9577-57da0e8284af";
        public const string Reconcile = "1cefa02b-06a6-41c2-abb7-63fc25cf7595";
    }

    public class ServiceRequest_Google
    {
        public const string CreateEvent = "f9439337-ca5c-4716-90a9-0ff43483fda0";
        public const string CancelEvent = "186f474e-78e8-4514-be83-337123b724fe";
        public const string AddAttendee = "0eaa9ad2-4a19-4217-9ac2-6aa85cfc4d7e";
        public const string RemoveAttendee = "704b2401-c565-4dad-88c2-30c16b0bc4b6";
        public const string OpenInGoogle = "3b3a1bd2-baaf-4fd6-b9bc-248f73e3d18e";
    }

    public class Accounting
    {
        public const string AccountingSection = "11058cc9-1610-461a-a411-774277800a66";
        public const string CreateInvoice = "e5411099-2692-4ace-9d55-6783861545fc";
        public const string ViewInvoice = "c50e62c5-9eeb-4e78-9d25-159900f02750";
        public const string EditInvoice = "eec28c7e-d0f8-4479-a412-7a065b019abd";
        public const string DeleteInvoice = "e1c29d4c-9d56-4975-ba2a-cbdf98054152";
        public const string ViewUnsentInvoices = "59c34f41-211e-444d-9ab6-62afff146138";
        public const string ViewUnpaidInvoices = "1c232fec-8e49-4826-83ca-6faad01ab93a";
        public const string SearchInvoices = "7a0cb736-e5f1-4bdf-8523-f114a981531f";
        public const string Analytics = "4fb03469-8bc8-4e08-b9d7-4893fd9642e3";
        public const string SendInvoice = "2da13a36-96ad-446b-b103-7e1372f32009";
        public const string DownloadInvoice = "26bc5406-9a82-4742-9429-00748696ec46";
        public const string GenerateInvoicePdf = "22fd35e4-ff3d-4fc3-aa03-3f19ca81bcef";
        public const string ManageReceipts = "f45af81c-dadd-427c-8c55-460bc7c3c8a7";
        public const string ManageInvoices = "3d1fd2ce-9c2d-4db0-9ef4-79b6bbc3bb19";
    }

    public class Services
    {
        public const string ServicesSection = "775431d0-6428-49c1-9bf9-56c9cc093c87";
        public const string Search = "fe9e09dd-e0ee-4ac8-af54-3170e07ea3ed";
        public const string Manage = "42355937-43d1-4fee-9244-68fedc23031e";
    }

    public class ServiceCatalogue
    {
        public const string View = "575d983d-6654-4d68-9bbf-74c7999bd090";
        public const string Manage = "ac53908a-f3ae-43cc-a1e1-2a21d8150961";
    }

    

    public class Collaborator
    {
        public const string CollaboratorSection = "05970a2b-6295-40bc-9512-aa49828250c5";
        public const string Create = "4153abe9-31f6-4187-aac6-a858c7563943";
        public const string ViewList = "7a25c09e-ea4e-4a46-b88c-e08c97fe4e21";
        public const string Search = "389a860a-177f-4d41-b3ba-bea4f6df9dd2";
    }

    public class Dashboard
    {
        public const string DashboardSection = "65755683-4f89-41c9-b07a-a780070a6369";
    }

    public class PhysicianCompany
    {
        public const string Section = "619f01cb-8c86-48a1-8a54-e65e9e84967e";
        public const string Create = "2ca149a0-4785-4039-9b0c-e481727ee14f";
        public const string Search = "5064ec71-b78f-4f18-b9c4-7ade80be3cba";

    }

    public class Reports
    {
        public const string View = "2cad1f58-0e74-4fc4-8e83-7eda889f0d9d";
    }
}
