using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DTOs
{
    public class CategoryDTO {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public CategoryDTO() { }

        public CategoryDTO(int categoryID, string name, string description) {
            ID = categoryID;
            Name = name;
            Description = description;
        }

    }
}
