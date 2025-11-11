using NoteApp.Models;
using SQLite;

namespace NoteApp
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection _db = null!;

        private static async Task Init()
        {
            if (_db != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "NoteApp.db3");
            _db = new SQLiteAsyncConnection(databasePath);
            await _db.CreateTableAsync<Note>();
        }

        public static async Task<List<Note>> GetNotes()
        {
            await Init();
            return await _db.Table<Note>().OrderByDescending(n => n.Date).ToListAsync();
        }

        public static async Task<Note> GetNote(int id)
        {
            await Init();
            return await _db.Table<Note>().FirstOrDefaultAsync(n => n.Id == id);
        }

        public static async Task<int> SaveNote(Note note)
        {
            await Init();
            if (note.Id != 0)
            {
                return await _db.UpdateAsync(note);
            }
            else
            {
                return await _db.InsertAsync(note);
            }
        }

        public static async Task<int> DeleteNote(Note note)
        {
            await Init();
            return await _db.DeleteAsync(note);
        }
    }
}
