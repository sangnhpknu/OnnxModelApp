namespace OnnxModelApp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;

public partial class ReportPage : ContentPage
{
    public ReportPage()
    {
        InitializeComponent();
        BindingContext = new ReportViewModel();
    }

    private void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        var viewModel = BindingContext as ReportViewModel;
        viewModel?.UpdateExercisesForDate(e.NewDate);
    }

    public class Exercise
    {
        public string ExerciseName { get; set; }
        public string Duration { get; set; }
    }

    public class ChartDataModel
    {
        public string Date { get; set; }
        public int ExercisesCount { get; set; }
        public double TimeSpent { get; set; } // Time in minutes
    }

    public class ReportViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ChartDataModel> ChartData { get; set; }
        public ObservableCollection<Exercise> ExercisesForSelectedDate { get; set; }
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                    UpdateExercisesForDate(_selectedDate);
                }
            }
        }

        public ReportViewModel()
        {
            // Initialize chart data
            ChartData = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel { Date = "2025-03-25", ExercisesCount = 4, TimeSpent = 35 },
                new ChartDataModel { Date = "2025-03-26", ExercisesCount = 3, TimeSpent = 40 },
                new ChartDataModel { Date = "2025-03-27", ExercisesCount = 1, TimeSpent = 30 },
                new ChartDataModel { Date = "2025-03-28", ExercisesCount = 3, TimeSpent = 40 },
            };

            ExercisesForSelectedDate = new ObservableCollection<Exercise>();

            // Initialize exercises for the first date
            _selectedDate = DateTime.Parse("2025-03-25");
            UpdateExercisesForDate(_selectedDate);
        }

        public void UpdateExercisesForDate(DateTime date)
        {
            ExercisesForSelectedDate.Clear();
            var dateString = date.ToString("yyyy-MM-dd");

            // Example logic to populate exercises based on date
            var exercises = dateString switch
            {
                "2025-03-25" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Dumbbell Curl", Duration = " 50 reps - 5 mins" },
                    new Exercise { ExerciseName = "Dumbbell Shoulder Press", Duration = "30 reps - 15 mins" },
                    new Exercise { ExerciseName = "Seated Dumbbell Front Raise", Duration = "30 reps - 5 mins" },
                    new Exercise { ExerciseName = "Dumbbell Standing Lateral Raise", Duration = "20 reps - 10 mins" },
                },
                "2025-03-26" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Barbell Upright Row", Duration = "30 reps - 10 mins" },
                    new Exercise { ExerciseName = "Dumbbell Seated Bent-Over Raise", Duration = "50 reps - 20 mins" },
                    new Exercise { ExerciseName = "Barbell Curl", Duration = "30 reps - 10 mins" }
                },
                "2025-03-27" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Concentration Curl", Duration = "50 reps - 30 mins" },
                },
                "2025-03-28" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Dumbbell Kickback", Duration = "30 reps - 15 mins" },
                    new Exercise { ExerciseName = "Wrist Curl", Duration = "40 reps - 10 mins" },
                    new Exercise { ExerciseName = "Barbell Standing Rear Wrist Curl", Duration = "40 reps - 15 mins" },                    
                },
                _ => new List<Exercise>()
            };

            foreach (var exercise in exercises)
            {
                ExercisesForSelectedDate.Add(exercise);
            }

            // Notify the UI of property changes
            OnPropertyChanged(nameof(ExercisesForSelectedDate));
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}