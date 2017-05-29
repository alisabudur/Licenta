using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Neuro;

namespace Licenta_Project.Services
{
    public class SoftmaxActivationFunction: IActivationFunction
    {
        public IEnumerable<double> Inputs { get; set; }

        public SoftmaxActivationFunction()
        {
        }

        public double Function(double x)
        {
            var value1 = Math.Exp(x);
            var value2 = Inputs.Sum(i => Math.Exp(i));
            var value = value1 / value2;
            return value;
        }

        public double Derivative(double x)
        {
            var value1 = x * Math.Exp(x);
            var value2 = Inputs.Sum(i => i * Math.Exp(i));
            var value = value1 / value2;
            return value;
        }

        public double Derivative2(double y)
        {
            var value1 = Math.Pow(y, 2) * Math.Exp(y);
            var value2 = Inputs.Sum(i => Math.Pow(i, 2) * Math.Exp(i));
            var value = value1 / value2;
            return value;
        }
    }
}
