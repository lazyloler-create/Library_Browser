# Library

Library is a book and ebook discovery app. It is intended to help readers find
books, compare available formats, and choose how they want to get a book: buy a
digital copy or order a printed copy.

## What the app is for

Readers should be able to search a broad catalog, learn about books, and move
from discovery to a purchase or order. The project is a work in progress, the
features below describe the planned experience and are not all implemented yet.

## Planned features

- **Book discovery:** Search by title, author, or ISBN and explore books by
  category or editions.
- **Book details:** View available information such as the title, author,
  publication date, ISBN, page count, rating, and price.
- **Digital editions:** Find books available as ebooks and purchase a digital
  copy.
- **Printed books:** Check print availability and place an order for a physical
  copy.
- **Catalog browsing:** Browse categories and discover books related to a
  selected subject.
- **Useful search tools:** Filter or sort results by author, category, rating,
  publication date, price, and format.
- **Reader accounts and library:** Save favorites, keep a wishlist, and access
  purchased ebooks and order history.
- **Book reviews:** Read and, eventually, submit ratings and reviews.

Digital purchases, physical ordering, user accounts, recommendations, and
reviews are ideas for the product; they are not currently implemented in this
project.

## Google Books API setup

The project uses the Google Books API for book information. Get an API key from
the [Google Books API documentation](https://developers.google.com/books/docs/v1/getting_started)
and add it to a `.env` file in the project root:

```dotenv
GOOGLE_API_KEY=your_google_books_api_key
```

# Do not commit your API key to version control.
