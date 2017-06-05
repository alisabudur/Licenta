using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.DAL
{
    public class DdsmContext : DbContext
    {
        public DdsmContext() : base("name=DDSMEntities")
        {
            
        }

        public virtual DbSet<DbCase> DbCases { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DbCaseConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
