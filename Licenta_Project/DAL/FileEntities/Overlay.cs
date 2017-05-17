using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.DAL
{
    public class Overlay
    {
        public int TotalAbnormalities { get; set; }
        public IEnumerable<Abnormality> Abnormalities { get; set; }
    }
}
