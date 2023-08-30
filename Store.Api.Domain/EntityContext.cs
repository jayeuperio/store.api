using Microsoft.EntityFrameworkCore;
using Store.Api.Domain.Data.Entities;

namespace Store.Api.Domain
{
    public class EntityContext: DbContext
    {
        public EntityContext(DbContextOptions<EntityContext> options)
            : base (options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Items>().ToTable("Items").HasKey(x => x.ItemId);
        }
    }
}
