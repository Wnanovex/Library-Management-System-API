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
    public class Author : Person {

        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        // Properties
        public int ID { get; set; }
        public int PersonID { get; set; }
        public string Biography { get; set; }

        public AuthorDTO ADTO {
            get { return (new AuthorDTO(base.PDTO, this.ID, this.PersonID, this.Biography)); }
        }

        public Author(AuthorDTO aDTO, enMode cMode = enMode.AddNew) 
            : base((PersonDTO)aDTO, (Person.enMode)cMode)
        {
            this.ID = aDTO.ID;
            this.PersonID = aDTO.PersonID;
            this.Biography = aDTO.Biography;

            this.Mode = cMode;
        }

        private bool _AddNewAuthor() {
            this.ID = AuthorData.AddNewAuthor(this.ADTO);
            return (this.ID != -1);
        }

        private bool _UpdateAuthor() {
            return AuthorData.UpdateAuthor(this.ADTO);
        }

        // 🔹 Find
        public static Author Find(int authorID) {
            AuthorDTO aDTO = AuthorData.GetAuthorByID(authorID);
            return aDTO != null ? new Author(aDTO, enMode.Update) : null;
        }

        //public static Author FindByPersonID(int personID) {
            
        //}

        // 🔹 Save (Insert or Update)
        public bool Save() {
            base.Mode = (Person.enMode) Mode;
            if (!base.Save()) return false;
            this.PersonID = base.ID;

            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewAuthor()){
                        Mode = enMode.Update;
                        return true;
                    }else{
                        return false;
                    }
                case enMode.Update:
                    return _UpdateAuthor();
            }
            return false;
        }

        // 🔹 Delete
        public bool Delete() {
            if (!AuthorData.DeleteAuthor(this.ID)) return false;
            return base.Delete();
        }

        // 🔹 Exists
        public static bool isAuthorExist(int authorID) {
            return AuthorData.IsAuthorExist(authorID);
        }

        // 🔹 Get all authors
        public static List<AuthorDTO> GetAllAuthors() {
            return AuthorData.GetAllAuthors();
        }


    }
}
