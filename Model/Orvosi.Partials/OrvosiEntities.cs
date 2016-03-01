using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Model
{
    public partial class OrvosiEntities : DbContext

    {
        public OrvosiEntities(string username)
            : this()
        {
            this.username = username;
        }
        /// <summary>
        /// This provides a wrapper around Database.BeginTransaction(). It records the transaction so that SaveChanges can make use of the same transaction
        /// </summary>
        /// <returns>A transaction object</returns>
        public DbContextTransaction BeginTransaction()
        {
            return transaction = this.Database.BeginTransaction();
        }

        private readonly string username;
        private DbContextTransaction transaction = null;

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, System.Collections.Generic.IDictionary<object, object> items)
        {
            var result = new DbEntityValidationResult(entityEntry, new List<DbValidationError>());
            result = base.ValidateEntity(entityEntry, items);
            Validate(result);
            return result;
        }

        public override Task<int> SaveChangesAsync()
        {
            //If username is null, then just let the default SUSER_NAME be recorded
            if (username == null)
                return base.SaveChangesAsync();

            // set the modified user for all pending changes
            foreach (var item in this.ChangeTracker.Entries())
            {
                item.Property("ModifiedUser").CurrentValue = username;
                item.Property("ModifiedDate").CurrentValue = DateTime.UtcNow;
            }

            //otherwise set the context info to the username
            this.Database.ExecuteSqlCommand("DECLARE @BinVar varbinary(128); SET @BinVar = CAST({0} AS varbinary(128) ); SET CONTEXT_INFO @BinVar;", username);

            // base.SaveChanges is the only place where entity framework raises relationship constraint errors.
            // After this, the only errors raised are directly from the physical store (SQL Server).
            int affectedRecordCount = 0;
            try
            {
                affectedRecordCount = base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (DbUpdateException e)
            {
                var be = e.GetBaseException() as SqlException;
                if (be != null)
                {
                    List<DbEntityValidationResult> results = new List<DbEntityValidationResult>(); ;

                    foreach (var item in be.Errors.OfType<SqlError>())
                    {
                        var errors = new List<DbValidationError>();
                        DbEntityValidationResult result = null;
                        // When the provider is SQL Server these are the error codes it will raise
                        switch (be.Number)
                        {
                            case 2601:
                                // Need a way to parse the message and pull this information
                                errors.Add(new DbValidationError("Email", "Email already exists"));
                                result = new DbEntityValidationResult(e.Entries.First(), errors);
                                results.Add(result);
                                break;
                            case 547:
                                errors.Add(new DbValidationError("StateID", "StateID is not set"));
                                result = new DbEntityValidationResult(e.Entries.First(), errors);
                                results.Add(result);
                                break;
                            default:
                                break;
                        }
                    }
                    if (be.Errors.OfType<SqlError>().Count() > 0)
                    {
                        throw new DbEntityValidationException("Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.", results, e);
                    }
                }
                throw;
            }
            return Task.FromResult(affectedRecordCount);
        }

        private static void Validate(DbEntityValidationResult result)
        {
            //TODO: Add in calls to specific validation methods
        }
    }

    public class PhysicianLicenseMeta
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]  // format used by Html.EditorFor
        public DateTime ExpiryDate;
    }

    [MetadataType(typeof(PhysicianLicenseMeta))]
    public partial class PhysicianLicense
    {
    }

    public class CityMeta
    {
        [Display(Name = "Province")]  // format used by Html.EditorFor
        public short ProvinceId;

        [Display(AutoGenerateField = false)]
        public DateTime ModifiedDate;

        [Display(AutoGenerateField = false)]
        public string ModifiedUser;
    }

    [MetadataType(typeof(CityMeta))]
    public partial class City
    {
    }
}
