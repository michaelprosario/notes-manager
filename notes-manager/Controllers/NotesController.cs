using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotesManager.Data;
using NotesManager.Models;

namespace NotesManager.Controllers
{
    public class NotesController : Controller
    {
        private readonly INotesRepository _repository;

        public NotesController(INotesRepository repository)
        {
            _repository = repository;
        }

        // GET: Notes
        public async Task<IActionResult> Index(string sortBy = "modified", bool ascending = false, string searchTerm = "")
        {
            IEnumerable<Note> notes;
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                notes = await _repository.SearchNotesAsync(searchTerm);
                ViewData["SearchTerm"] = searchTerm;
            }
            else
            {
                notes = await _repository.GetNotesSortedAsync(sortBy, ascending);
            }
            
            ViewData["SortBy"] = sortBy;
            ViewData["Ascending"] = ascending;
            
            return View(notes);
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var note = await _repository.GetNoteByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NoteName,Content")] Note note)
        {
            if (ModelState.IsValid)
            {
                // Check if note name already exists
                if (await _repository.NoteNameExistsAsync(note.NoteName))
                {
                    ModelState.AddModelError("NoteName", "A note with this name already exists.");
                    return View(note);
                }

                await _repository.CreateNoteAsync(note);
                TempData["SuccessMessage"] = $"Note '{note.NoteName}' created successfully.";
                return RedirectToAction(nameof(Index));
            }
            
            return View(note);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var note = await _repository.GetNoteByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            
            return View(note);
        }

        // POST: Notes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("NoteId,NoteName,Content,CreatedDate")] Note note)
        {
            if (id != note.NoteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check if new name conflicts with existing note
                if (await _repository.NoteNameExistsAsync(note.NoteName, note.NoteId))
                {
                    ModelState.AddModelError("NoteName", "A note with this name already exists.");
                    return View(note);
                }

                await _repository.UpdateNoteAsync(note);
                TempData["SuccessMessage"] = $"Note '{note.NoteName}' updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var note = await _repository.GetNoteByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var note = await _repository.GetNoteByIdAsync(id);
            var noteName = note?.NoteName ?? "Unknown";
            
            if (await _repository.DeleteNoteAsync(id))
            {
                TempData["SuccessMessage"] = $"Note '{noteName}' deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete note.";
            }
            
            return RedirectToAction(nameof(Index));
        }

        // POST: Notes/Rename
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rename(string id, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                return Json(new { success = false, message = "Note name cannot be empty." });
            }

            // Check if new name already exists
            if (await _repository.NoteNameExistsAsync(newName, id))
            {
                return Json(new { success = false, message = "A note with this name already exists." });
            }

            if (await _repository.RenameNoteAsync(id, newName))
            {
                return Json(new { success = true, message = $"Note renamed to '{newName}' successfully." });
            }
            
            return Json(new { success = false, message = "Failed to rename note." });
        }

        // GET: Notes/Search
        public async Task<IActionResult> Search(string term)
        {
            var notes = await _repository.SearchNotesAsync(term);
            return PartialView("_NotesList", notes);
        }
    }
}
