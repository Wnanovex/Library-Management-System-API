using LibraryManagementSystem.DAL;
using LibraryManagementSystem.DTOs;
using System.Data;

namespace LibraryManagementSystem.BLL
{
    public class Person {

        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode { get; set; }

        // Properties
        public int ID { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }  // "M" / "F"
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public string FullName => $"{FirstName} {LastName}";

        public PersonDTO PDTO {
            get { return (new PersonDTO(this.ID, this.FirstName, this.LastName ,this.DateOfBirth,
                this.Gender, this.Email, this.Phone, this.Address, this.City, this.CreatedAt, this.UpdatedAt)); }
        }

        public Person(PersonDTO PDTO, enMode cMode = enMode.AddNew) {
            ID = PDTO.ID;
            FirstName = PDTO.FirstName;
            LastName = PDTO.LastName;
            DateOfBirth = PDTO.DateOfBirth;
            Gender = PDTO.Gender;
            Email = PDTO.Email;
            Phone = PDTO.Phone;
            Address = PDTO.Address;
            City = PDTO.City;
            CreatedAt = PDTO.CreatedAt;
            UpdatedAt = PDTO.UpdatedAt;

            Mode = cMode;
        }

        private bool _AddNewPerson() {
            this.ID = PersonData.AddNewPerson(PDTO);
            return (this.ID != -1);
        }

        private bool _UpdatePerson() {
            return PersonData.UpdatePerson(PDTO);
        }

        // 🔹 Find
        public static Person Find(int personID) {
            PersonDTO pDTO = PersonData.GetPersonInfoByID(personID);
            return pDTO != null ? new Person(pDTO, enMode.Update) : null;
        }

        // 🔹 Save (Insert or Update)
        public bool Save() {
            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewPerson()){
                        Mode = enMode.Update;
                        return true;
                    }else{
                        return false;
                    }
                case enMode.Update:
                    return _UpdatePerson();
            }
            return false;
        }

        // 🔹 Delete
        public bool Delete() {
            return PersonData.DeletePerson(this.ID);
        }

        // 🔹 Exists
        public static bool isPersonExists(int personID) {
            return PersonData.IsPersonExist(personID);
        }

        // 🔹 Get all people
        public static List<PersonDTO> GetAllPeople() {
            return PersonData.GetAllPeople();
        }

    }
}
