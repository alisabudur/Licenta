using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.FileUtility
{
    public class Outline
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public IEnumerable<int> Chain { get; set; }
    }
}
