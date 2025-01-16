using Microsoft.Extensions.Logging;
using TrainingApp.Data;
using TrainingApp.Models;

namespace TrainingApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<WorkoutDatabase>(provider =>
            {
                var dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "workout.db3");
                return new WorkoutDatabase(dbPath);
            });

            // Register pages and view models for DI
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<CreateTrainingSessionPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // Initialize database on the main thread
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var serviceProvider = app.Services;
                await InitializeDatabase(serviceProvider);
            });

            return app;
        }

        // Example of seeding the database if necessary
        private static async Task InitializeDatabase(IServiceProvider services)
        {
            var database = services.GetRequiredService<WorkoutDatabase>();

            var sessions = await database.GetTrainingSessionsAsync();
            if (!sessions.Any())
            {
                // Add a sample training session if the database is empty
                await database.AddTrainingSessionAsync(new TrainingSession
                {
                    Name = "Sample Session",
                    Date = DateTime.Now
                });
            }
        }
    }
}
