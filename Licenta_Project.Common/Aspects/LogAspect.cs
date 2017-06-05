using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Extensibility;

namespace Licenta_Project.Common
{
    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Method)]
    public class LogAspect : OnMethodBoundaryAspect
    {
        [NonSerialized]
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public LogAspect()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        [OnMethodEntryAdvice, SelfPointcut]
        public override void OnEntry(MethodExecutionArgs args)
        {
            _logger.Info($"{args.Method.DeclaringType}.{args.Method.Name} started.");
            base.OnEntry(args);
        }

        [OnMethodExitAdvice, SelfPointcut]
        public override void OnExit(MethodExecutionArgs args)
        {
            _logger.Info($"{args.Method.DeclaringType}.{args.Method.Name} ended.");
            base.OnExit(args);
        }

        [OnMethodExceptionAdvice, SelfPointcut]
        public override void OnException(MethodExecutionArgs args)
        {
            _logger.Error(args.Exception, $"{args.Method.DeclaringType}.{args.Method.Name} failed.");
            base.OnException(args);
        }
    }
}
