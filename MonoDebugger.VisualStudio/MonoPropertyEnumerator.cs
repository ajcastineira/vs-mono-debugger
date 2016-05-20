using Microsoft.VisualStudio.Debugger.Interop;

namespace MonoDebugger.VisualStudio
{
    public class MonoPropertyEnumerator : Enumerator<DEBUG_PROPERTY_INFO, IEnumDebugPropertyInfo2>, IEnumDebugPropertyInfo2
    {
        public MonoPropertyEnumerator(DEBUG_PROPERTY_INFO[] properties) : base(properties)
        {
        }
    }
}