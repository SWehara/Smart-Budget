using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using Money_Management.ViewModel;

namespace Money_Management.Views
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();

            // Create an instance of the DashboardViewModel
            DashboardViewModel viewModel = new DashboardViewModel
            {
                // Initial data for the pie chart (these can be dynamic)
                FoodSeries = 5000,    // Example value for Food
                TravelSeries = 3000,  // Example value for Travel
                BillsSeries = 8000,   // Example value for Bills
                OtherSeries = 2000    // Example value for Other
            };

            // Set the DataContext for the window to this instance
            this.DataContext = viewModel;
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
            BudgetWindow budgetWindow = new BudgetWindow();
            budgetWindow.ShowDialog();
        }

        private void ViewReports_Click(object sender, RoutedEventArgs e)
        {
            ReportsWindow reportsWindow = new ReportsWindow();
            reportsWindow.ShowDialog();
        }
    }
}
