using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;

namespace Licenta_Project.Common
{
    public class BlobFilter : IBlobsFilter
    {
        public bool Check(Blob blob)
        {
            return (blob.Area >= 80);
        }
    }
}
