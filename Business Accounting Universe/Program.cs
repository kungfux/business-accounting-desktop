using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using BusinessAccountingUniverse.Localization;
using BusinessAccountingUniverse.Views;
using NLog;

namespace BusinessAccountingUniverse
{
    internal static class Program
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private const string MutexId = "d0b1994d-0a5d-4cd6-864f-7346fc81cc1f";
        private const string GithubIssueUrl = "https://github.com/kungfux/business-accounting/issues/new";

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

                Application.ThreadException += OnThreadException;
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogError(e.ExceptionObject);
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs t)
        {
            LogError(t.Exception);
        }

        private static void LogError(object data)
        {
            Log.Fatal(data);

            if (MessageBox.Show(string.Format(Localize.GetStringById("UnhandledException"), Environment.NewLine), 
                Localize.ApplicationName,MessageBoxButtons.YesNo, MessageBoxIcon.Error) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                Clipboard.SetText($"App name: {Localize.ApplicationName}{Environment.NewLine}" +
                    $"Version: {Assembly.GetEntryAssembly().GetName().Version}{Environment.NewLine}" +
                    $"Problem: {data}");

                MessageBox.Show(string.Format(Localize.GetStringById("DetailsCopiedToClipboard"), Environment.NewLine),
                    Localize.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(GithubIssueUrl);
            }
            catch (Exception e)
            {
                Log.Error($"Unable to open GItHub to submit an issue: {e.Message}");
            }

            Application.Exit();
        }
    }
}
