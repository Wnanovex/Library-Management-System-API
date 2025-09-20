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
    public class BookCopy {

        public enum enMode { AddNew, Update }
        public enMode Mode { get; set; }

        public enum enStatus : byte { Available = 1, Issued = 2, Damaged = 3, Lost = 4 }

        // Properties
        public int ID { get; set; }
        public int BookID { get; set; }
        public Book Book { get; set; }
        public enStatus Status { get; set; } 
        public DateTime DateAdded { get; set; }

        public BookCopyDTO BCDTO {
            get { return (new BookCopyDTO(this.ID, this.BookID, (BookCopyDTO.enStatus)this.Status ,this.DateAdded)); }
        }

        public BookCopy(BookCopyDTO bcDTO, enMode cMode = enMode.AddNew) {
            this.ID = bcDTO.ID;
            this.BookID = bcDTO.BookID;
            this.Book = Book.Find(this.BookID);
            this.Status = (enStatus)bcDTO.Status;
            this.DateAdded = bcDTO.DateAdded;

            this.BCDTO.Book = bcDTO.Book;

            Mode = cMode;
        }

        private bool _AddNewBookCopy() {
            this.ID = BookCopyData.AddNewBookCopy(this.BCDTO);
            return (this.ID != -1);
        }

        //private bool _UpdateBookCopy() {
        //    return BookCopyData.UpdateBookCopy(this.ID, this.BookID, (byte)this.Status);
        //}

        // 🔹 Find
        public static BookCopy Find(int copyID) {
            BookCopyDTO bcDTO = BookCopyData.GetBookCopyInfoByID(copyID);
            return bcDTO != null ? new BookCopy(bcDTO, enMode.Update) : null;
        }

        // 🔹 Save (Insert or Update)
        public bool Save() {
            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewBookCopy()){
                        Mode = enMode.Update;
                        return true;
                    }else{
                        return false;
                    }
                case enMode.Update:
                    //return _UpdateBookCopy();
                    return true;
            }
            return false;
        }

        // 🔹 Delete
        public static bool Delete(int copyID) {
            return BookCopyData.DeleteBookCopy(copyID);
        }

        public bool UpdateStatus(BookCopyDTO bcDTO) {
            this.Status = (enStatus)bcDTO.Status;
            bcDTO.ID = this.ID;
            return BookCopyData.UpdateBookCopyStatus(bcDTO);
        }

        // 🔹 Get all BookCopies
        public static List<BookCopyDTO> GetAllBookCopies() {
            return BookCopyData.GetAllBookCopies();
        }

        public static bool IsBookIssued(int copyID) {
            return BookCopyData.IsBookIssued(copyID);
        }

        public static bool IsBookAvailable(int copyID) {
            return BookCopyData.IsBookAvailable(copyID);
        }

    }
}
