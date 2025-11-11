// In Models/Note.cs
using SQLite;

namespace NoteApp.Models
{
    public class Note
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string MarkdownText { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}