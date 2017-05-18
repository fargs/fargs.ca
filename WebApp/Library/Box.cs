using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Box.V2;
using Box.V2.Config;
using Box.V2.JWTAuth;
using Box.V2.Models;
using Box.V2.Auth;
using WebApp.Library.Extensions;
using Box.V2.Exceptions;
using System.Configuration;
using Orvosi.Data;
using System.IO;

namespace WebApp.Library
{
    public class BoxManager
    {
        private BoxConfig boxConfig;
        private string clientId;
        private string clientSecret;
        private Uri redirectUri;
        private Guid adminUserId;
        private string adminBoxUserId = "257722377"; // lfarago@orvosi.ca
        private string enterpriseId = "785477";
        private string privateKey;
        private string jwtPrivateKeyPassword = "Orvosi2015";
        private string jwtPublicKeyId = "c24a31nu";
        private string accessToken;
        private string refreshToken;


        private BoxJWTAuth _BoxJWT;
        private BoxClient _client;
        private OAuthSession _OAuthSession;
        public BoxManager()
        {
            clientId = ConfigurationManager.AppSettings["BoxClientId"];
            clientSecret = ConfigurationManager.AppSettings["BoxClientSecret"];
            //redirectUri = new Uri(ConfigurationManager.AppSettings["BoxRedirectUri"]);
            //adminUserId = new Guid(ConfigurationManager.AppSettings["BoxAdminUserId"]);
            //boxConfig = new BoxConfig(clientId, clientSecret, redirectUri);
            privateKey = System.IO.File.ReadAllText(System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "private_key.pem"));
            var boxConfig = new BoxConfig(clientId, clientSecret, enterpriseId, privateKey, jwtPrivateKeyPassword, jwtPublicKeyId);
            _BoxJWT = new BoxJWTAuth(boxConfig);
        }

        private BoxClient _AdminClient;
        private string _AdminToken;

        public BoxClient AdminClient
        {
            get
            {
                if (_AdminClient == null)
                {
                    _AdminToken = _BoxJWT.AdminToken(); //valid for 60 minutes so should be cached and re-used
                    _AdminClient = _BoxJWT.AdminClient(_AdminToken);
                }
                return _AdminClient;
            }
        }

        public BoxClient AdminClientAsUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _AdminClient = null;
            }
            else if (_AdminClient == null)
            {
                _AdminToken = _BoxJWT.AdminToken(); //valid for 60 minutes so should be cached and re-used
                _AdminClient = _BoxJWT.AdminClient(_AdminToken, userId, true);
            }
            return _AdminClient;
        }

        private BoxClient _UserClient;
        private string _UserToken;
        public BoxClient UserClient(string userId)
        {
            _UserToken = _BoxJWT.UserToken(userId); //valid for 60 minutes so should be cached and re-used
            _UserClient = _BoxJWT.UserClient(_UserToken, userId);
            return _UserClient;
        }

        public BoxClient UserClient()
        {
            if (_UserClient == null)
            {
                _UserToken = _BoxJWT.UserToken(adminBoxUserId); //valid for 60 minutes so should be cached and re-used
                _UserClient = _BoxJWT.UserClient(_UserToken, adminBoxUserId);
            }
            return _UserClient;
        }


        internal BoxCollection<BoxUser> GetUsers()
        {
            return UserClient(adminBoxUserId).UsersManager.GetEnterpriseUsersAsync().Result;
        }

        internal BoxFolder GetFolder(string folderId)
        {
            return UserClient(adminBoxUserId).FoldersManager.GetInformationAsync(folderId, fields: new List<string>() { "name", "sync_state" }).Result;
        }

        internal async Task<BoxFolder> GetFolder(string folderId, string boxUserId)
        {
            try
            {
                var _client = UserClient(boxUserId);
                if (_client == null)
                {
                    return null;
                }
                return await _client.FoldersManager.GetInformationAsync(folderId, fields: new List<string>() { "name", "sync_state" });
            }
            catch (BoxException e)
            {
                if (e.Error.Code == "not_found" || e.Error.Name == "invalid_grant")
                {
                    return null;
                }
                throw e;
            }
        }

        public async Task<BoxFile> GetInvoiceFileInfo(string fileId, string folderId, Guid invoiceGuid)
        {
            var client = UserClient(adminBoxUserId);

            // use the invoice file id if we have it
            var file = fileId == null ? null : await client.FilesManager.GetInformationAsync(fileId, fields: new List<string> { "id", "name" });

            // the file was not in box
            if (file == null)
            {
                // try searching box using the object guid
                var results = await client.SearchManager.SearchAsync(invoiceGuid.ToString().Split('-')[0], type: "file", ancestorFolderIds: new List<string> { folderId }, contentTypes: new List<string> { "name" });

                // this potentially could result in multiple results which we would throw an error
                if (results.TotalCount > 1)
                {
                    throw new Exception("There are more than one invoices in the folder for this case, please delete directly from Box.");
                }
                // if one file is returned, it was found and upload a new version
                else if (results.TotalCount == 1)
                {
                    file = results.Entries.Single() as BoxFile;
                }
                // it was not found so upload a new file
                else
                {
                    return null;
                }
            }

            // upload a new version of the file
            return file;
        }

        public async Task<Stream> GetFileStream(string fileId)
        {
            var client = UserClient(adminBoxUserId);
            return await client.FilesManager.DownloadStreamAsync(fileId);
        }

        public async Task<BoxFile> UploadInvoiceAsync(string physicianInvoicesFolderId, string fileName, string fileId, Guid invoiceGuid, Stream stream)
        {
            var client = UserClient(adminBoxUserId);

            // use the invoice file id if we have it
            var file = fileId == null ? null : await client.FilesManager.GetInformationAsync(fileId, fields: new List<string> { "id" });

            // the file was not in box
            if (file == null)
            {
                // try searching box using the object guid
                var results = await client.SearchManager.SearchAsync(invoiceGuid.ToString().Split('-')[0], type: "file", ancestorFolderIds: new List<string> { physicianInvoicesFolderId }, contentTypes: new List<string> { "name" });

                // this potentially could result in multiple results which we would throw an error
                if (results.TotalCount > 1)
                {
                    throw new Exception("There are more than one invoices in the folder for this case, please delete directly from Box.");
                }
                // if one file is returned, it was found and upload a new version
                else if (results.TotalCount == 1)
                {
                    file = results.Entries.Single() as BoxFile;
                }
                // it was not found so upload a new file
                else
                {
                    var request = new BoxFileRequest
                    {
                        Name = fileName,
                        Parent = new BoxItemRequest { Id = physicianInvoicesFolderId }
                    };
                    return await client.FilesManager.UploadAsync(request, stream);
                }
            }

            // upload a new version of the file
            return await client.FilesManager.UploadNewVersionAsync(fileName, file.Id, stream);
        }

        internal BoxCollection<BoxCollaboration> GetCollaborations(string folderId)
        {
            return UserClient(adminBoxUserId).FoldersManager.GetCollaborationsAsync(folderId).Result;
        }

        public BoxCollection<BoxItem> GetPhysicianFolder(string PhysicianFolderId)
        {
            return UserClient(adminBoxUserId).FoldersManager.GetFolderItemsAsync(PhysicianFolderId, 200).Result;
        }

        public string GetCaseFolderPath(string ProvinceName, DateTime AppointmentDate, string CaseFolderName)
        {
            return string.Format("{0} {1}/{2}/{3}/{4}", ProvinceName, AppointmentDate.ToString("yyyy"), AppointmentDate.ToMonthFolderName(), AppointmentDate.ToWeekFolderName(), CaseFolderName);
        }

        public BoxFolder CreateAddOnFolder(string PhysicianFolderId, string ProvinceName, DateTime DueDate, string CaseFolderName, string FolderTemplateId)
        {
            var physicianFolder = UserClient(adminBoxUserId).FoldersManager.GetInformationAsync(PhysicianFolderId).Result;

            var addOnFolderName = "Addendums and Paper Reviews";
            var addOnFolder = UserClient(adminBoxUserId).FoldersManager.GetFolderItemsAsync(physicianFolder.Id, 50).Result.Entries.SingleOrDefault(i => i.Name.ToUpper() == addOnFolderName.ToUpper()) as BoxFolder;
            if (addOnFolder == null)
            {
                var request = new BoxFolderRequest();
                request.Parent = new BoxRequestEntity() { Id = physicianFolder.Id };
                request.Name = addOnFolderName;
                addOnFolder = UserClient(adminBoxUserId).FoldersManager.CreateAsync(request).Result;
            }

            var provinceYearFolderName = string.Format("{0} {1}", ProvinceName, DueDate.ToString("yyyy"));
            var provinceYearFolder = UserClient(adminBoxUserId).FoldersManager.GetFolderItemsAsync(addOnFolder.Id, 50).Result.Entries.SingleOrDefault(i => i.Name.ToUpper() == provinceYearFolderName.ToUpper()) as BoxFolder;
            if (provinceYearFolder == null)
            {
                var request = new BoxFolderRequest();
                request.Parent = new BoxRequestEntity() { Id = addOnFolder.Id };
                request.Name = provinceYearFolderName;
                provinceYearFolder = UserClient(adminBoxUserId).FoldersManager.CreateAsync(request).Result;
            }

            var caseFolderName = CaseFolderName;
            var caseFolder = UserClient(adminBoxUserId).FoldersManager.GetFolderItemsAsync(provinceYearFolder.Id, 50).Result.Entries.SingleOrDefault(i => i.Name.ToUpper() == caseFolderName.ToUpper()) as BoxFolder;
            if (caseFolder == null)
            {
                var request = new BoxFolderRequest();
                request.Id = FolderTemplateId;
                request.Parent = new BoxRequestEntity() { Id = provinceYearFolder.Id };
                request.Name = caseFolderName;
                caseFolder = UserClient(adminBoxUserId).FoldersManager.CopyAsync(request).Result;
            }

            return UserClient(adminBoxUserId).FoldersManager.GetInformationAsync(caseFolder.Id).Result;
        }

        public BoxFolder CreateCaseFolder(string PhysicianFolderId, string ProvinceName, DateTime AppointmentDate, string CaseFolderName, string FolderTemplateId)
        {
            var physicianFolder = UserClient(adminBoxUserId).FoldersManager.GetInformationAsync(PhysicianFolderId).Result;

            var provinceYearFolderName = string.Format("{0} {1}", ProvinceName, AppointmentDate.ToString("yyyy"));
            var provinceYearFolder = physicianFolder.Entries().SingleOrDefault(i => i.Name.ToUpper() == provinceYearFolderName.ToUpper()) as BoxFolder;
            if (provinceYearFolder == null)
            {
                var request = new BoxFolderRequest();
                request.Parent = new BoxRequestEntity() { Id = physicianFolder.Id };
                request.Name = provinceYearFolderName;
                provinceYearFolder = UserClient(adminBoxUserId).FoldersManager.CreateAsync(request).Result;
            }

            var monthFolderName = AppointmentDate.ToMonthFolderName();
            var monthFolder = UserClient(adminBoxUserId).FoldersManager.GetFolderItemsAsync(provinceYearFolder.Id, 50).Result.Entries.SingleOrDefault(i => i.Name.ToUpper() == monthFolderName.ToUpper()) as BoxFolder;
            if (monthFolder == null)
            {
                var request = new BoxFolderRequest();
                request.Parent = new BoxRequestEntity() { Id = provinceYearFolder.Id };
                request.Name = monthFolderName;
                monthFolder = UserClient(adminBoxUserId).FoldersManager.CreateAsync(request).Result;
            }

            var weekFolderName = AppointmentDate.ToWeekFolderName();
            var weekFolder = UserClient(adminBoxUserId).FoldersManager.GetFolderItemsAsync(monthFolder.Id, 50).Result.Entries.SingleOrDefault(i => i.Name.ToUpper() == weekFolderName.ToUpper()) as BoxFolder;
            if (weekFolder == null)
            {
                var request = new BoxFolderRequest();
                request.Parent = new BoxRequestEntity() { Id = monthFolder.Id };
                request.Name = weekFolderName;
                weekFolder = UserClient(adminBoxUserId).FoldersManager.CreateAsync(request).Result;
            }

            var caseFolderName = CaseFolderName;
            var caseFolder = UserClient(adminBoxUserId).FoldersManager.GetFolderItemsAsync(weekFolder.Id, 50).Result.Entries.SingleOrDefault(i => i.Name.ToUpper() == caseFolderName.ToUpper()) as BoxFolder;
            if (caseFolder == null)
            {
                var request = new BoxFolderRequest();
                request.Id = FolderTemplateId;
                request.Parent = new BoxRequestEntity() { Id = weekFolder.Id };
                request.Name = caseFolderName;
                caseFolder = UserClient(adminBoxUserId).FoldersManager.CopyAsync(request).Result;
            }

            return UserClient(adminBoxUserId).FoldersManager.GetInformationAsync(caseFolder.Id).Result;
        }

        public BoxFolder RenameCaseFolder(string boxFolderId, string caseFolderName)
        {
            var caseFolder = UserClient(adminBoxUserId).FoldersManager.GetInformationAsync(boxFolderId).Result;
            if (caseFolder == null)
            {
                throw new Exception("Folder does not exist");
            }

            var request = new BoxFolderRequest();
            request.Id = boxFolderId;
            request.Name = caseFolderName;
            caseFolder = UserClient(adminBoxUserId).FoldersManager.UpdateInformationAsync(request).Result;

            return caseFolder;
        }

        public BoxCollaboration AddCollaboration(string FolderId, string BoxUserId, string BoxLogin)
        {
            var request = new BoxCollaborationRequest()
            {
                Item = new BoxRequestEntity()
                {
                    Id = FolderId,
                    Type = BoxType.folder
                },
                AccessibleBy = new BoxCollaborationUserRequest()
                {
                    Type = BoxType.user,
                    Login = BoxLogin

                },
                Role = BoxCollaborationRoles.Editor
            };
            // Create the collaboration
            var collaboration = UserClient(adminBoxUserId).CollaborationsManager.AddCollaborationAsync(request).Result;
            //collaboration = AcceptCollaboration(collaboration.Id, BoxUserId);

            return collaboration;
        }

        public BoxFolder UpdateSyncState(string FolderId, string BoxUserId, BoxSyncStateType? Status)
        {
            var syncRequest = new BoxFolderRequest()
            {
                Id = FolderId,
                Type = BoxType.folder,
                SyncState = Status
            };
            return UserClient(BoxUserId).FoldersManager.UpdateInformationAsync(syncRequest).Result;
        }

        public BoxCollaboration AcceptCollaboration(string CollaborationId, string BoxUserId)
        {
            var request = new BoxCollaborationRequest()
            {
                Id = CollaborationId,
                Status = "accepted"
            };
            return UserClient(BoxUserId).CollaborationsManager.EditCollaborationAsync(request).Result;
        }

        public bool RemoveCollaboration(string CollaborationId)
        {
            return UserClient(adminBoxUserId).CollaborationsManager.RemoveCollaborationAsync(CollaborationId).Result;
        }

        public BoxCollaboration GetCollaboration(string CollaborationId)
        {
            return UserClient(adminBoxUserId).CollaborationsManager.GetCollaborationAsync(CollaborationId).Result;
        }
    }
}