using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
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
        }

        public static RoutedCommand PrintChartCommand = new RoutedCommand();
        public static RoutedCommand SaveChartCommand = new RoutedCommand();

        private const string ChartCircleType = "Circle";
        private const string ChartLinearType = "Linear";

        private const string SqlIncomes = 
            "select datestamp, sum(summa) from ba_cash_operations where summa > 0 and datestamp >= @d1 and datestamp <= @d2 group by datestamp order by datestamp asc;";
        private const string SqlCharges = 
            "select datestamp, sum(summa) from ba_cash_operations where summa < 0 and datestamp >= @d1 and datestamp <= @d2 group by datestamp order by datestamp asc;";
        private const string SqlIncomesAndCharges = 
            "select * from ((select sum(summa) from ba_cash_operations where datestamp >= @d1 and datestamp <= @d2 and summa > 0), " +
            "(select sum(summa) from ba_cash_operations where datestamp >= @d1 and datestamp <= @d2 and summa < 0));";

        private Chart _chart;

        #region Functionality methods
        private void PrintChart()
        {
            wfHost.Visibility = Visibility.Hidden;

            _chart = new Chart();

            var startDate = pickerPeriodStart.SelectedDate.GetValueOrDefault(DateTime.MaxValue);
            var endDate = pickerPeriodEnd.SelectedDate.GetValueOrDefault(DateTime.MaxValue);

            var chartReady = false;
            var chartType = ((ComboBoxItem) comboChartType.SelectedItem).Name;
            switch (((ComboBoxItem)comboDataType.SelectedItem).Name)
            {
                case "Incomes":
                    chartReady = chartType == ChartCircleType
                        ? BuildIncomesCircleChart(startDate, endDate)
                        : BuildIncomesLinearChart(startDate, endDate);
                    break;
                case "Charges":
                    chartReady = chartType == ChartCircleType
                        ? BuildChargesCircleChart(startDate, endDate)
                        : BuildChargesLinearChart(startDate, endDate);
                    break;
                case "IncomesAndCharges":
                    chartReady = chartType == ChartCircleType
                        ? BuildIncomesAndChargesCircleChart(startDate, endDate)
                        : BuildIncomesAbdChargesLinearChart(startDate, endDate);
                    break;
            }

            if (chartReady)
            {
                wfHost.Child = _chart;
                wfHost.Visibility = Visibility.Visible;
            }
        }

        private bool BuildIncomesLinearChart(DateTime startDate, DateTime endDate)
        {
            _chart.Series.Add("");
            _chart.Titles.Add("График прибыли за период " + Environment.NewLine + "с " + startDate.ToString("dd MMMM yyyy") + " по " + endDate.ToString("dd MMMM yyyy"));
            _chart.Legends.Add("");
            _chart.Legends[0].Title = "Легенда";
            _chart.Series[0].LegendText = "Прибыль";

            _chart.ChartAreas.Add("Прибыль");
            _chart.ChartAreas[0].AxisX.Title = "Дата";
            _chart.ChartAreas[0].AxisY.Title = "Сумма";

            _chart.Series[0].XValueType = ChartValueType.Date;

            _chart.Series[0].ChartType = SeriesChartType.Area;

            var table =
                App.sqlite.SelectTable(SqlIncomes,
                new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));

            if (table != null && table.Rows.Count > 0)
            {
                for (int a = 0; a < table.Rows.Count; a++)
                {
                    _chart.Series[0].Points.Add(new DataPoint(Convert.ToDateTime(table.Rows[a].ItemArray[0]).ToOADate(),
                        Convert.ToDouble(table.Rows[a].ItemArray[1])));
                    if (comboDisplayValues.IsChecked == true)
                    {
                        _chart.Series[0].Points[a].Label = $"{Convert.ToDouble(table.Rows[a].ItemArray[1]):C}";
                    }
                }
            }
            else
            {
                ShowMessage("Нет данных для построения графика!");
                return false;
            }
            return true;
        }

        private bool BuildIncomesCircleChart(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        private bool BuildChargesLinearChart(DateTime startDate, DateTime endDate)
        {
            _chart.Series.Add("");
            _chart.Titles.Add(
                $"График расходов за период {Environment.NewLine} с {startDate.ToString("dd MMMM yyyy")} по {endDate.ToString("dd MMMM yyyy")}");
            _chart.Legends.Add("");
            _chart.Legends[0].Title = "Легенда";
            _chart.Series[0].LegendText = "Расходы";

            _chart.ChartAreas.Add("Расходы");
            _chart.ChartAreas[0].AxisX.Title = "Дата";
            _chart.ChartAreas[0].AxisY.Title = "Сумма";

            _chart.Series[0].XValueType = ChartValueType.Date;

            _chart.Series[0].ChartType = SeriesChartType.Area;
            _chart.Series[0].Color = System.Drawing.Color.Maroon;

            var table =
                App.sqlite.SelectTable(SqlCharges,
                new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));

            if (table != null && table.Rows.Count > 0)
            {
                for (int a = 0; a < table.Rows.Count; a++)
                {
                    _chart.Series[0].Points.Add(new DataPoint(Convert.ToDateTime(table.Rows[a].ItemArray[0]).ToOADate(),
                        0 - Convert.ToDouble(table.Rows[a].ItemArray[1])));
                    if (comboDisplayValues.IsChecked == true)
                    {
                        _chart.Series[0].Points[a].Label = $"{Convert.ToDouble(table.Rows[a].ItemArray[1]):C}";
                    }
                }
            }
            else
            {
                ShowMessage("Нет данных для построения графика!");
                return false;
            }
            return true;
        }

        private bool BuildChargesCircleChart(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        private bool BuildIncomesAbdChargesLinearChart(DateTime startDate, DateTime endDate)
        {
            _chart.Series.Add("");
            _chart.Series.Add("");
            _chart.Titles.Add(
                $"График прибыли и расходов за период {Environment.NewLine} с {startDate.ToString("dd MMMM yyyy")} по {endDate.ToString("dd MMMM yyyy")}");
            _chart.Legends.Add("");
            _chart.Legends[0].Title = "Легенда";
            _chart.Series[0].LegendText = "Прибыль";
            _chart.Series[1].LegendText = "Расходы";

            _chart.ChartAreas.Add("Прибыль");
            _chart.ChartAreas[0].AxisX.Title = "Дата";
            _chart.ChartAreas[0].AxisY.Title = "Сумма";

            _chart.Series[0].XValueType = ChartValueType.Date;
            _chart.Series[1].XValueType = ChartValueType.Date;

            _chart.Series[0].ChartType = SeriesChartType.Line;
            _chart.Series[1].ChartType = SeriesChartType.Line;

            _chart.Series[1].Color = System.Drawing.Color.Maroon;

            var tableIncomes =
                App.sqlite.SelectTable(SqlIncomes,
                new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));

            var tableCharges =
                App.sqlite.SelectTable(SqlCharges,
                new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));

            if ((tableIncomes == null || tableIncomes.Rows.Count == 0) && (tableCharges == null || tableCharges.Rows.Count == 0))
            {
                ShowMessage("Нет данных для построения графика!");
                return false;
            }
            else
            {
                if (tableIncomes != null)
                {
                    for (int a = 0; a < tableIncomes.Rows.Count; a++)
                    {
                        _chart.Series[0].Points.Add(new DataPoint(Convert.ToDateTime(tableIncomes.Rows[a].ItemArray[0]).ToOADate(),
                            Convert.ToDouble(tableIncomes.Rows[a].ItemArray[1])));
                        if (comboDisplayValues.IsChecked == true)
                        {
                            _chart.Series[0].Points[a].Label =
                                $"{Convert.ToDouble(tableIncomes.Rows[a].ItemArray[1]):C}";
                        }
                    }
                }

                if (tableCharges != null)
                {
                    for (int a = 0; a < tableCharges.Rows.Count; a++)
                    {
                        _chart.Series[1].Points.Add(new DataPoint(Convert.ToDateTime(tableCharges.Rows[a].ItemArray[0]).ToOADate(),
                            0 - Convert.ToDouble(tableCharges.Rows[a].ItemArray[1])));
                        if (comboDisplayValues.IsChecked == true)
                        {
                            _chart.Series[1].Points[a].Label =
                                $"{Convert.ToDouble(tableCharges.Rows[a].ItemArray[1]):C}";
                        }
                    }
                }
            }
            return true;
        }

        private bool BuildIncomesAndChargesCircleChart(DateTime startDate, DateTime endDate)
        {
            _chart.Series.Add("");
            _chart.Titles.Add(
                $"График сравнения за период {Environment.NewLine} с {startDate.ToString("dd MMMM yyyy")} по {endDate.ToString("dd MMMM yyyy")}");
            _chart.Legends.Add("");
            _chart.Legends[0].Title = "Легенда";

            _chart.ChartAreas.Add("");

            _chart.Series[0].ChartType = SeriesChartType.Doughnut;

            DataRow row =
                App.sqlite.SelectRow(SqlIncomesAndCharges,
                new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));

            if (row != null)
            {
                double incomeSum = row.ItemArray[0] != DBNull.Value ? Convert.ToDouble(row.ItemArray[0]) : 0;
                double chargesSum = row.ItemArray[1] != DBNull.Value ? 0 - Convert.ToDouble(row.ItemArray[1]) : 0;

                _chart.Series[0].Points.Add(incomeSum);
                _chart.Series[0].Points[0].LegendText = "Доходы";
                if (comboDisplayValues.IsChecked == true)
                {
                    _chart.Series[0].Points[0].Label = incomeSum > 0 ? $"{incomeSum:C}" : "";
                }

                _chart.Series[0].Points.Add(chargesSum);
                _chart.Series[0].Points[1].LegendText = "Расходы";
                if (comboDisplayValues.IsChecked == true)
                {
                    _chart.Series[0].Points[1].Label = chargesSum > 0 ? $"{chargesSum:C}" : "";
                }
            }
            else
            {
                ShowMessage("Нет данных для построения графика!");
                return false;
            }
            return true;
        }

        private void ShowMessage(string text)
        {
            for (var visual = this as Visual; visual != null; visual = VisualTreeHelper.GetParent(visual) as Visual)
                if (visual is MetroWindow)
                {
                    ((MetroWindow)visual).ShowMessageAsync("Проблемка", $"{text}{Environment.NewLine}{App.sqlite.LastOperationErrorMessage}");
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
        #endregion

        #region Commands
        private void PrintChart_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute =
                comboChartType.SelectedIndex != -1 &
                comboDataType.SelectedIndex != -1 &
                pickerPeriodStart.SelectedDate != null &
                pickerPeriodEnd.SelectedDate != null &
                pickerPeriodStart.SelectedDate <= pickerPeriodEnd.SelectedDate;
        }

        private void PrintChart_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PrintChart();
        }

        private void SaveChart_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute =
                wfHost != null &&
                wfHost.Visibility == Visibility.Visible && // if WindowsFormsHost visible then _chart is built
                _chart != null;
        }

        private void SaveChart_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveChart();
        }
        #endregion
    }
}
