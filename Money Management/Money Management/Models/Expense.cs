using System;

namespace Money_Management.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
