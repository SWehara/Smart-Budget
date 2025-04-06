using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Money_Management.Views
{
    public partial class IncomeWindow : Window
    {
        public ObservableCollection<IncomeEntry> IncomeEntries { get; set; }

        private DashboardWindow _dashboardWindow; // Store reference to DashboardWindow

        // Constructor that accepts DashboardWindow as a parameter
        public IncomeWindow(DashboardWindow dashboardWindow)
        {
            InitializeComponent();
            _dashboardWindow = dashboardWindow; // Store the reference
            IncomeEntries = new ObservableCollection<IncomeEntry>();
            IncomeDataGrid.ItemsSource = IncomeEntries;

            // Optionally, you can use _dashboardWindow for other purposes
            // For example, you can access properties or methods of DashboardWindow here
        }

        // Default constructor (if needed in some cases)
        public IncomeWindow()
        {
            InitializeComponent();
            IncomeEntries = new ObservableCollection<IncomeEntry>();
            IncomeDataGrid.ItemsSource = IncomeEntries;
        }

        // Search functionality: Filter based on Source or Amount
        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();
            var filteredEntries = IncomeEntries.Where(entry =>
                entry.Source.ToLower().Contains(searchText) || entry.Amount.ToString().Contains(searchText)).ToList();
            IncomeDataGrid.ItemsSource = new ObservableCollection<IncomeEntry>(filteredEntries);
        }

        // Add income (example)
        private void AddIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            var newEntry = new IncomeEntry
            {
                Date = IncomeDatePicker.SelectedDate ?? DateTime.Now,
                Source = IncomeSourceTextBox.Text,
                Amount = decimal.Parse(IncomeAmountTextBox.Text)
            };
            IncomeEntries.Add(newEntry);
            UpdateTotalIncome();
        }

        // Update income entry (for editing selected entries)
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Get selected income entry from the grid and show edit form (you can implement this part)
        }

        // Delete income entry
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEntry = IncomeDataGrid.SelectedItem as IncomeEntry;
            if (selectedEntry != null)
            {
                IncomeEntries.Remove(selectedEntry);
                UpdateTotalIncome();
            }
        }

        // Update the total income display
        private void UpdateTotalIncome()
        {
            var totalIncome = IncomeEntries.Sum(entry => entry.Amount);
            TotalIncomeTextBlock.Text = $"Total Income: Rs.{totalIncome}";
        }

        // Go back to previous window
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Or navigate to another window
        }

        // Handle placeholder behavior in Search TextBox
        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Search by Source or Amount")
            {
                SearchTextBox.Text = "";
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Search by Source or Amount";
            }
        }
    }

    // Model class for Income Entry
    public class IncomeEntry
    {
        public DateTime Date { get; set; }
        public string Source { get; set; }
        public decimal Amount { get; set; }
    }
}

