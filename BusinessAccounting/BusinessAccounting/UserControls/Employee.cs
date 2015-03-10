using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace BusinessAccounting.UserControls
{
    public class Employee : INotifyPropertyChanged 
    {
        private int _id;
        private DateTime? _hired;
        private DateTime? _fired;
        private string _fullname;
        private string _document;
        private string _telephone;
        private string _address;
        private string _notes;
        private BitmapImage _photo;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public DateTime? Hired
        {
            get
            {
                return _hired;
            }
            set
            {
                _hired = value;
                OnPropertyChanged("Hired");
            }
        }

        public DateTime? Fired
        {
            get
            {
                return _fired;
            }
            set
            {
                _fired = value;
                OnPropertyChanged("Fired");
            }
        }

        public string FullName
        {
            get
            {
                return _fullname;
            }
            set
            {
                _fullname = value;
                OnPropertyChanged("FullName");
            }
        }

        public string Document
        {
            get
            {
                return _document;
            }
            set
            {
                _document = value;
                OnPropertyChanged("Document");
            }
        }

        public string Telephone
        {
            get
            {
                return _telephone;
            }
            set
            {
                _telephone = value;
                OnPropertyChanged("Telephone");
            }
        }

        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        public string Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                _notes = value;
                OnPropertyChanged("Notes");
            }
        }

        public BitmapImage Photo
        {
            get
            {
                return _photo;
            }
            set
            {
                _photo = value;
                OnPropertyChanged("Photo");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
