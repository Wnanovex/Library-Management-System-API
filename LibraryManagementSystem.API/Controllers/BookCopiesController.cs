using LibraryManagementSystem.BLL;
using LibraryManagementSystem.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/BookCopies")]
    [ApiController]
    public class BookCopiesController : ControllerBase {

        [HttpGet("All", Name ="GetAllBookCopies")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<BookCopyDTO>> GetAllBookCopies() { // Define a method to get all BookCopys.
            List<BookCopyDTO> BookCopiesList = LibraryManagementSystem.BLL.BookCopy.GetAllBookCopies();
            if (BookCopiesList.Count == 0)
                return NotFound("No BookCopies Found!");

            return Ok(BookCopiesList); // Returns the list of BookCopys.
        }

        [HttpPost(Name = "AddBookCopy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<BookCopyDTO> AddBookCopy(BookCopyDTO newBookCopyDTO) {

            LibraryManagementSystem.BLL.BookCopy BookCopy = new LibraryManagementSystem.BLL.BookCopy(newBookCopyDTO);
            BookCopy.Save();

            newBookCopyDTO.ID = BookCopy.ID;

            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetBookCopyById", new { id = newBookCopyDTO.ID }, newBookCopyDTO);
        }

        [HttpGet("{id}", Name = "GetBookCopyById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BookCopyDTO> GetBookCopyById(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            LibraryManagementSystem.BLL.BookCopy BookCopy = LibraryManagementSystem.BLL.BookCopy.Find(id);

            if (BookCopy == null)
                return NotFound($"BookCopy with ID {id} not found.");
            
            BookCopyDTO BCDTO = BookCopy.BCDTO; //here we get only the DTO object to send it back.
           
            return Ok(BCDTO); 
        }


        //here we use HttpDelete method
        [HttpDelete("{id}", Name = "DeleteBookCopy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteBookCopy(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            if(LibraryManagementSystem.BLL.BookCopy.Delete(id))         
                return Ok($"BookCopy with ID {id} has been deleted.");
            else
                return NotFound($"BookCopy with ID {id} not found. no rows deleted!");
        }


        //here we use http put method for update
        [HttpPut("{id}", Name = "UpdateStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BookCopyDTO> UpdateStatus(int id, BookCopyDTO updatedCopy) {

            LibraryManagementSystem.BLL.BookCopy Copy = LibraryManagementSystem.BLL.BookCopy.Find(id);        

            if (Copy == null)
                return NotFound($"Copy with ID {id} not found.");

            if(!Copy.UpdateStatus(updatedCopy))
                return NotFound($"Copy Status not updated");
 
            return Ok(Copy.BCDTO); 
        }


        [HttpGet("IsBookIssued/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> IsBookIssued(int id) {
            if (LibraryManagementSystem.BLL.BookCopy.IsBookIssued(id))
                return Ok(true);
            else
                return NotFound(false);
        }

        [HttpGet("IsBookAvailable/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> IsBookAvailable(int id) {
            if (LibraryManagementSystem.BLL.BookCopy.IsBookAvailable(id))
                return Ok(true);
            else
                return NotFound(false);
        }
    }
}
