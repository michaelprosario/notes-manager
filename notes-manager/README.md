# Notes Manager

A simple, efficient ASP.NET MVC application for managing text notes with SQLite storage.

## Features

- ✅ Create, read, update, and delete notes
- ✅ Real-time search functionality
- ✅ Sort notes by name, creation date, or last modified date
- ✅ Rename notes with validation
- ✅ Responsive design (mobile-friendly)
- ✅ Auto-save drafts to localStorage
- ✅ Keyboard shortcuts (Ctrl+N for new note, Ctrl+S to save, Ctrl+F to search)
- ✅ Character counter for note content
- ✅ SQLite database for lightweight storage

## Project Structure

```
notes-manager/
├── Controllers/
│   └── NotesController.cs          # Main controller for CRUD operations
├── Data/
│   ├── NotesDbContext.cs           # Entity Framework DbContext
│   ├── INotesRepository.cs         # Repository interface
│   └── NotesRepository.cs          # Repository implementation
├── Database/
│   └── CreateDatabase.sql          # Database schema
├── Models/
│   └── Note.cs                     # Note entity model
├── Views/
│   ├── Shared/
│   │   └── _Layout.cshtml          # Main layout template
│   ├── Notes/
│   │   ├── Index.cshtml            # Notes list view
│   │   ├── Create.cshtml           # Create note form
│   │   ├── Edit.cshtml             # Edit note form
│   │   ├── Details.cshtml          # Note details view
│   │   └── Delete.cshtml           # Delete confirmation
│   ├── _ViewStart.cshtml
│   └── _ViewImports.cshtml
├── wwwroot/
│   ├── css/
│   │   └── style.css               # Application styles
│   └── js/
│       └── app.js                  # Client-side JavaScript
├── Program.cs                       # Application entry point
├── appsettings.json                # Configuration settings
└── NotesManager.csproj             # Project file

```

## Technology Stack

- **Framework**: ASP.NET Core 8.0 MVC
- **Language**: C# 
- **Database**: SQLite with Entity Framework Core
- **Frontend**: HTML5, CSS3, Vanilla JavaScript
- **Architecture**: Repository pattern with MVC

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Any modern web browser

### Installation

1. Navigate to the project directory:
   ```bash
   cd notes-manager
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

5. Open your browser and navigate to:
   ```
   https://localhost:5001
   ```
   or
   ```
   http://localhost:5000
   ```

## Database

The application uses SQLite for data storage. The database file (`notes.db`) is created automatically on first run in the project root directory.

### Database Schema

**Notes Table:**
- `NoteId` (TEXT, PRIMARY KEY) - Unique identifier
- `NoteName` (TEXT, NOT NULL, UNIQUE) - User-defined note name
- `Content` (TEXT) - Note content
- `CreatedDate` (DATETIME) - Creation timestamp
- `LastModified` (DATETIME) - Last modification timestamp

## Usage

### Creating a Note
1. Click "+ New Note" button
2. Enter a unique note name
3. Add your content
4. Click "Create Note" or press Ctrl+S

### Editing a Note
1. Click the edit icon (📝) on any note card
2. Modify the name or content
3. Click "Save Changes" or press Ctrl+S

### Searching Notes
- Type in the search box to filter notes in real-time
- Search works on both note names and content
- Click "✕" to clear the search

### Sorting Notes
Use the sort dropdown to organize notes by:
- Recently Modified (default)
- Oldest Modified
- Name (A-Z)
- Name (Z-A)
- Newest Created
- Oldest Created

### Renaming a Note
1. Click the rename icon (✏️) on any note card
2. Enter a new name
3. Click "Rename"

### Deleting a Note
1. Click the delete icon (🗑️) on any note card
2. Confirm the deletion
3. Note will be permanently removed

## Keyboard Shortcuts

- **Ctrl+N** - Create new note
- **Ctrl+S** - Save current note (when editing)
- **Ctrl+F** - Focus search box
- **Esc** - Close modal dialogs

## Development

### Running in Development Mode

```bash
dotnet watch run
```

This enables hot reload for faster development.

### Building for Production

```bash
dotnet publish -c Release -o ./publish
```

## Configuration

Configuration settings are in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=notes.db"
  }
}
```

You can change the database location by modifying the connection string.

## Code Quality

This project follows the principles outlined in [speckit.constitution](../speckit.constitution):

- ✅ Clean, maintainable code with clear naming conventions
- ✅ Repository pattern for data access
- ✅ Model validation with data annotations
- ✅ Responsive, accessible UI (WCAG AA)
- ✅ Client-side and server-side validation
- ✅ Secure input handling and XSS protection

## Features Roadmap

Future enhancements (see [speckit.tasks](../speckit.tasks)):

- [ ] User authentication and authorization
- [ ] Cloud sync and backup
- [ ] Markdown/rich text support
- [ ] Tags and categories
- [ ] Note sharing functionality
- [ ] Version history
- [ ] Export functionality (PDF, Markdown, etc.)

## Troubleshooting

### Database Issues

If you encounter database errors:
1. Delete the `notes.db` file
2. Restart the application (it will recreate the database)

### Port Already in Use

If the default ports are in use, modify `launchSettings.json` or specify a different port:
```bash
dotnet run --urls "http://localhost:8080"
```

## License

This project is for educational and personal use.

## Contributing

This is a personal project, but suggestions and feedback are welcome!

---

**Built with ❤️ using ASP.NET Core MVC**
