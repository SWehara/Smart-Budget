using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace YourNamespace
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); // Ensure this is present
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ProfileImage.Source = new BitmapImage(new System.Uri(openFileDialog.FileName));
            }
        }

        private void ProceedButton_Click(object sender, RoutedEventArgs e)
        {
            string userName = NameTextBox.Text;
            if (!string.IsNullOrWhiteSpace(userName))
            {
                // Open the Dashboard Window
                DashboardWindow dashboardWindow = new DashboardWindow();
                dashboardWindow.Show(); // Show the new window

                this.Close(); // Close the current window (optional)
            }
            else
            {
                MessageBox.Show("Please enter your name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}