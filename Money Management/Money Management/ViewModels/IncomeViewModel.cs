using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Money_Management.ViewModels
{
    public class IncomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double _currentIncome;
        public double CurrentIncome
        {
            get { return _currentIncome; }
            set
            {
                _currentIncome = value;
                OnPropertyChanged(nameof(CurrentIncome));
            }
        }

        public ObservableCollection<IncomeRecord> IncomeRecords { get; set; }

        public IncomeViewModel()
        {
            IncomeRecords = new ObservableCollection<IncomeRecord>();
            
        }

        public void AddIncome(IncomeRecord income)
        {
            IncomeRecords.Add(income);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class IncomeRecord
    {
        public string Date { get; set; }
        public string Source { get; set; }
        public double Amount { get; set; }
    }
}
