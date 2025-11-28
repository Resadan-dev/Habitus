import React, { useEffect, useState } from 'react';
import { api } from '../api/api';
import { useSelector } from 'react-redux';
import { RootState } from '../store/store';

interface Activity {
    id: string;
    name: string;
    description: string;
    // Add other fields as needed
}

export const ActivityList: React.FC = () => {
    const [activities, setActivities] = useState<Activity[]>([]);
    const userId = useSelector((state: RootState) => state.user.userId);

    const fetchActivities = async () => {
        try {
            const data = await api.fetch('/api/activities');
            // Ensure data is an array
            if (Array.isArray(data)) {
                setActivities(data);
            } else {
                console.error('API returned non-array data:', data);
                setActivities([]);
            }
        } catch (error) {
            console.error('Failed to fetch activities:', error);
        }
    };

    useEffect(() => {
        fetchActivities();
    }, [userId]); // Refetch when userId changes

    const logSession = async (activityId: string) => {
        try {
            await api.fetch(`/api/activities/${activityId}/session`, {
                method: 'POST',
                body: JSON.stringify({ pagesRead: 10 }),
            });
            alert('Session logged successfully!');
        } catch (error) {
            console.error('Failed to log session:', error);
            alert('Failed to log session.');
        }
    };

    return (
        <div className="p-8 pt-24">
            <h1 className="text-3xl font-bold mb-6 text-gray-800">Activities</h1>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {activities.map((activity) => (
                    <div key={activity.id} className="bg-white rounded-xl shadow-lg p-6 hover:shadow-xl transition-shadow duration-300">
                        <h2 className="text-xl font-semibold mb-2 text-gray-900">{activity.name}</h2>
                        <p className="text-gray-600 mb-4">{activity.description}</p>
                        <button
                            onClick={() => logSession(activity.id)}
                            className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 transition-colors duration-200 text-sm font-medium"
                        >
                            Log 10 Pages
                        </button>
                    </div>
                ))}
            </div>
        </div>
    );
};
