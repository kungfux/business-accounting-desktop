using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;

namespace BusinessAccounting.UserControls
{
    /// <summary>
    /// Interaction logic for GraphicsPage.xaml
    /// </summary>
    public partial class GraphicsPage : UserControl
    {
        public GraphicsPage()
        {
            InitializeComponent();

            pickerPeriodStart.SelectedDate = new DateTime(2014, 1, 1);
            pickerPeriodEnd.SelectedDate = new DateTime(2015, 12, 31);
        }

        public static RoutedCommand PrintChartCommand = new RoutedCommand();
        public static RoutedCommand SaveChartCommand = new RoutedCommand();

        private Chart _chart;

        //private const string SqlIncomes = 
        //    "select datestamp, sum(summa) from ba_cash_operations where summa > 0 and datestamp >= @d1 and datestamp <= @d2 group by datestamp order by datestamp asc;";
        //private const string SqlCharges = 
        //    "select datestamp, sum(summa) from ba_cash_operations where summa < 0 and datestamp >= @d1 and datestamp <= @d2 group by datestamp order by datestamp asc;";
        //private const string SqlIncomesAndCharges = 
        //    "select * from ((select sum(summa) from ba_cash_operations where datestamp >= @d1 and datestamp <= @d2 and summa > 0), " +
        //    "(select sum(summa) from ba_cash_operations where datestamp >= @d1 and datestamp <= @d2 and summa < 0));";

        private void BuildChart()
        {
            wfHost.Visibility = Visibility.Hidden;

            var startDate = pickerPeriodStart.SelectedDate.GetValueOrDefault(DateTime.MaxValue);
            var startTime = new TimeSpan(0, 0, 0);
            startDate = startDate + startTime;

            var endDate = pickerPeriodEnd.SelectedDate.GetValueOrDefault(DateTime.MaxValue);
            var endTime = new TimeSpan(23, 59, 59);
            endDate = endDate + endTime;

            var isBuilt = false;
            var selectedChart = ((ComboBoxItem) comboChart.SelectedItem).Name;
            switch (selectedChart)
            {
                case "Circle":
                    isBuilt = BuildCircleIncomesAndExpensesCustomDatesChart((bool) comboDisplayValues.IsChecked.Value,
                        startDate, endDate);
                    break;
                case "Column":
                    isBuilt = BuildColumnIncomesAndExpensesPerYearChart((bool) comboDisplayValues.IsChecked.Value,
                        startDate);
                    break;
                case "Line":
                    isBuilt = BuildLineIncomesAndExpensesFor3YearsChart((bool) comboDisplayValues.IsChecked.Value,
                        startDate);
                    break;
            }

            if (isBuilt)
            {
                DisplayChart();
            }
            else
            {
                ShowMessage("Не удалось построить график!");
            }
        }

        private void DisplayChart()
        {
            wfHost.Child = _chart;
            wfHost.Visibility = Visibility.Visible;
        }

        private void ShowMessage(string text)
        {
            for (var visual = this as Visual; visual != null; visual = VisualTreeHelper.GetParent(visual) as Visual)
                if (visual is MetroWindow)
                {
                    ((MetroWindow) visual).ShowMessageAsync("Проблемка",
                        $"{text}{Environment.NewLine}{App.sqlite.LastOperationErrorMessage}");
                }
        }

        private void SaveChart()
        {
            var sfDialog = new System.Windows.Forms.SaveFileDialog
            {
                AddExtension = true,
                Filter = "Png (PNG)|*.png"
            };
            if (sfDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (sfDialog.FileName != "")
                {
                    _chart.SaveImage(sfDialog.FileName, ChartImageFormat.Png);
                }
            }
        }

        private void PrintChart_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (comboChart.SelectedIndex == 0 &
                pickerPeriodStart.SelectedDate != null &
                pickerPeriodEnd.SelectedDate != null &
                pickerPeriodStart.SelectedDate <= pickerPeriodEnd.SelectedDate)
            {
                e.CanExecute = true;
                return;
            }

            if (comboChart.SelectedIndex == 1 &
                pickerPeriodStart.SelectedDate != null)
            {
                e.CanExecute = true;
                return;
            }

            if (comboChart.SelectedIndex > 1)
            {
                e.CanExecute = true;
                return;
            }
        }

        private void PrintChart_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BuildChart();
        }

        private void SaveChart_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute =
                wfHost != null &&
                wfHost.Visibility == Visibility.Visible;
        }

        private void SaveChart_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveChart();
        }

        private bool BuildCircleIncomesAndExpensesCustomDatesChart(bool displayValues, DateTime startDate,
            DateTime endDate)
        {
            const string sql =
                "select sum(summa) from ba_cash_operations where datestamp >= @d1 and datestamp <= @d2 and summa > 0 " +
                "union " +
                "select sum(summa) from ba_cash_operations where datestamp >= @d1 and datestamp <= @d2 and summa < 0;";

            _chart = new Chart();
            _chart.Series.Add("");
            _chart.Titles.Add(
                $"Доходы и расходы за период {Environment.NewLine} с {startDate.ToString("dd MMMM yyyy")} по {endDate.ToString("dd MMMM yyyy")}");
            _chart.Legends.Add("");
            _chart.Legends[0].Title = "Легенда";

            _chart.ChartAreas.Add("");

            _chart.Series[0].ChartType = SeriesChartType.Doughnut;

            var result = App.sqlite.SelectTable(sql, new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));

            if (result != null && result.Rows.Count == 2)
            {
                var incomeSum = result.Rows[0].ItemArray[0] != DBNull.Value
                    ? Convert.ToDouble(result.Rows[0].ItemArray[0])
                    : 0;
                var chargesSum = result.Rows.Count >= 2 && result.Rows[1].ItemArray[0] != DBNull.Value
                    ? 0 - Convert.ToDouble(result.Rows[1].ItemArray[0])
                    : 0;

                _chart.Series[0].Points.Add(incomeSum);
                _chart.Series[0].Points[0].LegendText = "Доходы";
                if (displayValues)
                {
                    _chart.Series[0].Points[0].Label = $"{incomeSum:C}";
                }

                _chart.Series[0].Points.Add(chargesSum);
                _chart.Series[0].Points[1].LegendText = "Расходы";
                if (displayValues)
                {
                    _chart.Series[0].Points[1].Label = $"{chargesSum:C}";
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private bool BuildColumnIncomesAndExpensesPerYearChart(bool displayValues, DateTime date)
        {
            var startDate = new DateTime(date.Year, 1, 1, 0, 0, 0);
            var endDate = new DateTime(date.Year, 12, 31, 23, 59, 59);

            const string sqlIncomes =
                "select strftime('%Y', datestamp) as year, strftime('%m', datestamp) as month, sum(summa) as total " +
                "from ba_cash_operations where datestamp between @d1 and @d2 and summa > 0 " +
                "group by year, month;";
            const string sqlCharges =
                "select strftime('%Y', datestamp) as year, strftime('%m', datestamp) as month, sum(summa) as total " +
                "from ba_cash_operations where datestamp between @d1 and @d2 and summa < 0 " +
                "group by year, month;";

            _chart = new Chart();
            _chart.Series.Add("Доходы");
            _chart.Series.Add("Расходы");
            _chart.Titles.Add(
                $"Доходы и расходы за {date.Year} год");
            _chart.Legends.Add("");
            _chart.Legends[0].Title = "Легенда";

            _chart.ChartAreas.Add("");

            _chart.Series[0].ChartType = SeriesChartType.Column;
            _chart.Series[1].ChartType = SeriesChartType.Column;

            var totalsIncomes = new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            var totalsCharges = new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

            bool emptyResults = false;

            var resultIncomes = App.sqlite.SelectTable(sqlIncomes, new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));
            if (resultIncomes != null && resultIncomes.Rows.Count >= 1)
            {
                for (var a = 0; a < resultIncomes.Rows.Count; a++)
                {
                    var month = Convert.ToInt32(resultIncomes.Rows[a].ItemArray[1]);
                    totalsIncomes[month - 1] = Convert.ToDouble(resultIncomes.Rows[a].ItemArray[2]);
                }
            }
            else
            {
                emptyResults = true;
            }

            var resultCharges = App.sqlite.SelectTable(sqlCharges, new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));
            if (resultCharges != null && resultCharges.Rows.Count >= 1)
            {
                for (var a = 0; a < resultCharges.Rows.Count; a++)
                {
                    var month = Convert.ToInt32(resultCharges.Rows[a].ItemArray[1]);
                    totalsCharges[month - 1] = 0 - Convert.ToDouble(resultCharges.Rows[a].ItemArray[2]);
                }
            }
            else
            {
                if (emptyResults)
                {
                    return false;
                }
            }

            for (int a = 0; a < 12; a++)
            {
                _chart.Series[0].Points.Add(totalsIncomes[a]);
                _chart.Series[1].Points.Add(totalsCharges[a]);

                if (displayValues)
                {
                    _chart.Series[0].Points[a].Label = $"{totalsIncomes[a]:C}";
                    _chart.Series[1].Points[a].Label = $"{totalsCharges[a]:C}";
                }
            }

            return true;
        }

        private bool BuildLineIncomesAndExpensesFor3YearsChart(bool displayValues, DateTime date)
        {
            var startDate1 = new DateTime(date.Year, 1, 1, 0, 0, 0);
            var endDate1 = new DateTime(date.Year, 12, 31, 23, 59, 59);
            var startDate2 = new DateTime(date.AddYears(-1).Year, 1, 1, 0, 0, 0);
            var endDate2 = new DateTime(date.AddYears(-1).Year, 12, 31, 23, 59, 59);
            var startDate3 = new DateTime(date.AddYears(-2).Year, 1, 1, 0, 0, 0);
            var endDate3 = new DateTime(date.AddYears(-2).Year, 12, 31, 23, 59, 59);

            const string sql = "select datestamp, sum(summa) from ba_cash_operations where datestamp >= @d1 and datestamp <= @d2 and summa > 0";

            _chart = new Chart();
            _chart.Series.Add($"{startDate1.Year}");
            _chart.Series.Add($"{startDate2.Year}");
            _chart.Series.Add($"{startDate3.Year}");
            _chart.Titles.Add($"Доходы за {date.Year}-{date.AddYears(-2).Year} года");
            _chart.Legends.Add("");

            _chart.ChartAreas.Add("");

            _chart.Series[0].ChartType = SeriesChartType.FastLine;
            _chart.Series[1].ChartType = SeriesChartType.FastLine;
            _chart.Series[2].ChartType = SeriesChartType.FastLine;

            var resultIncomes = App.sqlite.SelectTable(sql, new SQLiteParameter("@d1", startDate1), new SQLiteParameter("@d2", endDate1),
                new SQLiteParameter("@d3", startDate2), new SQLiteParameter("@d4", endDate2),
                new SQLiteParameter("@d5", startDate3), new SQLiteParameter("@d6", endDate3));
            if (resultIncomes != null && resultIncomes.Rows.Count == 3)
            {
                _chart.Series[0].Points.Add(Convert.ToDouble(resultIncomes.Rows[0].ItemArray[0]));
                _chart.Series[1].Points.Add(Convert.ToDouble(resultIncomes.Rows[0].ItemArray[0]));
                _chart.Series[2].Points.Add(Convert.ToDouble(resultIncomes.Rows[0].ItemArray[0]));

                if (displayValues)
                {
                    //_chart.Series[0].Points[0].Label = $"{totalsIncomes[a]:C}";
                    //_chart.Series[1].Points[0].Label = $"{totalsCharges[a]:C}";
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
