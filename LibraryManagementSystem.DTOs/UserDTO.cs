using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DTOs
{
    public class UserDTO : PersonDTO {
        
        public int ID { get; set; }
        public int PersonID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte Role { get; set; } 
        public DateTime LastLogin { get; set; }
        public bool isActive { get; set; }

        public UserDTO() : base() { }

        public UserDTO(PersonDTO PDTO,
            int userID, int personID, string username, string password, byte role, DateTime lastLogin, bool isActive)
            : base(PDTO.ID, PDTO.FirstName, PDTO.LastName, PDTO.DateOfBirth, PDTO.Gender, PDTO.Email, PDTO.Phone, PDTO.Address, PDTO.City, PDTO.CreatedAt, PDTO.UpdatedAt)
        {
            this.ID = userID;
            this.PersonID = personID;
            this.Username = username;
            this.Password = password;
            this.Role = role;
            this.LastLogin = lastLogin;
            this.isActive = isActive;
        }

    }
}
