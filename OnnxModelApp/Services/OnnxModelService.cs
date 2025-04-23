using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Numpy;
using System.Linq;

namespace OnnxModelApp.Services;

public class OnnxModelService : IDisposable
{
    private readonly InferenceSession _session;

    public OnnxModelService()
    {
        var modelPath = Path.Combine(FileSystem.AppDataDirectory, "ma_cnn_gf.onnx");

        using var modelStream = FileSystem.OpenAppPackageFileAsync("ma_cnn_gf.onnx").Result;
        using var fileStream = File.Create(modelPath);
        modelStream.CopyTo(fileStream);

        _session = new InferenceSession(modelPath);
    }

    public (string exercisePrediction, string musclePrediction) PredictFromFile()
    {
        // Đọc file binary vào byte array
        using var imuStream = FileSystem.OpenAppPackageFileAsync("imu_input.bin").Result;
        using var emgStream = FileSystem.OpenAppPackageFileAsync("emg_input.bin").Result;

        var imuBytes = ReadAllBytes(imuStream);
        var emgBytes = ReadAllBytes(emgStream);

        // Chuyển từ bytes sang float[]
        var imuDataArray = new float[imuBytes.Length / 4];
        var emgDataArray = new float[emgBytes.Length / 4];

        Buffer.BlockCopy(imuBytes, 0, imuDataArray, 0, imuBytes.Length);
        Buffer.BlockCopy(emgBytes, 0, emgDataArray, 0, emgBytes.Length);

        var imuTensor = new DenseTensor<float>(imuDataArray, new int[] { 32, 12, 200 });
        var emgTensor = new DenseTensor<float>(emgDataArray, new int[] { 32, 8, 200 });

        var inputs = new List<NamedOnnxValue>
    {
        NamedOnnxValue.CreateFromTensor("input.17", imuTensor),
        NamedOnnxValue.CreateFromTensor("input.1", emgTensor)
    };

        using var results = _session.Run(inputs);

        // Lấy kết quả và xử lý
        var outputExercise = results.First(x => x.Name == "212").AsTensor<float>(); // [32,14]
        var outputMuscle = results.First(x => x.Name == "216").AsTensor<float>();   // [32,3]

        // Lấy mẫu đầu tiên trong batch (window đầu tiên) làm ví dụ
        var firstExerciseResult = Enumerable.Range(0, 14).Select(i => outputExercise[0, i]).ToArray();
        var firstMuscleResult = Enumerable.Range(0, 3).Select(i => outputMuscle[0, i]).ToArray();

        firstExerciseResult = Softmax(firstExerciseResult);
        firstMuscleResult = Softmax(firstMuscleResult);

        // Tên các bài tập và vùng cơ tương ứng
        string[] exercises = new string[]
        {
        "Dumbbell Shoulder Press", "Seated Dumbbell Front Raise", "Dumbbell Standing Lateral Raise", "Barbell Upright Row",
        "Dumbbell Seated Bent-Over Raise", "Barbell Curl", "Dumbbell Curl", "Concentration Curl",
        "Close-Grip Bench Press", "Dumbbell Kickback", "Wrist Curl", "Barbell Standing Rear Wrist Curl",
        "Reverse Wrist Curl", "Barbell Reverse Curl"
        };

        string[] muscleAreas = new string[] { "Deltoid", "Upper Arm", "Lower Arm" };

        // Xác định class có xác suất cao nhất
        int exerciseIndex = Array.IndexOf(firstExerciseResult, firstExerciseResult.Max());
        int muscleIndex = Array.IndexOf(firstMuscleResult, firstMuscleResult.Max());

        string predictedExercise = $"🏋️ You are performing:\n" +
        $"👉 {exercises[exerciseIndex]}\n" +
        $"(Reliability: {firstExerciseResult[exerciseIndex]:P1})";

        string predictedMuscle = $"💪 Best muscle area are activated:\n" +
        $"👉 {muscleAreas[muscleIndex]}\n" +
        $"👉 Activated Performance: {firstMuscleResult[muscleIndex]:P1}";


        return (predictedExercise, predictedMuscle);
    }

    // Hàm softmax
    private float[] Softmax(float[] logits)
    {
        var exp = logits.Select(x => Math.Exp((double)x)).ToArray();
        var sumExp = exp.Sum();
        return exp.Select(e => (float)(e / sumExp)).ToArray();
    }

    private byte[] ReadAllBytes(Stream input)
    {
        using var ms = new MemoryStream();
        input.CopyTo(ms);
        return ms.ToArray();
    }

    public void Dispose()
    {
        _session?.Dispose();
    }

}
