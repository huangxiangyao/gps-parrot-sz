using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Parrot
{
    public static class DllImport
    {
        [DllImport("User32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll")]
        public  static extern int SendMessage(int hWnd, int Msg, int wParam, string lParam);

        [DllImport("Kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr hProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

    }
}
