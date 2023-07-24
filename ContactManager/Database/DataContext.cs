using ContactManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DataContext() : base()
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        
    }
}
