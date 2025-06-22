using System;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace Money_Management.Views
{
    public partial class Profilewindow : Window
    {
        private string currentUsername;
        private string connectionString = "server=localhost;user id=root;password=@12345$Sw;database=smartgoaldb";

        public Profilewindow(string username)
        {
            InitializeComponent();
            currentUsername = username;
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT name, password FROM users WHERE name = @name";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", currentUsername);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                NameTextBox.Text = reader.GetString("name");
                                PasswordBox.Password = reader.GetString("password");
                            }
                            else
                            {
                                MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load profile: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newName = NameTextBox.Text.Trim();
            string newPassword = PasswordBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Both name and password are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    
                    if (newName != currentUsername)
                    {
                        string checkQuery = "SELECT COUNT(*) FROM users WHERE name = @newName";
                        using (var checkCmd = new MySqlCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@newName", newName);
                            long exists = (long)checkCmd.ExecuteScalar();
                            if (exists > 0)
                            {
                                MessageBox.Show("Username already exists. Choose a different name.");
                                return;
                            }
                        }
                    }

                    string updateQuery = "UPDATE users SET name = @newName, password = @newPassword WHERE name = @currentName";
                    using (var cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@newName", newName);
                        cmd.Parameters.AddWithValue("@newPassword", newPassword);
                        cmd.Parameters.AddWithValue("@currentName", currentUsername);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Profile updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                currentUsername = newName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update profile: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

