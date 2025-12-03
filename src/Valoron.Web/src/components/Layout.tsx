import React, { useState } from 'react';
import { User, LogOut, Menu, X } from 'lucide-react';
import { useSelector, useDispatch } from 'react-redux';
import type { RootState } from '../store/store';
import { setUserId } from '../store/userSlice';

interface LayoutProps {
    children: React.ReactNode;
}

export const Layout: React.FC<LayoutProps> = ({ children }) => {
    const userId = useSelector((state: RootState) => state.user.userId);
    const dispatch = useDispatch();
    const [isProfileOpen, setIsProfileOpen] = useState(false);
    const [tempUserId, setTempUserId] = useState(userId);

    const handleSaveUserId = () => {
        dispatch(setUserId(tempUserId));
        setIsProfileOpen(false);
    };

    return (
        <div className="min-h-screen bg-background text-foreground flex flex-col bg-[radial-gradient(ellipse_at_top,_var(--tw-gradient-stops))] from-primary/20 via-background to-background">
            {/* Top Navigation Bar */}
            <nav className="sticky top-0 z-50 w-full border-b border-border/40 bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60">
                <div className="container mx-auto px-4 h-16 flex items-center justify-between">
                    <div className="flex items-center gap-2">
                        <div className="w-8 h-8 bg-primary rounded-lg flex items-center justify-center">
                            <span className="text-primary-foreground font-bold text-xl">V</span>
                        </div>
                        <span className="font-bold text-xl tracking-tight">Valoron</span>
                    </div>

                    <div className="flex items-center gap-4">
                        <button
                            onClick={() => setIsProfileOpen(!isProfileOpen)}
                            className="flex items-center gap-2 hover:bg-accent hover:text-accent-foreground px-3 py-2 rounded-md transition-colors"
                        >
                            <div className="w-8 h-8 bg-secondary rounded-full flex items-center justify-center">
                                <User size={18} />
                            </div>
                            <span className="text-sm font-medium hidden md:block">
                                {userId.substring(0, 8)}...
                            </span>
                        </button>
                    </div>
                </div>
            </nav>

            {/* Profile Modal */}
            {isProfileOpen && (
                <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 backdrop-blur-sm">
                    <div className="bg-card border border-border rounded-xl shadow-lg w-full max-w-md p-6 m-4 animate-in fade-in zoom-in duration-200">
                        <div className="flex justify-between items-center mb-6">
                            <h2 className="text-xl font-bold">User Profile</h2>
                            <button onClick={() => setIsProfileOpen(false)} className="text-muted-foreground hover:text-foreground">
                                <X size={24} />
                            </button>
                        </div>

                        <div className="space-y-4">
                            <div>
                                <label className="block text-sm font-medium mb-1 text-muted-foreground">User ID</label>
                                <input
                                    type="text"
                                    value={tempUserId}
                                    onChange={(e) => setTempUserId(e.target.value)}
                                    className="w-full bg-secondary border-transparent focus:border-primary rounded-md px-3 py-2 text-sm font-mono"
                                />
                            </div>

                            <div className="flex justify-end gap-3 mt-6">
                                <button
                                    onClick={() => setIsProfileOpen(false)}
                                    className="px-4 py-2 rounded-md hover:bg-accent transition-colors text-sm font-medium"
                                >
                                    Cancel
                                </button>
                                <button
                                    onClick={handleSaveUserId}
                                    className="bg-primary text-primary-foreground px-4 py-2 rounded-md hover:bg-primary/90 transition-colors text-sm font-medium"
                                >
                                    Save Changes
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            )}

            {/* Main Content */}
            <main className="flex-1 container mx-auto px-4 py-8">
                {children}
            </main>
        </div>
    );
};
