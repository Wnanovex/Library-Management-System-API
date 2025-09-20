using LibraryManagementSystem.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/IssuedBooks")]
    [ApiController]
    public class IssuedBooksController : ControllerBase {

        [HttpGet("All", Name ="GetAllIssuedBooks")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<IssuedBookDTO>> GetAllIssuedBooks() { // Define a method to get all IssuedBooks.
            List<IssuedBookDTO> IssuedBooksList = LibraryManagementSystem.BLL.IssuedBook.GetAllIssuedBooks();
            if (IssuedBooksList.Count == 0)
                return NotFound("No Issued Books Found!");

            return Ok(IssuedBooksList); // Returns the list of IssuedBooks.
        }

        [HttpPost(Name = "AddIssuedBook")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IssuedBookDTO> AddIssuedBook(IssuedBookDTO newIssuedBookDTO) {

            LibraryManagementSystem.BLL.IssuedBook IssuedBook = new LibraryManagementSystem.BLL.IssuedBook(newIssuedBookDTO);
            IssuedBook.Save();

            newIssuedBookDTO.ID = IssuedBook.ID;

            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetIssuedBookById", new { id = newIssuedBookDTO.ID }, newIssuedBookDTO);
        }

        [HttpGet("{id}", Name = "GetIssuedBookById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IssuedBookDTO> GetIssuedBookById(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            LibraryManagementSystem.BLL.IssuedBook IssuedBook = LibraryManagementSystem.BLL.IssuedBook.Find(id);

            if (IssuedBook == null)
                return NotFound($"IssuedBook with ID {id} not found.");
            
            IssuedBookDTO IBDTO = IssuedBook.IBDTO; //here we get only the DTO object to send it back.
           
            return Ok(IBDTO);
        }


        //here we use HttpDelete method
        [HttpDelete("{id}", Name = "DeleteIssuedBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteIssuedBook(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            if(LibraryManagementSystem.BLL.IssuedBook.Delete(id))         
                return Ok($"IssuedBook with ID {id} has been deleted.");
            else
                return NotFound($"IssuedBook with ID {id} not found. no rows deleted!");
        }


        //here we use http put method for update
        [HttpPut("{id}", Name = "UpdateIssuedBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IssuedBookDTO> UpdateIssuedBook(int id, IssuedBookDTO updatedIssuedBook) {

            LibraryManagementSystem.BLL.IssuedBook IssuedBook = LibraryManagementSystem.BLL.IssuedBook.Find(id);        

            if (IssuedBook == null)
                return NotFound($"IssuedBook with ID {id} not found.");

            IssuedBook.MemberID = updatedIssuedBook.MemberID;
            IssuedBook.DueDate = updatedIssuedBook.DueDate;
            IssuedBook.Save();
 
            return Ok(IssuedBook.IBDTO); 
        }


        [HttpGet("MarkBookAsReturned/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> MarkBookAsReturned(int id) {
            if (LibraryManagementSystem.BLL.IssuedBook.MarkBookAsReturned(id))
                return Ok($"Book{id} Marked as returned");
            else
                return NotFound($"Not Found Book {id}");
        }

        [HttpGet("IsBookReturned/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> IsBookReturned(int id) {
            if (LibraryManagementSystem.BLL.IssuedBook.IsBookReturned(id))
                return Ok(true);
            else
                return NotFound(false);
        }


    }
}
