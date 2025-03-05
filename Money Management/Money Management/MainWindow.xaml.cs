using Microsoft.Win32;
using System.Windows;
using System.Windows.Media.Imaging;

namespace YourNamespace
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ProfileImage.Source = new BitmapImage(new System.Uri(openFileDialog.FileName));
            }
        }

        private void ProceedButton_Click(object sender, RoutedEventArgs e)
        {
            string userName = NameTextBox.Text;
            if (!string.IsNullOrEmpty(userName))
            {
                MessageBox.Show($"Welcome, {userName}!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please enter your name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}