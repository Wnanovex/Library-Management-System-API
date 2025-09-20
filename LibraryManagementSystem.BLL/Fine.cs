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
    public class Fine {

        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode { get; set; }

        // Properties
        public int ID { get; set; }
        public int IssueID { get; set; }
        public float Amount { get; set; } 
        public bool Paid { get; set; }
        public DateTime DatePaid { get; set; }

        public FineDTO FDTO {
            get { return (new FineDTO(this.ID, this.IssueID, this.Amount ,this.Paid, this.DatePaid)); }
        }

        public Fine(FineDTO fDTO, enMode cMode = enMode.AddNew) {
            this.ID = fDTO.ID;
            this.IssueID = fDTO.IssueID;
            this.Amount = fDTO.Amount;
            this.Paid = fDTO.Paid;
            this.DatePaid = fDTO.DatePaid;

            Mode = cMode;
        }

        private bool _AddNewFine() {
            this.ID = FineData.AddNewFine(this.FDTO);
            return (this.ID != -1);
        }

        //private bool _UpdateFine() {
        //    //return FineData.UpdateFine(this.FDTO);
        //}

        // 🔹 Find
        public static Fine Find(int fineID) {
            FineDTO fDTO = FineData.GetFineInfoByID(fineID);
            return fDTO != null ? new Fine(fDTO, enMode.Update) : null;
        }

        // 🔹 Save (Insert or Update)
        public bool Save() {
            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewFine()){
                        Mode = enMode.Update;
                        return true;
                    }else{
                        return false;
                    }
                case enMode.Update:
                    return true;
                    //return _UpdateFine();
            }
            return false;
        }

        // 🔹 Delete
        public static bool Delete(int fineID) {
            return FineData.DeleteFine(fineID);
        }

        // 🔹 Get all BookCopies
        public static List<FineDTO> GetAllFines() {
            return FineData.GetAllFines();
        }

        public static bool PayFine(int fineID) {
            return FineData.PayFine(fineID);
        }

    }
}
