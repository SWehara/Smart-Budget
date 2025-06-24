using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Money_Management.Models;
using MySql.Data.MySqlClient;
using OxyPlot;
using OxyPlot.Series;

namespace Money_Management.Views
{
    public partial class BudgetWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<BudgetCategory> Categories { get; set; } = new ObservableCollection<BudgetCategory>();

        private readonly string currentUsername;
        private readonly string connectionString = "server=localhost;user id=root;password=@12345$Sw;database=smartgoaldb";

        private double monthlyIncome;
        public double MonthlyIncome
        {
            get => monthlyIncome;
            set
            {
                monthlyIncome = value;
                OnPropertyChanged(nameof(MonthlyIncome));
                UpdateRemainingBudget();
            }
        }

        private double remainingBudget;
        public double RemainingBudget
        {
            get => remainingBudget;
            set
            {
                remainingBudget = value;
                OnPropertyChanged(nameof(RemainingBudget));
            }
        }

        public BudgetWindow(string username)
        {
            InitializeComponent();
            currentUsername = username;
            DataContext = this;
            WindowState = WindowState.Maximized;

            PopulateMonthYearDropdowns();
            LoadBudgetData();
        }

        private void PopulateMonthYearDropdowns()
        {
            for (int m = 1; m <= 12; m++)
                MonthComboBox.Items.Add(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m));
            MonthComboBox.SelectedIndex = DateTime.Now.Month - 1;

            for (int y = DateTime.Now.Year - 5; y <= DateTime.Now.Year + 1; y++)
                YearComboBox.Items.Add(y);
            YearComboBox.SelectedItem = DateTime.Now.Year;
        }

        private void LoadBudgetData()
        {
            int month = MonthComboBox.SelectedIndex + 1;
            int year = Convert.ToInt32(YearComboBox.SelectedItem);

            LoadMonthlyIncome(month, year);
            LoadCategoriesFromDatabase(month, year);
            LoadPieChart();
        }

        private void LoadMonthlyIncome(int month, int year)
        {
            MonthlyIncome = 0;
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT SUM(amount) FROM income WHERE username = @username AND MONTH(date) = @month AND YEAR(date) = @year";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", currentUsername);
            cmd.Parameters.AddWithValue("@month", month);
            cmd.Parameters.AddWithValue("@year", year);
            var result = cmd.ExecuteScalar();
            MonthlyIncome = result != DBNull.Value ? Convert.ToDouble(result) : 0;
        }

        private void LoadCategoriesFromDatabase(int month, int year)
        {
            Categories.Clear();

            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT id, name, limit_amount FROM budget_categories WHERE username = @username AND month = @month AND year = @year";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", currentUsername);
            cmd.Parameters.AddWithValue("@month", month);
            cmd.Parameters.AddWithValue("@year", year);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string categoryName = reader.GetString("name");
                double limit = reader.GetDouble("limit_amount");
                double spent = GetSpentAmount(categoryName, month, year);

                Categories.Add(new BudgetCategory
                {
                    Id = reader.GetInt32("id"),
                    Name = categoryName,
                    Limit = limit,
                    Spent = spent
                });
            }

            UpdateRemainingBudget();
        }

        private double GetSpentAmount(string category, int month, int year)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT SUM(amount) FROM expenses WHERE username = @username AND category = @category AND MONTH(date) = @month AND YEAR(date) = @year";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", currentUsername);
            cmd.Parameters.AddWithValue("@category", category);
            cmd.Parameters.AddWithValue("@month", month);
            cmd.Parameters.AddWithValue("@year", year);
            var result = cmd.ExecuteScalar();
            return result != DBNull.Value ? Convert.ToDouble(result) : 0;
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            string name = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (!double.TryParse(CategoryLimitBox.Text, out double limit) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Enter valid category and numeric limit.");
                return;
            }

            int month = MonthComboBox.SelectedIndex + 1;
            int year = Convert.ToInt32(YearComboBox.SelectedItem);

            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            string insert = "INSERT INTO budget_categories (username, name, limit_amount, spent, month, year) VALUES (@username, @name, @limit, 0, @month, @year)";
            using var cmd = new MySqlCommand(insert, conn);
            cmd.Parameters.AddWithValue("@username", currentUsername);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@limit", limit);
            cmd.Parameters.AddWithValue("@month", month);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.ExecuteNonQuery();

            Categories.Add(new BudgetCategory
            {
                Name = name,
                Limit = limit,
                Spent = 0
            });

            CategoryComboBox.SelectedIndex = -1;
            CategoryLimitBox.Clear();

            LoadPieChart();
            UpdateRemainingBudget();
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is BudgetCategory category)
            {
                CategoryComboBox.Text = category.Name;
                CategoryLimitBox.Text = category.Limit.ToString();
                Categories.Remove(category);

                using var conn = new MySqlConnection(connectionString);
                conn.Open();
                string delete = "DELETE FROM budget_categories WHERE id = @id";
                using var cmd = new MySqlCommand(delete, conn);
                cmd.Parameters.AddWithValue("@id", category.Id);
                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is BudgetCategory category)
            {
                if (MessageBox.Show($"Delete category '{category.Name}'?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using var conn = new MySqlConnection(connectionString);
                    conn.Open();
                    string delete = "DELETE FROM budget_categories WHERE id = @id";
                    using var cmd = new MySqlCommand(delete, conn);
                    cmd.Parameters.AddWithValue("@id", category.Id);
                    cmd.ExecuteNonQuery();

                    Categories.Remove(category);
                    LoadPieChart();
                    UpdateRemainingBudget();
                }
            }
        }

        private void LoadPieChart()
        {
            var model = new PlotModel { Title = "Spending Distribution" };
            var series = new PieSeries();

            foreach (var category in Categories.Where(c => c.Spent > 0))
            {
                series.Slices.Add(new PieSlice(category.Name, category.Spent));
            }

            model.Series.Add(series);
            PieChartView.Model = model;
        }

        private void CategoryDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Limit" && e.Row.Item is BudgetCategory category)
            {
                if (e.EditingElement is TextBox textBox && double.TryParse(textBox.Text, out double newLimit))
                {
                    category.Limit = newLimit;
                    SaveLimitToDatabase(category.Id, newLimit);
                    UpdateRemainingBudget();
                    LoadPieChart();
                }
            }
        }

        private void SaveLimitToDatabase(int id, double newLimit)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            string update = "UPDATE budget_categories SET limit_amount = @limit WHERE id = @id";
            using var cmd = new MySqlCommand(update, conn);
            cmd.Parameters.AddWithValue("@limit", newLimit);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        private void UpdateRemainingBudget()
        {
            double totalSpent = Categories.Sum(c => c.Spent);
            RemainingBudget = MonthlyIncome - totalSpent;
            if (RemainingSummary != null)
            {
                RemainingSummary.Text = $"You have Rs. {RemainingBudget:N0} left this month";
            }
        }

        private void MonthYear_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (MonthComboBox.SelectedIndex != -1 && YearComboBox.SelectedItem != null)
                LoadBudgetData();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new DashboardWindow(currentUsername).Show();
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
