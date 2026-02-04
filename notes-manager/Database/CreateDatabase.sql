-- Create Notes table
CREATE TABLE IF NOT EXISTS Notes (
    NoteId TEXT PRIMARY KEY,
    NoteName TEXT NOT NULL UNIQUE,
    Content TEXT,
    CreatedDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    LastModified DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Create indexes for performance
CREATE INDEX IF NOT EXISTS idx_note_name ON Notes(NoteName);
CREATE INDEX IF NOT EXISTS idx_last_modified ON Notes(LastModified DESC);

-- Set SQLite pragmas for optimal performance
PRAGMA journal_mode = WAL;
PRAGMA synchronous = NORMAL;
PRAGMA foreign_keys = ON;
