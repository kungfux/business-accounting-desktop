using System;
using System.Windows.Forms;
using BusinessAccountingUniverse.View;

namespace BusinessAccountingUniverse
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
