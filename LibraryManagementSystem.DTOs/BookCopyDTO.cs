using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DTOs
{
    public class BookCopyDTO {

        public enum enStatus : byte { Available = 1, Issued = 2, Damaged = 3, Lost = 4 }

        public int ID { get; set; }
        public int BookID { get; set; }
        public BookDTO Book { get; set; }
        public enStatus Status { get; set; } 
        public DateTime DateAdded { get; set; }


        public BookCopyDTO() { }

        public BookCopyDTO(int copyID, int bookID, enStatus status, DateTime dateAdded) {
            this.ID = copyID;
            this.BookID = bookID;
            //this.Book = BookData.GetBookInfoByID(this.BookID);
            this.Status = status;
            this.DateAdded = dateAdded;
        }

    }
}
