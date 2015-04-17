using System.ComponentModel;
using System.Drawing;

namespace BusinessAccounting.ViewModel
{
    class HeaderCompany : INotifyPropertyChanged
    {
        public string CompanyLogo 
        { 
            get 
            { 
                return _companyLogo; 
            } 
            set 
            {
                _companyLogo = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CompanyLogo"));
            } 
        }
        
        public string CompanyName
        { 
            get 
            { 
                return _companyName; 
            } 
            set 
            {
                _companyName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CompanyName"));
            } 
        }

        string _companyLogo = null;
        string _companyName = null;

        public HeaderCompany()
        {
            CompanyLogo = @"pack://application:,,,/BusinessAccounting;component/Resources/Logo.png";
            CompanyName = "PREVIEW COMPANY NAME";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
