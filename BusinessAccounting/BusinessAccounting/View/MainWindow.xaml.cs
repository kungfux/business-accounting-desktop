using BusinessAccountingControls;
using System.Windows;

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

            this.Top = Properties.Settings.Default.Top;
            this.Left = Properties.Settings.Default.Left;
            this.Height = Properties.Settings.Default.Height;
            this.Width = Properties.Settings.Default.Width;
            
            if (Properties.Settings.Default.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void BusinessAccountingWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Top = RestoreBounds.Top;
            Properties.Settings.Default.Left = RestoreBounds.Left;
            Properties.Settings.Default.Height = RestoreBounds.Height;
            Properties.Settings.Default.Width = RestoreBounds.Width;
            Properties.Settings.Default.WindowState = this.WindowState;

            Properties.Settings.Default.Save();
        }
    }
}
