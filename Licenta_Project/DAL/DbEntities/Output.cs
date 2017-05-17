using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.DAL
{
    public class Output: IBaseEntity
    {
        public int Id { get; set; }
        public double Patology { get; set; }
        public string ImagePath { get; set; }
    }
}
