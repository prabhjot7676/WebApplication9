using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication9.Models;

namespace WebApplication9.Controllers
{
    public class SubjectController : Controller
    {
        private readonly SchoolDbContext _context;

        public SubjectController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Subject/AddSubject
        public IActionResult AddSubject()
        {
            var viewModel = new AddSubjectViewModel
            {
                //Subject = new Subject(),
                AvailableTeachers = _context.Teachers.ToList(),

              
                LanguageOptions = _context.Languages
                    .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name })
                    .ToList()
            };

            return View(viewModel);
        }

        // POST: Subject/AddSubject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSubject(AddSubjectViewModel viewModel)
        {

            var selectedTeacherIds = viewModel.SelectedTeacherIds;
            _context.Subjects.Add(viewModel.Subject);
            _context.SaveChanges();
            if (selectedTeacherIds != null)
            {
                foreach (var teacherId in viewModel.SelectedTeacherIds)
                {
                    var subjectTeacher = new SubjectTeacher
                    {
                        SubjectId = viewModel.Subject.Id,
                        TeacherId = teacherId
                    };
                    _context.SubjectTeachers.Add(subjectTeacher);
                }
            }
           
                _context.SaveChanges();
                return RedirectToAction(nameof(ListSubjectsWithTeachers));
        }

        // GET: Subject/ListSubjects
        public IActionResult ListSubjects()
        {
            var subjects = _context.Subjects.ToList();
            return View(subjects);

           // return View(subjects);
        }
        public IActionResult ListSubjectsWithTeachers()
        {
            var subjectsWithTeachers = _context.Subjects
                .Include(s => s.SubjectTeachers)
                    .ThenInclude(st => st.Teacher)
                .ToList();

            return View(subjectsWithTeachers);
        }
    }
}
