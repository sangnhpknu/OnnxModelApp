using System.Timers;

namespace OnnxModelApp;

public partial class TrackingPage : ContentPage
{
    private System.Timers.Timer _exerciseTimer;
    private TimeSpan _exerciseDuration;

    private System.Timers.Timer _autoUpdateTimer;
    private int _repetitionsCount;
    private Random _random;

    public TrackingPage()
    {
        InitializeComponent();

        // Initialize Timer
        _exerciseDuration = TimeSpan.Zero;
        _exerciseTimer = new System.Timers.Timer(1000); // 1-second interval
        _exerciseTimer.Elapsed += OnTimerElapsed;

        _autoUpdateTimer = new System.Timers.Timer(2000); // 2-second interval
        _autoUpdateTimer.Elapsed += OnAutoUpdateElapsed;

        _repetitionsCount = 0;
        _random = new Random();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _exerciseTimer.Start();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _exerciseTimer.Stop();
        _autoUpdateTimer.Stop();
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        _exerciseDuration = _exerciseDuration.Add(TimeSpan.FromSeconds(1));
        MainThread.BeginInvokeOnMainThread(() =>
        {
            TimerLabel.Text = _exerciseDuration.ToString(@"hh\:mm\:ss");
        });
    }

    private void OnAutoUpdateElapsed(object sender, ElapsedEventArgs e)
    {
        _repetitionsCount += 1;
        double performance = _random.NextDouble() * 0.5 + 0.5; // 0.5 - 1.0

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Repetitions.Text = _repetitionsCount.ToString();
            PerformanceBar.Progress = performance;
        });
    }

    private void OnUpdateDataClicked(object sender, EventArgs e)
    {
        // Update exercise data (demo values)
        //ExerciseNameLabel.Text = "Barbell Curl";
        if (!_autoUpdateTimer.Enabled)
        {
            _autoUpdateTimer.Start();
        }
    }
}