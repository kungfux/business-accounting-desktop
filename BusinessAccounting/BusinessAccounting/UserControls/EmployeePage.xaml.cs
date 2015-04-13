using BusinessAccounting.Domain;
using BusinessAccounting.Repositories;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BusinessAccounting.UserControls
{
    /// <summary>
    /// Interaction logic for EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : UserControl
    {
        public EmployeePage()
        {
            InitializeComponent();
        }

        public static RoutedCommand NewEmployeeCommand = new RoutedCommand();
        public static RoutedCommand OpenEmployeeCommand = new RoutedCommand();
        public static RoutedCommand EditEmployeeCommand = new RoutedCommand();
        public static RoutedCommand SaveEmployeeCommand = new RoutedCommand();
        public static RoutedCommand DeleteEmployeeCommand = new RoutedCommand();
        public static RoutedCommand FindEmployeeCommand = new RoutedCommand();
        public static RoutedCommand FindAllEmployeesCommand = new RoutedCommand();
        public static RoutedCommand LoadAllHistoryCommand = new RoutedCommand();
        public static RoutedCommand LookupPhotoCommand = new RoutedCommand();
        public static RoutedCommand RemovePhotoCommand = new RoutedCommand();

        IEmployeeCardRepository emplCardRepo = new EmployeeCardRepository();

        #region Functionality methods
        private void SearchEmployees(bool pShowAll = false)
        {
            lbEmployees.ItemsSource = emplCardRepo.GetAll();

            //DataTable employees = null;
            //string query = "select id, fullname from ba_employees_cardindex";
            //if (pShowAll)
            //{
            //    query += ";";
            //}
            //else
            //{
            //    switch (((ComboBoxItem)comboSearchCriteria.SelectedItem).Name)
            //    {
            //        case "FullName":
            //            query += " where fullname like @data;";
            //            break;
            //        case "Phone":
            //            query += " where telephone like @data;";
            //            break;
            //    }
            //}

            //if (pShowAll)
            //{
            //    employees = App.sqlite.SelectTable(query);
            //}
            //else
            //{
            //    employees = App.sqlite.SelectTable(query,
            //                new SQLiteParameter("@data", "%" + inputSearchData.Text + "%"));
            //}

            //if (employees != null && employees.Rows.Count > 0)
            //{
            //    foreach (DataRow r in employees.Rows)
            //    {
            //        foundEmployees.Add(new Employee()
            //        {
            //            Id = Convert.ToInt32(r.ItemArray[0]),
            //            FullName = r.ItemArray[1].ToString()
            //        });
            //    }
            //}
            //else
            //{
            //    ShowMessage("Никто не найден!");
            //}

            //lbEmployees.ItemsSource = foundEmployees;
        }

        private void OpenEmployee()
        {
            //if (lbEmployees.SelectedIndex != -1)
            //{
            //    DataRow r = App.sqlite.SelectRow("select id, fullname, hired, fired, document, telephone, address, notes  from ba_employees_cardindex where id=@id;",
            //        new SQLiteParameter("@id", foundEmployees[lbEmployees.SelectedIndex].Id));
            //    if (r == null)
            //    {
            //        ShowMessage("Сотрудник не найден.");
            //        return;
            //    }
            //    openedEmployee = new Employee()
            //    {
            //        Id = Convert.ToInt32(r.ItemArray[0]),
            //        FullName = r.ItemArray[1].ToString(),
            //        Hired = r.ItemArray[2] != DBNull.Value ? (DateTime?)r.ItemArray[2] : null,
            //        Fired = r.ItemArray[3] != DBNull.Value ? (DateTime?)r.ItemArray[3] : null,
            //        Document = r.ItemArray[4].ToString(),
            //        Telephone = r.ItemArray[5].ToString(),
            //        Address = r.ItemArray[6].ToString(),
            //        Notes = r.ItemArray[7].ToString()
            //    };

            //    LoadPhoto();

            //    this.DataContext = openedEmployee;
            //    LoadSalaryHistory();
            //    ClearInputFields(false);
            //}
            throw new NotImplementedException("Not switched to ORM yet.");
        }

        private void LoadPhoto()
        {
            // retrieve photo
            //System.Drawing.Image image = App.sqlite.GetImage("select photo from ba_employees_cardindex where id=@id;",
            //    new SQLiteParameter("@id", openedEmployee.Id));

            //if (image != null)
            //{
            //    using (MemoryStream memory = new MemoryStream())
            //    {
            //        image.Save(memory, ImageFormat.Png);
            //        memory.Position = 0;
            //        BitmapImage bitmapImage = new BitmapImage();
            //        bitmapImage.BeginInit();
            //        bitmapImage.StreamSource = memory;
            //        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            //        bitmapImage.EndInit();

            //        openedEmployee.Photo = bitmapImage;
            //    }
            //}
            //else
            //{
            //    openedEmployee.Photo = new BitmapImage(new Uri("pack://application:,,,/Resources/image_noimage.png"));
            //}
            throw new NotImplementedException("Not switched to ORM yet.");
        }

        private void LoadSalaryHistory(bool all = false)
        {
            //string query =
            //    string.Format("select id, datestamp, summa, comment from ba_cash_operations where id in (select opid from ba_employees_cash where emid = @emid) order by id desc;",
            //    // TODO: add {0}
            //    all ? "" : "limit " + preloadRecordsCount);

            //salaryHistory = new List<CashTransaction>();

            //DataTable historyRecords = App.sqlite.SelectTable(query,
            //    new SQLiteParameter("@emid", openedEmployee.Id));
            //if (historyRecords != null)
            //{
            //    foreach (DataRow row in historyRecords.Rows)
            //    {
            //        salaryHistory.Add(new CashTransaction()
            //        {
            //            id = Convert.ToInt32(row.ItemArray[0].ToString()),
            //            date = Convert.ToDateTime(row.ItemArray[1]),
            //            sum = 0 - decimal.Parse(row.ItemArray[2].ToString()),
            //            comment = row.ItemArray[3].ToString()
            //        });
            //    }
            //    lvSalaryHistory.ItemsSource = salaryHistory;
            //}
            throw new NotImplementedException("Not switched to ORM yet.");
        }

        private bool SaveEmployee()
        {
            //if (openedEmployee.Id != 0)
            //{
            //    // change record
            //    return App.sqlite.ChangeData(
            //        "update ba_employees_cardindex set hired = @h, fired = @f, fullname = @name, document = @d, telephone = @t, address = @a, notes = @n where id = @id;",
            //        new SQLiteParameter("@h", openedEmployee.Hired),
            //        new SQLiteParameter("@f", openedEmployee.Fired),
            //        new SQLiteParameter("@name", openedEmployee.FullName),
            //        new SQLiteParameter("@d", openedEmployee.Document),
            //        new SQLiteParameter("@t", openedEmployee.Telephone),
            //        new SQLiteParameter("@a", openedEmployee.Address),
            //        new SQLiteParameter("@n", openedEmployee.Notes),
            //        new SQLiteParameter("@id", openedEmployee.Id)) > 0;
            //}
            //else
            //{
            //    // save new
            //    return App.sqlite.ChangeData(
            //        "insert into ba_employees_cardindex (hired, fired, fullname, document, telephone, address, notes) values (@h, @f, @name, @d, @t, @a, @n);",
            //        new SQLiteParameter("@h", openedEmployee.Hired),
            //        new SQLiteParameter("@f", openedEmployee.Fired),
            //        new SQLiteParameter("@name", openedEmployee.FullName),
            //        new SQLiteParameter("@d", openedEmployee.Document),
            //        new SQLiteParameter("@t", openedEmployee.Telephone),
            //        new SQLiteParameter("@a", openedEmployee.Address),
            //        new SQLiteParameter("@n", openedEmployee.Notes)) > 0;
            //}
            throw new NotImplementedException("Not switched to ORM yet.");
        }

        private bool DeleteEmployee()
        {
            //return App.sqlite.ChangeData("delete from ba_employees_cardindex where id=@id",
            //    new SQLiteParameter("@id", openedEmployee.Id)) > 0;
            throw new NotImplementedException("Not switched to ORM yet.");
        }

        private void ClearInputFields(bool isEnabled, bool clearValues = false)
        {
            pickerHiredDate.IsEnabled = isEnabled;
            pickerFiredDate.IsEnabled = isEnabled;
            inputEmplName.IsReadOnly = !isEnabled;
            inputEmplPhone.IsReadOnly = !isEnabled;
            inputEmplPassport.IsReadOnly = !isEnabled;
            inputEmplAddress.IsReadOnly = !isEnabled;
            inputEmplNotes.IsReadOnly = !isEnabled;

            if (clearValues)
            {
                pickerHiredDate.SelectedDate = null;
                pickerFiredDate.SelectedDate = null;
                inputEmplName.Text = "";
                inputEmplPhone.Text = "";
                inputEmplPassport.Text = "";
                inputEmplAddress.Text = "";
                inputEmplNotes.Text = "";
            }
        }

        private void ChoosePhoto()
        {
            //System.Windows.Forms.OpenFileDialog ofDialog = new System.Windows.Forms.OpenFileDialog();
            //ofDialog.AddExtension = true;
            //ofDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            //if (ofDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    if (ofDialog.FileName != "")
            //    {
            //        try
            //        {
            //            System.Drawing.Image image = System.Drawing.Image.FromFile(ofDialog.FileName); // check that file is image
            //            if (image == null)
            //            {
            //                ShowMessage("Выбранный файл не является изображением и не может быть использован в качестве фотографии!");
            //            }
            //        }
            //        catch (Exception) 
            //        {
            //            ShowMessage("Выбранный файл не является изображением и не может быть использован в качестве фотографии!");
            //        }

            //        if (!App.sqlite.PutFile(ofDialog.FileName, "update ba_employees_cardindex set photo = @file where id = @id",
            //            new SQLiteParameter("@id", openedEmployee.Id)))
            //        {
            //            ShowMessage("Не удалось сохранить фотографию сотрудника!");
            //        }
            //        else
            //        {
            //            LoadPhoto();
            //        }
            //    }
            //}
            throw new NotImplementedException("Not switched to ORM yet.");
        }

        private void ClearPhoto()
        {
            //if (App.sqlite.ChangeData("update ba_employees_cardindex set photo = null where id=@id;",
            //        new SQLiteParameter("@id", openedEmployee.Id)) > 0)
            //{
            //    LoadPhoto();
            //}
            //else
            //{
            //    ShowMessage("Не удалось удалить фотографию сотрудника!");
            //}
            throw new NotImplementedException("Not switched to ORM yet.");
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
            //        LoadSalaryHistory();
            //    }
            //}
            throw new NotImplementedException("Not switched to ORM yet.");
        }

        private void ShowMessage(string text)
        {
            //for (var visual = this as Visual; visual != null; visual = VisualTreeHelper.GetParent(visual) as Visual)
            //    if (visual is MetroWindow)
            //    {
            //        ((MetroWindow)visual).ShowMessageAsync("Проблемка", text + Environment.NewLine + App.sqlite.LastOperationErrorMessage,
            //            MessageDialogStyle.Affirmative);
            //    }
            throw new NotImplementedException("Not switched to ORM yet.");
        }
        #endregion

        #region Commands
        private void Find_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute =
                comboSearchCriteria.SelectedIndex != -1 && // search criteria is selected
                inputSearchData.Text != ""; // search key is defined
        }

        private void Find_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SearchEmployees();
        }

        private void FindAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void FindAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SearchEmployees(true);
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = lbEmployees.SelectedIndex != -1; // employee is selected
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenEmployee();
        }

        private void New_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //openedEmployee = new Employee();
            //this.DataContext = openedEmployee;
            //ClearInputFields(true);
        }

        private void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute =
            //    openedEmployee != null && 
            //    openedEmployee.Id != 0 &&
            //    !pickerHiredDate.IsEnabled;
        }

        private void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ClearInputFields(true);
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute =
            //    openedEmployee != null &&
            //    pickerHiredDate.IsEnabled &&
            //    pickerHiredDate.SelectedDate != null &&
            //    (pickerFiredDate.SelectedDate != null ? pickerHiredDate.SelectedDate <= pickerFiredDate.SelectedDate : true) &&
            //    inputEmplName.Text != "";
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //if (SaveEmployee())
            //{
            //    ClearInputFields(false, true);
            //    openedEmployee = null;
            //    this.DataContext = openedEmployee;
            //}
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute =
            //    openedEmployee != null && openedEmployee.Id != 0;
        }

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //if (DeleteEmployee())
            //{
            //    ClearInputFields(false, true);
            //    //foundEmployees.Remove(
            //    //    foundEmployees.Find(
            //    //        delegate(Employee empl) { return empl.Id == openedEmployee.Id; }));
            //    //lbEmployees.ItemsSource = foundEmployees;
            //    openedEmployee = null;
            //    this.DataContext = openedEmployee;
            //}
        }

        private void LoadAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = 
            //    openedEmployee != null &&
            //    openedEmployee.Id != 0 &&
            //    lvSalaryHistory.Items.Count <= preloadRecordsCount;
        }

        private void LoadAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LoadSalaryHistory(true);
        }

        private void LookupPhoto_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ChoosePhoto();
        }

        private void LookupPhoto_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute =
            //    openedEmployee != null && openedEmployee.Id != 0;
        }

        private void RemovePhoto_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ClearPhoto();
        }

        private void RemovePhoto_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute =
            //    openedEmployee != null && openedEmployee.Id != 0 &&
            //    openedEmployee.Photo != null;
        }
        #endregion

        private void listFoundEmpl_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenEmployee();
        }
    }
}
