using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.DAL
{
    public class Abnormality
    {
        public LessionType LessionType { get; set; }
        public int Assesment { get; set; }
        public int Subtlety { get; set; }
        public Patology Patology { get; set; }
        public int TotalOutlines { get; set; }
    }
}
