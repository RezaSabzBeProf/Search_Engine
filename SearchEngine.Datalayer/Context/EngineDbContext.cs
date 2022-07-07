using Microsoft.EntityFrameworkCore;
using SearchEngine.Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Datalayer.Context
{
    public class EngineDbContext : DbContext
    {
        public EngineDbContext(DbContextOptions<EngineDbContext> options) : base(options)
        {

        }


        #region Tabels
        public DbSet<Page> Pages { get; set; }


        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }

    }
}
