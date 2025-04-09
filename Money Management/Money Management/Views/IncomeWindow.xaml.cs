using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

        // Add income
        private void AddIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (IncomeAmountTextBox.Text == "" || IncomeSourceComboBox.SelectedItem == null || IncomeDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Please fill in all required fields.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newEntry = new IncomeEntry
            {
                Date = IncomeDatePicker.SelectedDate ?? DateTime.Now,
                Source = IncomeSourceComboBox.Text,
                Amount = decimal.Parse(IncomeAmountTextBox.Text)
            };

            IncomeEntries.Add(newEntry);
            UpdateTotalIncome();

            // Visual feedback
            MessageBox.Show("Income added successfully ✅", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Clear fields (optional UX)
            IncomeAmountTextBox.Text = "";
            IncomeSourceComboBox.SelectedIndex = 0;
            IncomeDatePicker.SelectedDate = null;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Edit logic (not implemented)
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEntry = IncomeDataGrid.SelectedItem as IncomeEntry;
            if (selectedEntry != null)
            {
                IncomeEntries.Remove(selectedEntry);
                UpdateTotalIncome();
            }
        }

        private void UpdateTotalIncome()
        {
            var totalIncome = IncomeEntries.Sum(entry => entry.Amount);
            TotalIncomeTextBlock.Text = $"Total Income: Rs.{totalIncome}";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public class IncomeEntry
        {
            public DateTime Date { get; set; }
            public string Source { get; set; }
            public decimal Amount { get; set; }
        }

        // Optional: Add custom source
        private void AddSourceButton_Click(object sender, RoutedEventArgs e)
        {
            var newSource = Microsoft.VisualBasic.Interaction.InputBox("Enter new income source:", "Add Source", "");
            if (!string.IsNullOrWhiteSpace(newSource))
            {
                var sources = ((string[])IncomeSourceComboBox.ItemsSource).ToList();
                if (!sources.Contains(newSource))
                {
                    sources.Add(newSource);
                    IncomeSourceComboBox.ItemsSource = sources;
                    IncomeSourceComboBox.SelectedItem = newSource;
                }
            }
        }
    }
}

