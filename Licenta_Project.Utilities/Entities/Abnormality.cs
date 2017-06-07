using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Math.Geometry;

namespace Licenta_Project.Utility
{
    public class Abnormality
    {
        public string LessionType { get; set; }
        public string Shape { get; set; }
        public string  Margins { get; set; }
        public int Assesment { get; set; }
        public int Subtlety { get; set; }
        public Patology Patology { get; set; }
        public int TotalOutlines { get; set; }
        public Outline Boundary { get; set; }
        public IEnumerable<Outline> Cores { get; set; }
    }
}
