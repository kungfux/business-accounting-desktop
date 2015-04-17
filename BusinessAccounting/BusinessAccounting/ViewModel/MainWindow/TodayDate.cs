using System;
using System.ComponentModel;
using System.Timers;

namespace BusinessAccounting.ViewModel.MainWindow
{
    class TodayDate : INotifyPropertyChanged
    {
        public string Text 
        { 
            get 
            {
                return _textToDisplay; 
            } 
        }

        private string _textToDisplay = null;

        public TodayDate()
        {
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _textToDisplay = DateTime.Now.ToString("dd MMM yy") + Environment.NewLine + DateTime.Now.DayOfWeek;
            PropertyChanged(this, new PropertyChangedEventArgs("Text"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
