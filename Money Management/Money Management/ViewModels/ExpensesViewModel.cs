using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Money_Management.ViewModels
{
    public class ExpensesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double _totalExpenses;
        public double TotalExpenses
        {
            get { return _totalExpenses; }
            set
            {
                _totalExpenses = value;
                OnPropertyChanged(nameof(TotalExpenses));
            }
        }

        public ObservableCollection<ExpenseRecord> ExpenseRecords { get; set; }

        public ExpensesViewModel()
        {
            ExpenseRecords = new ObservableCollection<ExpenseRecord>();
        }

        public void AddExpense(ExpenseRecord expense)
        {
            ExpenseRecords.Add(expense);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ExpenseRecord
    {
        public string Date { get; set; }
        public string Category { get; set; }
        public double Amount { get; set; }
        public string Notes { get; set; }
    }
}
