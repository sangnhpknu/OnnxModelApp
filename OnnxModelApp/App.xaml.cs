namespace OnnxModelApp
{
    public partial class App : Application
    {
        //public App(MainPage mainPage)
        //{
        //    InitializeComponent();

        //    MainPage = mainPage;
        //}
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
