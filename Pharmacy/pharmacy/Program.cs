using System;
using System.Threading;
using System.Windows.Forms;

namespace pharmacy
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            new Mutex(true, "pharmcy.exe", out bool create);
            if (!create)
            {
                MessageBox.Show("برنامه در حالت اجرا است", "App Running", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            //Setting_Bin.Load();

            Setting_Bin.Server = ".";
            Setting_Bin.Password = "1386";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Fr_Login());

            //Setting_Bin.Save();
        }
    }
}
