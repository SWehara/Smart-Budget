using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Money_Management
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
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFileDialog.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    ProfileImage.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ProceedButton_Click(object sender, RoutedEventArgs e)
        {
            string userName = NameTextBox.Text;
            if (!string.IsNullOrWhiteSpace(userName))
            {
                DashboardWindow dashboardWindow = new DashboardWindow();
                dashboardWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter your name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
