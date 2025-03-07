using System.Windows;

namespace Money_Management
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();
        }

        
        private void IncomeButton_Click(object sender, RoutedEventArgs e)
        {
            
            IncomeWindow incomeWindow = new IncomeWindow();

            
            incomeWindow.Show();

            
            this.Close();
        }

        private void ExpensesButton_Click(object sender, RoutedEventArgs e)
        {
            
            ExpensesWindow expensesWindow = new ExpensesWindow();

            
            expensesWindow.Show();

            
            this.Close();
        }
    }
}