using System.Windows;
using Money_Management.ViewModel;

namespace Money_Management.Views
{
    public partial class DashboardWindow : Window
    {
        private DashboardViewModel viewModel;

        public DashboardWindow()
        {
            InitializeComponent();
            viewModel = new DashboardViewModel();
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
            ExpensesWindow expensesWindow = new ExpensesWindow(this);
            expensesWindow.Show();
            this.Hide();
        }

        private void BudgetButton_Click(object sender, RoutedEventArgs e)
        {
            
            BudgetWindow budgetWindow = new BudgetWindow();
            budgetWindow.Show();
            this.Hide();
        }

        private void GoalsButton_Click(object sender, RoutedEventArgs e)
        {

            GoalsWindow goalsWindow = new GoalsWindow();
            goalsWindow.Show();
            this.Hide();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
            this.Hide();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        private void ProfileImage_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Profilewindow profileWindow = new Profilewindow();
            profileWindow.Show();
            this.Close();
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
