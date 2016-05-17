using System;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Debugger.Interop;
using Mono.Debugging.Client;
using Mono.Debugging.Soft;
using MonoDebugger.VisualStudio.Events;
using MonoDebugger.VisualStudio.VsUtils;

namespace MonoDebugger.VisualStudio
{
    [Guid("D78CF801-CE2A-499B-BF1F-C81742877A34")]
    public class MonoEngine : IDebugEngine2, IDebugProgram3, IDebugEngineLaunch2, IDebugSymbolSettings100
    {
        private string registryRoot;
        private ushort locale;
        private IDebugEventCallback2 callback;
        private Guid programId;
        private AD_PROCESS_ID processId;
        private SoftDebuggerSession session;

        public int EnumPrograms(out IEnumDebugPrograms2 ppEnum)
        {
            throw new NotImplementedException();
        }

        public int CreatePendingBreakpoint(IDebugBreakpointRequest2 request, out IDebugPendingBreakpoint2 pendingBreakpoint)
        {
            pendingBreakpoint = new MonoPendingBreakpoint(this, request);

            return VSConstants.S_OK;
        }

        public int SetException(EXCEPTION_INFO[] pException)
        {
            throw new NotImplementedException();
        }

        public int RemoveSetException(EXCEPTION_INFO[] pException)
        {
            throw new NotImplementedException();
        }

        public int RemoveAllSetExceptions(ref Guid guidType)
        {
            throw new NotImplementedException();
        }

        public int GetEngineId(out Guid engineGuid)
        {
            engineGuid = new Guid(Guids.EngineId);
            return VSConstants.S_OK;
        }

        public int DestroyProgram(IDebugProgram2 pProgram)
        {
            throw new NotImplementedException();
        }

        public int ContinueFromSynchronousEvent(IDebugEvent2 @event)
        {
            if (@event is ProgramDestroyEvent)
            {
                session.Detach();
                session.Dispose();
            }
            return VSConstants.S_OK;
        }

        public int SetLocale(ushort languageId)
        {
            this.locale = languageId;
            return VSConstants.S_OK;
        }

        public int SetRegistryRoot(string registryRoot)
        {
            this.registryRoot = registryRoot;
            return VSConstants.S_OK;
        }

        public int SetMetric(string metric, object value)
        {
            return VSConstants.S_OK;
        }

        public int CauseBreak()
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.EnumThreads(out IEnumDebugThreads2 ppEnum)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.GetName(out string programName)
        {
            programName = null;
            return VSConstants.S_OK;
        }

        int IDebugProgram2.GetProcess(out IDebugProcess2 ppProcess)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.Terminate()
        {
            return VSConstants.S_OK;
        }

        int IDebugProgram2.Attach(IDebugEventCallback2 pCallback)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.CanDetach()
        {
            return VSConstants.S_OK;
        }

        int IDebugProgram2.Detach()
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.GetProgramId(out Guid programId)
        {
            programId = this.programId;
            return VSConstants.S_OK;
        }

        int IDebugProgram2.GetDebugProperty(out IDebugProperty2 ppProperty)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.Execute()
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.Continue(IDebugThread2 pThread)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.Step(IDebugThread2 pThread, enum_STEPKIND sk, enum_STEPUNIT Step)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.GetEngineInfo(out string pbstrEngine, out Guid pguidEngine)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.EnumCodeContexts(IDebugDocumentPosition2 pDocPos, out IEnumDebugCodeContexts2 ppEnum)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.GetMemoryBytes(out IDebugMemoryBytes2 ppMemoryBytes)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.GetDisassemblyStream(enum_DISASSEMBLY_STREAM_SCOPE dwScope, IDebugCodeContext2 pCodeContext, out IDebugDisassemblyStream2 ppDisassemblyStream)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.EnumModules(out IEnumDebugModules2 ppEnum)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.GetENCUpdate(out object ppUpdate)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.EnumCodePaths(string pszHint, IDebugCodeContext2 pStart, IDebugStackFrame2 pFrame, int fSource, out IEnumCodePaths2 ppEnum, out IDebugCodeContext2 ppSafety)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram2.WriteDump(enum_DUMPTYPE DUMPTYPE, string pszDumpUrl)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.EnumThreads(out IEnumDebugThreads2 ppEnum)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.GetName(out string pbstrName)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.GetProcess(out IDebugProcess2 ppProcess)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.Terminate()
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.Attach(IDebugEventCallback2 pCallback)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.CanDetach()
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.Detach()
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.GetProgramId(out Guid pguidProgramId)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.GetDebugProperty(out IDebugProperty2 ppProperty)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.Execute()
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.Continue(IDebugThread2 pThread)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.Step(IDebugThread2 pThread, enum_STEPKIND sk, enum_STEPUNIT Step)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.GetEngineInfo(out string pbstrEngine, out Guid pguidEngine)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.EnumCodeContexts(IDebugDocumentPosition2 pDocPos, out IEnumDebugCodeContexts2 ppEnum)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.GetMemoryBytes(out IDebugMemoryBytes2 ppMemoryBytes)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.GetDisassemblyStream(enum_DISASSEMBLY_STREAM_SCOPE dwScope, IDebugCodeContext2 pCodeContext, out IDebugDisassemblyStream2 ppDisassemblyStream)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.EnumModules(out IEnumDebugModules2 ppEnum)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.GetENCUpdate(out object ppUpdate)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.EnumCodePaths(string pszHint, IDebugCodeContext2 pStart, IDebugStackFrame2 pFrame, int fSource, out IEnumCodePaths2 ppEnum, out IDebugCodeContext2 ppSafety)
        {
            throw new NotImplementedException();
        }

        int IDebugProgram3.WriteDump(enum_DUMPTYPE DUMPTYPE, string pszDumpUrl)
        {
            throw new NotImplementedException();
        }

        public int ExecuteOnThread(IDebugThread2 pThread)
        {
            throw new NotImplementedException();
        }

        public int LaunchSuspended(string server, IDebugPort2 port, string exe, string args, string directory, string environment, string options, enum_LAUNCH_FLAGS launchFlags, uint standardInput, uint standardOutput, uint standardError, IDebugEventCallback2 callback, out IDebugProcess2 process)
        {
            processId = new AD_PROCESS_ID();
            processId.ProcessIdType = (uint)enum_AD_PROCESS_ID.AD_PROCESS_ID_GUID;
            processId.guidProcessId = Guid.NewGuid();

            EngineUtils.CheckOk(port.GetProcess(processId, out process));
            this.callback = callback;

            session = new SoftDebuggerSession();
            session.TargetReady += (sender, eventArgs) =>
            {
                MonoEngineCreateEvent.Send(this);
                MonoProgramCreateEvent.Send(this);                
            };
            session.ExceptionHandler = exception => true;
            session.TargetExceptionThrown += (sender, x) => Console.WriteLine(x.Type);
            session.TargetExited += (sender, x) => Console.WriteLine(x.Type);
            session.TargetUnhandledException += (sender, x) => Console.WriteLine(x.Type);
            session.LogWriter = (stderr, text) => Console.WriteLine(text);
            session.OutputWriter = (stderr, text) => Console.WriteLine(text);
            session.TargetHitBreakpoint += (sender, x) => Console.WriteLine(x.Type);

            return VSConstants.S_OK;
        }

        public int ResumeProcess(IDebugProcess2 process)
        {
            IDebugPort2 port;
            EngineUtils.RequireOk(process.GetPort(out port));
            
            IDebugDefaultPort2 defaultPort = (IDebugDefaultPort2)port;
            IDebugPortNotify2 portNotify;
            EngineUtils.RequireOk(defaultPort.GetPortNotify(out portNotify));

            EngineUtils.RequireOk(portNotify.AddProgramNode(new MonoProgramNode(processId)));

            return VSConstants.S_OK;
        }

        public int Attach(IDebugProgram2[] programs, IDebugProgramNode2[] rgpProgramNodes, uint celtPrograms, IDebugEventCallback2 pCallback, enum_ATTACH_REASON dwReason)
        {
            var program = programs[0];
            IDebugProcess2 process;
            program.GetProcess(out process);
            Guid processId;
            process.GetProcessId(out processId);
            if (processId != this.processId.guidProcessId)
                return VSConstants.S_FALSE;

            EngineUtils.RequireOk(program.GetProgramId(out programId));

            session.Run(new SoftDebuggerStartInfo(new SoftDebuggerConnectArgs("", new IPAddress(new byte[] { 192, 168, 137, 3 }), 12345)), new DebuggerSessionOptions { ProjectAssembliesOnly = false });

            return VSConstants.S_OK;
        }

        public int CanTerminateProcess(IDebugProcess2 pProcess)
        {
            return VSConstants.S_OK;
        }

        public int TerminateProcess(IDebugProcess2 pProcess)
        {
            pProcess.Terminate();
            Send(new ProgramDestroyEvent(0), ProgramDestroyEvent.IID, null);
            return VSConstants.S_OK;
        }

        public int SetSymbolLoadState(int isManual, int loadAdjacentSymbols, string includeList, string excludeList)
        {
            return VSConstants.S_OK;
        }

        public void Send(IDebugEvent2 eventObject, string iidEvent, IDebugProgram2 program, IDebugThread2 thread)
        {
            uint attributes; 
            Guid riidEvent = new Guid(iidEvent);
            eventObject.GetAttributes(out attributes);
            callback.Event(this, null, program, thread, eventObject, ref riidEvent, attributes);
        }

        public void Send(IDebugEvent2 eventObject, string iidEvent, IDebugThread2 thread)
        {
            Send(eventObject, iidEvent, this, thread);
        }

    }
}