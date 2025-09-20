using LibraryManagementSystem.BLL;
using LibraryManagementSystem.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase {

        [HttpGet("All", Name ="GetAllUsers")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<UserDTO>> GetAllPeople() { // Define a method to get all Users.
            List<UserDTO> UsersList = LibraryManagementSystem.BLL.User.GetAllUsers();
            if (UsersList.Count == 0)
                return NotFound("No Users Found!");

            return Ok(UsersList); // Returns the list of Users.
        }

        [HttpPost(Name = "AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDTO> AddUser(UserDTO newUserDTO) {

            LibraryManagementSystem.BLL.User user = new LibraryManagementSystem.BLL.User(newUserDTO);
            user.Save();

            newUserDTO.ID = user.ID;

            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetUserById", new { id = newUserDTO.ID }, newUserDTO);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> GetUserById(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            LibraryManagementSystem.BLL.User user = LibraryManagementSystem.BLL.User.Find(id);

            if (user == null)
                return NotFound($"User with ID {id} not found.");
            
            UserDTO UDTO = user.UDTO; //here we get only the DTO object to send it back.
           
            return Ok(UDTO);
        }

        [HttpGet("{username}/{password}", Name = "GetUserByUsernameAndPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> GetUserByUsernameAndPassword(string username, string password) {

            LibraryManagementSystem.BLL.User user = LibraryManagementSystem.BLL.User.Find(username, password);

            if (user == null)
                return NotFound($"User with {username} {password} not found.");
            
            UserDTO UDTO = user.UDTO; //here we get only the DTO object to send it back.
           
            return Ok(UDTO);
        }


        //here we use http put method for update
        [HttpPut("{id}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> UpdateUser(int id, UserDTO updatedUser) {

            LibraryManagementSystem.BLL.User user = LibraryManagementSystem.BLL.User.Find(id);        

            if (user == null)
                return NotFound($"User with ID {id} not found.");

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.DateOfBirth = updatedUser.DateOfBirth;
            user.Gender = updatedUser.Gender;
            user.Email = updatedUser.Email;
            user.Phone = updatedUser.Phone;
            user.Address = updatedUser.Address;
            user.City = updatedUser.City;
            user.Username = updatedUser.Username;
            user.Password = updatedUser.Password;
            user.Role = updatedUser.Role;
            user.isActive = updatedUser.isActive;
            user.Save();
 
            return Ok(user.UDTO);
        }

        //here we use HttpDelete method
        [HttpDelete("{id}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteUser(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            LibraryManagementSystem.BLL.User user = LibraryManagementSystem.BLL.User.Find(id);        

            if (user == null)
                return NotFound($"User with ID {id} not found.");

            if(user.Delete())         
                return Ok($"User with ID {id} has been deleted.");
            else
                return NotFound($"User with ID {id} not found. no rows deleted!");
        }

        [HttpGet("IsUserExist/{userID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> IsUserExist(int userID) {
            if (LibraryManagementSystem.BLL.User.isUserExist(userID))
                return Ok(true);
            else
                return NotFound(false);
        }

        [HttpPut("ChangePassword/{id}", Name = "ChangePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> ChangePassword(int id, string new_password) {

            LibraryManagementSystem.BLL.User user = LibraryManagementSystem.BLL.User.Find(id);        

            if (user == null)
                return NotFound($"User with ID {id} not found.");

            user.ChangePassword(new_password);
 
            return Ok(user.UDTO);
        }
    }
}
