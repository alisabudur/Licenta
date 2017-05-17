using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.DAL
{
    public interface IDdsmFileRepository
    {
        void LoadCasesFromFiles();
        IEnumerable<Case> Cases { get; }
    }
}
