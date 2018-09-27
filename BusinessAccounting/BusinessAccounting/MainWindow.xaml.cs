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
using BusinessAccounting.Properties;

namespace BusinessAccounting
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _status;
        private bool _windowDisplayed;
        private GoogleDriveBackupStorage _backupStorage;

        public MainWindow()
        {
            InitializeComponent();

            dockPanel.DataContext = this;
            lblStatus.DataContext = this;

            MainMenuGrid.Opacity = 0;
            MainMenuGrid.Visibility = Visibility.Hidden;

            _backupStorage = new GoogleDriveBackupStorage(
                DatabaseFileFullPath: App.DatabaseFilePath, 
                RemoteFolderId: App.BackupRemoteFolderId, 
                RemoteFileId: App.BackupRemoteFileId, 
                BackupInterval: App.AutoBackupInterval);
            _backupStorage.OnStatusUpdated += BackupStorage_OnUpdateStatus;
            _backupStorage.OnFailed += BackupStorage_OnFailed;
            _backupStorage.OnCompleted += BackupStorage_OnCompleted;
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (_windowDisplayed)
                return;

            _windowDisplayed = true;

            LoadPage(new UserControls.CashPage());
            _backupStorage.MakeBackupIfOutOfDate();
        }

        private void ShowMessage(string text, string caption = "Проблемка")
        {
            for (var visual = this as Visual; visual != null; visual = VisualTreeHelper.GetParent(visual) as Visual)
            {
                var window = visual as MetroWindow;
                window?.ShowMessageAsync(caption, text);
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
            _backupStorage.MakeBackup();
        }

        private void BackupStorage_OnUpdateStatus(string status)
        {
            Status = status;
        }

        private void BackupStorage_OnFailed(string message)
        {
            Application.Current.Dispatcher.Invoke(() => ShowMessage(message, ResourcesRU.Backup));
        }

        private void BackupStorage_OnCompleted()
        {
            Application.Current.Dispatcher.Invoke(() => ShowMessage(ResourcesRU.BackupCompleted, ResourcesRU.Backup));
        }
    }
}
