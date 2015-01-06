using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Windows.Media.Animation;

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
            ClearWindow();
            UserControlGrid.Children.Add(new UserControls.CashPage());
        }

        private void MenuButtonGraphics_Click(object sender, RoutedEventArgs e)
        {
            OpenCloseMenu();
        }

        private void MenuButtonEmployee_Click(object sender, RoutedEventArgs e)
        {
            OpenCloseMenu();
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

        private void ClearWindow()
        {
            UserControlGrid.Children.Clear();
        }
    }
}
