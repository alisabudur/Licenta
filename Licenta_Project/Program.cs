using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licenta_Project.DAL;

namespace Licenta_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            var ddsm = new DDSM();
            ddsm.GetCases();
        }
    }
}