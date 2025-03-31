using Microsoft.AspNetCore.Mvc;
using DeviceStore.Data;
using DeviceStore.Models;
using System;
using System.Linq;

namespace DeviceStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public IActionResult Index()
        {
            
            return View();
        }

        
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _context.Categories.Select(c => new
            {
                c.TypeID,
                c.Name,
                c.Description,
                c.Status,
            }).ToList();

            return Json(new { data = categories });
        }

        public JsonResult GetCategoryById(Guid id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.TypeID == id); 
            if (category == null)
            {
                return Json(new { message = "Category not found" }); 
            }
            return Json(new { data = category }); 
        }

        
        [HttpPost]
        public IActionResult AddCategory([FromBody] Category model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Name))
            {
                return BadRequest("ข้อมูลไม่ถูกต้อง");
            }

            model.TypeID = Guid.NewGuid();
            _context.Categories.Add(model);
            _context.SaveChanges();

            return Ok(new { message = "เพิ่มหมวดหมู่สำเร็จ!" });
        }
        public JsonResult UpdateCategory([FromBody] Category model)
        {
            if (ModelState.IsValid)
            {
                var category = _context.Categories.FirstOrDefault(c => c.TypeID == model.TypeID);
                if (category != null)
                {
                    
                    category.Name = model.Name;
                    category.Description = model.Description;
                    category.Status = model.Status;

                    
                    _context.SaveChanges();

                    return Json(new { success = true, message = "Category updated successfully" });
                }
                return Json(new { success = false, message = "Category not found" });
            }

            return Json(new { success = false, message = "Invalid data" });
        }

        public JsonResult DeleteCategory(Guid id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.TypeID == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return Json(new { success = true, message = "Category deleted successfully" });
            }
            return Json(new { success = false, message = "Category not found" });
        }

    }
}
