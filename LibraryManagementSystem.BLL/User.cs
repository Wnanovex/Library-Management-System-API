using LibraryManagementSystem.DAL;
using LibraryManagementSystem.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL
{
    public class User : Person {

        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode { get; set; }

        // Properties
        public int ID { get; private set; }
        public int PersonID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte Role { get; set; } 
        public DateTime LastLogin { get; set; }
        public bool isActive { get; set; }

        public UserDTO UDTO {
            get { return (new UserDTO(base.PDTO, this.ID, this.PersonID, this.Username ,this.Password,
                this.Role, this.LastLogin, this.isActive)); }
        }
        
        public User(UserDTO uDTO, enMode cMode = enMode.AddNew)
            : base((PersonDTO)uDTO, (Person.enMode)cMode)
        {
            this.ID = uDTO.ID;
            this.PersonID = uDTO.PersonID;
            this.Username = uDTO.Username;
            this.Password = uDTO.Password;
            this.Role = uDTO.Role;
            this.LastLogin = uDTO.LastLogin;
            this.isActive = uDTO.isActive;
            this.Mode = cMode;
        }

        private bool _AddNewUser() {
            this.ID = UserData.AddNewUser(this.UDTO);
            return (this.ID != -1);
        }

        private bool _UpdateUser() {
            return UserData.UpdateUser(this.UDTO);
        }

        // 🔹 Find
        public static User Find(int userID) {
            UserDTO uDTO = UserData.GetUserByID(userID);
            return uDTO != null ? new User(uDTO, enMode.Update) : null;
        }

        //public static User FindByPersonID(int personID) {
            
        //}

        public static User Find(string username, string password) {
            UserDTO uDTO = UserData.GetUserByUsernameAndPassword(username, password);
            return uDTO != null ? new User(uDTO, enMode.Update) : null;
        }

        // 🔹 Save (Insert or Update)
        public bool Save() {
            base.Mode = (Person.enMode)Mode;
            if (!base.Save()) return false;
            this.PersonID = base.ID;

            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewUser()){
                        Mode = enMode.Update;
                        return true;
                    }else{
                        return false;
                    }
                case enMode.Update:
                    return _UpdateUser();
            }
            return false;
        }

        // 🔹 Delete
        public bool Delete() {            
            if (!UserData.DeleteUser(this.ID)) return false;
            return base.Delete();
        }

        // 🔹 Exists
        public static bool isUserExist(int userID) {
            return UserData.IsUserExist(userID);
        }

        // 🔹 Get all users
        public static List<UserDTO> GetAllUsers() {
            return UserData.GetAllUsers();
        }

        public bool ChangePassword(string new_password) {
            bool isSuccessed = UserData.ChangePassword(this.ID, new_password);
            if (isSuccessed) this.Password = new_password;
            return isSuccessed;
        }

    }
}
