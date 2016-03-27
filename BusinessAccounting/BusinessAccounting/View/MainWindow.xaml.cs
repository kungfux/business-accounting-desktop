using BusinessAccountingControls;
using System.Windows;
using System.ComponentModel;

namespace BusinessAccounting.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BusinessAccountingWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadAndRestoreWindowSize();
        }

        private void LoadAndRestoreWindowSize()
        {
            Top = Properties.Settings.Default.Top;
            Left = Properties.Settings.Default.Left;
            Height = Properties.Settings.Default.Height;
            Width = Properties.Settings.Default.Width;

            if (Properties.Settings.Default.WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void SaveWindowSize()
        {
            Properties.Settings.Default.Top = RestoreBounds.Top;
            Properties.Settings.Default.Left = RestoreBounds.Left;
            Properties.Settings.Default.Height = RestoreBounds.Height;
            Properties.Settings.Default.Width = RestoreBounds.Width;
            Properties.Settings.Default.WindowState = WindowState;

            Properties.Settings.Default.Save();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveWindowSize();
        }
    }
}
