using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System.Windows.Controls;

namespace Money_Management.Views
{
    public partial class SettingsWindow : Window
    {
        private string connectionString = "server=localhost;user id=root;password=@12345$Sw;database=smartgoaldb";
        private string currentUsername;
        private string selectedImagePath = null;

        public SettingsWindow(string username)
        {
            InitializeComponent();
            currentUsername = username;
            LoadUserSettings();
        }

        private void LoadUserSettings()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT currency, profile_image_path, dark_mode FROM users WHERE name = @username";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", currentUsername);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string currency = reader["currency"]?.ToString();
                                bool darkMode = Convert.ToBoolean(reader["dark_mode"]);
                                selectedImagePath = reader["profile_image_path"]?.ToString();

                                
                                if (!string.IsNullOrEmpty(currency))
                                {
                                    CurrencyComboBox.SelectedItem = CurrencyComboBox.Items.Cast<ComboBoxItem>()
                                        .FirstOrDefault(item => item.Content.ToString() == currency);
                                }

                                DarkModeCheckBox.IsChecked = darkMode;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = "Error loading settings: " + ex.Message;
                StatusTextBlock.Foreground = Brushes.Red;
            }
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            string currency = (CurrencyComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "LKR";
            bool darkMode = DarkModeCheckBox.IsChecked == true;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string updateQuery = "UPDATE users SET currency = @currency, profile_image_path = @imagePath, dark_mode = @darkMode WHERE name = @username";
                    using (var cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@currency", currency);
                        cmd.Parameters.AddWithValue("@imagePath", selectedImagePath ?? "");
                        cmd.Parameters.AddWithValue("@darkMode", darkMode);
                        cmd.Parameters.AddWithValue("@username", currentUsername);
                        cmd.ExecuteNonQuery();
                    }
                }

                StatusTextBlock.Text = "Settings saved successfully.";
                StatusTextBlock.Foreground = Brushes.LightGreen;
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = "Error saving settings: " + ex.Message;
                StatusTextBlock.Foreground = Brushes.Red;
            }
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png"
            };

            if (dialog.ShowDialog() == true)
            {
                selectedImagePath = dialog.FileName;
                StatusTextBlock.Text = "Profile image selected.";
                StatusTextBlock.Foreground = Brushes.LightGreen;
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
