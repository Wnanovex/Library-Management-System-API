using LibraryManagementSystem.BLL;
using LibraryManagementSystem.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/Books")]
    [ApiController]
    public class BooksController : ControllerBase {

        [HttpGet("All", Name ="GetAllBooks")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<BookDTO>> GetAllBooks() { // Define a method to get all Books.
            List<BookDTO> BooksList = LibraryManagementSystem.BLL.Book.GetAllBooks();
            if (BooksList.Count == 0)
                return NotFound("No Books Found!");

            return Ok(BooksList); // Returns the list of Books.
        }

        [HttpGet("{id}", Name = "GetBookById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BookDTO> GetBookById(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            LibraryManagementSystem.BLL.Book Book = LibraryManagementSystem.BLL.Book.Find(id);

            if (Book == null)
                return NotFound($"Book with ID {id} not found.");
            
            BookDTO BDTO = Book.BDTO; //here we get only the DTO object to send it back.
           
            return Ok(BDTO);
        }

        //here we use HttpDelete method
        [HttpDelete("{id}", Name = "DeleteBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteBook(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            //LibraryManagementSystem.BLL.Book Book = LibraryManagementSystem.BLL.Book.Find(id);        

            //if (Book == null)
            //    return NotFound($"Book with ID {id} not found.");

            if(LibraryManagementSystem.BLL.Book.Delete(id))         
                return Ok($"Book with ID {id} has been deleted.");
            else
                return NotFound($"Book with ID {id} not found. no rows deleted!");
        }

        public class BookFormDTO {
            public string Title { get; set; }
            public string ISBN { get; set; }
            public int CategoryID { get; set; }
            public int AuthorID { get; set; }
            public string Edition { get; set; }
            public int PublishedYear { get; set; }
            public string Language { get; set; }
            public string Description { get; set; }
            public string ShelfLocation { get; set; }
            public float DailyRate { get; set; }
            public DateTime DateAdded { get; set; }

            // For the image file upload
            public IFormFile ImageFile { get; set; }
        }


        [HttpPost("AddBook")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddBookWithImage([FromForm] BookFormDTO formDTO) {
            if (formDTO == null)
                return BadRequest("Invalid book data.");

            try {
                string imageName = null;
                if (formDTO.ImageFile != null && formDTO.ImageFile.Length > 0){
                    var folderPath = @"C:\Library-Books-Images";
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var fileName = Guid.NewGuid() + Path.GetExtension(formDTO.ImageFile.FileName);
                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await formDTO.ImageFile.CopyToAsync(stream);

                    imageName = fileName;
                }

                // Map formDTO to BookDTO
                var bookDTO = new BookDTO  {
                    ID = 0, // new book
                    Title = formDTO.Title,
                    ISBN = formDTO.ISBN,
                    CategoryID = formDTO.CategoryID,
                    AuthorID = formDTO.AuthorID,
                    Edition = formDTO.Edition,
                    PublishedYear = formDTO.PublishedYear,
                    Language = formDTO.Language,
                    Description = formDTO.Description,
                    ShelfLocation = formDTO.ShelfLocation,
                    DailyRate = formDTO.DailyRate,
                    ImageName = imageName,
                    DateAdded = formDTO.DateAdded
                };

                // Use your BLL Book class
                var book = new Book(bookDTO, Book.enMode.AddNew);
                if (!book.Save())
                    return StatusCode(500, "Failed to save book.");

                return CreatedAtAction(nameof(GetBookById), new { id = book.ID }, book.BDTO);
            }catch (Exception ex) {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }





        [HttpPut("UpdateBook")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateBookWithImage(int id, [FromForm] BookFormDTO form)  {
            try  {
                // 🔹 Handle image upload if present
                string image_name = null;
                if (form.ImageFile != null && form.ImageFile.Length > 0)  {
                    var folderPath = @"C:\Library-Books-Images";
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var fileName = Guid.NewGuid() + Path.GetExtension(form.ImageFile.FileName);
                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await form.ImageFile.CopyToAsync(stream);

                    image_name = fileName;
                }

                // 🔹 Get existing book (optional if needed)
                var existingBook = Book.Find(id);
                if (existingBook == null)
                    return NotFound("Book not found.");

                // 🔹 Create DTO
                var bookDTO = new BookDTO(
                    bookID: id,
                    title: form.Title,
                    ISBN: form.ISBN,
                    categoryID: form.CategoryID,
                    authorID: form.AuthorID,
                    edition: form.Edition,
                    publishedYear: form.PublishedYear,
                    language: form.Language,
                    description: form.Description,
                    shelfLocation: form.ShelfLocation,
                    dailyRate: form.DailyRate,
                    imageName: image_name ?? existingBook.ImageName, // keep old image if none uploaded
                    dateAdded: form.DateAdded
                );

                // 🔹 Save updated book
                var book = new Book(bookDTO, Book.enMode.Update);
                if (!book.Save())
                    return StatusCode(500, "Failed to update book.");

                return Ok(book.BDTO);
            }catch (Exception ex) {
                return BadRequest($"Error updating book: {ex.Message}");
            }
        }


        // Endpoint to retrieve image from the server
        [HttpGet("GetImage/{fileName}")]
        public IActionResult GetImage(string fileName) {
            var folderPath = @"C:\Library-Books-Images";
            var filePath = Path.Combine(folderPath, fileName);
            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
                return NotFound("Image not found.");

            // Open the image file for reading
            var image = System.IO.File.OpenRead(filePath);
            var mimeType = GetMimeType(filePath);

            // Return the file with the correct MIME type
            return File(image, mimeType);
        }

        private string GetMimeType(string filePath) {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream",
            };
        }

    }
}
