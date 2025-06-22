using System.Windows;

namespace Money_Management.Views
{
    public partial class HelpWindow : Window
    {
        private string currentUsername;

        public HelpWindow(string username)
        {
            InitializeComponent();
            currentUsername = username;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DashboardWindow dashboard = new DashboardWindow(currentUsername);
            dashboard.Show();
            this.Close();
        }
    }
}
