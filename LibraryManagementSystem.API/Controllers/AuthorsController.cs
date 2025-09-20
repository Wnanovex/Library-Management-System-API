using LibraryManagementSystem.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/Authors")]
    [ApiController]
    public class AuthorsController : ControllerBase {

        [HttpGet("All", Name ="GetAllAuthors")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<AuthorDTO>> GetAllAuthors() { // Define a method to get all Authors.
            List<AuthorDTO> AuthorsList = LibraryManagementSystem.BLL.Author.GetAllAuthors();
            if (AuthorsList.Count == 0)
                return NotFound("No Authors Found!");

            return Ok(AuthorsList); // Returns the list of Authors.
        }

        [HttpPost(Name = "AddAuthor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<AuthorDTO> AddAuthor(AuthorDTO newAuthorDTO) {

            LibraryManagementSystem.BLL.Author Author = new LibraryManagementSystem.BLL.Author(newAuthorDTO);
            Author.Save();

            newAuthorDTO.ID = Author.ID;
            newAuthorDTO.PersonID = Author.PersonID;

            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetAuthorById", new { id = newAuthorDTO.ID }, newAuthorDTO);
        }

        [HttpGet("{id}", Name = "GetAuthorById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AuthorDTO> GetAuthorById(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            LibraryManagementSystem.BLL.Author Author = LibraryManagementSystem.BLL.Author.Find(id);

            if (Author == null)
                return NotFound($"Author with ID {id} not found.");
            
            AuthorDTO ADTO = Author.ADTO; //here we get only the DTO object to send it back.
           
            return Ok(ADTO); 
        }

        //here we use http put method for update
        [HttpPut("{id}", Name = "UpdateAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AuthorDTO> UpdateAuthor(int id, AuthorDTO updatedAuthor) {

            LibraryManagementSystem.BLL.Author Author = LibraryManagementSystem.BLL.Author.Find(id);        

            if (Author == null)
                return NotFound($"Author with ID {id} not found.");

            Author.FirstName = updatedAuthor.FirstName;
            Author.LastName = updatedAuthor.LastName;
            Author.DateOfBirth = updatedAuthor.DateOfBirth;
            Author.Gender = updatedAuthor.Gender;
            Author.Email = updatedAuthor.Email;
            Author.Phone = updatedAuthor.Phone;
            Author.Address = updatedAuthor.Address;
            Author.City = updatedAuthor.City;
            Author.Biography = updatedAuthor.Biography;
            Author.Save();
 
            return Ok(Author.ADTO); 
        }

        //here we use HttpDelete method
        [HttpDelete("{id}", Name = "DeleteAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteAuthor(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            LibraryManagementSystem.BLL.Author Author = LibraryManagementSystem.BLL.Author.Find(id);        

            if (Author == null)
                return NotFound($"Author with ID {id} not found.");

            if(Author.Delete())         
                return Ok($"Author with ID {id} has been deleted.");
            else
                return NotFound($"Author with ID {id} not found. no rows deleted!");
        }

        [HttpGet("IsAuthorExist/{AuthorID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> IsAuthorExist(int AuthorID) {
            if (LibraryManagementSystem.BLL.Author.isAuthorExist(AuthorID))
                return Ok(true);
            else
                return NotFound(false);
        }

    }
}
