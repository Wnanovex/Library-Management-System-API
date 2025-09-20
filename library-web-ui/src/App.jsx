import 'bootstrap/dist/css/bootstrap.min.css';
import React from 'react';
import Home from './pages/Home';

function App() {
  return (
    <div className="App">
      <header className="bg-primary text-white p-3 mb-4">
        <div className="container">
          <h1>📚 Library Catalog</h1>
        </div>
      </header>

      <main>
        <Home />
      </main>

      <footer className="bg-light text-center py-3 mt-5">
        <div className="container">
          <small>&copy; {new Date().getFullYear()} My Library</small>
        </div>
      </footer>
    </div>
  );
}

export default App;