# EntityFramework.CodeFirst.DbViews

## What is `EntityFramework.CodeFirst.DbViews`?

`EntityFramework.CodeFirst.DbViews` is a small library that supports database views to `Entity Framework` in code first approach with migrations supports.

## How to use `EntityFramework.CodeFirst.DbViews`?

* Create POCO class with `DbViewAttribute`:

```csharp
[DbView]
public class PeopleView
{
    [Key]
    public int PersonId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
}
```

* Add Entity to DbContext:
```csharp
public class ViewsContext : DbContext
{
    private DbSet<PeopleView> PeopleViews { get; set; }
    public IQueryable<PeopleView> PeopleDbView => PeopleViews;
}
```

* Write sql script that creates appropriate db view:
```sql
CREATE VIEW PeopleViews AS
SELECT p.Id AS 'PersonId', p.Name, p.Surname, a.City, a.Street
FROM People p
LEFT JOIN Addresses a ON a.Id = p.AddressId
```

* Configure initializer in db context. first parameter in Initializer is path to location where sql scripts are located:
```csharp
public class ViewsContext : DbContext
{
    public ViewsContext() : base("EFViews")
    {
        Database.SetInitializer(new DropCreateDatabaseAlwaysDbViewsSupport<ViewsContext>(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Db", "Views")
        ));
    }

    ...
```
---

Library supports all default Initializers:

* `DropCreateDatabaseAlwaysDbViewsSupport`
* `CreateDatabaseIfNotExistsDbViewsSupport`
* `DropCreateDatabaseIfModelChangesDbViewsSupport`
* `MigrateDatabaseToLatestVersionDbViewsSupport`

When you generates automatic migrations remember to remove from them all lines related to dbviews. Initializer before migration remove all sql views. 
After migrations all views are recreated, thanks that sql views are also updates. 