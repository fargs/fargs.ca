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

namespace WebApp.Library
{
    public class BoxManager
    {
        private string clientId = "lm6h6x8il745vi1g49yqjcwaspeiqj6r";
        private string clientSecret = "RRTxW9neXNVlA5u5Uz86GfGDqrbwYpBC";
        private string enterpriseId = "785477";
        private string privateKey = System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "private_key.pem"));
        private string jwtPrivateKeyPassword = "Orvosi2015";
        private string jwtPublicKeyId = "c24a31nu";
        private Uri redirectUri = new Uri("http://localhost:43200/box/");
        private string accessToken = "Hl451RWgwUubxcL6f0PEkwKu3c0XbJ8D";
        private string refreshToken = "88Ap4kE4EeqVjKGMIkhfPpFI3R4ecMSvdbubQb2TDeaxjtSK5F2HQBIVR0eHUmg1";


        private BoxJWTAuth _BoxJWT;
        private BoxClient _client;
        public BoxManager()
        {
            //var boxConfig = new BoxConfig(clientId, clientSecret, enterpriseId, privateKey, jwtPrivateKeyPassword, jwtPublicKeyId);
            //_BoxJWT = new BoxJWTAuth(boxConfig);

            var config = new BoxConfig(clientId, clientSecret, redirectUri);
            var auth = new OAuthSession(accessToken, refreshToken, 3600, "bearer");
            _client = new BoxClient(config, auth);
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
        public BoxClient UserClient
        {
            get
            {
                if (_UserClient == null)
                {
                    //NOTE: you must set IsPlatformAccessOnly=true for an App User
                    //var userRequest = new BoxUserRequest() { Name = "CaseCoordinatorAgent", IsPlatformAccessOnly = true };
                    //var appUser = AdminClient.UsersManager.CreateEnterpriseUserAsync(userRequest).Result;
                    //var userList = AdminClient.UsersManager.GetEnterpriseUsersAsync(filterTerm: "AppUser_218182_xyGlkBUid6@boxdevedition.com").Result;
                    //var appUser = userList.Entries.First();
                    //get a user client
                    var userId = "257722377";
                    _UserToken = _BoxJWT.UserToken(userId); //valid for 60 minutes so should be cached and re-used
                    _UserClient = _BoxJWT.UserClient(_UserToken, userId);
                }
                return _UserClient;
            }
        }

        public BoxClient Client
        {
            get
            {
                if (_client == null || _client.Auth.Session.ExpiresIn < 0)
                {
                    var config = new BoxConfig(clientId, clientSecret, redirectUri);
                    var auth = new OAuthSession(accessToken, refreshToken, 3600, "bearer");
                    _client = new BoxClient(config, auth);
                }
                return _client;
            }
        }

        public BoxCollection<BoxItem> GetPhysicianFolder(string PhysicianFolderId)
        {
            return Client.FoldersManager.GetFolderItemsAsync(PhysicianFolderId, 200).Result;
        }

        public BoxFolder CreateCaseFolder(string PhysicianFolderId, string ProvinceName, DateTime AppointmentDate, string CaseFolderName)
        {
            BoxFolder physicianFolder = null;
            BoxFolder provinceYearFolder = null;
            BoxFolder monthFolder = null;
            BoxFolder weekFolder = null;
            BoxFolder caseFolder = null;

            physicianFolder = Client.FoldersManager.GetInformationAsync(PhysicianFolderId).Result;

            var provinceYearFolderName = string.Format("{0} {1}", ProvinceName, AppointmentDate.ToString("yyyy"));
            provinceYearFolder = physicianFolder.Entries().SingleOrDefault(i => i.Name == provinceYearFolderName) as BoxFolder;
            if (provinceYearFolder == null)
            {
                var request = new BoxFolderRequest();
                request.Parent = new BoxRequestEntity() { Id = physicianFolder.Id };
                request.Name = provinceYearFolderName;
                provinceYearFolder = Client.FoldersManager.CreateAsync(request).Result;
            }

            var monthFolderName = AppointmentDate.ToMonthFolderName();
            monthFolder = Client.FoldersManager.GetFolderItemsAsync(provinceYearFolder.Id, 50).Result.Entries.SingleOrDefault(i => i.Name == monthFolderName) as BoxFolder;
            if (monthFolder == null)
            {
                var request = new BoxFolderRequest();
                request.Parent = new BoxRequestEntity() { Id = provinceYearFolder.Id };
                request.Name = monthFolderName;
                monthFolder = Client.FoldersManager.CreateAsync(request).Result;
            }

            var weekFolderName = AppointmentDate.ToWeekFolderName();
            weekFolder = Client.FoldersManager.GetFolderItemsAsync(monthFolder.Id, 50).Result.Entries.SingleOrDefault(i => i.Name == weekFolderName) as BoxFolder;
            if (weekFolder == null)
            {
                var request = new BoxFolderRequest();
                request.Parent = new BoxRequestEntity() { Id = monthFolder.Id };
                request.Name = weekFolderName;
                weekFolder = Client.FoldersManager.CreateAsync(request).Result;
            }

            var caseFolderName = CaseFolderName;
            caseFolder = Client.FoldersManager.GetFolderItemsAsync(weekFolder.Id, 50).Result.Entries.SingleOrDefault(i => i.Name == caseFolderName) as BoxFolder;
            if (caseFolder == null)
            {
                var request = new BoxFolderRequest();
                request.Parent = new BoxRequestEntity() { Id = weekFolder.Id };
                request.Name = caseFolderName;
                caseFolder = Client.FoldersManager.CreateAsync(request).Result;
            }

            return Client.FoldersManager.GetInformationAsync(caseFolder.Id).Result;
        }

    }
}