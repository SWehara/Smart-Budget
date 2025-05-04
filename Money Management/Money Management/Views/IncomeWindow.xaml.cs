using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Money_Management.Views
{
    public partial class IncomeWindow : Window
    {
        public ObservableCollection<IncomeEntry> IncomeEntries { get; set; }
        private DashboardWindow _dashboardWindow;

        // Track selected item for editing
        private IncomeEntry selectedEntry = null;

        // Constructor that accepts the DashboardWindow
        public IncomeWindow(DashboardWindow dashboardWindow)
        {
            InitializeComponent();
            _dashboardWindow = dashboardWindow;
            InitializeIncomeWindow();
        }

        // Optional: You can remove this if you want to avoid using it without DashboardWindow
        public IncomeWindow()
        {
            InitializeComponent();
            InitializeIncomeWindow();
        }

        private void InitializeIncomeWindow()
        {
            IncomeEntries = new ObservableCollection<IncomeEntry>();
            IncomeDataGrid.ItemsSource = IncomeEntries;

            LoadDefaultSources();
        }

        // Load default income sources into the ComboBox
        private void LoadDefaultSources()
        {
            IncomeSourceComboBox.ItemsSource = new string[] { "Salary", "Freelance", "Gift", "Interest", "Other" };
            IncomeSourceComboBox.SelectedIndex = 0;
        }

        // Set or update income
        private void SetIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(IncomeAmountTextBox.Text) || IncomeSourceComboBox.SelectedItem == null)
            {
                MessageTextBlock.Text = "Please fill in all required fields.";
                MessageTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            if (!decimal.TryParse(IncomeAmountTextBox.Text, out decimal amount))
            {
                MessageTextBlock.Text = "Invalid amount format.";
                MessageTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            if (selectedEntry != null)
            {
                // Update existing entry
                selectedEntry.Amount = amount;
                selectedEntry.Source = IncomeSourceComboBox.Text;
                IncomeDataGrid.Items.Refresh();
                selectedEntry = null;

                MessageTextBlock.Text = "Income updated successfully!";
                MessageTextBlock.Foreground = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                // Add new entry
                var newEntry = new IncomeEntry
                {
                    Source = IncomeSourceComboBox.Text,
                    Amount = amount,
                    Date = DateTime.Now
                };

                IncomeEntries.Add(newEntry);

                MessageTextBlock.Text = "Income added successfully!";
                MessageTextBlock.Foreground = new SolidColorBrush(Colors.LightGreen);
            }

            UpdateTotalIncome();
            ClearForm();
        }

        // Update the total income display
        private void UpdateTotalIncome()
        {
            var totalIncome = IncomeEntries.Sum(entry => entry.Amount);
            CurrentIncomeTextBlock.Text = $"Rs. {totalIncome}";
        }

        // Clear form inputs
        private void ClearForm()
        {
            IncomeAmountTextBox.Clear();
            IncomeSourceComboBox.SelectedIndex = 0;
        }

        // Back button
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (_dashboardWindow != null)
            {
                _dashboardWindow.Show(); // Show the dashboard again
                this.Close();            // Close the income window
            }
            else
            {
                MessageBox.Show("Dashboard reference is missing.", "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Edit button
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is IncomeEntry entry)
            {
                IncomeAmountTextBox.Text = entry.Amount.ToString();
                IncomeSourceComboBox.SelectedItem = entry.Source;
                selectedEntry = (entry);
                UpdateTotalIncome();
            }
        }

        // Delete button
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is IncomeEntry entry)
            {
                IncomeEntries.Remove(entry);
                UpdateTotalIncome();
            }
        }

        // Income entry model
        public class IncomeEntry
        {
            public DateTime Date { get; set; } = DateTime.Now;
            public string Source { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
