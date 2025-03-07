using System.Windows;

namespace Money_Management
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();
        }

        // Add this method to handle the Income button click
        private void IncomeButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the IncomeWindow
            IncomeWindow incomeWindow = new IncomeWindow();

            // Show the IncomeWindow
            incomeWindow.Show();

            // Optionally, close the DashboardWindow (if needed)
            this.Close();
        }
    }
}