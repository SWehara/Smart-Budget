using LiveCharts;
using LiveCharts.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Money_Management.Helpers;


namespace Money_Management.ViewModel
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private double totalIncome;
        private double totalExpenses;
        private double remainingBudget;

        public SeriesCollection PieSeries { get; set; }

        public double TotalIncome
        {
            get => totalIncome;
            set
            {
                if (totalIncome != value)
                {
                    totalIncome = value;
                    OnPropertyChanged();
                    UpdateRemainingBudget();
                }
            }
        }

        public double TotalExpenses
        {
            get => totalExpenses;
            set
            {
                if (totalExpenses != value)
                {
                    totalExpenses = value;
                    OnPropertyChanged();
                    UpdateRemainingBudget();
                }
            }
        }

        public double RemainingBudget
        {
            get => remainingBudget;
            private set
            {
                if (remainingBudget != value)
                {
                    remainingBudget = value;
                    OnPropertyChanged();
                }
            }
        }

        public DashboardViewModel()
        {
            TotalIncome = 20000;
            TotalExpenses = 15000;

            PieSeries = new SeriesCollection
            {
                new PieSeries { Title = "Food", Values = new ChartValues<double> { 40 }, DataLabels = true },
                new PieSeries { Title = "Travel", Values = new ChartValues<double> { 25 }, DataLabels = true },
                new PieSeries { Title = "Bills", Values = new ChartValues<double> { 20 }, DataLabels = true },
                new PieSeries { Title = "Other", Values = new ChartValues<double> { 15 }, DataLabels = true }
            };
        }

        private void UpdateRemainingBudget()
        {
            RemainingBudget = TotalIncome - TotalExpenses;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
