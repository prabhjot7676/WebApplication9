using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebApplication9.Models;

namespace WebApplication9.Controllers
{
    public class TeacherController : Controller
    {
        private readonly SchoolDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeacherController(SchoolDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;   
        }

        public IActionResult AddTeacher()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTeacher(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                if (teacher.ImageFile != null && teacher.ImageFile.Length > 0)
                {
                    var uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    if (!Directory.Exists(uploadsDirectory))
                    {
                        Directory.CreateDirectory(uploadsDirectory);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + teacher.ImageFile.FileName;

                    var filePath = Path.Combine(uploadsDirectory, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        teacher.ImageFile.CopyTo(fileStream);
                    }
                    teacher.ImageUrl = "/uploads/" + uniqueFileName;
                }

                _context.Teachers.Add(teacher);
                _context.SaveChanges();
                return RedirectToAction(nameof(ListTeachers));
            }
            return View(teacher);
        }
        public IActionResult ListTeachers()
        {
            // Retrieve list of teachers from the database
            var teachers = _context.Teachers.ToList();

            // Optionally, you can pass the list of teachers to the view
            return View(teachers);
        }
    }
}
