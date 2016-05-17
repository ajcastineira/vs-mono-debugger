using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Debugger.Interop;
using MonoDebugger.VisualStudio.Events;
using MonoDebugger.VisualStudio.VsUtils;

namespace MonoDebugger.VisualStudio
{
    public class MonoPendingBreakpoint : IDebugPendingBreakpoint2
    {
        private MonoEngine engine;
        private IDebugBreakpointRequest2 request;
        private BP_REQUEST_INFO requestInfo; 
        private bool isDeleted;
        private bool isEnabled;
        private readonly List<MonoBoundBreakpoint> boundBreakpoints = new List<MonoBoundBreakpoint>();

        public MonoPendingBreakpoint(MonoEngine engine, IDebugBreakpointRequest2 request)
        {
            this.engine = engine;
            this.request = request;

            var requestInfo = new BP_REQUEST_INFO[1];
            EngineUtils.CheckOk(request.GetRequestInfo(enum_BPREQI_FIELDS.BPREQI_BPLOCATION, requestInfo));
            this.requestInfo = requestInfo[0];
        }

        public int CanBind(out IEnumDebugErrorBreakpoints2 error)
        {
            error = null;
            if (isDeleted || requestInfo.bpLocation.bpLocationType != (uint)enum_BP_LOCATION_TYPE.BPLT_CODE_FILE_LINE)
            {
                return VSConstants.S_FALSE;
            }

            return VSConstants.S_OK;
        }

        public int Bind()
        {
            lock (boundBreakpoints)
            {
                for (uint address = 0; address < 100; address++)
                {
                    MonoBreakpointResolution breakpointResolution = new MonoBreakpointResolution(engine, address, GetDocumentContext(address));
                    MonoBoundBreakpoint boundBreakpoint = new MonoBoundBreakpoint(engine, address, this, breakpointResolution);
                    boundBreakpoints.Add(boundBreakpoint);                    

                    engine.Send(new BreakpointBoundEvent(this, boundBreakpoint), BreakpointBoundEvent.IID, null);
                }

//                m_boundBreakpoints.Add(boundBreakpoint);
//                m_engine.DebuggedProcess.SetBreakpoint(addr, boundBreakpoint);
            }

            return VSConstants.S_OK;
        }

        public MonoDocumentContext GetDocumentContext(uint address)
        {
            var docPosition = (IDebugDocumentPosition2)Marshal.GetObjectForIUnknown(requestInfo.bpLocation.unionmember2);
            string documentName;
            EngineUtils.CheckOk(docPosition.GetFileName(out documentName));

            // Get the location in the document that the breakpoint is in.
            var startPosition = new TEXT_POSITION[1];
            var endPosition = new TEXT_POSITION[1];
            EngineUtils.CheckOk(docPosition.GetRange(startPosition, endPosition));           

            MonoMemoryAddress codeContext = new MonoMemoryAddress(engine, address, null);
            
            return new MonoDocumentContext(documentName, startPosition[0], startPosition[0], codeContext);
        }

        public int GetState(PENDING_BP_STATE_INFO[] state)
        {
            if (isDeleted)
                state[0].state = (enum_PENDING_BP_STATE)enum_BP_STATE.BPS_DELETED;
            else if (isEnabled)
                state[0].state = (enum_PENDING_BP_STATE)enum_BP_STATE.BPS_ENABLED;
            else 
                state[0].state = (enum_PENDING_BP_STATE)enum_BP_STATE.BPS_DISABLED;

            return VSConstants.S_OK;
        }

        public int GetBreakpointRequest(out IDebugBreakpointRequest2 request)
        {
            request = this.request;
            return VSConstants.S_OK;
        }

        public int Virtualize(int fVirtualize)
        {
            return VSConstants.S_OK;
        }

        public int Enable(int enable)
        {
            lock (boundBreakpoints)
            {
                isEnabled = enable != 0;

                foreach (var boundBreakpoint in boundBreakpoints)
                {
                    boundBreakpoint.Enable(enable);
                }                
            }

            return VSConstants.S_OK;
        }

        public int SetCondition(BP_CONDITION bpCondition)
        {
            return VSConstants.E_NOTIMPL;
        }

        public int SetPassCount(BP_PASSCOUNT bpPassCount)
        {
            return VSConstants.E_NOTIMPL;
        }

        public int EnumBoundBreakpoints(out IEnumDebugBoundBreakpoints2 enumerator)
        {
            lock (boundBreakpoints)
            {
                enumerator = new BoundBreakpointsEnumerator(boundBreakpoints.ToArray());
            }
            return VSConstants.S_OK;
        }

        public int EnumErrorBreakpoints(enum_BP_ERROR_TYPE errorType, out IEnumDebugErrorBreakpoints2 enumerator)
        {
            enumerator = null;
            return VSConstants.S_OK;
        }

        public int Delete()
        {
            if (!isDeleted)
            {
                lock (boundBreakpoints)
                {
                    for (var i = boundBreakpoints.Count - 1; i >= 0; i--)
                    {
                        boundBreakpoints[i].Delete();
                    }
                }                
            }
            return VSConstants.S_OK;
        }
    }
}