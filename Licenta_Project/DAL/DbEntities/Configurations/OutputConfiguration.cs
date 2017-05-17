using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.DAL
{
    public class OutputConfiguration: BaseEntityConfiguration<Output>
    {
        public OutputConfiguration()
        {
            Property(p => p.Patology).IsRequired();
            Property(p => p.ImagePath).IsRequired().HasMaxLength(255);
        }
    }
}
