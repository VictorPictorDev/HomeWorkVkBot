using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;

namespace Bot.Bl
{
    public interface IErrorReporter
    {
        void ReportError(Exception ex);
    }
    public interface IInfoReporter
    {
        void ReportInfo(string Text,long UserId);
    }
    public class ErrorReporter : IErrorReporter
    {
        public delegate void ReportMethod(Exception ex);
        public ReportMethod Reporter { get; private set; }
        public ErrorReporter(ReportMethod reporter)
        {
            Reporter = reporter;
        }
        public void ReportError(Exception ex)
        {
            Reporter(ex);
        }
    }
}
