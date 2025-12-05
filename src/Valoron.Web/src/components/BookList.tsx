import React, { useEffect, useState } from 'react';
import { api } from '../api/api';
import { useSelector } from 'react-redux';
import type { RootState } from '../store/store';
import { Book as BookIcon, Plus, Clock, CheckCircle2, Trash2 } from 'lucide-react';
import { CreateBookModal } from './CreateBookModal';
import { LogSessionModal } from './LogSessionModal';

interface Activity {
    id: string;
    title: string;
    category: {
        code: string;
        name: string;
    };
    difficulty: {
        value: number;
        name: string;
    };
    isCompleted: boolean;
    measurement: {
        unit: string;
        currentValue: number;
        targetValue: number;
    };
    status?: string;
    resourceId?: string;
}

export const BookList: React.FC = () => {
    const [books, setBooks] = useState<Activity[]>([]);
    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
    const [selectedBookId, setSelectedBookId] = useState<string | null>(null);
    const [selectedBookTitle, setSelectedBookTitle] = useState<string>('');
    const userId = useSelector((state: RootState) => state.user.userId);

    const fetchBooks = async () => {
        try {
            const data = await api.fetch('/api/activities');
            if (Array.isArray(data)) {
                const bookActivities = data.filter((a: Activity) => a.category.code === 'LRN' && a.status !== 'Abandoned');
                setBooks(bookActivities);
            }
        } catch (error) {
            console.error('Failed to fetch books:', error);
        }
    };

    useEffect(() => {
        fetchBooks();
    }, [userId]);

    const handleLogClick = (book: Activity) => {
        setSelectedBookId(book.id);
        setSelectedBookTitle(book.title);
    };

    const handleAbandonClick = async (book: Activity) => {
        if (!confirm(`Are you sure you want to abandon "${book.title}"?`)) return;

        try {
            if (!book.resourceId) {
                console.error('Book resourceId is missing');
                return;
            }
            await api.fetch(`/api/books/${book.resourceId}/abandon`, {
                method: 'POST'
            });
            fetchBooks();
        } catch (error) {
            console.error('Failed to abandon book:', error);
        }
    };

    return (
        <div className="space-y-6">
            <div className="flex justify-between items-center">
                <div className="flex items-center gap-3">
                    <div className="p-3 bg-primary/10 rounded-xl">
                        <BookIcon className="text-primary w-6 h-6" />
                    </div>
                    <div>
                        <h2 className="text-2xl font-bold text-foreground">My Library</h2>
                        <p className="text-sm text-muted-foreground">Track your reading progress</p>
                    </div>
                </div>
                <button
                    onClick={() => setIsCreateModalOpen(true)}
                    className="group relative inline-flex items-center justify-center px-6 py-3 font-medium text-primary-foreground transition-all duration-300 bg-primary rounded-xl hover:bg-primary/90 hover:shadow-lg hover:shadow-primary/30 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary"
                >
                    <Plus className="w-5 h-5 mr-2 transition-transform group-hover:rotate-90" />
                    Add Book
                </button>
            </div>

            {books.length === 0 ? (
                <div className="flex flex-col items-center justify-center py-20 bg-card/50 backdrop-blur-sm border border-dashed border-border rounded-2xl text-center group hover:border-primary/50 transition-colors">
                    <div className="p-4 bg-secondary rounded-full mb-4 group-hover:scale-110 transition-transform duration-300">
                        <BookIcon className="text-muted-foreground w-8 h-8 group-hover:text-primary transition-colors" />
                    </div>
                    <h3 className="text-xl font-bold text-foreground mb-2">No books yet</h3>
                    <p className="text-muted-foreground mb-6 max-w-sm">
                        Your library is empty. Start your journey by adding your first book to track.
                    </p>
                    <button
                        onClick={() => setIsCreateModalOpen(true)}
                        className="text-primary hover:text-primary/80 font-semibold hover:underline decoration-2 underline-offset-4 transition-all"
                    >
                        Add your first book
                    </button>
                </div>
            ) : (
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {books.map((book) => (
                        <div key={book.id} className="group relative bg-card/80 backdrop-blur-sm border border-border rounded-2xl p-6 hover:shadow-xl hover:shadow-primary/5 hover:-translate-y-1 transition-all duration-300">
                            <div className="absolute top-0 left-0 w-full h-1 bg-gradient-to-r from-transparent via-primary/50 to-transparent opacity-0 group-hover:opacity-100 transition-opacity" />

                            <div className="flex justify-between items-start mb-6">
                                <div className="p-3 bg-secondary/50 rounded-xl group-hover:bg-primary/10 transition-colors">
                                    <BookIcon className="text-foreground group-hover:text-primary transition-colors w-6 h-6" />
                                </div>
                                {book.isCompleted && (
                                    <span className="bg-green-500/10 text-green-600 text-xs font-bold px-3 py-1 rounded-full flex items-center gap-1 border border-green-500/20">
                                        <CheckCircle2 size={12} /> COMPLETED
                                    </span>
                                )}
                            </div>

                            <h3 className="text-xl font-bold mb-2 line-clamp-1 text-foreground group-hover:text-primary transition-colors">
                                {book.title}
                            </h3>

                            <div className="flex justify-between text-sm text-muted-foreground mb-4">
                                <span>Progress</span>
                                <span className="font-medium text-foreground">
                                    {Math.round((book.measurement.currentValue / book.measurement.targetValue) * 100)}%
                                </span>
                            </div>

                            {/* Progress Bar */}
                            <div className="h-2 bg-secondary rounded-full overflow-hidden mb-6">
                                <div
                                    className="h-full bg-gradient-to-r from-primary to-purple-400 transition-all duration-1000 ease-out"
                                    style={{ width: `${Math.min(100, (book.measurement.currentValue / book.measurement.targetValue) * 100)}%` }}
                                />
                            </div>

                            <div className="flex items-center justify-between gap-4">
                                <span className="text-xs font-medium text-muted-foreground bg-secondary px-2 py-1 rounded-md">
                                    {book.measurement.currentValue} / {book.measurement.targetValue} pages
                                </span>
                                <div className="flex gap-2 flex-1">
                                    <button
                                        onClick={() => handleLogClick(book)}
                                        disabled={book.isCompleted}
                                        className="flex-1 bg-secondary hover:bg-secondary/80 text-secondary-foreground py-2.5 rounded-xl transition-all text-sm font-semibold flex items-center justify-center gap-2 disabled:opacity-50 disabled:cursor-not-allowed hover:shadow-md active:scale-95"
                                    >
                                        <Clock size={16} />
                                        {book.isCompleted ? 'Done' : 'Log'}
                                    </button>
                                    {!book.isCompleted && (
                                        <button
                                            onClick={() => handleAbandonClick(book)}
                                            className="bg-red-500/10 hover:bg-red-500/20 text-red-600 p-2.5 rounded-xl transition-all hover:shadow-md active:scale-95"
                                            title="Abandon Book"
                                        >
                                            <Trash2 size={16} />
                                        </button>
                                    )}
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            <CreateBookModal
                isOpen={isCreateModalOpen}
                onClose={() => setIsCreateModalOpen(false)}
                onBookCreated={fetchBooks}
            />

            {selectedBookId && (
                <LogSessionModal
                    isOpen={!!selectedBookId}
                    onClose={() => setSelectedBookId(null)}
                    activityId={selectedBookId}
                    bookTitle={selectedBookTitle}
                    onSessionLogged={fetchBooks}
                />
            )}
        </div>
    );
};
