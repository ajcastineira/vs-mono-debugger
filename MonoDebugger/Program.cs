using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mono.Debugger.Soft;
using Mono.Debugging.Client;
using Mono.Debugging.Soft;

namespace MonoDebugger
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Run().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task Run()
        { 
            var completionSource = new TaskCompletionSource<object>();
            var session = new SoftDebuggerSession
            {
                
            };
            DebuggerLoggingService.CustomLogger = new CustomLogger();
            session.ExceptionHandler = exception =>
            {
                Console.WriteLine(exception);
                return true;
            };
            session.TargetReady += (sender, eventArgs) => completionSource.SetResult(null);
            session.TargetExceptionThrown += (sender, args) => Console.WriteLine(args.Type);
            session.TargetExited += (sender, args) => Console.WriteLine(args.Type);
            session.TargetSignaled += (sender, args) => Console.WriteLine(args.Type);
            session.TargetUnhandledException += (sender, args) => Console.WriteLine(args.Type);
            session.LogWriter = (stderr, text) => Console.WriteLine(text);

            session.OutputWriter = (stderr, text) =>
            {
                if (stderr)
                    Console.Error.Write(text);
                else
                    Console.Out.Write(text);
            };
            session.TargetEvent += (sender, args) => Console.WriteLine(args.Type);
            session.TargetHitBreakpoint += (sender, args) => Console.WriteLine(args.Type);
            session.TargetStarted += (sender, args) => Console.WriteLine("Target Started");
            session.TargetStopped += (sender, args) => Console.WriteLine(args.Type);
            session.TargetInterrupted += (sender, args) => Console.WriteLine(args.Type);
            session.TargetThreadStarted += (sender, args) => Console.WriteLine(args.Type);
            session.TargetThreadStopped += (sender, args) => Console.WriteLine(args.Type);
            session.CustomBreakEventHitHandler = (id, be) => true;
            session.BreakpointTraceHandler = (be, trace) => Console.WriteLine(be);
            var breakpoint = session.Breakpoints.Add(@"/home/kirk/TestDebug/Program.cs", 12);

            session.Run(new SoftDebuggerStartInfo(new SoftDebuggerConnectArgs("", new IPAddress(new byte[] { 192, 168, 1, 2 }), 12345)), new DebuggerSessionOptions { ProjectAssembliesOnly = false });
//            session.Run(new SoftDebuggerStartInfo(new SoftDebuggerConnectArgs("", new IPAddress(new byte[] { 192, 168, 137, 3 }), 12345)), new DebuggerSessionOptions {  });


            await completionSource.Task;
            Console.WriteLine("Ready");

//            var type = session.GetType("TestDebug.Program");
//            var method = type.GetMethod("Main");
//            session.VirtualMachine.SetBreakpoint(method, 0);

//            session.StepLine();

//            var thread = session.ActiveThread;
//            var backtrace = thread.Backtrace;
//            var frameCount = backtrace.FrameCount;

//            session.Continue();
            while (true)
            {
/*
                session.Stop();
                var threadMirrors = session.VirtualMachine.GetThreads();
                var stackFrames = threadMirrors[0].GetFrames();
                var frame = stackFrames[0];
                session.Continue();
*/
//                Thread.Sleep(1000);
                await Task.Delay(1000);

                var status = breakpoint.GetStatus(session);

                if (!session.IsRunning)
                {
                    var activeThread = session.ActiveThread;
                    var activeBacktrace = activeThread?.Backtrace;
                    var activeFrame = activeBacktrace?.GetFrame(0);                    
                    session.Continue();
                }
            }
            Console.ReadLine();
        }

        private class CustomLogger : ICustomLogger
        {
            public void LogError(string message, Exception ex)
            {
            }

            public void LogAndShowException(string message, Exception ex)
            {
            }

            public void LogMessage(string messageFormat, params object[] args)
            {
            }

            public string GetNewDebuggerLogFilename()
            {
                return @"c:\temp\debugger.log";
            }
        }
    }
}
