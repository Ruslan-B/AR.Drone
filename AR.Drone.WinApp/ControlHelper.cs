using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AR.Drone.WinApp
{
    public static class ControlHelper
    {
        public static void ExecuteOnUIThread(this Control control, Action action)
        {
            Task.Factory.StartNew(() => { if (control.IsHandleCreated && control.IsDisposed == false) control.Invoke(action); });
        }
    }
}