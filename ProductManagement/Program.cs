using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductManagement
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new ProductManager());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đổi connection string để chạy app");
            }
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
