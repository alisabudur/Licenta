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
        public string LeftCCPath { get; set; }
        public string LeftMLOPath { get; set; }
        public string RightCCPath { get; set; }
        public string RightMLOPath { get; set; }
        public int PatientAge { get; set; }
        public IEnumerable<Overlay> Overlays { get; set; }
    }
}
