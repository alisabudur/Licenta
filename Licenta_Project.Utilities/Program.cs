using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licenta_Project.Utility;

namespace Licenta_Project.Utilities
{
    class Program
    {
        static void Main(string[] args)
        {
            var ddsmUtility = new DdsmDbUtility();
            var fileUtility = new DdsmFileUtility();
            fileUtility.LoadCasesFromFiles();
            var cases = fileUtility.Cases;
            //ddsmUtility.PutCasesInDb();
        }
    }
}
