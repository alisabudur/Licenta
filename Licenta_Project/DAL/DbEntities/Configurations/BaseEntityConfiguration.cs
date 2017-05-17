using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.DAL
{
    public class BaseEntityConfiguration<T> : EntityTypeConfiguration<T> where T: class, IBaseEntity
    {
        public BaseEntityConfiguration()
        {
            HasKey(p => p.Id);
        }
    }
}
