using System;
using System.ComponentModel;

namespace Money_Management.Models
{
    public class FinancialGoal : INotifyPropertyChanged
    {
        private string _name;
        private double _targetAmount;
        private double _savedAmount;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Progress));
            }
        }

        public double TargetAmount
        {
            get => _targetAmount;
            set
            {
                _targetAmount = value;
                OnPropertyChanged(nameof(TargetAmount));
                OnPropertyChanged(nameof(Progress));
            }
        }

        public double SavedAmount
        {
            get => _savedAmount;
            set
            {
                _savedAmount = value;
                OnPropertyChanged(nameof(SavedAmount));
                OnPropertyChanged(nameof(Progress));
            }
        }

        public double Progress
        {
            get
            {
                if (TargetAmount == 0) return 0;
                return SavedAmount / TargetAmount;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
