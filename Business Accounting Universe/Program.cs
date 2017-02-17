using System;
using System.Threading;
using System.Windows.Forms;
using BusinessAccountingUniverse.View;
using MetroFramework;

namespace BusinessAccountingUniverse
{
    internal class Program
    {
        private const string MutexId = "d0b1994d-0a5d-4cd6-864f-7346fc81cc1f";

        [STAThread]
        internal static void Main()
        {
            bool singleApp;
            using (new Mutex(true, MutexId, out singleApp))
            {
                if (!singleApp)
                {
                    MessageBox.Show($"The program is already running.{Environment.NewLine}" + 
                        "Running two or more simultaneous sessions is not recommended.",
                        "Business Accounting Universe", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
}
