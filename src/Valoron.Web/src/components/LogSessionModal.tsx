import React, { useState } from 'react';
import { X, BookOpen } from 'lucide-react';
import { api } from '../api/api';

interface LogSessionModalProps {
    isOpen: boolean;
    onClose: () => void;
    activityId: string;
    bookTitle: string;
    onSessionLogged: () => void;
}

export const LogSessionModal: React.FC<LogSessionModalProps> = ({
    isOpen,
    onClose,
    activityId,
    bookTitle,
    onSessionLogged
}) => {
    const [pagesRead, setPagesRead] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    if (!isOpen) return null;

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsLoading(true);
        try {
            await api.fetch(`/api/activities/${activityId}/session`, {
                method: 'POST',
                body: JSON.stringify({
                    pagesRead: parseInt(pagesRead)
                }),
            });
            onSessionLogged();
            onClose();
            setPagesRead('');
        } catch (error) {
            console.error('Failed to log session:', error);
            alert('Failed to log session');
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
                        <div>
                            <h2 className="text-xl font-bold">Log Reading</h2>
                            <p className="text-xs text-muted-foreground">for {bookTitle}</p>
                        </div>
                    </div>
                    <button onClick={onClose} className="text-muted-foreground hover:text-foreground transition-colors">
                        <X size={24} />
                    </button>
                </div>

                <form onSubmit={handleSubmit} className="p-6 space-y-4">
                    <div>
                        <label className="block text-sm font-medium mb-1.5">Pages Read</label>
                        <input
                            type="number"
                            required
                            min="1"
                            value={pagesRead}
                            onChange={(e) => setPagesRead(e.target.value)}
                            className="w-full bg-secondary border-transparent focus:border-primary focus:ring-1 focus:ring-primary rounded-md px-3 py-2 text-sm transition-all"
                            placeholder="e.g. 25"
                            autoFocus
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
                            {isLoading ? 'Logging...' : 'Log Session'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};
