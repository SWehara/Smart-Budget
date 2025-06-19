using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Money_Management.Models;
using MySql.Data.MySqlClient;

namespace Money_Management.Views
{
    public partial class GoalsWindow : Window
    {
        public GoalsWindow()
        {
            InitializeComponent();
            LoadGoals();
        }

        private string connectionString = "server=localhost;user id=root;password=@12345$Sw;database=smartgoaldb";

        // Class to represent a goal
        public class Goal
        {
            public string Name { get; set; }
            public decimal Target { get; set; }
            public string DueDate { get; set; }
            public decimal Progress { get; set; }
            public string Status { get; set; }
        }

        private void CreateGoalButton_Click(object sender, RoutedEventArgs e)
        {
            string name = GoalNameTextBox.Text.Trim();
            string targetText = TargetAmountTextBox.Text.Trim();
            DateTime? dueDate = DueDatePicker.SelectedDate;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(targetText) || dueDate == null)
            {
                GoalMessageTextBlock.Text = "Please fill all fields.";
                return;
            }

            if (!decimal.TryParse(targetText, out decimal targetAmount))
            {
                GoalMessageTextBlock.Text = "Invalid amount.";
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string insertQuery = "INSERT INTO goals (name, target_amount, due_date, progress) VALUES (@name, @target, @dueDate, 0)";
                    using (var cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@target", targetAmount);
                        cmd.Parameters.AddWithValue("@dueDate", dueDate.Value.ToString("yyyy-MM-dd"));
                        cmd.ExecuteNonQuery();
                    }
                }

                GoalMessageTextBlock.Text = "Goal created successfully!";
                LoadGoals(); // Refresh list
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadGoals()
        {
            var goalList = new List<Goal>();

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT name, target_amount, due_date, progress FROM goals";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var target = reader.GetDecimal("target_amount");
                            var progress = reader.GetDecimal("progress");
                            var status = progress >= target ? "Completed" : "In Progress";

                            goalList.Add(new Goal
                            {
                                Name = reader.GetString("name"),
                                Target = target,
                                DueDate = Convert.ToDateTime(reader["due_date"]).ToShortDateString(),
                                Progress = progress,
                                Status = status
                            });
                        }
                    }
                }

                GoalsDataGrid.ItemsSource = goalList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load goals: " + ex.Message);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DashboardWindow dashboard = new DashboardWindow();
            dashboard.Show();
            this.Close();
        }


        private void AddProgressButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            StackPanel panel = button.Parent as StackPanel;
            TextBox amountBox = panel.Children[0] as TextBox;

            string goalName = amountBox.Tag.ToString();
            string amountText = amountBox.Text.Trim();

            if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Enter a valid amount.");
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string updateQuery = "UPDATE goals SET progress = progress + @amount WHERE name = @name";
                    using (var cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@name", goalName);
                        cmd.ExecuteNonQuery();
                    }
                }

                GoalMessageTextBlock.Text = $"Added Rs. {amount} to '{goalName}'";
                LoadGoals(); // Refresh UI
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating progress: " + ex.Message);
            }
        }
        private void EditGoalButton_Click(object sender, RoutedEventArgs e)
        {
            var goal = (sender as FrameworkElement).DataContext as Goal;

            string newTargetStr = Microsoft.VisualBasic.Interaction.InputBox(
                $"Edit target for '{goal.Name}' (current: {goal.Target})", "Edit Goal", goal.Target.ToString());

            if (decimal.TryParse(newTargetStr, out decimal newTarget))
            {
                try
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string updateQuery = "UPDATE goals SET target_amount = @target WHERE name = @name";
                        using (var cmd = new MySqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@target", newTarget);
                            cmd.Parameters.AddWithValue("@name", goal.Name);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    LoadGoals();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error editing goal: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Invalid input.");
            }
        }
            private void DeleteGoalButton_Click(object sender, RoutedEventArgs e)
        {
            var goal = (sender as FrameworkElement).DataContext as Goal;

            var result = MessageBox.Show($"Are you sure you want to delete '{goal.Name}'?",
                                         "Confirm Delete", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM goals WHERE name = @name";
                        using (var cmd = new MySqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@name", goal.Name);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    LoadGoals();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting goal: " + ex.Message);
                }
            }
        }

    }

}


