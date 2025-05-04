using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;
using Money_Management.Models;

namespace YourApp.Data
{
    public class GoalRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public void AddGoal(Goal goal)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO goals (title, description, category, target_date, is_completed)
                                 VALUES (@title, @description, @category, @targetDate, @isCompleted)";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", goal.Title);
                cmd.Parameters.AddWithValue("@description", goal.Description);
                cmd.Parameters.AddWithValue("@category", goal.Category);
                cmd.Parameters.AddWithValue("@targetDate", goal.TargetDate);
                cmd.Parameters.AddWithValue("@isCompleted", goal.IsCompleted);
                cmd.ExecuteNonQuery();
            }
        }

        // You can later add: GetAllGoals(), UpdateGoal(), DeleteGoal()
    }
}
