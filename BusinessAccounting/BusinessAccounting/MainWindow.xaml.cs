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
            MenuContext mc = new MenuContext();
            mc.MenuHeadText = "Главная";
            this.MenuInfo.DataContext = mc;
        }

        private string activePageName = "Главная";

        public class MenuContext
        {
            public string MenuHeadText { get; set; }
        }

        private void MenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                var b = sender as Button;
                MenuContext mc = new MenuContext();
                mc.MenuHeadText = b.Tag.ToString();
                this.MenuInfo.DataContext = mc;
            }
        }

        private void MenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                var b = sender as Button;
                MenuContext mc = new MenuContext();
                mc.MenuHeadText = activePageName;
                this.MenuInfo.DataContext = mc;
            }
        }
    }
}
