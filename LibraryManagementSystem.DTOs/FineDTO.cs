using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DTOs
{
    public class FineDTO {

        public int ID { get; set; }
        public int IssueID { get; set; }
        public float Amount { get; set; } 
        public bool Paid { get; set; }
        public DateTime DatePaid { get; set; }


        public FineDTO() { }

        public FineDTO(int fineID, int issueID, float amount, bool paid, DateTime datePaid) {
            this.ID = fineID;
            this.IssueID = issueID;
            this.Amount = amount;
            this.Paid = paid;
            this.DatePaid = datePaid;
        }

    }
}
