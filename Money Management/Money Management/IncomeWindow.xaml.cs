using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

namespace Money_Management
{
    public partial class IncomeWindow : Window
    {
        // List to store income amounts
        private List<double> incomeList = new List<double>();

        public IncomeWindow()
        {
            InitializeComponent();
        }

        // Event handler for the "Add Income" button
        private void AddIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input
            if (string.IsNullOrEmpty(IncomeSourceTextBox.Text) || string.IsNullOrEmpty(IncomeAmountTextBox.Text))
            {
                MessageBox.Show("Please enter both income source and amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Try to parse the income amount
            if (double.TryParse(IncomeAmountTextBox.Text, out double incomeAmount))
            {
                // Add the income amount to the list
                incomeList.Add(incomeAmount);

                // Calculate the total income
                double totalIncome = CalculateTotalIncome();

                // Update the total income display
                TotalIncomeTextBlock.Text = $"Total Income: {totalIncome:N2}";

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
    }
}