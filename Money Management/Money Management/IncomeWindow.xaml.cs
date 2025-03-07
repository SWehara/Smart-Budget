using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Money_Management
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
            if (string.IsNullOrEmpty(IncomeSourceTextBox.Text) || string.IsNullOrEmpty(IncomeAmountTextBox.Text))
            {
                MessageBox.Show("Please enter both income source and amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (double.TryParse(IncomeAmountTextBox.Text, out double incomeAmount))
            {
                incomeList.Add(incomeAmount);
                double totalIncome = CalculateTotalIncome();
                TotalIncomeTextBlock.Text = $"Total Income: {totalIncome:N2}";
                IncomeSourceTextBox.Clear();
                IncomeAmountTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a valid income amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private double CalculateTotalIncome()
        {
            double total = 0;
            foreach (double amount in incomeList)
            {
                total += amount;
            }
            return total;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DashboardWindow dashboardWindow = new DashboardWindow();
            dashboardWindow.Show();
            this.Close();
        }
    }
}
