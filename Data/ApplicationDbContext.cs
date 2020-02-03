using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XmlSigner.Data.Models;

namespace XmlSigner.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser<long>, IdentityRole<long>, long>
    {
        //Table List - Auth tables are added by default
        public DbSet<XmlFile> XmlFiles { get; set; }
        //Table List End
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //https://github.com/armancse100/ASP.NetCore-MySQL-Login-CRUD/blob/master/InventoryManagement/InventoryManagementDbContext.cs#L51
        }
        */
    }
}
