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
    public class Book {

        public enum enMode { AddNew, Update }
        public enMode Mode { get; set; }

        // Properties
        public int ID { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public int AuthorID { get; set; }
        public Author Author { get; set; }
        public string Edition { get; set; }
        public int PublishedYear { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public int TotalCopies { get; set; }
        public string ShelfLocation { get; set; }
        public float DailyRate { get; set; }
        public string ImageName { get; set; }
        public DateTime DateAdded { get; set; }

        public BookDTO BDTO {
            get { return (new BookDTO(this.ID, this.Title, this.ISBN ,this.CategoryID,
                this.AuthorID, this.Edition, this.PublishedYear, this.Language, this.Description, this.ShelfLocation, 
                this.DailyRate, this.ImageName, this.DateAdded)); }
        }

        public Book(BookDTO bDTO, enMode cMode = enMode.AddNew)  {
            this.ID = bDTO.ID;
            this.Title = bDTO.Title;
            this.ISBN = bDTO.ISBN;
            this.CategoryID = bDTO.CategoryID;
            this.Category = Category.Find(this.CategoryID);
            this.AuthorID = bDTO.AuthorID;
            this.Author = Author.Find(this.AuthorID);
            this.Edition = bDTO.Edition;
            this.PublishedYear = bDTO.PublishedYear;
            this.Language = bDTO.Language;
            this.Description = bDTO.Description;
            this.ShelfLocation = bDTO.ShelfLocation;
            this.DailyRate = bDTO.DailyRate;
            this.ImageName = bDTO.ImageName;
            this.DateAdded = bDTO.DateAdded;

            this.BDTO.Category = this.Category.CDTO;
            this.BDTO.Author = this.Author.ADTO;

            Mode = cMode;
        }

        private bool _AddNewBook() {
            this.ID = BookData.AddNewBook(this.BDTO);
            return (this.ID != -1);
        }

        private bool _UpdateBook() {
            return BookData.UpdateBook(this.BDTO);
        }

        // 🔹 Find
        public static Book Find(int bookID) {
            BookDTO bDTO = BookData.GetBookInfoByID(bookID);
            return bDTO != null ? new Book(bDTO, enMode.Update) : null;
        }

        // 🔹 Save (Insert or Update)
        public bool Save() {
            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewBook()){
                        Mode = enMode.Update;
                        return true;
                    }else{
                        return false;
                    }
                case enMode.Update:
                    return _UpdateBook();
            }
            return false;
        }

        // 🔹 Delete
        public static bool Delete(int bookID) {
            return BookData.DeleteBook(bookID);
        }

        // 🔹 Exists
        //public static bool isBookExist(int bookID) {
        //    return BookData.IsBookExist(bookID);
        //}

        // 🔹 Get all Books
        public static List<BookDTO> GetAllBooks() {
            return BookData.GetAllBooks();
        }

        //public static DataTable GetAllBooksByCategoryID() {
        //    return BookData.GetAllBooksByCategoryID();
        //}

    }
}
