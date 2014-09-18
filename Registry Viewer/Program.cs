using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; // Administrator Rights
using System.Security.Principal;
namespace My_Project
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application. Also gives the program Admin rights
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
                {
                    ProcessStartInfo proc = new ProcessStartInfo();
                    proc.UseShellExecute = true;
                    proc.WorkingDirectory = Environment.CurrentDirectory;
                    proc.FileName = Application.ExecutablePath;
                    proc.Verb = "runas";

                    try
                    {
                        Process.Start(proc);
                    }
                    catch
                    {
                        // The user refused the elevation.
                        // Do nothing and return directly ...
                        return;
                    }
                    Application.Exit();    
                }
                else
                    Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                if (ex != null) {
                    //MessageBox.Show(ex.ToString());
                }
            }
        
        }
    }
}
