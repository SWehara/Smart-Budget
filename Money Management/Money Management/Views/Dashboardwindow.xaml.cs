using System.Windows;

namespace Money_Management.Views
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();
        }

        // Income Button Click: Open the IncomeWindow and hide the DashboardWindow
        private void IncomeButton_Click(object sender, RoutedEventArgs e)
        {
            IncomeWindow incomeWindow = new IncomeWindow(this); // Pass the current DashboardWindow instance to IncomeWindow
            incomeWindow.Show();
            this.Hide();
        }

        // Expenses Button Click: Open ExpensesWindow (ensure ExpensesWindow is implemented)
        private void ExpensesButton_Click(object sender, RoutedEventArgs e)
        {
            ExpensesWindow expensesWindow = new ExpensesWindow();
            expensesWindow.Show();
        }

        // Profile Image Click: Open the Profile Window and close the DashboardWindow
        private void ProfileImage_Click(object sender, RoutedEventArgs e)
        {
            Profilewindow profileWindow = new Profilewindow();
            profileWindow.Show();
            this.Close();
        }
    }
}
