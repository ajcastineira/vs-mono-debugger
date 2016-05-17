using System;

namespace MonoDebugger.VisualStudio.VsUtils
{
    public class ComponentException : Exception
    {
        public int Code { get; }

        public ComponentException(int code)
        {
            Code = code;
        }
    }
}