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
    public class Member : Person {

        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode { get; set; }

        // Properties
        public int ID { get; private set; }
        public int PersonID { get; set; } 
        public DateTime DateJoined { get; set; }

        public MemberDTO MDTO {
            get { return (new MemberDTO(base.PDTO, this.ID, this.PersonID, this.DateJoined)); }
        }

        public Member(MemberDTO mDTO, enMode cMode = enMode.AddNew)
                : base((PersonDTO)mDTO, (Person.enMode)cMode)
        {
            this.ID = mDTO.ID;
            this.PersonID = mDTO.PersonID;
            this.DateJoined = mDTO.DateJoined;

            Mode = cMode;
        }

        private bool _AddNewMember() {
            this.ID = MemberData.AddNewMember(this.MDTO);
            return (this.ID != -1);
        }

        //private bool _UpdateMember() {
        //    return MemberData.UpdateMember(this.MDTO);
        //}


        // 🔹 Find
        public static Member Find(int memberID) {
            MemberDTO mDTO = MemberData.GetMemberByID(memberID);
            return mDTO != null ? new Member(mDTO, enMode.Update) : null;
        }

        //public static Member FindByPersonID(int personID) {
           
        //}


        // 🔹 Save (Insert or Update)
        public bool Save() {
            base.Mode = (Person.enMode) Mode;
            if (!base.Save()) return false;
            this.PersonID = base.ID;

            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewMember()){
                        Mode = enMode.Update;
                        return true;
                    }else{
                        return false;
                    }
                case enMode.Update:
                    //return _UpdateMember();
                    return true;
            }
            return false;
        }

        // 🔹 Delete
        public bool Delete() {
            if (!MemberData.DeleteMember(this.ID)) return false;
            return base.Delete();
        }

        // 🔹 Exists
        public static bool isMemberExist(int memberID) {
            return MemberData.IsMemberExist(memberID);
        }

        // 🔹 Get all Members
        public static List<MemberDTO> GetAllMembers() {
            return MemberData.GetAllMembers();
        }


    }
}
