using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using XDatabase;

namespace BusinessAccounting.UserControls
{
    /// <summary>
    /// Interaction logic for GraphicsPage.xaml
    /// </summary>
    public partial class GraphicsPage
    {
        public GraphicsPage()
        {
            InitializeComponent();
        }

        public static readonly RoutedCommand PrintChartCommand = new RoutedCommand();
        public static readonly RoutedCommand SaveChartCommand = new RoutedCommand();

        private Chart _chart;
        private readonly Font _titleFont = new Font("Arial", 16, System.Drawing.FontStyle.Bold);

        private void BuildChart()
        {
            WfHost.Visibility = Visibility.Hidden;

            var startDate = PickerPeriodStart.SelectedDate.GetValueOrDefault(DateTime.MaxValue);
            var endDate = PickerPeriodEnd.SelectedDate.GetValueOrDefault(DateTime.MaxValue);
            var displayValues = Convert.ToBoolean(ComboDisplayValues.IsChecked.Value);

            var isBuilt = false;

            var selectedChart = ((ComboBoxItem)ComboChart.SelectedItem).Name;
            switch (selectedChart)
            {
                case "Period":
                    isBuilt = BuildTotalsChartForPeriod(displayValues, startDate, endDate);
                    break;
                case "Year":
                    isBuilt = BuildTotalsChartForTheYear(displayValues, startDate);
                    break;
                case "Year3":
                    isBuilt = BuildTotalsChartFor3Years(displayValues, startDate);
                    break;
            }

            if (isBuilt)
            {
                WfHost.Child = _chart;
                WfHost.Visibility = Visibility.Visible;
            }
            else
            {
                ShowMessage(string.IsNullOrEmpty(App.Sqlite.LastErrorMessage)
                    ? "Нет данных для построения графика!"
                    : $"Не удалось построить график! Ошибка:{Environment.NewLine}{App.Sqlite.LastErrorMessage}");
            }
        }

        private void ShowMessage(string text)
        {
            for (var visual = this as Visual; visual != null; visual = VisualTreeHelper.GetParent(visual) as Visual)
                if (visual is MetroWindow)
                {
                    ((MetroWindow)visual).ShowMessageAsync("Проблемка", text);
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
            if (ComboChart.SelectedIndex == 0 &
                PickerPeriodStart.SelectedDate != null &
                PickerPeriodEnd.SelectedDate != null &
                PickerPeriodStart.SelectedDate <= PickerPeriodEnd.SelectedDate)
            {
                e.CanExecute = true;
                return;
            }

            if (ComboChart.SelectedIndex >= 1 &&
                PickerPeriodStart.SelectedDate != null)
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
                WfHost != null &&
                WfHost.Visibility == Visibility.Visible;
        }

        private void SaveChart_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveChart();
        }

        private bool BuildTotalsChartForPeriod(bool displayValues, DateTime startDate, DateTime endDate)
        {
            const string sql = "select Sum(summa) from ba_cash_operations where datestamp between @d1 and @d2 and summa > 0 "+
                "union all select Sum(summa) from ba_cash_operations where datestamp between @d1 and @d2 and summa < 0";

            startDate = startDate + new TimeSpan(0, 0, 0);
            endDate = endDate + new TimeSpan(23, 59, 59);

            _chart = new Chart();

            var title = _chart.Titles.Add($"Сумма доходов и расходов за период {Environment.NewLine} с {startDate.ToString("dd MMMM yyyy")} по {endDate.ToString("dd MMMM yyyy")}");
            title.Font = _titleFont;

            _chart.Legends.Add("Легенда").Title = "Легенда";
            _chart.ChartAreas.Add("");

            _chart.Series.Add("");          
            _chart.Series[0].ChartType = SeriesChartType.Doughnut;

            var result = App.Sqlite.SelectTable(sql, new XParameter("@d1", startDate), new XParameter("@d2", endDate));

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

        private bool BuildTotalsChartForTheYear(bool displayValues, DateTime date)
        {
            var startDate = new DateTime(date.Year, 1, 1, 0, 0, 0);
            var endDate = new DateTime(date.Year, 12, 31, 23, 59, 59);

            const string sqlIncomes =
                "select strftime('%Y', datestamp) as year, strftime('%m', datestamp) as month, Sum(summa) as total " +
                "from ba_cash_operations where datestamp between @d1 and @d2 and summa > 0 " +
                "group by year, month;";
            const string sqlCharges =
                "select strftime('%Y', datestamp) as year, strftime('%m', datestamp) as month, Sum(summa) as total " +
                "from ba_cash_operations where datestamp between @d1 and @d2 and summa < 0 " +
                "group by year, month;";

            _chart = new Chart();

            var title = _chart.Titles.Add($"Доходы и расходы за {date.Year} год помесячно");
            title.Font = _titleFont;
            _chart.Legends.Add("Легенда").Title = "Легенда";

            _chart.ChartAreas.Add("");
            _chart.ChartAreas[0].AxisX.Title = "Месяц";
            _chart.ChartAreas[0].AxisY.Title = "Сумма";
            MakeAreaZoomable(_chart.ChartAreas[0]);

            _chart.Series.Add("Доходы");
            _chart.Series.Add("Расходы");

            _chart.Series[0].ChartType = SeriesChartType.Column;
            _chart.Series[1].ChartType = SeriesChartType.Column;

            var totalsIncomes = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var totalsCharges = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var emptyResults = false;

            var resultIncomes = App.Sqlite.SelectTable(sqlIncomes, new XParameter("@d1", startDate), new XParameter("@d2", endDate));
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

            var resultCharges = App.Sqlite.SelectTable(sqlCharges, new XParameter("@d1", startDate), new XParameter("@d2", endDate));
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

        private bool BuildTotalsChartFor3Years(bool displayValues, DateTime date)
        {
            const string sql = 
                "select strftime('%Y', datestamp) as year, strftime('%m', datestamp) as month, Sum(summa) from BA_CASH_OPERATIONS " +
                "where datestamp between @d1 and @d2 and summa > 0 group by year, month;";

            _chart = new Chart();
            
            var title = _chart.Titles.Add($"Доходы за {date.Year - 2}-{date.Year} годы");
            title.Font = _titleFont;
            _chart.Legends.Add("Легенда").Title = "Легенда";

            _chart.Series.Add($"{date.Year}");
            _chart.Series.Add($"{date.Year - 1}");
            _chart.Series.Add($"{date.Year - 2}");

            _chart.Series[0].LegendText = $"{date.Year}";
            _chart.Series[0].ChartType = SeriesChartType.Line;
            _chart.Series[0].BorderWidth = 5;
            _chart.Series[1].LegendText = $"{date.Year - 1}";
            _chart.Series[1].ChartType = SeriesChartType.Line;
            _chart.Series[1].BorderWidth = 5;
            _chart.Series[2].LegendText = $"{date.Year - 2}";
            _chart.Series[2].ChartType = SeriesChartType.Line;
            _chart.Series[2].BorderWidth = 5;

            _chart.ChartAreas.Add("");
            _chart.ChartAreas[0].AxisX.Title = "Месяц";
            _chart.ChartAreas[0].AxisY.Title = "Сумма";
            MakeAreaZoomable(_chart.ChartAreas[0]);

            var startDate = new DateTime(date.Year, 1, 1, 0, 0, 0);
            var endDate = new DateTime(date.Year, 12, 31, 23, 59, 59);

            var values = new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            var emptyDataCount = 0;

            for (var a = 0; a < 3; a++)
            {
                var data = App.Sqlite.SelectTable(sql, new XParameter("@d1", startDate), new XParameter("@d2", endDate));
                if (data != null && data.Rows.Count > 0)
                {
                    foreach (DataRow row in data.Rows)
                    {
                        var month = Convert.ToInt32(row.ItemArray[1]) - 1;
                        var sum = Convert.ToDouble(row.ItemArray[2]);
                        values[month] = sum;
                    }

                    for (var i = 0; i < 12; i++)
                    {
                        _chart.Series[a].Points.Add(new DataPoint(i + 1, values[i]));
                        if (displayValues)
                        {
                            _chart.Series[a].Points[i].Label = $"{values[i]:C}";
                        }
                    }
                }
                else
                {
                    emptyDataCount++;
                }

                startDate = startDate.AddYears(-1);
                endDate = endDate.AddYears(-1);
                values = new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            }

            return emptyDataCount < 3;
        }

        private static void MakeAreaZoomable(ChartArea chartArea)
        {
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorX.Interval = 0;
            chartArea.AxisX.ScaleView.Zoomable = true;
            chartArea.AxisX.ScrollBar.IsPositionedInside = true;
            chartArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chartArea.AxisX.ScaleView.SmallScrollMinSize = 0;

            chartArea.CursorY.IsUserEnabled = true;
            chartArea.CursorY.IsUserSelectionEnabled = true;
            chartArea.CursorY.Interval = 0;
            chartArea.AxisY.ScaleView.Zoomable = true;
            chartArea.AxisY.ScrollBar.IsPositionedInside = true;
            chartArea.AxisY.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chartArea.AxisY.ScaleView.SmallScrollMinSize = 0;
        }
    }
}
