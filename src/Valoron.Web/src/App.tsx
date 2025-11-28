import React from 'react';
import { Provider } from 'react-redux';
import { store } from './store/store';
import { UserSwitcher } from './components/UserSwitcher';
import { ActivityList } from './components/ActivityList';

function App() {
  return (
    <Provider store={store}>
      <div className="min-h-screen bg-gray-100">
        <UserSwitcher />
        <ActivityList />
      </div>
    </Provider>
  );
}

export default App;
