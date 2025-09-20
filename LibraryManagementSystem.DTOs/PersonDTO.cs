namespace LibraryManagementSystem.DTOs
{
    public class PersonDTO {

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }  // "M" / "F"
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public PersonDTO() { }

        public PersonDTO(int personID, string firstName, string lastName, DateTime dateOfBirth,
                          string gender, string email, string phone,
                          string address, string city, DateTime createdAt, DateTime updatedAt) {
            ID = personID;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Email = email;
            Phone = phone;
            Address = address;
            City = city;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

    }
}
