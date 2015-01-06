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

namespace BusinessAccounting.UserControls
{
    /// <summary>
    /// Interaction logic for CashPage.xaml
    /// </summary>
    public partial class CashPage : UserControl
    {
        public CashPage()
        {
            InitializeComponent();

            List<HistoryRecord> history = new List<HistoryRecord>();
            history.Add(new HistoryRecord() { id = 4, date = DateTime.Now, sum = 100, comment = "Прибыль"});
            history.Add(new HistoryRecord() { id = 3, date = DateTime.Now, sum = 90.5M, comment = "Прибыль" });
            history.Add(new HistoryRecord() { id = 2, date = DateTime.Now, sum = 50, comment = "Прибыль" });
            history.Add(new HistoryRecord() { id = 1, date = DateTime.Now, sum = 90, comment = "Прибыль" });
            HistoryTable.ItemsSource = history;
        }
    }
}
