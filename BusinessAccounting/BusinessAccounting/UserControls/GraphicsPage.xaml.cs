using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
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
        }

        public static RoutedCommand PrintChartCommand = new RoutedCommand();
        public static RoutedCommand SaveChartCommand = new RoutedCommand();

        private Chart _chart;

        private void BuildChart()
        {
            wfHost.Visibility = Visibility.Hidden;

            var startDate = pickerPeriodStart.SelectedDate.GetValueOrDefault(DateTime.MaxValue);
            var endDate = pickerPeriodEnd.SelectedDate.GetValueOrDefault(DateTime.MaxValue);

            var isBuilt = false;
            var selectedChart = ((ComboBoxItem)comboChart.SelectedItem).Name;
            switch (selectedChart)
            {
                case "Circle":
                    isBuilt = BuildTotalsChart(comboDisplayValues.IsChecked.Value,
                        startDate, endDate);
                    break;
                case "Column":
                    isBuilt = BuildColumnIncomesAndExpensesPerYearChart(comboDisplayValues.IsChecked.Value,
                        startDate);
                    break;
                case "Line":
                    isBuilt = BuildLineIncomesAndExpensesFor3YearsChart(comboDisplayValues.IsChecked.Value,
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
                    ((MetroWindow)visual).ShowMessageAsync("Проблемка",
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

            if (comboChart.SelectedIndex >= 1 &&
                pickerPeriodStart.SelectedDate != null)
            {
                e.CanExecute = true;
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

        private bool BuildTotalsChart(bool displayValues, DateTime startDate, DateTime endDate)
        {
            const string sql =
                "select sum(summa) from ba_cash_operations where datestamp >= @d1 and datestamp <= @d2 and summa > 0 union " +
                "select sum(summa) from ba_cash_operations where datestamp >= @d1 and datestamp <= @d2 and summa < 0;";

            startDate = startDate + new TimeSpan(0, 0, 0);
            endDate = endDate + new TimeSpan(23, 59, 59);

            _chart = new Chart();
            _chart.Series.Add("");
            _chart.Titles.Add($"Итоги доходов и расходов за период {Environment.NewLine} с {startDate.ToString("dd MMMM yyyy")} по {endDate.ToString("dd MMMM yyyy")}.");
            _chart.Legends.Add("Легенда").Title = "Легенда";
            _chart.ChartAreas.Add("");
            _chart.Series[0].ChartType = SeriesChartType.Doughnut;

            var result = App.sqlite.SelectTable(sql, new SQLiteParameter("@d1", startDate), new SQLiteParameter("@d2", endDate));

            if (result != null && result.Rows.Count == 2)
            {
                var incomeSum = result.Rows[0].ItemArray[0] != DBNull.Value ? Convert.ToDouble(result.Rows[0].ItemArray[0]) : 0;
                var chargesSum = result.Rows.Count >= 2 && result.Rows[1].ItemArray[0] != DBNull.Value ? 0 - Convert.ToDouble(result.Rows[1].ItemArray[0]) : 0;

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

            _chart.ChartAreas.Add("");
            _chart.ChartAreas[0].CursorX.IsUserEnabled = true;
            _chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            _chart.ChartAreas[0].CursorX.Interval = 0;
            _chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            _chart.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            _chart.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            _chart.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 0;

            _chart.ChartAreas[0].CursorY.IsUserEnabled = true;
            _chart.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            _chart.ChartAreas[0].CursorY.Interval = 0;
            _chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            _chart.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            _chart.ChartAreas[0].AxisY.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            _chart.ChartAreas[0].AxisY.ScaleView.SmallScrollMinSize = 0;

            _chart.Series.Add("Доходы");
            _chart.Series.Add("Расходы");

            _chart.Titles.Add($"Доходы и расходы за {date.Year} год");
            _chart.Legends.Add("Легенда").Title = "Легенда";

            _chart.Series[0].ChartType = SeriesChartType.Column;
            _chart.Series[1].ChartType = SeriesChartType.Column;

            var totalsIncomes = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var totalsCharges = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var emptyResults = false;

            var resultIncomes = App.sqlite.SelectTable(sqlIncomes, new SQLiteParameter("@d1", startDate), new SQLiteParameter("@d2", endDate));
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

            var resultCharges = App.sqlite.SelectTable(sqlCharges, new SQLiteParameter("@d1", startDate), new SQLiteParameter("@d2", endDate));
            if (resultCharges != null && resultCharges.Rows.Count >= 1)
            {
                for (var a = 0; a < resultCharges.Rows.Count; a++)
                {
                    var month = Convert.ToInt32(resultCharges.Rows[a].ItemArray[1]);
                    totalsCharges[month - 1] = 0 - Convert.ToDouble(resultCharges.Rows[a].ItemArray[2]);
                }
            }
            else if (emptyResults)
            {
                return false;
            }

            for (var a = 0; a < 12; a++)
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
            const string sql = "select datestamp, summa from ba_cash_operations where datestamp between @d1 and @d2 and summa > 0";

            _chart = new Chart();
            _chart.Series.Add($"{date.Year}");
            _chart.Series.Add($"{date.Year - 1}");
            _chart.Series.Add($"{date.Year - 2}");
            _chart.Titles.Add($"Доходы за {date.Year}-{date.Year - 2} года");
            _chart.Legends.Add("");

            for (var a = 0; a < 3; a++)
            {
                _chart.ChartAreas.Add($"year{a + 1}");
                _chart.ChartAreas[a].CursorX.IsUserEnabled = true;
                _chart.ChartAreas[a].CursorX.IsUserSelectionEnabled = true;
                _chart.ChartAreas[a].CursorX.Interval = 0;
                _chart.ChartAreas[a].AxisX.ScaleView.Zoomable = true;
                _chart.ChartAreas[a].AxisX.ScrollBar.IsPositionedInside = true;
                _chart.ChartAreas[a].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
                _chart.ChartAreas[a].AxisX.ScaleView.SmallScrollMinSize = 0;

                _chart.ChartAreas[a].CursorY.IsUserEnabled = true;
                _chart.ChartAreas[a].CursorY.IsUserSelectionEnabled = true;
                _chart.ChartAreas[a].CursorY.Interval = 0;
                _chart.ChartAreas[a].AxisY.ScaleView.Zoomable = true;
                _chart.ChartAreas[a].AxisY.ScrollBar.IsPositionedInside = true;
                _chart.ChartAreas[a].AxisY.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
                _chart.ChartAreas[a].AxisY.ScaleView.SmallScrollMinSize = 0;
            }

            _chart.Series[0].ChartType = SeriesChartType.FastLine;
            _chart.Series[0].XValueType = ChartValueType.Date;
            _chart.Series[0].ChartArea = "year1";

            _chart.Series[1].ChartType = SeriesChartType.FastLine;
            _chart.Series[1].XValueType = ChartValueType.Date;
            _chart.Series[1].ChartArea = "year2";

            _chart.Series[2].ChartType = SeriesChartType.FastLine;
            _chart.Series[2].XValueType = ChartValueType.Date;
            _chart.Series[2].ChartArea = "year3";

            _chart.Legends.Add("Легенда").Title = "Легенда";

            var startDate = new DateTime(date.Year, 1, 1, 0, 0, 0);
            var endDate = new DateTime(date.Year, 12, 31, 23, 59, 59);

            for (var a = 0; a < 3; a++)
            {
                var data = App.sqlite.SelectTable(sql, new SQLiteParameter("@d1", startDate), new SQLiteParameter("@d2", endDate));
                if (data != null && data.Rows.Count > 0)
                {
                    for (var row = 0; row < data.Rows.Count; row++)
                    {
                        _chart.Series[a].Points.Add(
                            new DataPoint(Convert.ToDateTime(data.Rows[row].ItemArray[0]).ToOADate(),
                            Convert.ToDouble(data.Rows[row].ItemArray[1])));

                        if (displayValues)
                        {
                            _chart.Series[a].Points[row].Label = $"{Convert.ToDouble(data.Rows[row].ItemArray[1]):C}";
                        }
                    }
                }
                else
                {
                    _chart.Series[a].Points.Add(startDate.ToOADate());
                }

                startDate = startDate.AddYears(-1);
                endDate = endDate.AddYears(-1);
            }

            return true;
        }
    }
}
