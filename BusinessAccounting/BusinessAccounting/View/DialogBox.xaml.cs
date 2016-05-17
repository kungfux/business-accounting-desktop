using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BusinessAccounting.View
{
    /// <summary>
    /// Interaction logic for ExceptionDialog.xaml
    /// </summary>
    public partial class DialogBox : Window, INotifyPropertyChanged
    {
        public DialogBox(Window owner, string pMessage, DialogBoxType pType, Exception pException = null)
        {
            InitializeComponent();

            if (owner == null && 
                (Application.Current.MainWindow == null ||
                Application.Current.MainWindow.GetType() != typeof(MainWindow)))
            {
                // Low-level error occur
                if (pException != null)
                {
                    BoxMessage = pException.Message;
                }
                else
                {
                    BoxMessage = this.FindResource("AppLowLevelExceptionMessage").ToString();
                }

                MessageBox.Show(BoxMessage, this.FindResource("AppName").ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
                App.Current.Shutdown();
                return;
            }

            Owner = owner ?? Application.Current.MainWindow;

            switch(pType)
            {
                case DialogBoxType.Information:
                    BoxAutoTitle = this.FindResource("DialogBoxMessageCaption").ToString().ToUpper();
                    BoxAutoIcon = @"pack://application:,,,/BusinessAccounting;component/Resources/Images/Information.png";
                    BoxAutoBackgroundColor = new SolidColorBrush(Colors.Blue);
                    break;
                case DialogBoxType.Warning:
                    BoxAutoTitle = this.FindResource("DialogBoxWarningCaption").ToString().ToUpper();
                    BoxAutoIcon = @"pack://application:,,,/BusinessAccounting;component/Resources/Images/Warning.png";
                    BoxAutoBackgroundColor = new SolidColorBrush(Colors.Orange);
                    break;
                case DialogBoxType.Error:
                    BoxAutoTitle = this.FindResource("DialogBoxErrorCaption").ToString().ToUpper();
                    BoxAutoIcon = @"pack://application:,,,/BusinessAccounting;component/Resources/Images/Error.png";
                    BoxAutoBackgroundColor = new SolidColorBrush(Colors.Red);
                    break;
                default:
                    BoxAutoTitle = "ERROR";
                    break;
            }

            BoxMessage = pMessage;

            if (pException != null)
            {
                BoxDetailsMessage = string.Concat(pException.Message, Environment.NewLine, pException.StackTrace);
            }
        }

        private string _boxAutoTitle;
        public string BoxAutoTitle
        {
            get { return _boxAutoTitle; }
            set
            {
                _boxAutoTitle = value;
                OnPropertyChanged("BoxAutoTitle");
            }
        }

        private string _boxAutoIcon;
        public string BoxAutoIcon
        {
            get { return _boxAutoIcon; }
            set
            {
                _boxAutoIcon = value;
                OnPropertyChanged("BoxAutoIcon");
            }
        }

        private Brush _boxAutoBackgroundColor;
        public Brush BoxAutoBackgroundColor
        {
            get { return _boxAutoBackgroundColor; }
            set
            {
                _boxAutoBackgroundColor = value;
                OnPropertyChanged("BoxAutoBackgroundColor");
            }
        }

        private string _boxMessage;
        public string BoxMessage
        {
            get { return _boxMessage; }
            set
            {
                _boxMessage = value;
                OnPropertyChanged("BoxMessage");
            }
        }

        private string _boxDetailsMessage;
        public string BoxDetailsMessage
        {
            get { return _boxDetailsMessage; }
            set
            {
                _boxDetailsMessage = value;
                OnPropertyChanged("BoxDetailsMessage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
