using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls;
using System.Windows.Media.Animation;
using MahApps.Metro.Controls.Dialogs;

namespace BusinessAccounting
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            MainMenuGrid.Opacity = 0;
            MainMenuGrid.Visibility = Visibility.Hidden;
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
                window?.ShowMessageAsync("Проблемка", text + Environment.NewLine + App.Sqlite.LastErrorMessage);
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
    }
}
