using Microsoft.VisualStudio.Debugger.Interop;

namespace MonoDebugger.VisualStudio
{
    public class MonoPropertyInfoEnum : Enumerator<DEBUG_PROPERTY_INFO, IEnumDebugPropertyInfo2>, IEnumDebugPropertyInfo2
    {
        public MonoPropertyInfoEnum(DEBUG_PROPERTY_INFO[] data) : base(data)
        {
        }
    }
}