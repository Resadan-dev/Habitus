import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import type { RootState } from '../store/store';
import { setUserId } from '../store/userSlice';

export const UserSwitcher: React.FC = () => {
    const userId = useSelector((state: RootState) => state.user.userId);
    const dispatch = useDispatch();

    return (
        <div className="fixed top-4 right-4 bg-card/90 backdrop-blur-sm border border-border p-4 rounded-xl shadow-lg z-50 transition-all hover:shadow-xl">
            <label className="block text-xs font-bold text-muted-foreground uppercase tracking-wider mb-2">
                Current User ID
            </label>
            <input
                type="text"
                value={userId}
                onChange={(e) => dispatch(setUserId(e.target.value))}
                className="w-80 px-4 py-2 bg-secondary/50 border border-input rounded-lg focus:outline-none focus:ring-2 focus:ring-primary/50 font-mono text-sm text-foreground transition-all"
            />
        </div>
    );
};
