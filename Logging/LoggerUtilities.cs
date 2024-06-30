using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public static class LoggerUtilities
    {
        public static IDisposable? LogCaller(this ILogger logger,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            return logger.BeginScope("File: {FileName}, Line: {Line}, Name: {Name}", fileName, lineNumber, memberName);
        }
    }
}
