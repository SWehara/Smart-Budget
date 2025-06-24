using System;
using System.ComponentModel;

public class BudgetCategory : INotifyPropertyChanged
{
    public int Id { get; set; }

    private string name;
    public string Name
    {
        get => name;
        set { name = value; OnPropertyChanged(nameof(Name)); }
    }

    private double limit;
    public double Limit
    {
        get => limit;
        set
        {
            limit = value;
            OnPropertyChanged(nameof(Limit));
            OnPropertyChanged(nameof(Remaining));
            OnPropertyChanged(nameof(WarningLevel));
        }
    }

    private double spent;
    public double Spent
    {
        get => spent;
        set
        {
            spent = value;
            OnPropertyChanged(nameof(Spent));
            OnPropertyChanged(nameof(Remaining));
            OnPropertyChanged(nameof(WarningLevel));
        }
    }

    public double Remaining => Math.Max(Limit - Spent, 0);

    public string WarningLevel
    {
        get
        {
            if (Spent > Limit) return "Over";
            if (Limit > 0 && Spent >= 0.85 * Limit) return "Near";
            return "OK";
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
