
using LiveCharts;
using LiveCharts.Wpf;
using System.ComponentModel;

namespace Money_Management.ViewModel
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        public SeriesCollection PieSeries { get; set; }

        public DashboardViewModel()
        {
            PieSeries = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Food",
                    Values = new ChartValues<double> { 40 },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Travel",
                    Values = new ChartValues<double> { 25 },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Bills",
                    Values = new ChartValues<double> { 20 },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Other",
                    Values = new ChartValues<double> { 15 },
                    DataLabels = true
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
