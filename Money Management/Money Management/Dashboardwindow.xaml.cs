using System;
using System.Windows;
using System.Windows.Controls;

namespace YourNamespace
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent(); // Ensure this is present
        }

        // Event handler for the Settings button
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Settings button clicked!");
        }

        // Event handler for the Help button
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Help button clicked!");
        }

        // Event handler for the Bill Reminders button
        private void BillRemindersButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Bill Reminders button clicked!");
        }

        // Event handler for the Weekly Report button
        private void WeeklyReportButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Weekly Report button clicked!");
        }

        // Event handler for the Profile Details button
        private void ProfileDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Profile Details button clicked!");
        }
    }
}
