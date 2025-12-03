import React, { useState } from 'react';
import { X, BookOpen } from 'lucide-react';
import { api } from '../api/api';

interface CreateBookModalProps {
    isOpen: boolean;
    onClose: () => void;
    onBookCreated: () => void;
}

export const CreateBookModal: React.FC<CreateBookModalProps> = ({ isOpen, onClose, onBookCreated }) => {
    const [title, setTitle] = useState('');
    const [author, setAuthor] = useState('');
    const [totalPages, setTotalPages] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    if (!isOpen) return null;

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsLoading(true);
        try {
            await api.fetch('/api/books', {
                method: 'POST',
                body: JSON.stringify({
                    title,
                    author,
                    totalPages: parseInt(totalPages)
                }),
            });
            onBookCreated();
            onClose();
            // Reset form
            setTitle('');
            setAuthor('');
            setTotalPages('');
        } catch (error) {
            console.error('Failed to create book:', error);
            alert('Failed to create book');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4">
            <div className="bg-card border border-border rounded-xl shadow-2xl w-full max-w-md animate-in fade-in zoom-in duration-200">
                <div className="flex justify-between items-center p-6 border-b border-border">
                    <div className="flex items-center gap-2">
                        <BookOpen className="text-primary" size={24} />
                        <h2 className="text-xl font-bold">Add New Book</h2>
                    </div>
                    <button onClick={onClose} className="text-muted-foreground hover:text-foreground transition-colors">
                        <X size={24} />
                    </button>
                </div>

                <form onSubmit={handleSubmit} className="p-6 space-y-4">
                    <div>
                        <label className="block text-sm font-medium mb-1.5">Book Title</label>
                        <input
                            type="text"
                            required
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            className="w-full bg-secondary border-transparent focus:border-primary focus:ring-1 focus:ring-primary rounded-md px-3 py-2 text-sm transition-all"
                            placeholder="e.g. The Hobbit"
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium mb-1.5">Author</label>
                        <input
                            type="text"
                            required
                            value={author}
                            onChange={(e) => setAuthor(e.target.value)}
                            className="w-full bg-secondary border-transparent focus:border-primary focus:ring-1 focus:ring-primary rounded-md px-3 py-2 text-sm transition-all"
                            placeholder="e.g. J.R.R. Tolkien"
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium mb-1.5">Total Pages</label>
                        <input
                            type="number"
                            required
                            min="1"
                            value={totalPages}
                            onChange={(e) => setTotalPages(e.target.value)}
                            className="w-full bg-secondary border-transparent focus:border-primary focus:ring-1 focus:ring-primary rounded-md px-3 py-2 text-sm transition-all"
                            placeholder="e.g. 300"
                        />
                    </div>

                    <div className="pt-4 flex justify-end gap-3">
                        <button
                            type="button"
                            onClick={onClose}
                            className="px-4 py-2 rounded-md hover:bg-accent transition-colors text-sm font-medium"
                        >
                            Cancel
                        </button>
                        <button
                            type="submit"
                            disabled={isLoading}
                            className="bg-primary text-primary-foreground px-4 py-2 rounded-md hover:bg-primary/90 transition-colors text-sm font-medium disabled:opacity-50"
                        >
                            {isLoading ? 'Creating...' : 'Create Book'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};
