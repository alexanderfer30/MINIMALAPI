using Microsoft.EntityFrameworkCore;
using CRM.API.Models.EN;

namespace CRM.API.Models.DAL
{
    public class CRMContext : DbContext
    {
        public CRMContext(DbContextOptions<CRMContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
    }
}
