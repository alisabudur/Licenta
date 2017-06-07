using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.DAL
{
    public class DbCase: IBaseEntity
    {
        public int Id { get; set; }
        public double PatientAge { get; set; }
        public double ImageMean { get; set; }
        public double ImageMax { get; set; }
        public double ImageMin { get; set; }
        public double ImageStdDev { get; set; }
        public double ImageSkew { get; set; }
        public double ImageKurt { get; set; }
        public string ImagePath { get; set; }
        public double Density { get; set; }
        public double Patology { get; set; }
    }
}
