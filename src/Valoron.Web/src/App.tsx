import React from 'react';
import { Provider } from 'react-redux';
import { store } from './store/store';
import { Layout } from './components/Layout';
import { CharacterStats } from './components/CharacterStats';
import { BookList } from './components/BookList';

function App() {
  return (
    <Provider store={store}>
      <Layout>
        <div className="max-w-6xl mx-auto space-y-12">
          <header className="mb-12 text-center md:text-left">
            <h1 className="text-4xl md:text-6xl font-extrabold tracking-tight lg:text-7xl mb-4 bg-gradient-to-r from-primary to-purple-400 bg-clip-text text-transparent">
              Habitus
            </h1>
            <p className="text-xl text-muted-foreground max-w-2xl">
              Gamify your life. Track your progress. Level up your reality.
            </p>
          </header>

          <CharacterStats />
          <BookList />
        </div>
      </Layout>
    </Provider>
  );
}

export default App;
