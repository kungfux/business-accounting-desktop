using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Xclass.Database;

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

            inputDate.DisplayDate = DateTime.Now;

            LoadHistory();
        }

        private const int preloadRecordsCount = 30;
        List<CashTransaction> history = new List<CashTransaction>();
        List<Employee> employees = new List<Employee>();

        #region Functionality methods
        // load history of cash operations from db and fill listview
        private void LoadHistory(bool all = false)
        {
            string query = string.Format("select id, datestamp, summa, comment from ba_cash_operations order by id desc {0};",
                all ? "" : "limit " + preloadRecordsCount);

            history = new List<CashTransaction>();

            DataTable historyRecords = App.sqlite.SelectTable(query);
            if (historyRecords != null)
            {
                foreach (DataRow row in historyRecords.Rows)
                {
                    history.Add(new CashTransaction()
                    {
                        id = Convert.ToInt32(row.ItemArray[0].ToString()),
                        date = Convert.ToDateTime(row.ItemArray[1]),
                        sum = decimal.Parse(row.ItemArray[2].ToString()),
                        comment = row.ItemArray[3].ToString()
                    });
                }
                lvHistory.ItemsSource = history;
            }
            else
            {
                groupHistory.Header = "Нет последних записей";
                groupHistory.IsEnabled = false;
            }
        }

        private void LoadEmployees()
        {
            employees = new List<Employee>();

            DataTable employeesData = App.sqlite.SelectTable("select id, fullname from ba_employees_cardindex where fired is null;");
            if (employeesData != null && employeesData.Rows.Count > 0)
            {
                foreach (DataRow r in employeesData.Rows)
                {
                    employees.Add(new Employee()
                    {
                        Id = Convert.ToInt32(r.ItemArray[0]),
                        FullName = r.ItemArray[1].ToString()
                    });
                }
            }
            comboEmployee.ItemsSource = employees;
        }

        // save new cash operation to db
        private void SaveRecord()
        {
            bool result = false;

            if ((bool)SalaryMode.IsChecked)
            {
                result = 2 == App.sqlite.PerformTransaction(new SQLiteQueryStatement[] {
                    new SQLiteQueryStatement() {
                         QuerySql = "insert into ba_cash_operations (datestamp, summa, comment) values (@D, @s, @c);",
                         QueryParameters = new SQLiteParameter[] {
                            new SQLiteParameter("@d", inputDate.SelectedDate),
                            new SQLiteParameter("@s", Convert.ToDecimal(inputSum.Text)),
                            new SQLiteParameter("@c", inputComment.Text != "" ? inputComment.Text : null)
                          }
                    },
                    new SQLiteQueryStatement() {
                        QuerySql = "insert into ba_employees_cash (emid, opid) values (@e, (select max(ba_cash_operations.id) from ba_cash_operations));",
                        QueryParameters = new SQLiteParameter[] {
                            new SQLiteParameter("@e", employees[comboEmployee.SelectedIndex].Id)
                        }
                    }
                });
            }
            else
            {
                result = 1 == App.sqlite.ChangeData("insert into ba_cash_operations (datestamp, summa, comment) values (@d, @s, @c);",
                    new SQLiteParameter("@d", inputDate.SelectedDate),
                    new SQLiteParameter("@s", Convert.ToDecimal(inputSum.Text)),
                    new SQLiteParameter("@c", inputComment.Text != "" ? inputComment.Text : null));
            }

            if (result)
            {
                inputDate.SelectedDate = null;
                inputSum.Text = "";
                inputComment.Text = "";
                comboEmployee.SelectedIndex = -1;
                LoadHistory();
            }
            else
            {
                ShowMessage("Не удалось сохранить запись в базе данных!");
                return;
            }
        }

        private async void bRemoveHistoryRecord_Click(object sender, RoutedEventArgs e)
        {
            CashTransaction record = null;

            for (var visual = sender as Visual; visual != null; visual = VisualTreeHelper.GetParent(visual) as Visual)
                if (visual is GridViewRowPresenter)
                {
                    var row = (GridViewRowPresenter)visual;
                    record = (CashTransaction)row.DataContext;
                    break;
                }

            await AskAndDelete(string.Format("Удалить запись?{0}{0}Информация об удаляемой записи:{0} Дата: {1:dd MMMM yyyy}{0} Сумма: {2:C}{0} Комментарий: {3}",
                Environment.NewLine, record.date, record.sum, record.comment), record);
        }

        private async Task AskAndDelete(string question, CashTransaction record)
        {
            MetroWindow w = (MetroWindow)this.Parent.GetParentObject().GetParentObject();
            MessageDialogResult result = await w.ShowMessageAsync("Вопросик", question, MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                if (App.sqlite.ChangeData("delete from ba_cash_operations where id = @id;",
                        new SQLiteParameter("@id", record.id)) <= 0)
                {
                    ShowMessage("Не удалось удалить запись из базы данных!");
                }
                else
                {
                    LoadHistory();
                }
            }
        }

        private void ShowMessage(string text)
        {
            for (var visual = this as Visual; visual != null; visual = VisualTreeHelper.GetParent(visual) as Visual)
                if (visual is MetroWindow)
                {
                    ((MetroWindow)visual).ShowMessageAsync("Проблемка", text + Environment.NewLine + App.sqlite.LastOperationErrorMessage,
                        MessageDialogStyle.Affirmative);
                }
        }
        #endregion

        #region Commands
        private void SaveRecord_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            decimal sum;

            e.CanExecute =
                (bool)SalaryMode.IsChecked & 
                (
                    comboEmployee != null &&
                    comboEmployee.SelectedIndex != -1 && // employee is selected
                    decimal.TryParse(inputSum.Text, out sum) && // sum is entered
                    sum < 0 // sum is less then zero because you spent money
                )
                ||
                !(bool)SalaryMode.IsChecked &
                (
                    inputDate.SelectedDate != null && // date is selected
                    inputSum.Text.Length > 0 && // sum is entered
                    decimal.TryParse(inputSum.Text, out sum) == true // sum is correct
                );
        }

        private void SaveRecord_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveRecord();
        }

        private void LoadHistory_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = lvHistory.Items.Count <= preloadRecordsCount;
        }

        private void LoadHistory_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LoadHistory(true);
        }
        #endregion

        private void SalaryMode_IsCheckedChanged(object sender, EventArgs e)
        {
            GridSalary.Visibility = (bool)SalaryMode.IsChecked ? Visibility.Visible : Visibility.Collapsed;
            if ((bool)SalaryMode.IsChecked)
            {
                LoadEmployees();
            }
        }
    }
}
