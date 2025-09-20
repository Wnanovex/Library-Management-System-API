using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DTOs
{
    public class MemberDTO : PersonDTO {
       
        public int ID { get; set; }
        public int PersonID { get; set; } 
        public DateTime DateJoined { get; set; }

        public MemberDTO() : base() { }

        public MemberDTO(PersonDTO PDTO, int memberID, int personID, DateTime dateJoined)
            : base(PDTO.ID, PDTO.FirstName, PDTO.LastName, PDTO.DateOfBirth, PDTO.Gender, PDTO.Email, PDTO.Phone, PDTO.Address, PDTO.City, PDTO.CreatedAt, PDTO.UpdatedAt)
        {
            this.ID = memberID;
            this.PersonID = personID;
            this.DateJoined = dateJoined;
        }

    }
}
