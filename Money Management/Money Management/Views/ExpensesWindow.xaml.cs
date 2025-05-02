using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Money_Management.Views
{
    public partial class ExpensesWindow : Window
    {
        public ObservableCollection<Expense> ExpensesList { get; set; } = new ObservableCollection<Expense>();

        public ExpensesWindow()
        {
            InitializeComponent();
            ExpensesDataGrid.ItemsSource = ExpensesList;
            ExpenseDatePicker.SelectedDate = DateTime.Now;
        }

        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            string amountText = ExpenseAmountTextBox.Text.Trim();
            string notes = NotesTextBox.Text.Trim();
            string category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
            DateTime? expenseDate = ExpenseDatePicker.SelectedDate;

            if (string.IsNullOrWhiteSpace(amountText) || string.IsNullOrWhiteSpace(category) || expenseDate == null)
            {
                MessageBox.Show("Please complete all required fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(amountText, @"^\d+(\.\d{1,2})?$"))
            {
                MessageBox.Show("Please enter a valid numeric amount.", "Invalid Amount", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal amount = decimal.Parse(amountText);

            ExpensesList.Add(new Expense
            {
                Date = expenseDate.Value.ToString("dd-MM-yyyy"),
                Category = category,
                Amount = $"Rs. {amount:0.00}",
                Notes = notes
            });

            UpdateTotalExpenses();
            ResetFields();

            ExpenseMessageTextBlock.Text = "Expense added successfully!";
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

        private void ResetFields()
        {
            ExpenseAmountTextBox.Text = "";
            NotesTextBox.Text = "";
            CategoryComboBox.SelectedIndex = -1;
            ExpenseDatePicker.SelectedDate = DateTime.Now;
            ExpenseMessageTextBlock.Text = "";
        }

        private void EditExpense_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Expense expense)
            {
                MessageBox.Show("Edit functionality can be added here.");
                // Example: Load fields with current values for editing.
            }
        }

        private void DeleteExpense_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Expense expense)
            {
                if (MessageBox.Show("Are you sure you want to delete this expense?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ExpensesList.Remove(expense);
                    UpdateTotalExpenses();
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Or navigate to another window
        }
    }

    public class Expense
    {
        public string Date { get; set; }
        public string Category { get; set; }
        public string Amount { get; set; }
        public string Notes { get; set; }
    }
}
