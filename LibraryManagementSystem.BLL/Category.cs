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
    public class Category {

        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode { get; set; }

        // Properties
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public CategoryDTO CDTO {
            get { return (new CategoryDTO(this.ID, this.Name, this.Description)); }
        }

        public Category(CategoryDTO CDTO, enMode cMode = enMode.AddNew) {
            this.ID = CDTO.ID;
            this.Name = CDTO.Name;
            this.Description = CDTO.Description;

            Mode = cMode;
        }

        private bool _AddNewCategory() {
            this.ID = CategoryData.AddNewCategory(this.CDTO);
            return (this.ID != -1);
        }

        private bool _UpdateCategory() {
            return CategoryData.UpdateCategory(this.CDTO);
        }

        // 🔹 Find
        public static Category Find(int categoryID) {
            CategoryDTO cDTO = CategoryData.GetCategoryInfoByID(categoryID);
            return cDTO != null ? new Category(cDTO, enMode.Update) : null;
        }

        // 🔹 Save (Insert or Update)
        public bool Save() {
            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewCategory()){
                        Mode = enMode.Update;
                        return true;
                    }else{
                        return false;
                    }
                case enMode.Update:
                    return _UpdateCategory();
            }
            return false;
        }

        // 🔹 Delete
        public static bool Delete(int CategoryID) {
            return CategoryData.DeleteCategory(CategoryID);
        }


        // 🔹 Get all Categories
        public static List<CategoryDTO> GetAllCategories() {
            return CategoryData.GetAllCategories();
        }


    }
}
