using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebApplication9.Models;

namespace WebApplication9.Controllers
{
    public class StudentController : Controller
    {
        private readonly SchoolDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StudentController(SchoolDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Student/AddStudent
        public IActionResult AddStudent()
        {
            return View();
        }

        // POST: Student/AddStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                if (student.ImageFile != null && student.ImageFile.Length > 0)
                {
                    var uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    if (!Directory.Exists(uploadsDirectory))
                    {
                        Directory.CreateDirectory(uploadsDirectory);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + student.ImageFile.FileName;

                    var filePath = Path.Combine(uploadsDirectory, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                         student.ImageFile.CopyTo(fileStream);
                    }

                    // Set the ImageUrl property with the relative path to the uploaded file
                    student.ImageUrl = "/uploads/" + uniqueFileName;
                }

                _context.Students.Add(student);
                _context.SaveChanges();
                return RedirectToAction(nameof(ListStudents));
            }
            return View(student);
        }

        // GET: Student/ListStudents
        public IActionResult ListStudents(string className, string searchString)
        {
            var students = from s in _context.Students
                           select s;

            if (!string.IsNullOrEmpty(className))
            {
                students = students.Where(s => s.Class == className);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Name.Contains(searchString));
            }

            var viewModel = new StudentListViewModel
            {
                Students = students.ToList(),
                ClassName = className,
                SearchString = searchString
            };

            return View(viewModel);
        }
    }
}
