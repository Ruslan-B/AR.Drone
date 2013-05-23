using System;
using System.Windows.Forms;
using AI.Core.System;

namespace AR.Drone.WinApp
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // register LD_LIBRARY_PATH on window 
            InteropHelper.RegisterLibrariesSearchPath(Environment.GetEnvironmentVariable(InteropHelper.LD_LIBRARY_PATH));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}