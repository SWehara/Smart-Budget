using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Management.Models
{
        public class Category : INotifyPropertyChanged
        {
            public string Name { get; set; }
            public double Limit { get; set; }
            public double Spent { get; set; }

            private double _remaining;
            public double Remaining
            {
                get => _remaining;
                set
                {
                    _remaining = value;
                    OnPropertyChanged(nameof(Remaining));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

