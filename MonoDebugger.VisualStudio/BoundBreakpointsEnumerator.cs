using Microsoft.VisualStudio.Debugger.Interop;

namespace MonoDebugger.VisualStudio
{
    public class BoundBreakpointsEnumerator : Enumerator<IDebugBoundBreakpoint2, IEnumDebugBoundBreakpoints2>, IEnumDebugBoundBreakpoints2
    {
        public BoundBreakpointsEnumerator(IDebugBoundBreakpoint2[] data) : base(data)
        {
        }

        public int Next(uint celt, IDebugBoundBreakpoint2[] rgelt, ref uint fetched)
        {
            return Next(celt, rgelt, out fetched);
        }
    }
}