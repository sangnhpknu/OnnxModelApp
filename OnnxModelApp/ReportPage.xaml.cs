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
                new ChartDataModel { Date = "1", ExercisesCount = 3, TimeSpent = 15 },
                new ChartDataModel { Date = "2", ExercisesCount = 2, TimeSpent = 20 },
                new ChartDataModel { Date = "5", ExercisesCount = 3, TimeSpent = 40 },
                new ChartDataModel { Date = "6", ExercisesCount = 2, TimeSpent = 30 },
                new ChartDataModel { Date = "9", ExercisesCount = 3, TimeSpent = 50 },
                new ChartDataModel { Date = "10", ExercisesCount = 3, TimeSpent = 55 },
                new ChartDataModel { Date = "13", ExercisesCount = 2, TimeSpent = 40 },
                new ChartDataModel { Date = "14", ExercisesCount = 3, TimeSpent = 70 },
                new ChartDataModel { Date = "17", ExercisesCount = 3, TimeSpent = 70 },
                new ChartDataModel { Date = "18", ExercisesCount = 2, TimeSpent = 45 },
                new ChartDataModel { Date = "21", ExercisesCount = 3, TimeSpent = 80 },
                new ChartDataModel { Date = "22", ExercisesCount = 3, TimeSpent = 100 },
                new ChartDataModel { Date = "25", ExercisesCount = 3, TimeSpent = 80 },
                new ChartDataModel { Date = "26", ExercisesCount = 3, TimeSpent = 90 },
                new ChartDataModel { Date = "29", ExercisesCount = 3, TimeSpent = 100 },
                new ChartDataModel { Date = "30", ExercisesCount = 3, TimeSpent = 105 },
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
                "2025-03-01" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Dumbbell Shoulder Press", Duration = "20 reps - 5 mins" },
                    new Exercise { ExerciseName = "Dumbbell Curl", Duration = "20 reps - 5 mins" },
                    new Exercise { ExerciseName = "Concentration Curl", Duration = "20 reps - 5 mins" },
                },
                "2025-03-02" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Close-Grip Bench Press", Duration = " 40 reps - 15 mins" },
                    new Exercise { ExerciseName = "Barbell Curl", Duration = "50 reps - 15 mins" },
                },
                "2025-03-05" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Barbell Curl", Duration = " 50 reps - 10 mins" },
                    new Exercise { ExerciseName = "Dumbbell Curl", Duration = "60 reps - 15 mins" },
                    new Exercise { ExerciseName = "Concentration Curl", Duration = "60 reps - 15 mins" },
                },
                "2025-03-06" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Wrist Curl", Duration = " 50 reps - 15 mins" },
                    new Exercise { ExerciseName = "Reverse Wrist Curl", Duration = "50 reps - 15 mins" },
                },
                "2025-03-09" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Dumbbell Standing Lateral Raise", Duration = " 50 reps - 15 mins" },
                    new Exercise { ExerciseName = "Dumbbell Seated Bent-Over Raise", Duration = "50 reps - 20 mins" },
                    new Exercise { ExerciseName = "Dumbbell Shoulder Press", Duration = "40 reps - 15 mins" },
                },
                "2025-03-10" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Dumbbell Kickback", Duration = "50 reps - 20 mins" },
                    new Exercise { ExerciseName = "Close-Grip Bench Press", Duration = "50 reps - 20 mins" },
                    new Exercise { ExerciseName = "Barbell Curl", Duration = "30 reps - 15 mins" },
                },
                "2025-03-13" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Barbell Standing Rear Wrist Curl", Duration = " 60 reps - 20 mins" },
                    new Exercise { ExerciseName = "Barbell Reverse Curl", Duration = "40 reps - 20 mins" },
                },
                "2025-03-14" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Dumbbell Standing Lateral Raise", Duration = " 60 reps - 25 mins" },
                    new Exercise { ExerciseName = "Dumbbell Shoulder Press", Duration = "60 reps - 25 mins" },
                    new Exercise { ExerciseName = "Seated Dumbbell Front Raise", Duration = "50 reps - 20 mins" },
                },
                "2025-03-17" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Concentration Curl", Duration = " 60 reps - 25 mins" },
                    new Exercise { ExerciseName = "Dumbbell Curl", Duration = "60 reps - 25 mins" },
                    new Exercise { ExerciseName = "Barbell Curl", Duration = "50 reps - 20 mins" },
                },
                "2025-03-18" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Barbell Reverse Curl", Duration = " 50 reps - 20 mins" },
                    new Exercise { ExerciseName = "Barbell Standing Rear Wrist Curl", Duration = "50 reps - 25 mins" },
                },
                "2025-03-21" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Dumbbell Shoulder Press", Duration = " 70 reps - 25 mins" },
                    new Exercise { ExerciseName = "Seated Dumbbell Front Raise", Duration = "90 reps - 35 mins" },
                    new Exercise { ExerciseName = "Dumbbell Standing Lateral Raise", Duration = "50 reps - 20 mins" },
                },
                "2025-03-22" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Barbell Curl", Duration = " 70 reps - 25 mins" },
                    new Exercise { ExerciseName = "Dumbbell Curl", Duration = "90 reps - 35 mins" },
                    new Exercise { ExerciseName = "Concentration Curl", Duration = "90 reps - 40 mins" },
                },
                "2025-03-25" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Wrist Curl", Duration = " 80 reps - 30 mins" },
                    new Exercise { ExerciseName = "Reverse Wrist Curl", Duration = "80 reps - 30 mins" },
                    new Exercise { ExerciseName = "Barbell Reverse Curl", Duration = "60 reps - 20 mins" },
                },
                "2025-03-26" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Barbell Upright Row", Duration = "80 reps - 30 mins" },
                    new Exercise { ExerciseName = "Dumbbell Seated Bent-Over Raise", Duration = "80 reps - 40 mins" },
                    new Exercise { ExerciseName = "Dumbbell Shoulder Press", Duration = "60 reps - 20 mins" }
                },
                "2025-03-29" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Barbell Curl", Duration = " 70 reps - 25 mins" },
                    new Exercise { ExerciseName = "Dumbbell Curl", Duration = "90 reps - 35 mins" },
                    new Exercise { ExerciseName = "Concentration Curl", Duration = "90 reps - 40 mins" },
                },
                "2025-03-30" => new List<Exercise>
                {
                    new Exercise { ExerciseName = "Dumbbell Kickback", Duration = "60 reps - 25 mins" },
                    new Exercise { ExerciseName = "Wrist Curl", Duration = "80 reps - 40 mins" },
                    new Exercise { ExerciseName = "Barbell Standing Rear Wrist Curl", Duration = "40 reps - 40 mins" },
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