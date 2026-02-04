using System.Collections.Generic;
using System.Threading.Tasks;
using NotesManager.Models;

namespace NotesManager.Data
{
    public interface INotesRepository
    {
        Task<IEnumerable<Note>> GetAllNotesAsync();
        Task<Note?> GetNoteByIdAsync(string noteId);
        Task<Note?> GetNoteByNameAsync(string noteName);
        Task<bool> NoteNameExistsAsync(string noteName, string? excludeNoteId = null);
        Task<Note> CreateNoteAsync(Note note);
        Task<Note> UpdateNoteAsync(Note note);
        Task<bool> DeleteNoteAsync(string noteId);
        Task<bool> RenameNoteAsync(string noteId, string newName);
        Task<IEnumerable<Note>> SearchNotesAsync(string searchTerm);
        Task<IEnumerable<Note>> GetNotesSortedAsync(string sortBy, bool ascending = true);
    }
}
