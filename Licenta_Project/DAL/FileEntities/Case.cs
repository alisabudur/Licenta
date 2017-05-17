using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.DAL
{
    public class Case
    {
        public IDictionary<ImageName, CaseImage> Images { get; set; }
        public int PatientAge { get; set; }

        public int Density { get; set; }
    }
}
