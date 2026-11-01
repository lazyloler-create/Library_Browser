using Microsoft.AspNetCore.Mvc;
using Library.Data;
using Library.Models;

namespace Library.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly Logger _logger;
    private readonly LibraryDbContext _context;

    //needed for creating HttpClient instances with custom config
    private readonly IHttpClientFactory _httpClientFactory;

    public BookController(
        IHttpClientFactory httpClientFactory,
        LibraryDbContext context,
        Logger logger)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
        _logger = logger;
    }
    
    [HttpGet("search/{q}")]
    //searches a book by title using google books api
    public async Task<IActionResult> SearchBook([FromQuery] string q)
    {   
        //checks if title is empty
        if (string.IsNullOrWhiteSpace(q))
        {
            _logger.LogError("Book search was requested without a title");
            return BadRequest("Book title required!");
        }

        //gets api key from .env file
        var apiKey = Environment.GetEnvironmentVariable("GOOGLE_BOOKS_API_KEY");
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogError("GOOGLE_BOOKS_API_KEY is missing or empty");
            return StatusCode(500, "GOOGLE_BOOKS_API_KEY is missing or is empty!");
        }

        //url for google books api
        var url = $"https://www.googleapis.com/books/v1/volumes?q={Uri.EscapeDataString(q)}" + $"&key={Uri.EscapeDataString(apiKey)}";

        //creates a http client from IHttpClientFactory
        var httpClient = _httpClientFactory.CreateClient();
        using var response = await httpClient.GetAsync(url);
        _logger.LogInfo($"Google Books search returned HTTP status code {(int)response.StatusCode}");
        var json = await response.Content.ReadAsStringAsync();

        //returns content from json as a string
        return Content(json, "application/json");
    }

    [HttpGet("{isbn}")]
    //gets book by ISBN from a user database
    public async Task<ActionResult<BookClass>> GetBookByIsbn(long isbn)
    {
        _logger.LogInfo($"Looking up book with ISBN {isbn}");
        var book = await _context.Books.FindAsync(isbn);

        if (book is null)
        {
            _logger.LogError($"No book found with ISBN {isbn}");
            return NotFound();
        }
        _logger.LogSuccess($"Found book with isbn {isbn}: {book.Title} by {book.Author}");
        return Ok(book);
    }

    [HttpPost()]
    public async Task<ActionResult<BookClass>> addBook(BookClass newBook)
    {
        if(newBook == null)
        {
            return BadRequest();
        }

        _context.Books.Add(newBook);
        await _context.SaveChangesAsync();
        _logger.LogSuccess($"Added book with ISBN {newBook.ISBN}");

        return CreatedAtAction(nameof(GetBookByIsbn), new{isbn = newBook.ISBN}, newBook);
    }

    [HttpPut("{isbn}")]
    public async Task<IActionResult> updateBook(long isbn, BookClass updatedBook)
    {
        if(isbn != updatedBook.ISBN){
            _logger.LogError($"Invalid ISBN: {isbn}");
            return BadRequest("The ISBN in the request body must match the ISBN in the URL!");
        }
                
        var book = await _context.Books.FindAsync(isbn);
        if(book is null){
            _logger.LogWarning($"Book is empty or invalid");
            return BadRequest();
        }
        
        book.Title = updatedBook.Title;
        _logger.LogInfo("Successfully updated book title!");
        book.Author = updatedBook.Author;
        _logger.LogInfo("Successfully updated book author!");
        book.DatePublished = updatedBook.DatePublished;
        _logger.LogInfo("Successfully updated books published date!");
        book.Pages = updatedBook.Pages;
        _logger.LogInfo("Successfully updated pages of the book!");
        book.PriceUSD = updatedBook.PriceUSD;
        _logger.LogInfo("Successfully updated price of the book!");
        book.Rating = updatedBook.Rating;
        _logger.LogInfo("Successfully updated rating of book!");

        await _context.SaveChangesAsync();
        _logger.LogInfo($"Updated book with ISBN {isbn}");
        return NoContent();
    }

    [HttpDelete("{isbn}")]
    public async Task<IActionResult> deleteBook(long isbn)
    {
        var book = await _context.Books.FindAsync(isbn);
        if(book == null){
            _logger.LogWarning($"Book with isbn: {isbn} not found!");
            return NotFound();
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        _logger.LogInfo($"Deleted book with ISBN {isbn}");
        return NoContent();
    }
}