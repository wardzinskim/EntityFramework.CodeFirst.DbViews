using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.CodeFirst.DbViews
{
    public class MigrateDatabaseToLatestVersionDbViewsSupport<TContext, TMigrationsConfiguration>
        : MigrateDatabaseToLatestVersion<TContext, TMigrationsConfiguration> where TMigrationsConfiguration : DbMigrationsConfiguration<TContext>, new() where TContext : DbContext
    {
        private readonly string _pathToDirectoryWithSqlScript;

        public MigrateDatabaseToLatestVersionDbViewsSupport(string pathToDirectoryWithSqlScript)
        {
            _pathToDirectoryWithSqlScript = pathToDirectoryWithSqlScript;
        }

        public MigrateDatabaseToLatestVersionDbViewsSupport(string connectionStringName,
            string pathToDirectoryWithSqlScript) : base(connectionStringName)
        {
            _pathToDirectoryWithSqlScript = pathToDirectoryWithSqlScript;
        }

        public MigrateDatabaseToLatestVersionDbViewsSupport(bool useSuppliedContext,
            string pathToDirectoryWithSqlScript) : base(useSuppliedContext)
        {
            _pathToDirectoryWithSqlScript = pathToDirectoryWithSqlScript;
        }

        public MigrateDatabaseToLatestVersionDbViewsSupport(bool useSuppliedContext, TMigrationsConfiguration configuration,
            string pathToDirectoryWithSqlScript) : base(useSuppliedContext, configuration)
        {
            _pathToDirectoryWithSqlScript = pathToDirectoryWithSqlScript;
        }

        public override void InitializeDatabase(TContext context)
        {
            var dbExists = context.Database.Exists();
            var dbCompatibleWithModel = dbExists && context.Database.CompatibleWithModel(false);
            if (dbExists && !dbCompatibleWithModel)
            {
                DbViewsInitializationHelper.DropAllDbViews(context.Database);
            }

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
