using System;

public class Goal
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime TargetDate { get; set; }
    public bool IsCompleted { get; set; }

    public double Progress
    {
        get
        {
            
            return IsCompleted ? 100 : 0;
        }
    }
}
