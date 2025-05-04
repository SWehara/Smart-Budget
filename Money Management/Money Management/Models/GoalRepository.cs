using System.Configuration;
using MySql.Data.MySqlClient;
using Money_Management.Models;
using System.Collections.Generic;

namespace Money_Management.Data
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

                using (var cmd = new MySqlCommand(query, conn))
                {
                    
                    cmd.Parameters.AddWithValue("@title", goal.Title);
                    cmd.Parameters.AddWithValue("@description", goal.Description);
                    cmd.Parameters.AddWithValue("@category", goal.Category);
                    cmd.Parameters.AddWithValue("@targetDate", goal.TargetDate);
                    cmd.Parameters.AddWithValue("@isCompleted", goal.IsCompleted);

                    // Execute the query to insert the goal into the database.
                    cmd.ExecuteNonQuery();
                }
            }
        }

       

        public List<Goal> GetAllGoals()
        {
            List<Goal> goals = new List<Goal>();

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM goals";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            goals.Add(new Goal
                            {
                                Id = reader.GetInt32("id"),
                                Title = reader.GetString("title"),
                                Description = reader.GetString("description"),
                                Category = reader.GetString("category"),
                                TargetDate = reader.GetDateTime("target_date"),
                                IsCompleted = reader.GetBoolean("is_completed")
                            });
                        }
                    }
                }
            }

            return goals;
        }
    }
}
