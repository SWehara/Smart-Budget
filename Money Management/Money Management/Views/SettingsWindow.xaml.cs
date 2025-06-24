using System;
using System.Windows;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace Money_Management.Views
{
    public partial class SettingsWindow : Window
    {
        private string connectionString = "server=localhost;user id=root;password=@12345$Sw;database=smartgoaldb";
        private string currentUsername;

        public SettingsWindow(string username)
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            currentUsername = username;
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            string oldPwd = OldPasswordBox.Password.Trim();
            string newPwd = NewPasswordBox.Password.Trim();
            string confirmPwd = ConfirmPasswordBox.Password.Trim();

            if (!string.IsNullOrWhiteSpace(oldPwd) || !string.IsNullOrWhiteSpace(newPwd))
            {
                if (newPwd != confirmPwd)
                {
                    StatusTextBlock.Text = "New passwords do not match.";
                    StatusTextBlock.Foreground = Brushes.Red;
                    return;
                }

                try
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        string checkPwdQuery = "SELECT password FROM users WHERE name = @username";
                        using (var checkCmd = new MySqlCommand(checkPwdQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@username", currentUsername);
                            string currentPwd = checkCmd.ExecuteScalar()?.ToString();

                            if (currentPwd != oldPwd)
                            {
                                StatusTextBlock.Text = "Old password is incorrect.";
                                StatusTextBlock.Foreground = Brushes.Red;
                                return;
                            }
                        }

                        string updatePwdQuery = "UPDATE users SET password = @newPwd WHERE name = @username";
                        using (var pwdCmd = new MySqlCommand(updatePwdQuery, conn))
                        {
                            pwdCmd.Parameters.AddWithValue("@newPwd", newPwd);
                            pwdCmd.Parameters.AddWithValue("@username", currentUsername);
                            pwdCmd.ExecuteNonQuery();
                        }

                        StatusTextBlock.Text = "Password updated successfully.";
                        StatusTextBlock.Foreground = Brushes.LightGreen;
                    }
                }
                catch (Exception ex)
                {
                    StatusTextBlock.Text = "Error saving settings: " + ex.Message;
                    StatusTextBlock.Foreground = Brushes.Red;
                }
            }
        }

        private void DeleteAccountButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete your account? This cannot be undone.",
                                         "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM users WHERE username = @username";
                        using (var cmd = new MySqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@username", currentUsername);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Account deleted successfully.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

                    
                    new RegisterWindow().Show();

                    
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting account: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DashboardWindow dashboard = new DashboardWindow(currentUsername);
            dashboard.Show();
            this.Close();
        }
    }
}
