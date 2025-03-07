using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Money_Management
{
    public partial class ExpensesWindow : Window
    {
        public ExpensesWindow()
        {
            InitializeComponent();
        }

        // Event handler for when the TextBox gets focus
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = ""; // Clear the placeholder text
                textBox.Foreground = Brushes.Black; // Change text color to black
            }
        }

        // Event handler for when the TextBox loses focus
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Foreground = Brushes.Gray; // Change text color to gray
                if (textBox.Name == "ExpenseNameTextBox")
                {
                    textBox.Text = "Enter Expense Name"; // Restore placeholder text
                }
                else if (textBox.Name == "ExpenseAmountTextBox")
                {
                    textBox.Text = "Enter Expense Amount"; // Restore placeholder text
                }
            }
        }

        // Event handler for the "Add Expense" button
        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            string expenseName = ExpenseNameTextBox.Foreground == Brushes.Gray ? "" : ExpenseNameTextBox.Text;
            string expenseAmount = ExpenseAmountTextBox.Foreground == Brushes.Gray ? "" : ExpenseAmountTextBox.Text;

            if (string.IsNullOrEmpty(expenseName) || string.IsNullOrEmpty(expenseAmount))
            {
                MessageBox.Show("Please enter both the expense name and amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show($"Expense Added: {expenseName} - {expenseAmount}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Clear the input fields
            ExpenseNameTextBox.Text = "Enter Expense Name";
            ExpenseNameTextBox.Foreground = Brushes.Gray;
            ExpenseAmountTextBox.Text = "Enter Expense Amount";
            ExpenseAmountTextBox.Foreground = Brushes.Gray;
        }

        // Event handler for the "Close" button
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the window
        }
    }
}