using OnnxModelApp.Services;

namespace OnnxModelApp
{
    public partial class MainPage : ContentPage
    {
        private readonly OnnxModelService _onnxModelService;

        public MainPage(OnnxModelService onnxModelService)
        {
            InitializeComponent();
            _onnxModelService = onnxModelService;
        }

        private void PredictButton_Clicked(object sender, EventArgs e)
        {
            var (exerciseMsg, muscleMsg) = _onnxModelService.PredictFromFile();

            ExerciseLabel.Text = exerciseMsg;
            MuscleLabel.Text = muscleMsg;
        }
    }
}
