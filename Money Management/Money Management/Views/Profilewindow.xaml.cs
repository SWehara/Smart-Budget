using System;
using System.Windows;
using MySql.Data.MySqlClient;
using Money_Management.Views; 


namespace Money_Management.Views
{
    public partial class ProfileWindow : Window
    {
        private readonly string _username;
        private readonly string connectionString = "server=localhost;user id=root;password=@12345$Sw;database=smartgoaldb";

        public ProfileWindow(string username)
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            _username = username;
            LoadUserData();
        }

        private void LoadUserData()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT username, full_name, email, phone, date_registered FROM users WHERE username = @username";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", _username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UsernameBox.Text = reader["username"].ToString();
                            FullNameBox.Text = reader["full_name"].ToString();
                            EmailBox.Text = reader["email"].ToString();
                            PhoneBox.Text = reader["phone"].ToString();

                            if (reader["date_registered"] != DBNull.Value)
                                DateRegisteredBox.Text = Convert.ToDateTime(reader["date_registered"]).ToString("yyyy-MM-dd");
                            else
                                DateRegisteredBox.Text = "N/A";
                        }
                    }
                }
            }
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameBox.Text.Trim();
            string email = EmailBox.Text.Trim();
            string phone = PhoneBox.Text.Trim();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE users SET full_name = @fullName, email = @email, phone = @phone WHERE username = @username";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@fullName", fullName);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@username", _username);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Profile updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DashboardWindow dashboard = new DashboardWindow(_username);
            dashboard.Show();
            this.Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                

                MainWindow loginWindow = new MainWindow();
                loginWindow.Show();
                this.Close();
            }
        }


    }
}
