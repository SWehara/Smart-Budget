using System;
using System.Windows;

namespace Money_Management.Views
{
    public partial class IncomeWindow : Window
    {
        // Store the reference to the DashboardWindow
        private DashboardWindow dashboardWindow;

        // Updated constructor to accept a DashboardWindow instance
        public IncomeWindow(DashboardWindow dashboardWindow)
        {
            InitializeComponent();
            this.dashboardWindow = dashboardWindow;
        }

        // Home Button Click: Return to the DashboardWindow
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            // Show the dashboard window and close the IncomeWindow
            dashboardWindow.Show();
            this.Close();
        }

        // Add Income Button Click: Update the total income display
        private void AddIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            // Example implementation to update the total income text block
            int income;
            if (int.TryParse(IncomeAmountTextBox.Text, out income))
            {
                // Parse the current total income from the text block (assumes "Total Income: Rs.0" format)
                int currentIncome = 0;
                string totalText = TotalIncomeTextBlock.Text.Replace("Total Income: Rs.", "").Trim();
                int.TryParse(totalText, out currentIncome);

                // Update total income
                currentIncome += income;
                TotalIncomeTextBlock.Text = $"Total Income: Rs.{currentIncome}";
            }
            else
            {
                MessageBox.Show("Please enter a valid income amount.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
