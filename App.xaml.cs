using TrainingApp.Data;

namespace TrainingApp
{
    public partial class App : Application
    {
        private static WorkoutDatabase _database;

        public static WorkoutDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new WorkoutDatabase(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "workout.db3"));
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
