using BusinessAccounting.ViewModel;
using MaterialDesignThemes.Wpf;
using System.Windows;

namespace BusinessAccounting.View
{
    public partial class MainWindow : Window
    {
        public static Snackbar Snackbar;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel(MainSnackbar.MessageQueue);
            Snackbar = this.MainSnackbar;
        }
    }
}
