using System;
using System.Windows;
using Microsoft.Win32;  // For file dialog

namespace Money_Management.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Event handler for the Upload button
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            // Open file dialog to select an image
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Set the image source to the selected file
                UserProfileImage1.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(openFileDialog.FileName));
                UserProfileImage1.Visibility = Visibility.Visible; // Show the image
                UploadButton1.Content = "Change Profile Picture"; // Change button text after image is uploaded
            }
        }

        // Event handler for the Proceed button
        private void ProceedButton_Click(object sender, RoutedEventArgs e)
        {
            // Basic validation for name and password
            string name = NameTextBox1.Text;
            string password = PasswordBox1.Password;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both name and password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Open the Dashboard Window
            DashboardWindow dashboardWindow = new DashboardWindow();
            dashboardWindow.Show();  // Open the Dashboard Window

            this.Close();  // Close the MainWindow (to not keep the user on the initial window)
        }
    }
}

