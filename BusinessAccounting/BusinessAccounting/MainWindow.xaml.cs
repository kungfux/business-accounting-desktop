using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls;
using System.Windows.Media.Animation;
using BusinessAccounting.Integrations;
using MahApps.Metro.Controls.Dialogs;

namespace BusinessAccounting
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            dockPanel.DataContext = this;
            lblStatus.DataContext = this;

            MainMenuGrid.Opacity = 0;
            MainMenuGrid.Visibility = Visibility.Hidden;
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }

        private void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            OpenCloseMenu();
        }

        private void MenuButtonCash_Click(object sender, RoutedEventArgs e)
        {
            OpenCloseMenu();
            LoadPage(new UserControls.CashPage());
        }

        private void MenuButtonGraphics_Click(object sender, RoutedEventArgs e)
        {
            OpenCloseMenu();
            LoadPage(new UserControls.GraphicsPage());
        }

        private void MenuButtonEmployee_Click(object sender, RoutedEventArgs e)
        {
            OpenCloseMenu();
            LoadPage(new UserControls.EmployeePage());
        }

        private void OpenCloseMenu()
        {
            if (MainMenuGrid.Opacity == 0)
            {
                MainMenuGrid.Visibility = Visibility.Visible;
            }
            var animation = new DoubleAnimation
            {
                From = MainMenuGrid.Opacity > 0 ? 1 : 0,
                To = MainMenuGrid.Opacity > 0 ? 0 : 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };
            animation.Completed += animation_Completed;
            MainMenuGrid.BeginAnimation(OpacityProperty, animation);
        }

        private void animation_Completed(object sender, EventArgs e)
        {
            if (MainMenuGrid.Opacity == 0)
            {
                MainMenuGrid.Visibility = Visibility.Hidden;
            }
        }

        private void LoadPage(UIElement pPage)
        {
            UserControlGrid.Children.Clear();
            UserControlGrid.Children.Add(pPage);
        }

        private bool _windowDisplayed;

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (_windowDisplayed)
                return;

            _windowDisplayed = true;

            LoadPage(new UserControls.CashPage());
        }

        private void ShowMessage(string text)
        {
            for (var visual = this as Visual; visual != null; visual = VisualTreeHelper.GetParent(visual) as Visual)
            {
                var window = visual as MetroWindow;
                window?.ShowMessageAsync("Проблемка", text);
            }
        }

        private void OpenDbFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Directory.Exists(App.DatabasePath))
                {
                    ShowMessage($"Папка по адресу {App.DatabasePath} не найдена.");
                    return;
                }

                Process.Start("explorer", App.DatabasePath);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void MakeBackup_Click(object sender, RoutedEventArgs e)
        {
            var backup = new GoogleDriveBackupStorage();
            backup.OnUpdateStatus += Backup_OnUpdateStatus;
            backup.OnFailed += Backup_OnFailed;
            backup.MakeBackup(App.DatabaseFilePath, App.BackupRemoteFolderId);
        }

        private void Backup_OnFailed(string message)
        {
            Application.Current.Dispatcher.Invoke(() => ShowMessage(message));
        }

        private void Backup_OnUpdateStatus(string status)
        {
            Status = status;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
