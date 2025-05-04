using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Money_Management.Views
{
    public partial class BudgetWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<BudgetCategory> Categories { get; set; } = new ObservableCollection<BudgetCategory>();
        public ObservableCollection<Goal> Goals { get; set; } = new ObservableCollection<Goal>();

        private double monthlyIncome;
        public double MonthlyIncome
        {
            get => monthlyIncome;
            set
            {
                monthlyIncome = value;
                OnPropertyChanged(nameof(MonthlyIncome));
                UpdateRemainingBudget();
            }
        }

        private double remainingBudget;
        public double RemainingBudget
        {
            get => remainingBudget;
            set
            {
                remainingBudget = value;
                OnPropertyChanged(nameof(RemainingBudget));
            }
        }

        // ✅ Corrected constructor name
        public BudgetWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Example mock data
            Categories.Add(new BudgetCategory { Name = "Food", Limit = 3000, Spent = 2200 });
            Categories.Add(new BudgetCategory { Name = "Transport", Limit = 1500, Spent = 1700 });
            Categories.Add(new BudgetCategory { Name = "Eating Out", Limit = 2000, Spent = 1800 });

            Goals.Add(new Goal { Name = "Save for New Phone", Target = 5000, Saved = 2500 });

            UpdateRemainingBudget();
        }

        private void SetIncome_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(IncomeInput.Text, out double income))
            {
                MonthlyIncome = income;
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            Categories.Add(new BudgetCategory { Name = "New Category", Limit = 0, Spent = 0 });
        }

        private void UpdateRemainingBudget()
        {
            double totalSpent = Categories.Sum(c => c.Spent);
            RemainingBudget = MonthlyIncome - totalSpent;

            if (RemainingSummary != null)
            {
                RemainingSummary.Text = $"You have Rs. {RemainingBudget:N0} left this month";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class BudgetCategory
    {
        public string Name { get; set; }
        public double Limit { get; set; }
        public double Spent { get; set; }
        public double Remaining => Limit - Spent;
    }

    public class Goal
    {
        public string Name { get; set; }
        public double Target { get; set; }
        public double Saved { get; set; }
        public double Progress => Target == 0 ? 0 : (Saved / Target) * 100;
    }
}
