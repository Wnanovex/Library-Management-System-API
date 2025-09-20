using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DTOs
{
    public class BookDTO {

        public int ID { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int CategoryID { get; set; }
        public CategoryDTO Category { get; set; }
        public int AuthorID { get; set; }
        public AuthorDTO Author { get; set; }
        public string Edition { get; set; }
        public int PublishedYear { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public int TotalCopies { get; set; }
        public string ShelfLocation { get; set; }
        public float DailyRate { get; set; }
        public string ImageName { get; set; }
        public DateTime DateAdded { get; set; }


        public BookDTO() { }

        public BookDTO(int bookID, string title, string ISBN, int categoryID, int authorID, 
                string edition, int publishedYear, string language, string description, string shelfLocation, float dailyRate, string imageName, DateTime dateAdded) {
            this.ID = bookID;
            this.Title = title;
            this.ISBN = ISBN;
            this.CategoryID = categoryID;
            this.AuthorID = authorID;
            this.Edition = edition;
            this.PublishedYear = publishedYear;
            this.Language = language;
            this.Description = description;
            this.ShelfLocation = shelfLocation;
            this.DailyRate = dailyRate;
            this.ImageName = imageName;
            this.DateAdded = dateAdded;
        }

    }
}
