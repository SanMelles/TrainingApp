using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrainingApp.Models;

namespace TrainingApp.Data
{
    public class WorkoutDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public WorkoutDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<TrainingSession>().Wait();
        }

        public Task<List<TrainingSession>> GetTrainingSessionsAsync()
        {
            return _database.Table<TrainingSession>().ToListAsync();
        }

        public Task<int> AddTrainingSessionAsync(TrainingSession session)
        {
            return _database.InsertAsync(session);
        }

        public async Task<bool> SaveTrainingSessionAsync(TrainingSession session, List<TrainingSessionExercise> exercises)
        {
            try
            {
                await _database.InsertAsync(session);
                foreach (var exercise in exercises)
                {
                    exercise.TrainingSessionId = session.Id;
                    await _database.InsertAsync(exercise);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving training session: {ex.Message}");
                return false;
            }
        }

        public Task<List<TrainingSessionExercise>> GetExercisesForSessionAsync(int sessionId)
        {
            return _database.Table<TrainingSessionExercise>()
                            .Where(e => e.TrainingSessionId == sessionId)
                            .ToListAsync();
        }

        public Task<int> SaveExerciseAsync(TrainingSessionExercise exercise)
        {
            return _database.InsertAsync(exercise);
        }
    }
}
