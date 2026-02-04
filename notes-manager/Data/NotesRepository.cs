using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotesManager.Models;

namespace NotesManager.Data
{
    public class NotesRepository : INotesRepository
    {
        private readonly NotesDbContext _context;

        public NotesRepository(NotesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            return await _context.Notes
                .OrderByDescending(n => n.LastModified)
                .ToListAsync();
        }

        public async Task<Note?> GetNoteByIdAsync(string noteId)
        {
            return await _context.Notes.FindAsync(noteId);
        }

        public async Task<Note?> GetNoteByNameAsync(string noteName)
        {
            return await _context.Notes
                .FirstOrDefaultAsync(n => n.NoteName == noteName);
        }

        public async Task<bool> NoteNameExistsAsync(string noteName, string? excludeNoteId = null)
        {
            var query = _context.Notes.Where(n => n.NoteName == noteName);
            
            if (!string.IsNullOrEmpty(excludeNoteId))
            {
                query = query.Where(n => n.NoteId != excludeNoteId);
            }
            
            return await query.AnyAsync();
        }

        public async Task<Note> CreateNoteAsync(Note note)
        {
            note.NoteId = Guid.NewGuid().ToString();
            note.CreatedDate = DateTime.UtcNow;
            note.LastModified = DateTime.UtcNow;
            
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            
            return note;
        }

        public async Task<Note> UpdateNoteAsync(Note note)
        {
            note.LastModified = DateTime.UtcNow;
            
            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return note;
        }

        public async Task<bool> DeleteNoteAsync(string noteId)
        {
            var note = await _context.Notes.FindAsync(noteId);
            if (note == null)
            {
                return false;
            }
            
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> RenameNoteAsync(string noteId, string newName)
        {
            var note = await _context.Notes.FindAsync(noteId);
            if (note == null)
            {
                return false;
            }
            
            note.NoteName = newName;
            note.LastModified = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<IEnumerable<Note>> SearchNotesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllNotesAsync();
            }
            
            searchTerm = searchTerm.ToLower();
            
            return await _context.Notes
                .Where(n => n.NoteName.ToLower().Contains(searchTerm) || 
                           (n.Content != null && n.Content.ToLower().Contains(searchTerm)))
                .OrderByDescending(n => n.LastModified)
                .ToListAsync();
        }

        public async Task<IEnumerable<Note>> GetNotesSortedAsync(string sortBy, bool ascending = true)
        {
            IQueryable<Note> query = _context.Notes;
            
            query = sortBy.ToLower() switch
            {
                "name" => ascending ? query.OrderBy(n => n.NoteName) : query.OrderByDescending(n => n.NoteName),
                "created" => ascending ? query.OrderBy(n => n.CreatedDate) : query.OrderByDescending(n => n.CreatedDate),
                "modified" => ascending ? query.OrderBy(n => n.LastModified) : query.OrderByDescending(n => n.LastModified),
                _ => query.OrderByDescending(n => n.LastModified)
            };
            
            return await query.ToListAsync();
        }
    }
}
