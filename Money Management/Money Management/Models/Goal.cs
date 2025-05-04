using System;

public class Goal
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime TargetDate { get; set; }
    public bool IsCompleted { get; set; }

    // Read-only Progress property
    public double Progress
    {
        get
        {
            // Temporary logic: mark 100% if completed
            return IsCompleted ? 100 : 0;
        }
    }
}
