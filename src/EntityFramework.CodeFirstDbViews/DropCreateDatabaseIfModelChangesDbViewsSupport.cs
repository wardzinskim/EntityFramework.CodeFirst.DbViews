using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.CodeFirst.DbViews
{
    public class DropCreateDatabaseIfModelChangesDbViewsSupport<TContext> : DropCreateDatabaseIfModelChanges<TContext> where TContext : DbContext
    {
        private readonly string _pathToDirectoryWithSqlScript;

        public DropCreateDatabaseIfModelChangesDbViewsSupport(string pathToDirectoryWithSqlScript)
        {
            _pathToDirectoryWithSqlScript = pathToDirectoryWithSqlScript;
        }


        public override void InitializeDatabase(TContext context)
        {
            var dbCompatibleWithModel = context.Database.Exists() && context.Database.CompatibleWithModel(false);

            base.InitializeDatabase(context);

            if (!dbCompatibleWithModel)
            {
                var dbViewNames = DbViewsInitializationHelper.DeleteTables(context);
                DbViewsInitializationHelper.CreateViews(context.Database, _pathToDirectoryWithSqlScript);
                DbViewsInitializationHelper.ValidateIfAllViewsHaveBeenCreated(context.Database, dbViewNames);
            }
        }
    }
}
