using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.Utility
{
    public enum Patology
    {
        Benign = 1,
        Malignant = 2,
        Normal = 3,
        Undefined = 4,
    }

    public enum LessionType
    {
        Mass = 10,
        Calcification = 20,
        Undefined = 30,
    }

    public enum ImageName
    {
        LeftCC = 1,
        LeftMLO = 2,
        RightCC = 3,
        RightMLO = 4,
    }
}
