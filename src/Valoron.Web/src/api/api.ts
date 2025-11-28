import { store } from '../store/store';

const BASE_URL = 'http://localhost:5183';

export const api = {
    fetch: async (endpoint: string, options: RequestInit = {}) => {
        const state = store.getState();
        const userId = state.user.userId;

        const headers = {
            'Content-Type': 'application/json',
            'x-user-id': userId,
            ...options.headers,
        } as HeadersInit;

        const response = await fetch(`${BASE_URL}${endpoint}`, {
            ...options,
            headers,
        });

        if (!response.ok) {
            throw new Error(`API Error: ${response.statusText}`);
        }

        // Handle empty responses
        const text = await response.text();
        return text ? JSON.parse(text) : {};
    },
};
