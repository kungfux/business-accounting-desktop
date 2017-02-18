using System;
using System.Threading;
using System.Windows.Forms;
using BusinessAccountingUniverse.Localization;
using BusinessAccountingUniverse.Views;

namespace BusinessAccountingUniverse
{
    internal static class Program
    {
        private const string MutexId = "d0b1994d-0a5d-4cd6-864f-7346fc81cc1f";

        private static readonly Localize Localize = new Localize();

        [STAThread]
        internal static void Main()
        {
            bool singleApp;
            using (new Mutex(true, MutexId, out singleApp))
            {
                if (!singleApp)
                {
                    MessageBox.Show(
                        string.Format(Localize.GetStringById("ProgramIsAlreadyRunning"), Environment.NewLine),
                        Localize.ApplicationName, 
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
