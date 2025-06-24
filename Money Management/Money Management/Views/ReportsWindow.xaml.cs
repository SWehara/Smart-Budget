using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using Microsoft.Win32;

namespace Money_Management.Views
{
    public partial class ReportsWindow : Window
    {
        private readonly string connectionString = "server=localhost;user id=root;password=@12345$Sw;database=smartgoaldb";
        private readonly string currentUsername;

        public ReportsWindow(string username)
        {
            InitializeComponent();
            currentUsername = username;
            WindowState = WindowState.Maximized;
            LoadMonthYearOptions();
            LoadSummaryData();
        }

        private void LoadMonthYearOptions()
        {
            for (int m = 1; m <= 12; m++)
                MonthComboBox.Items.Add(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m));
            MonthComboBox.SelectedIndex = DateTime.Now.Month - 1;

            for (int y = DateTime.Now.Year - 5; y <= DateTime.Now.Year + 1; y++)
                YearComboBox.Items.Add(y);
            YearComboBox.SelectedItem = DateTime.Now.Year;
        }

        private void LoadSummaryData()
        {
            int month = MonthComboBox.SelectedIndex + 1;
            int year = Convert.ToInt32(YearComboBox.SelectedItem);

            double income = GetTotal("income", month, year);
            double expenses = GetTotal("expenses", month, year);
            double balance = income - expenses;

            IncomeTextBlock.Text = $"Rs. {income:N2}";
            ExpensesTextBlock.Text = $"Rs. {expenses:N2}";
            BalanceTextBlock.Text = $"Rs. {balance:N2}";
        }

        private double GetTotal(string table, int month, int year)
        {
            double total = 0;
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = $"SELECT SUM(amount) FROM {table} WHERE username = @username AND MONTH(date) = @month AND YEAR(date) = @year";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", currentUsername);
            cmd.Parameters.AddWithValue("@month", month);
            cmd.Parameters.AddWithValue("@year", year);
            var result = cmd.ExecuteScalar();
            if (result != DBNull.Value)
                total = Convert.ToDouble(result);
            return total;
        }

        private List<(string Source, double Amount, string Notes)> GetIncomeDetails(int month, int year)
        {
            var list = new List<(string, double, string)>();
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT source, amount, notes FROM income WHERE username=@username AND MONTH(date)=@month AND YEAR(date)=@year";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", currentUsername);
            cmd.Parameters.AddWithValue("@month", month);
            cmd.Parameters.AddWithValue("@year", year);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add((reader.GetString(0), reader.GetDouble(1), reader.GetString(2)));
            return list;
        }

        private List<(string Category, double Amount, string Notes)> GetExpenseDetails(int month, int year)
        {
            var list = new List<(string, double, string)>();
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT category, amount, notes FROM expenses WHERE username=@username AND MONTH(date)=@month AND YEAR(date)=@year";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", currentUsername);
            cmd.Parameters.AddWithValue("@month", month);
            cmd.Parameters.AddWithValue("@year", year);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add((reader.GetString(0), reader.GetDouble(1), reader.GetString(2)));
            return list;
        }

        private List<(string Name, double TargetAmount, double Progress)> GetGoalProgress()
        {
            var list = new List<(string, double, double)>();
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT name, target_amount, progress FROM goals WHERE username=@username";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", currentUsername);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add((reader.GetString("name"), reader.GetDouble("target_amount"), reader.GetDouble("progress")));
            return list;
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (MonthComboBox.SelectedIndex != -1 && YearComboBox.SelectedItem != null)
                LoadSummaryData();
        }

        private void ExportToPDF_Click(object sender, RoutedEventArgs e)
        {
            int month = MonthComboBox.SelectedIndex + 1;
            int year = Convert.ToInt32(YearComboBox.SelectedItem);

            double income = GetTotal("income", month, year);
            double expenses = GetTotal("expenses", month, year);
            double balance = income - expenses;

            var incomeList = GetIncomeDetails(month, year);
            var expenseList = GetExpenseDetails(month, year);
            var goalList = GetGoalProgress();

            var dialog = new SaveFileDialog
            {
                Filter = "PDF Files|*.pdf",
                FileName = $"MonthlyReport_{month}_{year}_Full.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                QuestPDF.Settings.License = LicenseType.Community;
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(50);
                        page.Header().Text("Monthly Finance Report").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                        page.Content().Column(col =>
                        {
                            col.Spacing(15);
                            col.Item().Text($"Username: {currentUsername}");
                            col.Item().Text($"Month: {System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {year}");
                            col.Item().Text($"Total Income: Rs. {income:N2}");
                            col.Item().Text($"Total Expenses: Rs. {expenses:N2}");
                            col.Item().Text($"Remaining Balance: Rs. {balance:N2}");

                            if (incomeList.Count > 0)
                            {
                                col.Item().Text("Income Details:").FontSize(16).Bold();
                                col.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(150);
                                        columns.ConstantColumn(100);
                                        columns.RelativeColumn();
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Text("Source").Bold();
                                        header.Cell().Text("Amount").Bold();
                                        header.Cell().Text("Notes").Bold();
                                    });

                                    foreach (var i in incomeList)
                                    {
                                        table.Cell().Text(i.Source);
                                        table.Cell().Text($"Rs. {i.Amount:N2}");
                                        table.Cell().Text(i.Notes);
                                    }
                                });
                            }

                            if (expenseList.Count > 0)
                            {
                                col.Item().Text("Expense Details:").FontSize(16).Bold();
                                col.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(150);
                                        columns.ConstantColumn(100);
                                        columns.RelativeColumn();
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Text("Category").Bold();
                                        header.Cell().Text("Amount").Bold();
                                        header.Cell().Text("Notes").Bold();
                                    });

                                    foreach (var e in expenseList)
                                    {
                                        table.Cell().Text(e.Category);
                                        table.Cell().Text($"Rs. {e.Amount:N2}");
                                        table.Cell().Text(e.Notes);
                                    }
                                });
                            }

                            if (goalList.Count > 0)
                            {
                                col.Item().Text("Goal Progress:").FontSize(16).Bold();
                                col.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(150);
                                        columns.ConstantColumn(100);
                                        columns.ConstantColumn(100);
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Text("Goal").Bold();
                                        header.Cell().Text("Target").Bold();
                                        header.Cell().Text("Saved").Bold();
                                    });

                                    foreach (var g in goalList)
                                    {
                                        table.Cell().Text(g.Name);
                                        table.Cell().Text($"Rs. {g.TargetAmount:N2}");
                                        table.Cell().Text($"Rs. {g.Progress:N2}");
                                    }
                                });
                            }
                        });

                        page.Footer().AlignCenter().Text(x =>
                        {
                            x.Span("Generated on ");
                            x.Span(DateTime.Now.ToString("f")).SemiBold();
                        });
                    });
                }).GeneratePdf(dialog.FileName);

                MessageBox.Show($"PDF exported successfully to:\n{dialog.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new DashboardWindow(currentUsername).Show();
            Close();
        }
    }
}

