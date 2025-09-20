using LibraryManagementSystem.BLL;
using LibraryManagementSystem.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/People")]
    [ApiController]
    public class PeopleController : ControllerBase {

        [HttpGet("All", Name ="GetAllPeople")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PersonDTO>> GetAllPeople() { // Define a method to get all People.
            List<PersonDTO> PeopleList = LibraryManagementSystem.BLL.Person.GetAllPeople();
            if (PeopleList.Count == 0)
                return NotFound("No People Found!");

            return Ok(PeopleList); // Returns the list of People.
        }

        [HttpPost(Name = "AddPerson")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PersonDTO> AddPerson(PersonDTO newPersonDTO) {

            LibraryManagementSystem.BLL.Person person = new LibraryManagementSystem.BLL.Person(new PersonDTO(newPersonDTO.ID, newPersonDTO.FirstName, newPersonDTO.LastName,
                newPersonDTO.DateOfBirth, newPersonDTO.Gender, newPersonDTO.Email, newPersonDTO.Phone, newPersonDTO.Address, newPersonDTO.City, newPersonDTO.CreatedAt, newPersonDTO.UpdatedAt));
            person.Save();

            newPersonDTO.ID = person.ID;

            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetPersonById", new { id = newPersonDTO.ID }, newPersonDTO);
        }

        [HttpGet("{id}", Name = "GetPersonById")]
        public ActionResult<PersonDTO> GetPersonById(int id) {
            var person = Person.Find(id); // your data access method

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        //here we use http put method for update
        [HttpPut("{id}", Name = "UpdatePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PersonDTO> UpdatePerson(int id, PersonDTO updatedPerson) {
            LibraryManagementSystem.BLL.Person person = LibraryManagementSystem.BLL.Person.Find(id);        

            if (person == null)
                return NotFound($"User with ID {id} not found.");

            person.FirstName = updatedPerson.FirstName;
            person.LastName = updatedPerson.LastName;
            person.DateOfBirth = updatedPerson.DateOfBirth;
            person.Gender = updatedPerson.Gender;
            person.Email = updatedPerson.Email;
            person.Phone = updatedPerson.Phone;
            person.Address = updatedPerson.Address;
            person.City = updatedPerson.City;
            person.Save();
 
            return Ok(person.PDTO); 
        }

    }
}
