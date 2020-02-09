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
        public override int SaveChanges()
        {
            AddOrUpdateTimestamps();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddOrUpdateTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddOrUpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AddOrUpdateTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddOrUpdateTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));

            //long currentUserId = -1;    //-1 for Anonimous

            foreach (var entity in entities)
            {
                //Should store location also from here- http://www.jerriepelser.com/blog/aspnetcore-geo-location-from-ip-address/

                if (entity.State == EntityState.Added)
                {
                    ((BaseModel)entity.Entity).CreateTime = DateTime.UtcNow;
                    //((BaseModel)entity.Entity).CreateTime = currentUserId;
                    //((BaseEntity)entity.Entity).CreatorIPAddress = _httpContext.Connection.RemoteIpAddress.ToString();
                }
                else
                {
                    this.Entry(((BaseModel)entity.Entity)).Property(e => e.CreateTime).IsModified = false;
                    //this.Entry(((BaseModel)entity.Entity)).Property(e => e.CreatorUserId).IsModified = false;
                    //this.Entry(((BaseEntity)entity.Entity)).Property(e => e.CreatorUserId).CreatorIPAddress = false;

                    //Set updated At time
                    ((BaseModel)entity.Entity).LastUpdateTime = DateTime.UtcNow;
                    //((BaseModel)entity.Entity).LastModifireUserId = currentUserId;
                    //((BaseEntity)entity.Entity).LastModifireIPAddress = _httpContext.Connection.RemoteIpAddress.ToString();
                }
            }
        }
*/
    }
}
