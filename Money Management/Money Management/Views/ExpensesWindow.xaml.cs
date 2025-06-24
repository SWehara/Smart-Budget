using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace Money_Management.Views
{
    public partial class ExpensesWindow : Window
    {
        public ObservableCollection<Expense> ExpensesList { get; set; } = new ObservableCollection<Expense>();
        private string currentUsername;
        private Expense selectedExpense = null;

        public ExpensesWindow(string username)
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            currentUsername = username;
            ExpensesDataGrid.ItemsSource = ExpensesList;
            LoadMonthYearFilters();
            LoadExpensesFromDatabase(DateTime.Now.Month, DateTime.Now.Year);
        }

        private void LoadMonthYearFilters()
        {
            for (int m = 1; m <= 12; m++)
                MonthComboBox.Items.Add(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m));

            for (int y = DateTime.Now.Year - 5; y <= DateTime.Now.Year + 1; y++)
                YearComboBox.Items.Add(y.ToString());

            MonthComboBox.SelectedIndex = DateTime.Now.Month - 1;
            YearComboBox.SelectedItem = DateTime.Now.Year.ToString();
        }

        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(ExpenseAmountTextBox.Text.Trim(), out decimal amount) ||
                CategoryComboBox.SelectedItem == null ||
                !ExpenseDatePicker.SelectedDate.HasValue)
            {
                ExpenseMessageTextBlock.Text = "Please complete all fields correctly.";
                return;
            }

            string category = ((ComboBoxItem)CategoryComboBox.SelectedItem).Content.ToString();
            string notes = NotesTextBox.Text.Trim();
            DateTime date = ExpenseDatePicker.SelectedDate.Value;

            if (selectedExpense != null)
            {
                UpdateExpenseInDatabase(selectedExpense.Id, amount, category, notes, date);
                selectedExpense = null;
            }
            else
            {
                InsertExpenseToDatabase(amount, category, notes, date);
            }

            LoadExpensesFromDatabase(date.Month, date.Year);
            ClearForm();
        }

        private void ClearForm()
        {
            ExpenseAmountTextBox.Clear();
            CategoryComboBox.SelectedIndex = -1;
            NotesTextBox.Clear();
            ExpenseDatePicker.SelectedDate = DateTime.Now;
            ExpenseMessageTextBlock.Text = "";
        }

        private void InsertExpenseToDatabase(decimal amount, string category, string notes, DateTime date)
        {
            string query = "INSERT INTO expenses (username, amount, category, notes, date) VALUES (@username, @amount, @category, @notes, @date)";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", currentUsername);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@notes", notes);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void UpdateExpenseInDatabase(int id, decimal amount, string category, string notes, DateTime date)
        {
            string query = "UPDATE expenses SET amount=@amount, category=@category, notes=@notes, date=@date WHERE id=@id AND username=@username";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@notes", notes);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@username", currentUsername);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void LoadExpensesFromDatabase(int month, int year)
        {
            ExpensesList.Clear();
            string query = "SELECT id, amount, category, notes, date FROM expenses WHERE username = @username AND MONTH(date) = @month AND YEAR(date) = @year";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", currentUsername);
                    cmd.Parameters.AddWithValue("@month", month);
                    cmd.Parameters.AddWithValue("@year", year);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ExpensesList.Add(new Expense
                            {
                                Id = reader.GetInt32("id"),
                                Amount = $"Rs. {reader.GetDecimal("amount"):0.00}",
                                Category = reader.GetString("category"),
                                Notes = reader.GetString("notes"),
                                Date = reader.GetDateTime("date").ToString("dd-MM-yyyy")
                            });
                        }
                    }
                }
            }

            UpdateTotalExpenses();
        }

        private void UpdateTotalExpenses()
        {
            decimal total = 0;
            foreach (var item in ExpensesList)
            {
                if (decimal.TryParse(item.Amount.Replace("Rs. ", ""), out decimal amt))
                {
                    total += amt;
                }
            }
            TotalExpenseTextBlock.Text = $"Rs. {total:0.00}";
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MonthComboBox.SelectedIndex != -1 && YearComboBox.SelectedItem != null)
            {
                int month = MonthComboBox.SelectedIndex + 1;
                int year = int.Parse(YearComboBox.SelectedItem.ToString());
                LoadExpensesFromDatabase(month, year);
            }
        }

        private void EditExpense_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Expense expense)
            {
                selectedExpense = expense;
                ExpenseAmountTextBox.Text = expense.Amount.Replace("Rs. ", "");
                NotesTextBox.Text = expense.Notes;
                ExpenseDatePicker.SelectedDate = DateTime.ParseExact(expense.Date, "dd-MM-yyyy", null);
                CategoryComboBox.SelectedItem = CategoryComboBox.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(i => i.Content.ToString() == expense.Category);
            }
        }

        private void DeleteExpense_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Expense expense)
            {
                if (MessageBox.Show("Delete this expense?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DeleteExpenseFromDatabase(expense.Id);
                    ExpensesList.Remove(expense);
                    UpdateTotalExpenses();
                }
            }
        }

        private void DeleteExpenseFromDatabase(int id)
        {
            string query = "DELETE FROM expenses WHERE id=@id AND username=@username";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@username", currentUsername);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new DashboardWindow(currentUsername).Show();
            this.Close();
        }

        private string ConnectionString => "server=localhost;user id=root;password=@12345$Sw;database=smartgoaldb";
    }

    public class Expense
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Category { get; set; }
        public string Amount { get; set; }
        public string Notes { get; set; }
    }
}
