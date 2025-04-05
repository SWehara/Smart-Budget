using Money_Management.Views;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Money_Management.Views
{
    public partial class IncomeWindow : Window
    {
        private List<double> incomeList = new List<double>();

        public IncomeWindow()
        {
            InitializeComponent();
        }

        private void AddIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if the income source and amount are provided
            if (string.IsNullOrEmpty(IncomeSourceTextBox.Text) || string.IsNullOrEmpty(IncomeAmountTextBox.Text))
            {
                MessageBox.Show("Please enter both income source and amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Try parsing the income amount
            if (double.TryParse(IncomeAmountTextBox.Text, out double incomeAmount))
            {
                // Add the income amount to the list
                incomeList.Add(incomeAmount);

                // Calculate the total income and update the TextBlock
                double totalIncome = CalculateTotalIncome();
                TotalIncomeTextBlock.Text = $"Total Income: Rs.{totalIncome:N2}";

                // Clear the input fields
                IncomeSourceTextBox.Clear();
                IncomeAmountTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a valid income amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to calculate the total income
        private double CalculateTotalIncome()
        {
            double total = 0;
            foreach (double amount in incomeList)
            {
                total += amount;
            }
            return total;
        }

        // Home button click handler to navigate to Dashboard
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            DashboardWindow dashboardWindow = new DashboardWindow();
            dashboardWindow.Show();
            this.Close();
        }
    }
}

