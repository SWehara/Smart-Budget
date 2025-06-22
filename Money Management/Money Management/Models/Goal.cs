namespace Money_Management.Models
{
    public class Goal
    {
        public string Name { get; set; }
        public decimal Target { get; set; }
        public string DueDate { get; set; }  // You can change to DateTime if needed
        public decimal Progress { get; set; }

        public string Status
        {
            get
            {
                return Progress >= Target ? "Completed" : "In Progress";
            }
        }
    }
}
