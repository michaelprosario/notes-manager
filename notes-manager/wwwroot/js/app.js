// Notes Manager - Client-side JavaScript

// Toast notifications
function showToast(message, type = 'success') {
    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;
    toast.textContent = message;
    toast.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        padding: 1rem 1.5rem;
        background: ${type === 'success' ? '#28a745' : '#dc3545'};
        color: white;
        border-radius: 4px;
        box-shadow: 0 4px 8px rgba(0,0,0,0.2);
        z-index: 9999;
        animation: slideIn 0.3s ease-out;
    `;
    
    document.body.appendChild(toast);
    
    setTimeout(() => {
        toast.style.animation = 'slideOut 0.3s ease-out';
        setTimeout(() => toast.remove(), 300);
    }, 3000);
}

// Auto-hide alerts after 5 seconds
document.addEventListener('DOMContentLoaded', function() {
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(alert => {
        setTimeout(() => {
            alert.style.transition = 'opacity 0.3s';
            alert.style.opacity = '0';
            setTimeout(() => alert.remove(), 300);
        }, 5000);
    });
});

// Confirm delete actions
document.addEventListener('click', function(e) {
    if (e.target.classList.contains('delete-btn') && !e.target.closest('form')) {
        if (!confirm('Are you sure you want to delete this note? This action cannot be undone.')) {
            e.preventDefault();
        }
    }
});

// Character counter for textareas
const textareas = document.querySelectorAll('textarea.form-control');
textareas.forEach(textarea => {
    const counter = document.createElement('div');
    counter.className = 'char-counter';
    counter.style.cssText = 'text-align: right; font-size: 0.875rem; color: #6c757d; margin-top: 0.25rem;';
    textarea.parentNode.appendChild(counter);
    
    function updateCounter() {
        const length = textarea.value.length;
        counter.textContent = `${length.toLocaleString()} characters`;
    }
    
    textarea.addEventListener('input', updateCounter);
    updateCounter();
});

// Auto-save draft to localStorage
const noteForm = document.querySelector('form[action*="Create"], form[action*="Edit"]');
if (noteForm) {
    const noteNameInput = noteForm.querySelector('input[name="NoteName"]');
    const contentTextarea = noteForm.querySelector('textarea[name="Content"]');
    
    const draftKey = 'note-draft-' + (noteForm.action.includes('Edit') ? 
        noteForm.querySelector('input[name="NoteId"]')?.value : 'new');
    
    // Load draft on page load
    const draft = localStorage.getItem(draftKey);
    if (draft && !noteNameInput.value && !contentTextarea.value) {
        const { noteName, content } = JSON.parse(draft);
        if (confirm('A draft was found. Would you like to restore it?')) {
            noteNameInput.value = noteName;
            contentTextarea.value = content;
        } else {
            localStorage.removeItem(draftKey);
        }
    }
    
    // Save draft on input
    let saveTimeout;
    function saveDraft() {
        clearTimeout(saveTimeout);
        saveTimeout = setTimeout(() => {
            const draft = {
                noteName: noteNameInput.value,
                content: contentTextarea.value,
                timestamp: new Date().toISOString()
            };
            localStorage.setItem(draftKey, JSON.stringify(draft));
        }, 1000);
    }
    
    noteNameInput.addEventListener('input', saveDraft);
    contentTextarea.addEventListener('input', saveDraft);
    
    // Clear draft on successful submit
    noteForm.addEventListener('submit', function() {
        localStorage.removeItem(draftKey);
    });
}

// Keyboard shortcuts
document.addEventListener('keydown', function(e) {
    // Ctrl/Cmd + N: New note
    if ((e.ctrlKey || e.metaKey) && e.key === 'n') {
        const createLink = document.querySelector('a[href*="Create"]');
        if (createLink) {
            e.preventDefault();
            window.location.href = createLink.href;
        }
    }
    
    // Ctrl/Cmd + F: Focus search
    if ((e.ctrlKey || e.metaKey) && e.key === 'f') {
        const searchInput = document.getElementById('searchInput');
        if (searchInput) {
            e.preventDefault();
            searchInput.focus();
        }
    }
    
    // ESC: Close modal
    if (e.key === 'Escape') {
        const modal = document.querySelector('.modal[style*="display: flex"]');
        if (modal) {
            modal.style.display = 'none';
        }
    }
});

// Add animation keyframes
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from {
            transform: translateX(100%);
            opacity: 0;
        }
        to {
            transform: translateX(0);
            opacity: 1;
        }
    }
    
    @keyframes slideOut {
        from {
            transform: translateX(0);
            opacity: 1;
        }
        to {
            transform: translateX(100%);
            opacity: 0;
        }
    }
`;
document.head.appendChild(style);

// Highlight search terms in results
function highlightSearchTerms(text, searchTerm) {
    if (!searchTerm) return text;
    
    const regex = new RegExp(`(${searchTerm})`, 'gi');
    return text.replace(regex, '<mark>$1</mark>');
}

// Export function for future use
window.NotesApp = {
    showToast,
    highlightSearchTerms
};
