using SQLite;
using System.Collections.ObjectModel;
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
            _database.CreateTableAsync<TrainingSessionExercise>().Wait();
        }

        public Task<List<TrainingSession>> GetTrainingSessionsAsync()
        {
            return _database.Table<TrainingSession>().ToListAsync();
        }

        public Task<int> AddTrainingSessionAsync(TrainingSession session)
        {
            return _database.InsertAsync(session);
        }

        public async Task<TrainingSession> SaveTrainingSessionAsync(TrainingSession session)
        {
            var sessionId = await _database.InsertAsync(session);
            session.Id = sessionId;

            return session;
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

        public async Task<int> DeleteExercisesBySessionIdAsync(int sessionId)
        {
            var exercises = await _database.Table<TrainingSessionExercise>()
                                           .Where(e => e.TrainingSessionId == sessionId)
                                           .ToListAsync();
            return exercises.Count;
        }

        public async Task SaveExercisesAsync(IEnumerable<TrainingSessionExercise> exercises)
        {
            foreach (var exercise in exercises)
            {
                await _database.InsertAsync(exercise);
            }
        }

        public async Task DeleteExerciseByIdAsync(int exerciseId)
        {
            await _database.Table<TrainingSessionExercise>()
                           .DeleteAsync(e => e.Id == exerciseId);
        }

        public async Task DeleteSessionByIdAsync(int sessionId)
        {
            // Delete all exercises associated with the session
            await _database.Table<TrainingSessionExercise>()
                           .DeleteAsync(e => e.TrainingSessionId == sessionId);

            // Delete the session itself
            await _database.Table<TrainingSession>()
                           .DeleteAsync(s => s.Id == sessionId);
        }

    }
}
