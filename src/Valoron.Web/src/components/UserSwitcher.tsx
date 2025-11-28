import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { RootState } from '../store/store';
import { setUserId } from '../store/userSlice';

export const UserSwitcher: React.FC = () => {
    const userId = useSelector((state: RootState) => state.user.userId);
    const dispatch = useDispatch();

    return (
        <div className="fixed top-4 right-4 bg-white p-4 rounded-lg shadow-md z-50">
            <label className="block text-sm font-medium text-gray-700 mb-1">
                Current User ID
            </label>
            <input
                type="text"
                value={userId}
                onChange={(e) => dispatch(setUserId(e.target.value))}
                className="w-80 px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 font-mono text-sm"
            />
        </div>
    );
};
