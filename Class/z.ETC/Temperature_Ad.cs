using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace T_Align
{
    class Temperature_Ad
    {
        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);
        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
        static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);
        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
        static extern bool FreeLibrary(int hModule);
        [DllImport("kernel32.dll", EntryPoint = "GetLastError")]
        static extern int GetLastError();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool _SvApiLibInitialize();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool _SvApiLibUnInitialize();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool _SvReadIoPortByteEx(ushort port, ref byte value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool _SvWriteIoPortByteEx(ushort port, byte value);

        private static _SvApiLibInitialize installDriver;
        private static _SvApiLibUnInitialize removeDriver;
        private static _SvReadIoPortByteEx getPortVal;
        private static _SvWriteIoPortByteEx setPortVal;

        [DllImport(@"SvApiLibx64.dll")]
        unsafe public static extern bool SvApiLibInitialize();

        private const byte SIO_INDEX_PORT = 0x2E;
        private const byte SIO_DATA_PORT = 0x2F;
        uint baseaddr;

        public Temperature_Ad()
        {
            try
            {
                IntPtr hModule = LoadLibrary("SvApiLibx64.dll");
                IntPtr pFuncAddr = IntPtr.Zero;

                pFuncAddr = GetProcAddress(hModule, "SvApiLibInitialize");
                installDriver = (_SvApiLibInitialize)Marshal.GetDelegateForFunctionPointer(pFuncAddr, typeof(_SvApiLibInitialize));


                pFuncAddr = GetProcAddress(hModule, "SvApiLibUnInitialize");
                removeDriver = (_SvApiLibUnInitialize)Marshal.GetDelegateForFunctionPointer(pFuncAddr, typeof(_SvApiLibUnInitialize));

                pFuncAddr = GetProcAddress(hModule, "SvReadIoPortByteEx");
                getPortVal = (_SvReadIoPortByteEx)Marshal.GetDelegateForFunctionPointer(pFuncAddr, typeof(_SvReadIoPortByteEx));

                pFuncAddr = GetProcAddress(hModule, "SvWriteIoPortByteEx");
                setPortVal = (_SvWriteIoPortByteEx)Marshal.GetDelegateForFunctionPointer(pFuncAddr, typeof(_SvWriteIoPortByteEx));

                bool Test = installDriver();
                get_temp_base();
            }
            catch { }
        }

        private void get_temp_base()
        {
            sio_enter_config();
            IoWrite8(SIO_INDEX_PORT, 0x07);
            IoWrite8(SIO_DATA_PORT, 0x04);
            IoWrite8(SIO_INDEX_PORT, 0x60);
            baseaddr = (uint)Convert.ToInt32(IoRead8(SIO_DATA_PORT)) << 8;
            IoWrite8(SIO_INDEX_PORT, 0x61);
            baseaddr |= IoRead8(SIO_DATA_PORT);
            sio_exit_config();
        }

        private void sio_enter_config()
        {
            IoWrite8(SIO_INDEX_PORT, 0x87);
            IoWrite8(SIO_INDEX_PORT, 0x01);
            IoWrite8(SIO_INDEX_PORT, 0x55);
            IoWrite8(SIO_INDEX_PORT, 0x55);
        }

        private void sio_exit_config()
        {
            IoWrite8(SIO_INDEX_PORT, 0x02);
            IoWrite8(SIO_DATA_PORT, 0x02);
        }

        private void IoWrite8(UInt16 portAddr, byte portVal)
        {
            bool bReturn = setPortVal(portAddr, portVal);
        }
        static byte IoRead8(ushort portAddr)
        {
            byte val = 0;
            getPortVal(portAddr, ref val);

            return val;
        }

        public ushort get_cpu_temp()
        {
            IoWrite8((UInt16)(baseaddr + 0x05), 0x29);
            return IoRead8((ushort)(baseaddr + 0x06));
        }

        public ushort get_sys_temp()
        {
            IoWrite8((ushort)(baseaddr + 0x05), 0x2a);
            return IoRead8((ushort)(baseaddr + 0x06));
        }
    }
}
