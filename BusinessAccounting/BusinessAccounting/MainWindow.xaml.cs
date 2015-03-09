using System;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using System.Windows.Media.Animation;
using MahApps.Metro.Controls.Dialogs;

namespace BusinessAccounting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            // set opacity and visibility to hidden to display objects in design mode
            GridMenu.Opacity = 0;
            GridMenu.Visibility = Visibility.Hidden;
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
            if (GridMenu.Opacity == 0)
            {
                GridMenu.Visibility = Visibility.Visible;
            }
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = GridMenu.Opacity > 0 ? 1 : 0;
            animation.To = GridMenu.Opacity > 0 ? 0 : 1;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            animation.Completed += animation_Completed;
            GridMenu.BeginAnimation(OpacityProperty, animation);
        }

        void animation_Completed(object sender, EventArgs e)
        {
            // hide objects if they are not visible already
            // to avoid clicks
            if (GridMenu.Opacity == 0)
            {
                GridMenu.Visibility = Visibility.Hidden;
            }
        }

        private void LoadPage(UserControl pPage)
        {
            UserControlGrid.Children.Clear();
            UserControlGrid.Children.Add(pPage);
        }

        private bool WindowDisplayed = false;

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (WindowDisplayed)
                return;

            WindowDisplayed = true;

            if (!App.sqlite.TestConnection("Data Source=ba.sqlite;Version=3;UTF8Encoding=True;foreign keys=true;FailIfMissing=true", true, false))
            {
                this.ShowMessageAsync("Проблемка", "Не удалось установить соединение с базой данных.", MessageDialogStyle.Affirmative);
            }

            // open default page
            LoadPage(new UserControls.CashPage());
        }
    }
}
