using LibraryManagementSystem.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/Categories")]
    [ApiController]
    public class CategoriesController : ControllerBase {

        [HttpGet("All", Name ="GetAllCategories")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CategoryDTO>> GetAllCategories() { // Define a method to get all Categories.
            List<CategoryDTO> CategoriesList = LibraryManagementSystem.BLL.Category.GetAllCategories();
            if (CategoriesList.Count == 0)
                return NotFound("No Categories Found!");

            return Ok(CategoriesList); // Returns the list of Categories.
        }

        [HttpPost(Name = "AddCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CategoryDTO> AddCategory(CategoryDTO newCategoryDTO) {
            LibraryManagementSystem.BLL.Category Category = new LibraryManagementSystem.BLL.Category(newCategoryDTO);
            Category.Save();

            newCategoryDTO.ID = Category.ID;

            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetCategoryById", new { id = newCategoryDTO.ID }, newCategoryDTO);
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CategoryDTO> GetCategoryById(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            LibraryManagementSystem.BLL.Category Category = LibraryManagementSystem.BLL.Category.Find(id);

            if (Category == null)
                return NotFound($"Category with ID {id} not found.");
            
            CategoryDTO CDTO = Category.CDTO; //here we get only the DTO object to send it back.
           
            return Ok(CDTO); 
        }

        //here we use http put method for update
        [HttpPut("{id}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CategoryDTO> UpdateCategory(int id, CategoryDTO updatedCategory) {

            LibraryManagementSystem.BLL.Category Category = LibraryManagementSystem.BLL.Category.Find(id);        

            if (Category == null)
                return NotFound($"Category with ID {id} not found.");

            Category.Name = updatedCategory.Name;
            Category.Description = updatedCategory.Description;
            Category.Save();
 
            return Ok(Category.CDTO); 
        }

        //here we use HttpDelete method
        [HttpDelete("{id}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteCategory(int id) {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            //LibraryManagementSystem.BLL.Category Category = LibraryManagementSystem.BLL.Category.Find(id);        

            //if (Category == null)
            //    return NotFound($"Category with ID {id} not found.");

            if(LibraryManagementSystem.BLL.Category.Delete(id))         
                return Ok($"Category with ID {id} has been deleted.");
            else
                return NotFound($"Category with ID {id} not found. no rows deleted!");
        }


    }
}
