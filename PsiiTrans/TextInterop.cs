using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace PsiiTrans
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OutputInfo
    {
        public long handle;
        public uint processId;
        public ulong addr;
        public ulong ctx;
        public ulong ctx2;
        public string name;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool OutputCallback(OutputInfo outinfo, [MarshalAs(UnmanagedType.LPWStr)] string message);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ThreadEventCallback(OutputInfo outinfo);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ProcessEventHandler(uint dword);

    class TextInterop
    {
        [DllImport("textracthost.dll")]
        public static extern void Start(ProcessEventHandler Connect, ProcessEventHandler Disconnect, ThreadEventCallback Create, ThreadEventCallback Destroy, OutputCallback Output);

        [DllImport("textracthost.dll")]
        public static extern void InjectProcess(uint processId);
    }
}
