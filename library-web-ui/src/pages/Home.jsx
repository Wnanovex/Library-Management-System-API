import React, { useEffect, useState } from 'react';
import axios from 'axios';
import BookCard from '../components/BookCard';

function Home() {
  const [books, setBooks] = useState([]);

  useEffect(() => {
    axios.get('http://192.168.1.5:5000/api/Books/All/') // Adjust route
      .then(res => setBooks(res.data))
      .catch(err => console.error(err));
  }, []);

  return (
    <div className="container mt-4">
      <div className="row">
        {books.map(book => (
          <div className="col-md-4" key={book.id}>
            <BookCard book={book} />
          </div>
        ))}
      </div>
    </div>
  );
}

export default Home;