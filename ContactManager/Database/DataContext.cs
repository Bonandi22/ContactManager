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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define the one-to-many relationship between Person and Contact
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.person)       // Contact has one Person
                .WithMany(p => p.Contacts)   // Person has many Contacts
                .HasForeignKey(c => c.PersonId); // Foreign key property in the Contact entity                
        }
    }
}
