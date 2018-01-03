using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFViews.Db.Model;
using EFViews.Db.Views;
using EntityFramework.CodeFirst.DbViews;

namespace EFViews.Db
{
    public class ViewsContext : DbContext
    {
        public ViewsContext() : base("EFViews")
        {
            Database.SetInitializer(new DropCreateDatabaseAlwaysDbViewsSupport<ViewsContext>(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Db", "Views")
            ));
        }


        public DbSet<Person> People { get; set; }
        public DbSet<Address> Addresses { get; set; }



        public IQueryable<PeopleView> PeopleDbView => PeopleViews;
        public IQueryable<NameView> NameDbViews => NameViews;

        private DbSet<PeopleView> PeopleViews { get; set; }
        private DbSet<NameView> NameViews { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
