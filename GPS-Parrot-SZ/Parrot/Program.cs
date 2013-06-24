using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using log4net;
using System.Diagnostics;

namespace Parrot
{
    static class Program
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof(MainForm));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (CheckAppInstance())
            {
                MessageBox.Show("您只能在同一台计算机上运行一个程序副本。", Application.ProductName,
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // http://www.techmango.com/blog/article/DotNet/Cross_thread_operation_not_valid.htm
            Control.CheckForIllegalCrossThreadCalls = false;

            Application.Run(new MainForm());
        }

        private static bool CheckAppInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (process.MainModule.FileName == current.MainModule.FileName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
