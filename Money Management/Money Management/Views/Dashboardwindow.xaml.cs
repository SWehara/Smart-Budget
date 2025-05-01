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
            ExpensesWindow expensesWindow = new ExpensesWindow();
            expensesWindow.Show();
            this.Hide();
        }

        private void BudgetButton_Click(object sender, RoutedEventArgs e)
        {
            // Your logic for Budget button
            MessageBox.Show("Budget button clicked.");
        }


        private void GoalsButton_Click(object sender, RoutedEventArgs e)
        {
            // Add your logic here (e.g., open a goals window)
            MessageBox.Show("Goals button clicked.");

        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
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
