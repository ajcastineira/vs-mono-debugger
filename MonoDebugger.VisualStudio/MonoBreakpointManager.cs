using System.Collections.Generic;
using Mono.Debugging.Client;

namespace MonoDebugger.VisualStudio
{
    public class MonoBreakpointManager
    {
        public MonoEngine Engine { get; }
        public MonoPendingBreakpoint this[BreakEvent breakEvent] => breakpoints[breakEvent];

        private Dictionary<BreakEvent, MonoPendingBreakpoint> breakpoints = new Dictionary<BreakEvent, MonoPendingBreakpoint>();

        public MonoBreakpointManager(MonoEngine engine)
        {
            Engine = engine;
        }

        public void Add(BreakEvent breakEvent, MonoPendingBreakpoint pendingBreakpoint) 
        {
            breakpoints[breakEvent] = pendingBreakpoint;
        }
    }
}