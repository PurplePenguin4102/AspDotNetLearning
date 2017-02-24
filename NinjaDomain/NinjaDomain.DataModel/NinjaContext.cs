using System.Data.Entity;
using NinjaDomain.Classes;
using NinjaDomain.Classes.Interfaces;
using System;
using System.Linq;

namespace NinjaDomain.DataModel
{
    public class NinjaContext : DbContext
    {
        public DbSet<Ninja> Ninjas { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<NinjaEquipment> Equipment { get; set; }

        // we can use this method to tap into the model creation process and
        // hook in our own configuration
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // we tell the model to ignore a field
            modelBuilder.Types().Configure(c => c.Ignore("IsDirty"));
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var history in this.ChangeTracker.Entries()
                .Where(entity => entity.Entity is IModificationHistory && (entity.State == EntityState.Added || entity.State == EntityState.Modified))
                .Select(e => e.Entity as IModificationHistory))
            {
                history.DateModified = DateTime.Now;
                if (history.DateCreated == DateTime.MinValue)
                {
                    history.DateCreated = DateTime.Now;
                }
                
            }
            int result =  base.SaveChanges();
            foreach (var history in this.ChangeTracker.Entries()
                .Where(e => e.Entity is IModificationHistory)
                .Select(e => e.Entity as IModificationHistory))
            {
                history.IsDirty = false;
            }
            return result;
        }
    }
}
