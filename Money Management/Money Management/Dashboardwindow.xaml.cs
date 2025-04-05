using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        }

        private void ExpensesButton_Click(object sender, RoutedEventArgs e)
        {
            ExpensesWindow expensesWindow = new ExpensesWindow();
            expensesWindow.Show();
        }
    }
}