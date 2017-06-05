using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.Common
{
    public static class StringExtensions
    {
        public static int ToInt(this string str)
        {
            int x = 0;

            if (int.TryParse(str, out x))
            {
                return x;
            }
            return x;
        }
    }
}
