using System;
using System.Windows;
using Microsoft.Win32;  

namespace Money_Management.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                UserProfileImage1.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(openFileDialog.FileName));
                UserProfileImage1.Visibility = Visibility.Visible;
                UploadButton1.Content = "Change Profile Picture";
            }
        }

        
        private void ProceedButton1_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox1.Text;
            string password = PasswordBox1.Password;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both name and password.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.", "Invalid Password", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            DashboardWindow dashboardWindow = new DashboardWindow();
            dashboardWindow.Show();
            this.Close();
        }
    }
}
