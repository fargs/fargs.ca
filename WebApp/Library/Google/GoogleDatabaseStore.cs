using Google.Apis.Util.Store;
using ImeHub.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using WebApp.Library.Extensions;

namespace WebApp.Library
{
    public class GoogleDatabaseStore : IDataStore
    {
        private IImeHubDbContext db;
        private Guid userId;
        private const string providerName = "google";
        public GoogleDatabaseStore(IImeHubDbContext db, Guid userId)
        {
            this.db = db;
            this.userId = userId;
        }
        public Task ClearAsync()
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key MUST have a value");
            }

            var generatedKey = GenerateStoredKey(key, typeof(T));
            var item = db.UserClaims.FirstOrDefault(x => x.ClaimType == generatedKey);
            if (item != null)
            {
                db.UserClaims.Remove(item);
                await db.SaveChangesAsync();
            }
        }

        public Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key MUST have a value");
            }

            var generatedKey = GenerateStoredKey(key, typeof(T));
            var item = db.UserClaims.FirstOrDefault(x => x.ClaimType == generatedKey);
            T value = item == null ? default(T) : JsonConvert.DeserializeObject<T>(item.ClaimValue);
            return Task.FromResult<T>(value);
        }

        public async Task StoreAsync<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key MUST have a value");
            }

            var generatedKey = GenerateStoredKey(key, typeof(T));
            string json = JsonConvert.SerializeObject(value);

            var item = await db.UserClaims.SingleOrDefaultAsync(x => x.ClaimType == generatedKey);

            if (item == null)
            {
                db.UserClaims.Add(new UserClaim { UserId = userId, ClaimType = generatedKey, ClaimValue = json });
            }
            else
            {
                item.ClaimValue = json;
            }

            await db.SaveChangesAsync();
        }

        private static string GenerateStoredKey(string key, Type t)
        {
            return string.Format("{0}", t.FullName);
        }
    }
}