using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.CodeFirst.DbViews
{
    public class CreateDatabaseIfNotExistsDbViewsSupport<TContext> : CreateDatabaseIfNotExists<TContext> where TContext : DbContext
    {
        private readonly string _pathToDirectoryWithSqlScript;

        public CreateDatabaseIfNotExistsDbViewsSupport(string pathToDirectoryWithSqlScript)
        {
            _pathToDirectoryWithSqlScript = pathToDirectoryWithSqlScript;
        }

        public override void InitializeDatabase(TContext context)
        {
            var dbExists = context.Database.Exists();
            
            base.InitializeDatabase(context);

            if (!dbExists)
            {
                var dbViewNames = DbViewsInitializationHelper.DeleteTables(context);
                DbViewsInitializationHelper.CreateViews(context.Database, _pathToDirectoryWithSqlScript);
                DbViewsInitializationHelper.ValidateIfAllViewsHaveBeenCreated(context.Database, dbViewNames);
            }
        }
    }
}
