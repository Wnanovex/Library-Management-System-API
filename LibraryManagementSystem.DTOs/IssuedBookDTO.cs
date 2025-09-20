using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DTOs
{
    public class IssuedBookDTO {

        public int ID { get; set; }
        public int CopyID { get; set; }
        public BookCopyDTO Copy { get; set; }
        public int MemberID { get; set; }
        public MemberDTO MemberInfo { get; set; }
        public int IssuedBy { get; set; } 
        public UserDTO IssuedUser { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsReturned { get; set; }


        public IssuedBookDTO() { }

        public IssuedBookDTO(int ID, int copyID, int memberID, int issuedBy, DateTime issueDate, 
                    DateTime dueDate, DateTime returnDate, bool isReturned) {
            this.ID = ID;
            this.CopyID = copyID;
            //this.Copy = BookCopyData.GetBookCopyInfoByID(this.CopyID);
            this.MemberID = memberID;
            //this.MemberInfo = MemberData.GetMemberByID(this.MemberID);
            this.IssuedBy = issuedBy;
            //this.IssuedUser = UserData.GetUserByID(this.IssuedBy);
            this.IssueDate = issueDate;
            this.DueDate = dueDate;
            this.ReturnDate = returnDate;
            this.IsReturned = isReturned;
        }

    }
}
