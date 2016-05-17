using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudioTools;
using Microsoft.Win32;

namespace MonoDebugger.VisualStudio
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideDebugEngine("Mono Debug Engine", typeof(MonoProgramProvider), typeof(MonoEngine), Guids.EngineId, false, false, false)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class MonoDebuggerPackage : Package
    {
        /// <summary>
        /// MonoDebuggerPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "3a89d0ae-b482-4f2a-a8d0-da51583ce95c";

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoDebuggerPackage"/> class.
        /// </summary>
        public MonoDebuggerPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (mcs != null)
            {
                // Create the command for the menu item.
                var menuCommandID = new CommandID(Guids.AttachCommandGroupGuid, (int)Guids.AttachCommandId);
                var menuItem = new MenuCommand(AttachDebugger, menuCommandID);
                mcs.AddCommand(menuItem);
            }
        }

        private void AttachDebugger(object sender, EventArgs e)
        {
            var debugger = (IVsDebugger4)GetService(typeof(IVsDebugger));
            var debugTargets = new VsDebugTargetInfo4[1];
            debugTargets[0].dlo = (uint)DEBUG_LAUNCH_OPERATION.DLO_CreateProcess;
            debugTargets[0].bstrExe = "TestDebug.exe";
            debugTargets[0].guidLaunchDebugEngine = new Guid(Guids.EngineId);

//            var guidDbgEngine = new Guid(Guids.EngineId);
//            var pGuids = Marshal.AllocCoTaskMem(Marshal.SizeOf(guidDbgEngine));
//            Marshal.StructureToPtr(guidDbgEngine, pGuids, false);
//            debugTargets[0].pDebugEngines = pGuids;
         
            var processInfo = new VsDebugTargetProcessInfo[debugTargets.Length];

            debugger.LaunchDebugTargets4(1, debugTargets, processInfo);

//            if (pGuids != IntPtr.Zero)
//                Marshal.FreeCoTaskMem(pGuids);
        }
    }
}
