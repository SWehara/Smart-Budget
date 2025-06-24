using System.Windows;
using Money_Management.ViewModel;

namespace Money_Management.Views
{
    public partial class DashboardWindow : Window
    {
        private DashboardViewModel viewModel;
        private string currentUsername;

        public DashboardWindow(string username)
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            currentUsername = username;
            viewModel = new DashboardViewModel();
            this.DataContext = viewModel;

            
            GreetingTextBlock.Text = $"Welcome back, {currentUsername}";
        }

        private void IncomeButton_Click(object sender, RoutedEventArgs e)
        {
            IncomeWindow incomeWindow = new IncomeWindow(currentUsername);
            incomeWindow.Show();
            this.Hide();
        }

        private void ExpensesButton_Click(object sender, RoutedEventArgs e)
        {
            ExpensesWindow expensesWindow = new ExpensesWindow(currentUsername);
            expensesWindow.Show();
            this.Hide();
        }

        private void BudgetButton_Click(object sender, RoutedEventArgs e)
        {
            BudgetWindow budgetWindow = new BudgetWindow(currentUsername);
            budgetWindow.Show();
            this.Hide();
        }

        private void GoalsButton_Click(object sender, RoutedEventArgs e)
        {
            GoalsWindow goalsWindow = new GoalsWindow(currentUsername);
            goalsWindow.Show();
            this.Hide();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow(currentUsername);
            helpWindow.Show();
            this.Hide();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(currentUsername);
            settingsWindow.Show();
            this.Close();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfileWindow profileWindow = new ProfileWindow(currentUsername);
            profileWindow.Show();
            this.Close();
        }


        private void MonthReportButton_Click(object sender, RoutedEventArgs e)
        {
            ReportsWindow reportsWindow = new ReportsWindow(currentUsername);
            reportsWindow.Show();
            this.Hide();
        }


        public void UpdateIncome(double amount)
        {
            viewModel.TotalIncome += amount;
        }

        public void UpdateExpense(double amount)
        {
            viewModel.TotalExpenses += amount;
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
