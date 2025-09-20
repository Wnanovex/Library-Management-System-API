using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DTOs
{
    public class AuthorDTO : PersonDTO {
        
        public int ID { get; set; }
        public int PersonID { get; set; } 
        public string Biography { get; set; }

        public AuthorDTO() : base() { }

        public AuthorDTO(PersonDTO PDTO, int authorID, int personID, string biography)
            : base(PDTO.ID, PDTO.FirstName, PDTO.LastName, PDTO.DateOfBirth, PDTO.Gender, PDTO.Email, PDTO.Phone, PDTO.Address, PDTO.City, PDTO.CreatedAt, PDTO.UpdatedAt)
        {
            this.ID = authorID;
            this.PersonID = personID;
            this.Biography = biography;
        }

    }
}
