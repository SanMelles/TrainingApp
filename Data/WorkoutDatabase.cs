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
            _database.CreateTableAsync<TrainingSessionExercise>().Wait();
        }

        // Fetch all training sessions
        public Task<List<TrainingSession>> GetTrainingSessionsAsync()
        {
            return _database.Table<TrainingSession>().ToListAsync();
        }

        // Add a new training session
        public Task<int> AddTrainingSessionAsync(TrainingSession session)
        {
            return _database.InsertAsync(session);
        }

        // Save a training session and return it
        public async Task<TrainingSession> SaveTrainingSessionAsync(TrainingSession session)
        {
            if (session.Id == 0)
            {
                await _database.InsertAsync(session);
            }
            else
            {
                await _database.UpdateAsync(session);
            }
            return session;
        }

        // Fetch exercises for a specific session
        public Task<List<TrainingSessionExercise>> GetExercisesForSessionAsync(int sessionId)
        {
            return _database.Table<TrainingSessionExercise>()
                            .Where(e => e.TrainingSessionId == sessionId)
                            .ToListAsync();
        }

        // Save a single exercise
        public Task<int> SaveExerciseAsync(TrainingSessionExercise exercise)
        {
            return _database.InsertAsync(exercise);
        }

        // Delete all exercises by session ID
        public Task<int> DeleteExercisesBySessionIdAsync(int sessionId)
        {
            return _database.Table<TrainingSessionExercise>()
                            .DeleteAsync(e => e.TrainingSessionId == sessionId);
        }

        // Save multiple exercises
        public async Task SaveExercisesAsync(IEnumerable<TrainingSessionExercise> exercises)
        {
            foreach (var exercise in exercises)
            {
                await _database.InsertAsync(exercise);
            }
        }

        // Delete a single exercise by its ID
        public Task<int> DeleteExerciseByIdAsync(int exerciseId)
        {
            return _database.Table<TrainingSessionExercise>()
                            .DeleteAsync(e => e.Id == exerciseId);
        }

        // Delete a training session and its associated exercises
        public async Task DeleteSessionByIdAsync(int sessionId)
        {
            // Delete all exercises associated with the session
            await _database.Table<TrainingSessionExercise>()
                           .DeleteAsync(e => e.TrainingSessionId == sessionId);

            // Delete the session itself
            await _database.Table<TrainingSession>()
                           .DeleteAsync(s => s.Id == sessionId);
        }

        public Task<List<TrainingSession>> GetAllTrainingSessionsAsync()
        {
            return _database.Table<TrainingSession>().ToListAsync();
        }
    }
}
