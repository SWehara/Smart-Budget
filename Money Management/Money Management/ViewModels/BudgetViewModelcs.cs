using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Money_Management.ViewModels
{
    public class BudgetViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<BudgetCategory> Categories { get; set; } = new ObservableCollection<BudgetCategory>();
        public ObservableCollection<Goal> Goals { get; set; } = new ObservableCollection<Goal>();

        private double income;
        public double Income
        {
            get => income;
            set
            {
                income = value;
                OnPropertyChanged(nameof(Income));
                UpdateProgress();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void AddCategory(string name, double limit)
        {
            Categories.Add(new BudgetCategory { Name = name, Limit = limit });
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            foreach (var category in Categories)
            {
                category.Remaining = category.Limit - category.Spent;
            }

            foreach (var goal in Goals)
            {
                goal.Progress = Income > 0 ? (goal.Saved / goal.Target) * 100 : 0;
            }
        }
    }

    public class BudgetCategory
    {
        public string Name { get; set; }
        public double Limit { get; set; }
        public double Spent { get; set; }
        public double Remaining { get; set; }
    }

    public class Goal
    {
        public string Name { get; set; }
        public double Target { get; set; }
        public double Saved { get; set; }
        public double Progress { get; set; }
    }
}
