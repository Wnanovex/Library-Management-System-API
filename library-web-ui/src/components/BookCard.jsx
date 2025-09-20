import React from 'react';
import Card from 'react-bootstrap/Card';

function BookCard({ book }) {
  return (
    <Card className="m-2" style={{ width: '18rem' }}>
      <Card.Img
        variant="top"
        src={`http://192.168.1.5/api/Books/GetImage/${book.imageName}`} // Or use book.imageName if it's a full URL
        alt={book.title}
        style={{ height: '250px', objectFit: 'cover' }}
      />
      <Card.Body>
        <Card.Title>{book.title}</Card.Title>
        <Card.Text>{book.description?.substring(0, 100)}...</Card.Text>
        <p><strong>Author:</strong> {book.author?.firstName} {book.author?.lastName} </p>
      </Card.Body>
    </Card>
  );
}

export default BookCard;