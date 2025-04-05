using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Money_Management.Views
{
    public partial class ExpensesWindow : Window
    {
        public ExpensesWindow()
        {
            InitializeComponent();
        }

        
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = ""; 
                textBox.Foreground = Brushes.Black; 
            }
        }

        
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Foreground = Brushes.Gray; 
                if (textBox.Name == "ExpenseNameTextBox")
                {
                    textBox.Text = "Enter Expense Name"; 
                }
                else if (textBox.Name == "ExpenseAmountTextBox")
                {
                    textBox.Text = "Enter Expense Amount"; 
                }
            }
        }

        
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

            
            ExpenseNameTextBox.Text = "Enter Expense Name";
            ExpenseNameTextBox.Foreground = Brushes.Gray;
            ExpenseAmountTextBox.Text = "Enter Expense Amount";
            ExpenseAmountTextBox.Foreground = Brushes.Gray;
        }

        
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}