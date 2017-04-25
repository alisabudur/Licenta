using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licenta_Project.DAL;
using Licenta_Project.Services;

namespace Licenta_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            var ann = new AnnService();
            ann.Ann();
            //var ddsm = new DDSM();
            //ddsm.GetCases();
            //ddsm.PutInputInDb();
            //ddsm.PutOutputInDb();
        }
    }
}