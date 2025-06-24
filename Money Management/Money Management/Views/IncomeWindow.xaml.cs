using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace Money_Management.Views
{
    public partial class IncomeWindow : Window
    {
        public ObservableCollection<IncomeEntry> IncomeEntries { get; set; } = new ObservableCollection<IncomeEntry>();
        private string currentUsername;
        private IncomeEntry selectedEntry = null;

        public IncomeWindow(string username)
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            currentUsername = username;
            IncomeDataGrid.ItemsSource = IncomeEntries;
            LoadDefaultSources();
            LoadMonthYearFilters();
            LoadIncomeByMonth(DateTime.Now.Month, DateTime.Now.Year);
        }

        private void LoadDefaultSources()
        {
            IncomeSourceComboBox.ItemsSource = new string[] { "Salary", "Freelance", "Gift", "Interest", "Other" };
            IncomeSourceComboBox.SelectedIndex = 0;
        }

        private void LoadMonthYearFilters()
        {
            for (int i = 1; i <= 12; i++)
                MonthComboBox.Items.Add(new DateTime(2000, i, 1).ToString("MMMM"));

            int currentYear = DateTime.Now.Year;
            for (int y = currentYear - 5; y <= currentYear + 1; y++)
                YearComboBox.Items.Add(y);

            MonthComboBox.SelectedIndex = DateTime.Now.Month - 1;
            YearComboBox.SelectedItem = currentYear;
        }

        private void LoadIncomeByMonth(int month, int year)
        {
            IncomeEntries.Clear();
            string connStr = "server=localhost;user id=root;password=@12345$Sw;database=smartgoaldb";

            using (var conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT id, source, amount, notes, date 
                                     FROM income 
                                     WHERE username = @username AND MONTH(date) = @month AND YEAR(date) = @year";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", currentUsername);
                        cmd.Parameters.AddWithValue("@month", month);
                        cmd.Parameters.AddWithValue("@year", year);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IncomeEntries.Add(new IncomeEntry
                                {
                                    Id = reader.GetInt32("id"),
                                    Source = reader.GetString("source"),
                                    Amount = reader.GetDecimal("amount"),
                                    Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? "" : reader.GetString("notes"),
                                    Date = reader.GetDateTime("date")
                                });
                            }
                        }
                    }
                    UpdateTotalIncome();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load income: " + ex.Message);
                }
            }
        }

        private void SetIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(IncomeAmountTextBox.Text, out decimal amount) || IncomeSourceComboBox.SelectedItem == null)
            {
                MessageTextBlock.Text = "Please enter valid amount and source.";
                MessageTextBlock.Foreground = Brushes.Red;
                return;
            }

            string source = IncomeSourceComboBox.Text.Trim();
            string notes = IncomeNotesTextBox.Text.Trim();
            DateTime now = DateTime.Now;

            if (selectedEntry != null)
            {
                selectedEntry.Source = source;
                selectedEntry.Amount = amount;
                selectedEntry.Notes = notes;

                UpdateIncomeInDatabase(selectedEntry);
                MessageTextBlock.Text = "Income updated!";
                selectedEntry = null;
            }
            else
            {
                var newEntry = new IncomeEntry
                {
                    Source = source,
                    Amount = amount,
                    Notes = notes,
                    Date = now
                };

                SaveIncomeToDatabase(newEntry);
                IncomeEntries.Add(newEntry);
                MessageTextBlock.Text = "Income added!";
            }

            MessageTextBlock.Foreground = Brushes.LightGreen;
            ClearForm();
            UpdateTotalIncome();
        }

        private void SaveIncomeToDatabase(IncomeEntry entry)
        {
            string connStr = "server=localhost;user id=root;password=@12345$Sw;database=smartgoaldb";

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "INSERT INTO income (username, source, amount, notes, date) VALUES (@username, @source, @amount, @notes, @date)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", currentUsername);
                    cmd.Parameters.AddWithValue("@source", entry.Source);
                    cmd.Parameters.AddWithValue("@amount", entry.Amount);
                    cmd.Parameters.AddWithValue("@notes", entry.Notes);
                    cmd.Parameters.AddWithValue("@date", entry.Date);
                    cmd.ExecuteNonQuery();
                    entry.Id = (int)cmd.LastInsertedId;
                }
            }
        }

        private void UpdateIncomeInDatabase(IncomeEntry entry)
        {
            string connStr = "server=localhost;user id=root;password=@12345$Sw;database=smartgoaldb";
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "UPDATE income SET source = @source, amount = @amount, notes = @notes WHERE id = @id AND username = @username";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@source", entry.Source);
                    cmd.Parameters.AddWithValue("@amount", entry.Amount);
                    cmd.Parameters.AddWithValue("@notes", entry.Notes);
                    cmd.Parameters.AddWithValue("@id", entry.Id);
                    cmd.Parameters.AddWithValue("@username", currentUsername);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is IncomeEntry entry)
            {
                if (MessageBox.Show("Delete this entry?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    IncomeEntries.Remove(entry);
                    DeleteFromDatabase(entry.Id);
                    UpdateTotalIncome();
                }
            }
        }

        private void DeleteFromDatabase(int id)
        {
            string connStr = "server=localhost;user id=root;password=@12345$Sw;database=smartgoaldb";
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "DELETE FROM income WHERE id = @id AND username = @username";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@username", currentUsername);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is IncomeEntry entry)
            {
                selectedEntry = entry;
                IncomeAmountTextBox.Text = entry.Amount.ToString();
                IncomeSourceComboBox.SelectedItem = entry.Source;
                IncomeNotesTextBox.Text = entry.Notes;
            }
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (MonthComboBox.SelectedIndex >= 0 && YearComboBox.SelectedItem is int year)
            {
                int month = MonthComboBox.SelectedIndex + 1;
                LoadIncomeByMonth(month, year);
            }
        }

        private void ClearForm()
        {
            IncomeAmountTextBox.Clear();
            IncomeNotesTextBox.Clear();
            IncomeSourceComboBox.SelectedIndex = 0;
        }

        private void UpdateTotalIncome()
        {
            decimal total = IncomeEntries.Sum(e => e.Amount);
            CurrentIncomeTextBlock.Text = $"Rs. {total:N2}";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DashboardWindow d = new DashboardWindow(currentUsername);
            d.Show();
            Close();
        }

        public class IncomeEntry
        {
            public int Id { get; set; }
            public string Source { get; set; }
            public decimal Amount { get; set; }
            public string Notes { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
