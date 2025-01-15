namespace TrainingApp
{
    public partial class App : Application
    {
        const int WindowWidth = 500;
        const int WindowHeight = 900;

        private static WorkoutDatabase _database;

        public static WorkoutDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "workout.db3");
                    _database = new WorkoutDatabase(dbPath);
                }
                return _database;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
