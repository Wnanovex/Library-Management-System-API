using LibraryManagementSystem.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/Members")]
    [ApiController]
    public class MembersController : ControllerBase {

        [HttpGet("All", Name ="GetAllMembers")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<MemberDTO>> GetAllMembers() { // Define a method to get all Members.
            List<MemberDTO> MembersList = LibraryManagementSystem.BLL.Member.GetAllMembers();
            if (MembersList.Count == 0)
                return NotFound("No Members Found!");

            return Ok(MembersList); // Returns the list of Members.
        }

        [HttpPost(Name = "AddMember")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MemberDTO> AddMember(MemberDTO newMemberDTO) {

            LibraryManagementSystem.BLL.Member Member = new LibraryManagementSystem.BLL.Member(newMemberDTO);
            Member.Save();

            newMemberDTO.ID = Member.ID;
            newMemberDTO.PersonID = Member.PersonID;

            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetMemberById", new { id = newMemberDTO.ID }, newMemberDTO);
        }

        [HttpGet("{id}", Name = "GetMemberById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MemberDTO> GetMemberById(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            LibraryManagementSystem.BLL.Member Member = LibraryManagementSystem.BLL.Member.Find(id);

            if (Member == null)
                return NotFound($"Member with ID {id} not found.");
            
            MemberDTO MDTO = Member.MDTO; //here we get only the DTO object to send it back.
           
            return Ok(MDTO);
        }

        //here we use http put method for update
        //[HttpPut("{id}", Name = "UpdateMember")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public ActionResult<MemberDTO> UpdateMember(int id, MemberDTO updatedMember) {

        //    LibraryManagementSystem.BLL.Member Member = LibraryManagementSystem.BLL.Member.Find(id);        

        //    if (Member == null)
        //        return NotFound($"Member with ID {id} not found.");

        //    Member.FirstName = updatedMember.FirstName;
        //    Member.LastName = updatedMember.LastName;
        //    Member.DateOfBirth = updatedMember.DateOfBirth;
        //    Member.Gender = updatedMember.Gender;
        //    Member.Email = updatedMember.Email;
        //    Member.Phone = updatedMember.Phone;
        //    Member.Address = updatedMember.Address;
        //    Member.City = updatedMember.City;
        //    Member.Membername = updatedMember.Membername;
        //    Member.Password = updatedMember.Password;
        //    Member.Role = updatedMember.Role;
        //    Member.isActive = updatedMember.isActive;
        //    Member.Save();
 
        //    return Ok(Member.UDTO); 
        //}

        //here we use HttpDelete method
        [HttpDelete("{id}", Name = "DeleteMember")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteMember(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            LibraryManagementSystem.BLL.Member Member = LibraryManagementSystem.BLL.Member.Find(id);        

            if (Member == null)
                return NotFound($"Member with ID {id} not found.");

            if(Member.Delete())         
                return Ok($"Member with ID {id} has been deleted.");
            else
                return NotFound($"Member with ID {id} not found. no rows deleted!");
        }

        [HttpGet("IsMemberExist/{MemberID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> IsMemberExist(int MemberID) {
            if (LibraryManagementSystem.BLL.Member.isMemberExist(MemberID))
                return Ok(true);
            else
                return NotFound(false);
        }

    }
}
