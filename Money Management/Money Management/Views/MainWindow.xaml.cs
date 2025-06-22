using System;
using System.Windows;
using Microsoft.Win32;
using MySql.Data.MySqlClient;

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
            string username = NameTextBox1.Text.Trim();
            string password = PasswordBox1.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.", "Invalid Password", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (IsValidUser(username, password))
            {
                DashboardWindow dashboardWindow = new DashboardWindow(username);
                dashboardWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidUser(string username, string password)
        {
            string connectionString = "server=localhost;user id=root;password=@12345$Sw;database=smartgoaldb";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        long count = (long)command.ExecuteScalar();
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        private void OpenRegisterWindow_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            register.ShowDialog();
        }
    }
}
