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

        public IncomeWindow(DashboardWindow dashboardWindow)
        {
            InitializeComponent();
            _dashboardWindow = dashboardWindow;
            IncomeEntries = new ObservableCollection<IncomeEntry>();
            IncomeDataGrid.ItemsSource = IncomeEntries;

            LoadDefaultSources();
        }

        public IncomeWindow()
        {
            InitializeComponent();
            IncomeEntries = new ObservableCollection<IncomeEntry>();
            IncomeDataGrid.ItemsSource = IncomeEntries;

            LoadDefaultSources();
        }

        // Load default income sources into the ComboBox
        private void LoadDefaultSources()
        {
            IncomeSourceComboBox.ItemsSource = new string[]
            {
                "Salary", "Freelance", "Gift", "Interest", "Other"
            };
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

            try
            {
                var newEntry = new IncomeEntry
                {
                    Source = IncomeSourceComboBox.Text,
                    Amount = decimal.Parse(IncomeAmountTextBox.Text)
                };

                // Add or update the income entry
                IncomeEntries.Add(newEntry);
                UpdateTotalIncome();

                MessageTextBlock.Text = "Income added/updated successfully!";
                MessageTextBlock.Foreground = new SolidColorBrush(Colors.LightGreen);

                // Optionally clear the fields for better UX
                IncomeAmountTextBox.Clear();
                IncomeSourceComboBox.SelectedIndex = 0;
            }
            catch (FormatException ex)
            {
                MessageTextBlock.Text = $"Invalid amount format: {ex.Message}";
                MessageTextBlock.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        // Update the total income display
        private void UpdateTotalIncome()
        {
            var totalIncome = IncomeEntries.Sum(entry => entry.Amount);
            CurrentIncomeTextBlock.Text = $"Rs. {totalIncome}";
        }

        // Handle the Back button click event
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            
            this.Hide();
        }

        // Income entry model
        public class IncomeEntry
        {
            public string Source { get; set; }
            public decimal Amount { get; set; }
        }
    }
}

