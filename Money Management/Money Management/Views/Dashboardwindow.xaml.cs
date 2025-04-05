using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Money_Management.Views
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

        private void ProfileImage_Click(object sender, RoutedEventArgs e)
        {
            Profilewindow profileWindow = new Profilewindow(); // Replace with actual Profile window class
            profileWindow.Show(); // Open Profile window
            this.Close(); // Close the current window (Dashboard window)
        }
    }
}