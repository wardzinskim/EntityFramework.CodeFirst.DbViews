using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.MappingViews;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFViews.Db;
using EFViews.Db.Model;
using EFViews.Db.Views;

namespace EFViews
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new ViewsContext())
            {
                context.People.Add(new Person()
                {
                    Address = new Address
                    {
                        City = "Warsaw",
                        Street = "Dubois"
                    },
                    Name = "John",
                    Surname = "Kowalski"
                });
                context.SaveChanges();


                var people = context.Set<PeopleView>().AsQueryable().ToList();

                foreach (var view in people)
                {
                    Console.WriteLine($"{view.PersonId} {view.Name} {view.Surname} {view.City} {view.Street}");
                }
            }

        }
    }
}
