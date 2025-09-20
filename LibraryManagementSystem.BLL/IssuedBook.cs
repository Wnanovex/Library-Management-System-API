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
    public class IssuedBook {

        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode { get; set; }

        // Properties
        public int ID { get; set; }
        public int CopyID { get; set; }
        public BookCopy Copy { get; set; }
        public int MemberID { get; set; }
        public Member MemberInfo { get; set; }
        public int IssuedBy { get; set; } 
        public User IssuedUser { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsReturned { get; set; }

        public IssuedBookDTO IBDTO {
            get { return (new IssuedBookDTO(this.ID, this.CopyID, this.MemberID ,this.IssuedBy, this.IssueDate,
                        this.DueDate, this.ReturnDate, this.IsReturned)); }
        }


        public IssuedBook(IssuedBookDTO ibDTO, enMode cMode = enMode.AddNew) {
            this.ID = ibDTO.ID;
            this.CopyID = ibDTO.CopyID;
            this.Copy = BookCopy.Find(this.CopyID);
            this.MemberID = ibDTO.MemberID;
            this.MemberInfo = Member.Find(this.MemberID);
            this.IssuedBy = ibDTO.IssuedBy;
            this.IssuedUser = User.Find(this.IssuedBy);
            this.IssueDate = ibDTO.IssueDate;
            this.DueDate = ibDTO.DueDate;
            this.ReturnDate = ibDTO.DueDate;
            this.IsReturned = ibDTO.IsReturned;

            this.IBDTO.Copy = ibDTO.Copy;
            this.IBDTO.MemberInfo = ibDTO.MemberInfo;
            this.IBDTO.IssuedUser = ibDTO.IssuedUser;

            Mode = cMode;
        }

        private bool _AddNewIssuedBook() {
            this.ID = IssuedBookData.AddNewIssuedBook(this.IBDTO);
            return (this.ID != -1);
        }

        private bool _UpdateIssuedBook() {
            return IssuedBookData.UpdateIssuedBook(this.IBDTO);
        }

        // 🔹 Find
        public static IssuedBook Find(int issueID) {
            IssuedBookDTO ibDTO = IssuedBookData.GetIssuedBookInfoByID(issueID);
            return ibDTO != null ? new IssuedBook(ibDTO, enMode.Update) : null;
        }

        // 🔹 Save (Insert or Update)
        public bool Save() {
            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewIssuedBook()){
                        Mode = enMode.Update;
                        return true;
                    }else{
                        return false;
                    }
                case enMode.Update:
                    return _UpdateIssuedBook();
            }
            return false;
        }

        // 🔹 Delete
        public static bool Delete(int issueID) {
            return IssuedBookData.DeleteIssuedBook(issueID);
        }


        // 🔹 Get all IssuedBooks
        public static List<IssuedBookDTO> GetAllIssuedBooks() {
            return IssuedBookData.GetAllIssuedBooks();
        }

        public static bool MarkBookAsReturned(int issueID) {
            return IssuedBookData.MarkBookAsReturned(issueID);
        }

        public static bool IsBookReturned(int issueID) {
            return IssuedBookData.IsBookReturned(issueID);
        }

    }
}
