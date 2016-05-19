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
using Model;

namespace WebApp.Library
{
    public class BoxManager
    {
        private BoxConfig boxConfig;
        private string clientId;
        private string clientSecret;
        private Uri redirectUri;
        private string adminUserId;
        private string enterpriseId = "785477";
        private string privateKey = System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "private_key.pem"));
        private string jwtPrivateKeyPassword = "Orvosi2015";
        private string jwtPublicKeyId = "c24a31nu";
        private string accessToken;
        private string refreshToken;


        private BoxJWTAuth _BoxJWT;
        private BoxClient _client;
        private OAuthSession _OAuthSession;
        public BoxManager()
        {
            //var boxConfig = new BoxConfig(clientId, clientSecret, enterpriseId, privateKey, jwtPrivateKeyPassword, jwtPublicKeyId);
            //_BoxJWT = new BoxJWTAuth(boxConfig);
            clientId = ConfigurationManager.AppSettings["BoxClientId"];
            clientSecret = ConfigurationManager.AppSettings["BoxClientSecret"];
            redirectUri = new Uri(ConfigurationManager.AppSettings["BoxRedirectUri"]);
            adminUserId = ConfigurationManager.AppSettings["BoxAdminUserId"];
            boxConfig = new BoxConfig(clientId, clientSecret, redirectUri);
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

        private BoxClient _UserClient;
        private string _UserToken;
        public BoxClient UserClient(string userId)
        {
            if (_UserClient == null)
            {
                //NOTE: you must set IsPlatformAccessOnly=true for an App User
                //var userRequest = new BoxUserRequest() { Name = "CaseCoordinatorAgent", IsPlatformAccessOnly = true };
                //var appUser = AdminClient.UsersManager.CreateEnterpriseUserAsync(userRequest).Result;
                //var userList = AdminClient.UsersManager.GetEnterpriseUsersAsync(filterTerm: "AppUser_218182_xyGlkBUid6@boxdevedition.com").Result;
                //var appUser = userList.Entries.First();
                //get a user client
                _UserToken = _BoxJWT.UserToken(userId); //valid for 60 minutes so should be cached and re-used
                _UserClient = _BoxJWT.UserClient(_UserToken, userId);
            }
            return _UserClient;
        }


        public BoxClient Client(string asUser = "")
        {
            if (_client == null || _client.Auth.Session.ExpiresIn < 0 || !string.IsNullOrEmpty(asUser))
            {
                GetBoxTokens_Result tokens;
                using (var db = new OrvosiEntities())
                {
                    tokens = db.GetBoxTokens(adminUserId).First();
                }
                var auth = new OAuthSession(tokens.BoxAccessToken, tokens.BoxRefreshToken, 3600, "bearer");
                if (string.IsNullOrEmpty(asUser))
                {
                    _client = new BoxClient(boxConfig, auth);
                }
                else
                {
                    _client = new BoxClient(boxConfig, auth, asUser: asUser);
                }

                _client.Auth.SessionAuthenticated += (sender, args) =>
                {
                    using (var db = new OrvosiEntities())
                    {
                        db.SaveBoxTokens(args.Session.AccessToken, args.Session.RefreshToken, adminUserId);
                    }
                    _client = new BoxClient(boxConfig, args.Session);
                };

                //_client.Auth.SessionInvalidated += async (sender, args) =>
                //{
                //    var refresh = await _client.Auth.RefreshAccessTokenAsync(_client.Auth.Session.AccessToken);
                //    using (var db = new OrvosiEntities())
                //    {
                //        db.SaveBoxTokens(refresh.AccessToken, refresh.RefreshToken, adminUserId);
                //    }
                //    _client = new BoxClient(config, refresh);
                //};

            }
            return _client;
        }

        internal BoxCollection<BoxUser> GetUsers()
        {
            return Client().UsersManager.GetEnterpriseUsersAsync().Result;
        }

        internal BoxFolder GetFolder(string folderId)
        {
            return Client().FoldersManager.GetInformationAsync(folderId, fields: new List<string>() { "name", "sync_state" }).Result;
        }

        internal async Task<BoxFolder> GetFolder(string folderId, string boxUserId)
        {
            try
            {
                return await Client(boxUserId).FoldersManager.GetInformationAsync(folderId, fields: new List<string>() { "name", "sync_state" });
            }
            catch (BoxException e)
            {
                if (e.Error.Code == "not_found")
                {
                    return null;
                }
                throw e;
            }
        }

        internal BoxCollection<BoxCollaboration> GetCollaborations(string folderId)
        {
            return Client().FoldersManager.GetCollaborationsAsync(folderId).Result;
        }

        public BoxCollection<BoxItem> GetPhysicianFolder(string PhysicianFolderId)
        {
            return Client().FoldersManager.GetFolderItemsAsync(PhysicianFolderId, 200).Result;
        }

        public string GetCaseFolderPath(string ProvinceName, DateTime AppointmentDate, string CaseFolderName)
        {
            return string.Format("{0} {1}/{2}/{3}/{4}", ProvinceName, AppointmentDate.ToString("yyyy"), AppointmentDate.ToMonthFolderName(), AppointmentDate.ToWeekFolderName(), CaseFolderName);
        }

        public BoxFolder CreateCaseFolder(string PhysicianFolderId, string ProvinceName, DateTime AppointmentDate, string CaseFolderName)
        {
            BoxFolder physicianFolder = null;
            BoxFolder provinceYearFolder = null;
            BoxFolder monthFolder = null;
            BoxFolder weekFolder = null;
            BoxFolder caseFolder = null;

            physicianFolder = Client().FoldersManager.GetInformationAsync(PhysicianFolderId).Result;

            var provinceYearFolderName = string.Format("{0} {1}", ProvinceName, AppointmentDate.ToString("yyyy"));
            provinceYearFolder = physicianFolder.Entries().SingleOrDefault(i => i.Name == provinceYearFolderName) as BoxFolder;
            if (provinceYearFolder == null)
            {
                var request = new BoxFolderRequest();
                request.Parent = new BoxRequestEntity() { Id = physicianFolder.Id };
                request.Name = provinceYearFolderName;
                provinceYearFolder = Client().FoldersManager.CreateAsync(request).Result;
            }

            var monthFolderName = AppointmentDate.ToMonthFolderName();
            monthFolder = Client().FoldersManager.GetFolderItemsAsync(provinceYearFolder.Id, 50).Result.Entries.SingleOrDefault(i => i.Name == monthFolderName) as BoxFolder;
            if (monthFolder == null)
            {
                var request = new BoxFolderRequest();
                request.Parent = new BoxRequestEntity() { Id = provinceYearFolder.Id };
                request.Name = monthFolderName;
                monthFolder = Client().FoldersManager.CreateAsync(request).Result;
            }

            var weekFolderName = AppointmentDate.ToWeekFolderName();
            weekFolder = Client().FoldersManager.GetFolderItemsAsync(monthFolder.Id, 50).Result.Entries.SingleOrDefault(i => i.Name == weekFolderName) as BoxFolder;
            if (weekFolder == null)
            {
                var request = new BoxFolderRequest();
                request.Parent = new BoxRequestEntity() { Id = monthFolder.Id };
                request.Name = weekFolderName;
                weekFolder = Client().FoldersManager.CreateAsync(request).Result;
            }

            var caseFolderName = CaseFolderName;
            caseFolder = Client().FoldersManager.GetFolderItemsAsync(weekFolder.Id, 50).Result.Entries.SingleOrDefault(i => i.Name == caseFolderName) as BoxFolder;
            if (caseFolder == null)
            {
                var request = new BoxFolderRequest();
                request.Parent = new BoxRequestEntity() { Id = weekFolder.Id };
                request.Name = caseFolderName;
                caseFolder = Client().FoldersManager.CreateAsync(request).Result;
            }

            return Client().FoldersManager.GetInformationAsync(caseFolder.Id).Result;
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
            var collaboration = Client().CollaborationsManager.AddCollaborationAsync(request).Result;
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
            return Client(BoxUserId).FoldersManager.UpdateInformationAsync(syncRequest).Result;
        }

        public BoxCollaboration AcceptCollaboration(string CollaborationId, string BoxUserId)
        {
            var request = new BoxCollaborationRequest()
            {
                Id = CollaborationId,
                Status = "accepted"
            };
            return Client(BoxUserId).CollaborationsManager.EditCollaborationAsync(request).Result;
        }

        public bool RemoveCollaboration(string CollaborationId)
        {
            return Client().CollaborationsManager.RemoveCollaborationAsync(CollaborationId).Result;
        }

        public BoxCollaboration GetCollaboration(string CollaborationId)
        {
            return Client().CollaborationsManager.GetCollaborationAsync(CollaborationId).Result;
        }
    }
}