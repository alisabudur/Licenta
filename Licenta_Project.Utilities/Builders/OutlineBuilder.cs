using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licenta_Project.Common;

namespace Licenta_Project.Utility
{
    public class OutlineBuilder
    {
        public Outline Outline { get; }

        public OutlineBuilder()
        {
            Outline = new Outline();
        }

        public void BuildOutline(string boundaryLine)
        {
            var values = boundaryLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)?
                .ToArray();

            if(values.Length < 4)
                return;

            var x = values[0].ToInt();
            var y = values[1].ToInt();
            var chain = values.Skip(2).Take(values.Length - 3).Select(i => i.ToInt()).ToArray();

            Outline.StartX = x;
            Outline.StartY = y;
            Outline.Chain = chain;
        }
    }
}
