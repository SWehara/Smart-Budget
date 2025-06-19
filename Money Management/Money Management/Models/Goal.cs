using Microsoft.VisualBasic; // for InputBox to work


namespace Money_Management.Models
{
    public class Goal
    {
        public string Name { get; set; }
        public decimal Target { get; set; }
        public string DueDate { get; set; }  // You can use DateTime if you prefer
        public decimal Progress { get; set; }
        public string Status { get; set; }
    }
}
