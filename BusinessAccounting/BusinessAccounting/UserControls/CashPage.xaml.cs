using BusinessAccounting.Domain;
using BusinessAccounting.Repositories;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

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

        public static RoutedCommand SaveRecordCommand = new RoutedCommand();
        public static RoutedCommand LoadHistoryCommand = new RoutedCommand();

        ICashOperationRepository cashOperationRepo = new CashOperationRepository();
        IEmployeeCardRepository employeeCardRepo = new EmployeeCardRepository();

        #region Functionality methods
        // load history of cash operations from db and fill listview
        private void LoadHistory(bool pShowAll = false)
        {
            lvHistory.ItemsSource = pShowAll 
                ? cashOperationRepo.GetAll() 
                : cashOperationRepo.GetLast50();

            //if (lvHistory.Items.Count == 0)
            //{
            //    groupHistory.Header = "Нет последних записей";
            //    groupHistory.IsEnabled = false;
            //}
        }

        private void LoadEmployees()
        {
            comboEmployee.ItemsSource = employeeCardRepo.GetAll();
        }

        // save new cash operation to db
        private void SaveRecord()
        {
            if ((bool)SalaryMode.IsChecked)
            {
                //result = 2 == App.sqlite.PerformTransaction(new SQLiteQueryStatement[] {
                //    new SQLiteQueryStatement() {
                //         QuerySql = "insert into ba_cash_operations (datestamp, summa, comment) values (@D, @s, @c);",
                //         QueryParameters = new SQLiteParameter[] {
                //            new SQLiteParameter("@d", inputDate.SelectedDate),
                //            new SQLiteParameter("@s", Convert.ToDecimal(inputSum.Text)),
                //            new SQLiteParameter("@c", inputComment.Text != "" ? inputComment.Text : null)
                //          }
                //    },
                //    new SQLiteQueryStatement() {
                //        QuerySql = "insert into ba_employees_cash (emid, opid) values (@e, (select max(ba_cash_operations.id) from ba_cash_operations));",
                //        QueryParameters = new SQLiteParameter[] {
                //            new SQLiteParameter("@e", employees[comboEmployee.SelectedIndex].Id)
                //        }
                //    }
                //});
            }
            else
            {
                var newCashOperation = new CashOperation() 
                { 
                    Date = (DateTime)inputDate.SelectedDate,
                    Sum = Convert.ToDecimal(inputSum.Text),
                    Comment = inputComment.Text != "" ? inputComment.Text : null

                };
                cashOperationRepo.Add(newCashOperation);

                inputDate.SelectedDate = null;
                inputSum.Text = "";
                inputComment.Text = "";
                comboEmployee.SelectedIndex = -1;
                LoadHistory();
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
            //if (result == MessageDialogResult.Affirmative)
            //{
            //    if (App.sqlite.ChangeData("delete from ba_cash_operations where id = @id;",
            //            new SQLiteParameter("@id", record.id)) <= 0)
            //    {
            //        ShowMessage("Не удалось удалить запись из базы данных!");
            //    }
            //    else
            //    {
            //        LoadHistory();
            //    }
            //}
            throw new NotImplementedException("Not switched to ORM yet.");
        }

        //private void ShowMessage(string text)
        //{
        //    //for (var visual = this as Visual; visual != null; visual = VisualTreeHelper.GetParent(visual) as Visual)
        //    //    if (visual is MetroWindow)
        //    //    {
        //    //        ((MetroWindow)visual).ShowMessageAsync("Проблемка", text + Environment.NewLine + App.sqlite.LastOperationErrorMessage,
        //    //            MessageDialogStyle.Affirmative);
        //    //    }
        //    throw new NotImplementedException("Not switched to ORM yet.");
        //}
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
            //e.CanExecute = lvHistory.Items.Count <= preloadRecordsCount;
            e.CanExecute = true;
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
