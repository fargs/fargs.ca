using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Model
{
    public partial class OrvosiEntities

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

        public override int SaveChanges()
        {
            //If username is null, then just let the default SUSER_NAME be recorded
            if (username == null)
                return base.SaveChanges();

            // set the modified user for all pending changes
            foreach (var item in this.ChangeTracker.Entries())
            {
                item.Property("ModifiedUser").CurrentValue = username;
            }

            //otherwise set the context info to the username
            this.Database.ExecuteSqlCommand("DECLARE @BinVar varbinary(128); SET @BinVar = CAST({0} AS varbinary(128) ); SET CONTEXT_INFO @BinVar;", username);

            return base.SaveChanges();
        }
    }
}
