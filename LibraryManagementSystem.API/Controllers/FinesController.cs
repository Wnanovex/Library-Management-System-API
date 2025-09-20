using LibraryManagementSystem.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/Fines")]
    [ApiController]
    public class FinesController : ControllerBase {

        [HttpGet("All", Name ="GetAllFines")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<FineDTO>> GetAllFines() { // Define a method to get all Fines.
            List<FineDTO> FinesList = LibraryManagementSystem.BLL.Fine.GetAllFines();
            if (FinesList.Count == 0)
                return NotFound("No Fines Found!");

            return Ok(FinesList); // Returns the list of Fines.
        }

        [HttpPost(Name = "AddFine")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<FineDTO> AddFine(FineDTO newFineDTO) {

            LibraryManagementSystem.BLL.Fine Fine = new LibraryManagementSystem.BLL.Fine(newFineDTO);
            Fine.Save();

            newFineDTO.ID = Fine.ID;

            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetFineById", new { id = newFineDTO.ID }, newFineDTO);
        }

        [HttpGet("{id}", Name = "GetFineById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<FineDTO> GetFineById(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            LibraryManagementSystem.BLL.Fine Fine = LibraryManagementSystem.BLL.Fine.Find(id);

            if (Fine == null)
                return NotFound($"Fine with ID {id} not found.");
            
            FineDTO FDTO = Fine.FDTO; //here we get only the DTO object to send it back.
           
            return Ok(FDTO); 
        }


        //here we use HttpDelete method
        [HttpDelete("{id}", Name = "DeleteFine")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteFine(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            if(LibraryManagementSystem.BLL.Fine.Delete(id))         
                return Ok($"Fine with ID {id} has been deleted.");
            else
                return NotFound($"Fine with ID {id} not found. no rows deleted!");
        }



        [HttpGet("PayFine/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> PayFine(int id) {
            if (LibraryManagementSystem.BLL.Fine.PayFine(id))
                return Ok($"Fine {id} Paied");
            else
                return NotFound($"Fine {id} Not Paied");
        }


    }
}
