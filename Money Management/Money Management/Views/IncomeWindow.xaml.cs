using System;
using System.Windows;

namespace Money_Management.Views
{
    public partial class IncomeWindow : Window
    {
        private DashboardWindow dashboardWindow;
        private decimal totalIncome = 0;

        public IncomeWindow(DashboardWindow dashboardWindow)
        {
            InitializeComponent();
            this.dashboardWindow = dashboardWindow;
            IncomeDatePicker.SelectedDate = DateTime.Now;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            dashboardWindow.Show();
            this.Close();
        }

        private void AddIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            string source = IncomeSourceTextBox.Text.Trim();
            string amountText = IncomeAmountTextBox.Text.Trim();
            DateTime? date = IncomeDatePicker.SelectedDate;

            // Validate income source
            if (string.IsNullOrWhiteSpace(source))
            {
                MessageBox.Show("Please enter an income source.", "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate income amount
            if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid income amount.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate date
            if (date == null)
            {
                MessageBox.Show("Please select a valid date.", "Missing Date", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Add to the ListBox
            IncomeListBox.Items.Add($"{date.Value.ToShortDateString()} - {source} - Rs.{amount}");

            // Update the total income
            totalIncome += amount;
            TotalIncomeTextBlock.Text = $"Total Income: Rs.{totalIncome}";

            // Clear the input fields
            IncomeSourceTextBox.Clear();
            IncomeAmountTextBox.Clear();
            IncomeDatePicker.SelectedDate = DateTime.Now;
        }
    }
}

