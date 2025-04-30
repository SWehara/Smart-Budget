using System.Windows;
using Money_Management.ViewModel;

namespace Money_Management.Views
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();

            // Set the DataContext with default pie chart values
            this.DataContext = new DashboardViewModel();
        }

        private void IncomeButton_Click(object sender, RoutedEventArgs e)
        {
            IncomeWindow incomeWindow = new IncomeWindow(this);
            incomeWindow.Show();
            this.Hide();
        }

        private void ExpensesButton_Click(object sender, RoutedEventArgs e)
        {
            ExpensesWindow expensesWindow = new ExpensesWindow();
            expensesWindow.Show();
            this.Hide();
        }

        private void ProfileImage_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Profilewindow profileWindow = new Profilewindow();
            profileWindow.Show();
            this.Close();
        }

        private void AddIncome_Click(object sender, RoutedEventArgs e)
        {
            IncomeWindow incomeWindow = new IncomeWindow(this);
            incomeWindow.Show();
            this.Hide();
        }

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            ExpensesWindow expensesWindow = new ExpensesWindow();
            expensesWindow.Show();
            this.Hide();
        }

        private void SetBudget_Click(object sender, RoutedEventArgs e)
        {
            Budget budgetWindow = new Budget();
            budgetWindow.ShowDialog();
        }

        private void ViewReports_Click(object sender, RoutedEventArgs e)
        {
            ReportsWindow reportsWindow = new ReportsWindow();
            reportsWindow.ShowDialog();
        }
    }
}
